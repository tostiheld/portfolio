using System;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using System.Linq;

using Roadplus.Server.Communication.Protocol;
using Roadplus.Server.EntityManagement;

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

        private volatile bool searching;
        private Thread searchThread;
        private int baudRate;

        private MessageExchange messageExchange;

        public RoadLinkManager(MessageExchange exchange, int baudrate)
            : base(exchange,
                   "text",
                   new PlainTextFormat())

        {
            searchThread = new Thread(new ParameterizedThreadStart(Search));
            baudRate = baudrate;
            searching = false;

            exchange.NewActivity += Exchange_NewActivity;
            messageExchange = exchange;
        }

        private void Exchange_NewActivity(object sender, NewActivityEventArgs e)
        {
            if (e.NewActivity.From.Type == LinkType.UI &&
                e.NewActivity.Type == ActivityType.Get &&
                e.NewActivity.Parameters[0].ToString() == "ports")
            {
                if (searching)
                {
                    Response response = new Response(
                        ResponseType.Information,
                        e.NewActivity.From,
                        this.GetType(),
                        new string[] { "Serial discover in progress" });
                    messageExchange.Post(response);
                }
                else
                {
                    searchThread.Start(e.NewActivity.From);
                }
            }
        }

        private void Search(object returnLink)
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

            if (returnLink is Link)
            {
                List<string> newports = new List<string>();

                foreach (Link l in Links)
                {
                    newports.Add(l.Address);
                }

                Response respone = new Response(
                    ResponseType.Acknoledge,
                    returnLink as Link,
                    this.GetType(),
                    newports.ToArray());
                messageExchange.Post(respone);
            }


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
        }

        protected override void AtStop()
        {
            if (searching)
            {
                Trace.WriteLine(
                    "Ending serial discover in progress, hang on a few seconds...");
                searchThread.Join();
            }
        }

        #endregion
    }
}

