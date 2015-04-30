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
        private readonly string DiscoverString = 
            PlainTextFormat.MessageStart + "discover" +
            PlainTextFormat.MessageSplit + PlainTextFormat.MessageTerminator;

        private readonly string OkString = 
            PlainTextFormat.MessageStart + "ok" +
            PlainTextFormat.MessageSplit + PlainTextFormat.MessageTerminator;

        private const int BufferSize = 64;

        private bool searching;
        private Thread searchThread;
        private System.Timers.Timer searchTimer;
        private int baudRate;

        private MessageExchange messageExchange;

        public RoadLinkManager(MessageExchange exchange,
                               int baudrate,
                               int interval)
            : base(exchange,
                   new PlainTextFormat())

        {
            searchTimer = new System.Timers.Timer(
                TimeSpan.FromSeconds(interval)
                        .TotalMilliseconds);
            searchTimer.Elapsed += searchTimer_Elapsed;
            searchTimer.Enabled = false;
            searchTimer.Start();

            searchThread = new Thread(new ThreadStart(Search));
            baudRate = baudrate;
            searching = false;

            exchange.NewActivity += Exchange_NewActivity;
            messageExchange = exchange;
        }

        private void searchTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            searchTimer.Enabled = false;
            searchThread = new Thread(new ThreadStart(Search));
            searchThread.Start();
        }

        private void Exchange_NewActivity(object sender, NewActivityEventArgs e)
        {
            if (e.NewActivity.Type == ActivityType.Get)
            {
                foreach (KeyValuePair<Type, object> pair in e.NewActivity.Payload)
                {
                    if (pair.Key == typeof(String) &&
                        pair.Value.ToString() == "ports")
                    {
                        List<string> addresses = new List<string>();
                        foreach (Link l in Links)
                        {
                            addresses.Add(l.Address);
                        }

                        Response response = new Response(
                            ResponseType.Acknoledge,
                            ActivityType.Get,
                            addresses.ToArray(),
                            e.NewActivity.SourceAddress);

                        messageExchange.Post(response);
                    }
                }
            }
        }

        private void Search()
        {
            searching = true;

            // filter out ports we already have
            // TODO: can we optimise this?
            List<string> ports = new List<string>();
            foreach (string s in SerialPort.GetPortNames())
            {
                foreach (Link l in Links)
                {
                    if (s == l.Address)
                    {
                        continue;
                    }
                }

                ports.Add(s);
            }

            foreach (string s in ports)
            {
                DetectRoadAt(s);
            }

            searchTimer.Enabled = true;
            searching = false;
        }

        private void DetectRoadAt(string port)
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
                    RoadLink newLink = new RoadLink(this, testPort);
                    NewLink(newLink);
                }
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
                    return;
                }

                throw;
            }
        }

        #region implemented abstract members of Channel

        protected override void AtStart()
        {
            searchThread.Start();
        }

        protected override void AtStop()
        {
            searchTimer.Stop();
            if (searching)
            {
                Trace.WriteLine(
                    "Finishing serial discover in progress, hang on a few seconds...");
                searchThread.Join();
            }
        }

        #endregion
    }
}

