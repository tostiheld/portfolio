using System;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

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
        private EventWaitHandle searchWait;
        private int baudRate;
        private int timeOut;

        public RoadLinkManager(MessageExchange exchange, int baudrate, int timeout)
            : base(exchange,
                   "text",
                   new PlainTextFormat())

        {
            //searchThread = new Thread(new ThreadStart(Search));
            baudRate = baudrate;
            searchWait = new EventWaitHandle(false, EventResetMode.AutoReset);
            timeOut = timeout;
        }

        private async Task Search()
        {
            while (search)
            {
                string[] ports = SerialPort.GetPortNames();

                foreach (string s in ports)
                {
                    await DetectRoadAt(s);
                }

                // only poll every n seconds
                // otherwise cpu usage will go through the roof
                //Thread.Sleep(TimeSpan.FromSeconds(timeOut));
            }
        }

        private async Task DetectRoadAt(string port)
        {
            try
            {
                bool used = false;
                foreach (Link l in Links)
                {
                    if (l.Address == port)
                    {
                        used = true;
                        break;
                    }
                }

                if (used)
                {
                    return;
                }

                string message = "";
                SerialPort testPort = new SerialPort(port, baudRate);
                testPort.Open();
                Thread.Sleep(2000);
                byte[] bytes = Encoding.ASCII.GetBytes(DiscoverString);
                await testPort.BaseStream.WriteAsync(bytes, 0, bytes.Length);
                Thread.Sleep(100);
                if (testPort.BytesToRead > 0)
                {
                    byte[] buffer = new byte[BufferSize];
                    await testPort.BaseStream.ReadAsync(buffer, 0, buffer.Length);

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
            search = true;
            Task.Run((Func<Task>)Search);
        }

        protected override void BeforeStop()
        {
            search = false;
        }

        protected override void AtStop()
        {
        }

        #endregion
    }
}

