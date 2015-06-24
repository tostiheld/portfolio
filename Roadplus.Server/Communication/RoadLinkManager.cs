using System;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;

using Roadplus.Server.API;

namespace Roadplus.Server.Communication
{
    public class RoadLinkManager : Channel
    {
        private const string DiscoverString = ">11:;";
        private const string OkString       = ">ok:;";
        private const int    BufferSize     = 32;

        private Thread discoverThread;
        private bool startDiscovering;
        private int baudRate;
        private int discoverInterval;

        public RoadLinkManager(CommandProcessor commandprocessor,
                               int baudrate,
                               int interval)
            : base(commandprocessor)

        {
            baudRate = baudrate;
            discoverInterval = interval;
        }

        private void Discover()
        {
            EventWaitHandle waithandler = new EventWaitHandle(
                    false, 
                    EventResetMode.AutoReset,
                    Guid.NewGuid().ToString());
            
            while (startDiscovering)
            {
                // possible fix for OSX's toomanyfilesexception
                GC.Collect();

                string[] ports = SerialPort.GetPortNames();

                foreach (string p in ports)
                {
                    using (SerialPort sp = new SerialPort(p, baudRate))
                    {
                        if (DetectRoadAt(sp))
                        {
                            RoadLink rl = new RoadLink(this, sp);
                            NewLink(rl);
                        }
                    }
                }

                // discover at interval to reduce cpu usage
                waithandler.WaitOne(TimeSpan.FromSeconds(discoverInterval));
            }
        }

        private bool DetectRoadAt(SerialPort port)
        {
            try
            {
                string message = "";
                SerialPort testPort = new SerialPort(port, baudRate);
                testPort.Open();

                // give the serial device 3 seconds to wake up
                Thread.Sleep(3000);
                testPort.Write(DiscoverString);

                // give the device a few milliseconds to reply
                Thread.Sleep(100);
                if (testPort.BytesToRead > 0)
                {
                    byte[] buffer = new byte[BufferSize];
                    testPort.Read(buffer, 0, buffer.Length);
                    ASCIIEncoding encoder = new ASCIIEncoding();
                    message = encoder.GetString(buffer);
                }
                testPort.Close();

                if (message.Contains(OkString))
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                if (ex is UnauthorizedAccessException ||
                    ex is ArgumentOutOfRangeException ||
                    ex is ArgumentException ||
                    ex is IOException ||
                    ex is InvalidOperationException)
                {
                    // this means the port is not suitable 
                    // so it is safe to ignore these 
                    // exceptions now
                    return false;
                }
                throw;
            }
        }

        protected override void AtStart()
        {
            discoverThread = new Thread(new ThreadStart(Discover));
            startDiscovering = true;
            discoverThread.Start();
        }

        protected override void AtStop()
        {
            startDiscovering = false;
            discoverThread.Join();
        }
    }
}

