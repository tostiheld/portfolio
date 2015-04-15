using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using vtortola.WebSockets;

namespace Roadplus.Server.Communication
{
    public class WSSession
    {
        public delegate void OnMessageReceived(object sender, MessageReceivedEventArgs e);
        public event OnMessageReceived MessageReceived;
        public event EventHandler Disconnected;

        public IPAddress IP
        {
            get
            {
                return webSocket.LocalEndpoint.Address;
            }
        }

        private WebSocket webSocket;
        private string buffer;

        public WSSession(WebSocket socket)
        {
            if (socket == null)
            {
                throw new ArgumentNullException("socket");
            }

            webSocket = socket;
            Task.Run((Func<Task>)Receive);
        }

        private async Task Receive()
        {
            while (webSocket.IsConnected)
            {
                WebSocketMessageReadStream stream = await webSocket.ReadMessageAsync(CancellationToken.None)
                                                                   .ConfigureAwait(false);

                // we'll only handle text for now.
                if (stream.MessageType == WebSocketMessageType.Text)
                {
                    byte[] bytes = new byte[Settings.BufferSize];
                    int length = stream.Read(bytes, 0, Settings.BufferSize);

                    if (length == 0)
                    {
                        break;
                    }

                    ASCIIEncoding encoding = new ASCIIEncoding();
                    string message = encoding.GetString(bytes);
                    buffer += message;
                    ProcessMessages();
                }
            }

            if (Disconnected != null)
            {
                Disconnected(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Only processes new string messages.
        /// Expects messages in simple ASCII protocol format.
        /// </summary>
        private void ProcessMessages()
        {
            int start = buffer.IndexOf(Message.MessageStart);
            if (start != -1)
            {
                int end = buffer.IndexOf(Message.MessageTerminator);
                if (end != -1)
                {
                    string msg = buffer.Substring(
                        start, (end - start) + 1);
                    buffer = buffer.Substring(end + 1);

                    string cmd = msg.Substring(1, 4);
                    string data = msg.Substring(4, msg.Length - 6);

                    Message received = Message.FromString(cmd, data);

                    // drop message if unknown command
                    // TODO: do we need a notification?
                    if (received != null &&
                        MessageReceived != null)
                    {
                        MessageReceivedEventArgs e = new MessageReceivedEventArgs(
                            received);
                        MessageReceived(this, e);
                    }
                }
            }
        }

        public void End()
        {
            webSocket.Close();
        }
    }
}

