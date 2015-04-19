using System;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Threading;

using Roadplus.Server.Communication.Protocol;

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

        private volatile bool search;
        private Thread searchThread;
        private int baudRate;

        public RoadLinkManager(MessageExchange exchange, int baudrate)
            : base(exchange,
                   "text",
                   new PlainTextFormat())

        {
            searchThread = new Thread(new ThreadStart(Search));
            baudRate = baudrate;
        }

        private void Search()
        {
            while (search)
            {
                string[] ports = SerialPort.GetPortNames();

                foreach (string s in ports)
                {
                    try
                    {
                        string message = "";
                        SerialPort testPort = new SerialPort(s, baudRate);
                        testPort.Open();
                        testPort.Write(DiscoverString);
                        if (testPort.BytesToRead > 0)
                        {
                            byte[] buffer = new byte[BufferSize];
                            testPort.Read(buffer, 0, BufferSize);

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
                            continue;
                        }

                        throw;
                    }
                }
            }
        }

        #region implemented abstract members of Channel

        protected override void AtStart()
        {
            search = true;
            searchThread.Start();
        }

        protected override void AtStop()
        {
            search = false;
            searchThread.Join();
        }

        #endregion
    }
}

