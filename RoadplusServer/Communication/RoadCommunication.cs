using System;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace Roadplus.Server.Communication
{
    public class RoadCommunication
    {
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public string PortName
        {
            get
            {
                return Port.PortName;
            }
        }

        private Thread receiveThread;
        private volatile bool isReceiving;
        private SerialPort Port;
        private string buffer;

        public RoadCommunication(string port)
        {
            Port = new SerialPort(port, Settings.BaudRate);
            // test if everything works
            Port.Open();
            Port.Close();
            receiveThread = new Thread(new ThreadStart(Receive));
        }

        private void Receive()
        {
            while (isReceiving)
            {
                if (Port.IsOpen)
                {
                    if (Port.BytesToRead > 0)
                    {
                        byte[] bytes = new byte[Settings.BufferSize];
                        buffer += Port.Read(bytes, 0, Settings.BufferSize);

                        ASCIIEncoding encoder = new ASCIIEncoding();
                        string message = encoder.GetString(bytes);
                        buffer += message;
                        Source source = new Source(SourceTypes.Road, Port.ToString());
                        Message received = Utilities.ProcessMessages(source, ref buffer);
                        if (received != null &&
                            MessageReceived != null)
                        {
                            MessageReceivedEventArgs e = new MessageReceivedEventArgs(
                                received);
                            MessageReceived(this, e);
                        }
                    }
                }
                else
                {
                    isReceiving = false;
                }
            }
        }

        public void Start()
        {
            isReceiving = true;
            Port.Open();
            receiveThread.Start();
        }

        public void Stop()
        {
            isReceiving = false;
            Port.Close();
            receiveThread.Join();
        }
    }
}

