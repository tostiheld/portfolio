using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using vtortola.WebSockets;

namespace Roadplus.Server.Communication
{
    public class WSSession
    {
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;
        public event EventHandler<EventArgs> Disconnected;

        public IPAddress IP
        {
            get
            {
                return webSocket.LocalEndpoint.Address;
            }
        }
        public SourceTypes SourceType { get; set; } 

        private WebSocket webSocket;
        private string buffer;

        public WSSession(WebSocket socket)
        {
            if (socket == null)
            {
                throw new ArgumentNullException("socket");
            }

            SourceType = SourceTypes.Unidentified;
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

                    UTF8Encoding encoding = new UTF8Encoding();
                    string message = encoding.GetString(bytes);
                    buffer += message;

                    Source source = new Source(SourceType, IP);
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

            if (Disconnected != null)
            {
                Disconnected(this, EventArgs.Empty);
            }
        }

        public void Send(Message message)
        {
            if (webSocket.IsConnected)
            {
                byte[] bytes;
                using (MemoryStream ms = new MemoryStream())
                {
                    DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(Message));
                    json.WriteObject(ms, message);
                    bytes = ms.ToArray();
                }

                UTF8Encoding encoder = new UTF8Encoding();
                string sendstring = encoder.GetString(bytes);

                using (WebSocketMessageWriteStream stream = 
                       webSocket.CreateMessageWriter(WebSocketMessageType.Text))
                {
                    using (StreamWriter sw = new StreamWriter(stream, Encoding.UTF8))
                    {
                        sw.Write(sendstring);
                    }
                }
            }
        }

        public void End()
        {
            webSocket.Close();

            if (Disconnected != null)
            {
                Disconnected(this, EventArgs.Empty);
            }
        }
    }
}

