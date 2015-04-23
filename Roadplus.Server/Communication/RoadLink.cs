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

        public override string Address
        {
            get
            {
                return Port.PortName;
            }
        }

        private SerialPort Port;
        private string buffer;
        private bool startReceiving;
        private bool receiving;
        private Thread receiveThread;

        public RoadLink(Channel parent, SerialPort port)
            : base(parent)
        {
            receiveThread = new Thread(new ThreadStart(Receive));
            Port = port;
            port.DataReceived += Port_DataReceived;
            startReceiving = false;
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (startReceiving)
            {
                receiveThread.Start();
            }
        }

        private void Receive()
        {
            receiving = true;
            if (Port.IsOpen)
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
            else
            {
                receiving = false;
                Stop();
            }
            receiving = false;
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

        public override void Send(string data)
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
            Port.Open();
            startReceiving = true;
        }

        public override void Stop()
        {
            if (receiving)
            {
                receiveThread.Join();
            }

            startReceiving = false;
            Port.Close();
            OnDisconnected();
        }

        #endregion
    }
}

