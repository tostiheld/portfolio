using System;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Threading;

using Roadplus.Server.Communication.Protocol;

namespace Roadplus.Server.Communication
{
    public class RoadLink : Link
    {
        private const int BufferSize = 32;

        public override string Address
        {
            get
            {
                return Port.PortName;
            }
        }

        private SerialPort Port;
        private Thread receiveThread;
        private volatile bool receive;
        private string buffer;

        public RoadLink(Channel parent, SerialPort port)
            : base(parent)
        {
            Port = port;

            receiveThread = new Thread(new ThreadStart(Receive));
        }

        private void Receive()
        {
            while (receive)
            {
                if (Port.IsOpen)
                {
                    if (Port.BytesToRead > 0)
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
                else
                {
                    Stop();
                }
            }
        }

        private string FindMessages()
        {
            int start = buffer.IndexOf(PlainTextFormat.MessageStart);
            if (start != -1)
            {
                int end = buffer.IndexOf(PlainTextFormat.MessageTerminator);
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

        #region implemented abstract members of Link

        protected override void Send(string data)
        {
            if (Port.IsOpen)
            {
                Port.Write(data);
            }
            else
            {
                Stop();
            }
        }

        public override void Start()
        {
            buffer = "";
            receive = true;
            receiveThread.Start();
        }

        public override void Stop()
        {
            receive = false;
            receiveThread.Join();
            OnDisconnected();
        }

        #endregion
    }
}

