using System;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Threading;

using Roadplus.Server.API;

namespace Roadplus.Server.Communication
{
    public class RoadLink : Link
    {
        private const int BufferSize = 32;
        private const string MessageStart = ">";
        private const string MessageEnd   = ";";

        public override string Address
        {
            get
            {
                return Port.PortName;
            }
        }

        private SerialPort Port;
        private string buffer;

        private Thread receiveThread;
        private bool startReceiving;

        public RoadLink(Channel parent, SerialPort port)
            : base(parent)
        {
            if (port == null)
            {
                throw new ArgumentNullException();
            }

            Port = port;
        }

        private void Receive()
        {
            Port.Open();

            EventWaitHandle waithandler = new EventWaitHandle(
                false, 
                EventResetMode.AutoReset,
                Guid.NewGuid().ToString());

            while (Port.IsOpen &&
                   startReceiving)
            {
                try
                {
                    while (Port.BytesToRead > 0)
                    {
                        byte[] bytes = new byte[BufferSize];
                        Port.Read(bytes, 0, BufferSize);

                        ASCIIEncoding encoder = new ASCIIEncoding();
                        string message = encoder.GetString(bytes);
                        buffer += message;

                        string found = FindMessages();
                        if (found != null)
                        {
                            Parent.Post(this, found);
                        }
                    }
                }
                catch (IOException)
                {
                    break;
                }

                waithandler.WaitOne(1);
            }

            Port.Close();
        }

        private string FindMessages()
        {
            int start = buffer.IndexOf(MessageStart);
            if (start != -1)
            {
                int end = buffer.IndexOf(MessageEnd);
                if (end != -1)
                {
                    string msg = buffer.Substring(
                        start, (end - start) + 1);
                    buffer = buffer.Substring(end + 1);

                    return msg;
                }
            }

            return null;
        }

        protected override void DoSend(string data)
        {
            if (Port.IsOpen)
            {
                Port.Write(data);
            }
            else
            {
                try
                {
                    Port.Open();
                    Port.Write(data);
                }
                catch (IOException)
                {
                    Stop();
                }
            }
        }

        public override void Start()
        {
            buffer = "";
            startReceiving = true;
            receiveThread = new Thread(new ThreadStart(Receive));
            receiveThread.Start();
        }

        public override void Stop()
        {
            startReceiving = false;
            receiveThread.Join();

            if (Port.IsOpen)
            {
                Port.Close();
            }

            OnDisconnected();
        }
    }
}

