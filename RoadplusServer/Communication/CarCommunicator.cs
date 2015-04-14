using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace Roadplus.Server.Communication
{
    public class CarCommunicator : Communicator
    {
        public IPAddress IP
        {
            get
            {
                if (socket != null)
                {
                    return ((IPEndPoint)socket.RemoteEndPoint).Address;
                }

                return IPAddress.None;
            }
        }

        private Socket socket;
        private NetworkStream dataStream;

        public CarCommunicator(Socket s)
            : base()
        {
            socket = s;
            dataStream = new NetworkStream(this.socket);
        }

        protected override bool ValidateChannel(string channel)
        {
            return true;
        }

        protected override string FillBuffer()
        {
            if (socket.Connected)
            {
                try
                {
                    byte[] buffer = new byte[Settings.CarReceiveBufferSize];
                    int length = dataStream.Read(
                        buffer,
                        0, 
                        Settings.CarReceiveBufferSize);

                    if (length == 0)
                    {
                        break;
                    }

                    ASCIIEncoding encode = new ASCIIEncoding();
                    return encode.GetString(buffer);
                }
                catch (System.IO.IOException)
                {
                    // assume the client disconnected
                    break;
                }
            }

            BeforeClose();
        }

        protected override bool Configure()
        {
            return true;
        }

        protected override void BeforeClose()
        {
            try
            {
                if (socket.Connected)
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void Send(Message message)
        {
            throw new NotImplementedException();
        }
    }
}

