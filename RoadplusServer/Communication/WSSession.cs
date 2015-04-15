using System;
using System.Threading;
using System.Threading.Tasks;
using vtortola.WebSockets;

namespace Roadplus.Server.Communication
{
    public class WSSession
    {
        public const string MessageStart = ">";
        public const string MessageSplit = ":";
        public const string MessageTerminator = ";";

        public delegate void OnMessageReceived(object sender, MessageReceivedEventArgs e);
        public event OnMessageReceived MessageReceived;

        private WebSocket webSocket;
        private string buffer;

        public WSSession(WebSocket socket)
        {
            webSocket = socket;
            Task.Run((Func<Task>)Receive);
        }

        private async Task Receive()
        {
            while (webSocket.IsConnected)
            {
                string message = await webSocket.ReadStringAsync(CancellationToken.None)
                                                .ConfigureAwait(false);

                if (message != null)
                {
                    buffer += message;
                    ProcessMessages();
                }
            }
        }

        private void ProcessMessages()
        {
            int start = buffer.IndexOf(MessageStart);
            if (start != -1)
            {
                int end = buffer.IndexOf(MessageTerminator);
                if (end != -1)
                {
                    string msg = buffer.Substring(
                        start, (end - start) + 1);
                    buffer = buffer.Substring(end + 1);

                    throw new NotImplementedException();

                    string cmd = "";
                    string data = "";

                    Message received = Message.FromString(cmd, data);

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
    }
}

