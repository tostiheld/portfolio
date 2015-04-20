using System;
using System.Text;
using vtortola.WebSockets;
using System.Threading;
using System.Threading.Tasks;

using Roadplus.Server.Communication.Protocol;

namespace Roadplus.Server.Communication
{
    public class WSSession : Link
    {
        public override string Address
        {
            get
            {
                return webSocket.LocalEndpoint.Address.ToString();
            }
        }

        private WebSocket webSocket;
        private bool stopping;

        public WSSession(Channel parent, WebSocket socket)
            : base (parent)
        {
            webSocket = socket;
        }

        private async Task Receive()
        {
            while (webSocket.IsConnected)
            {
                string message = await webSocket.ReadStringAsync(CancellationToken.None)
                                                .ConfigureAwait(false);

                if (message == null)
                {
                    continue;
                }

                Parent.Post(this, message);
            }

            Stop();
        }

        #region implemented abstract members of Link

        protected override void Send(string data)
        {
            if (webSocket.IsConnected)
            {
                webSocket.WriteString(data);
            }
            else
            {
                Stop();
            }
        }

        public override void Start()
        {
            Task.Run((Func<Task>)Receive);
        }

        public override void Stop()
        {
            if (!stopping)
            {
                stopping = true;
                webSocket.Close();
                OnDisconnected();
            }
        }

        #endregion
    }
}

