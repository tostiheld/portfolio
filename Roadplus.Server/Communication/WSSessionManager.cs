using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using vtortola.WebSockets;
using vtortola.WebSockets.Rfc6455;

using Roadplus.Server.API;

namespace Roadplus.Server.Communication
{
    public class WSSessionManager : Channel
    {
        private WebSocketListener listener;

        public WSSessionManager(MessageExchange exchange,
                                IPEndPoint endpoint)
            : base (exchange,
                    new JSONFormat())
        {
            listener = new WebSocketListener(endpoint);
            WebSocketFactoryRfc6455 standard = new WebSocketFactoryRfc6455(listener);
            listener.Standards.RegisterStandard(standard);
        }

        private async Task Listen()
        {
            while (listener.IsStarted)
            {
                WebSocket socket = await listener.AcceptWebSocketAsync(CancellationToken.None)
                    .ConfigureAwait(false);

                if (socket != null)
                {
                    WSSession session = new WSSession(this, socket);
                    NewLink(session);
                }
            }
        }

        #region implemented abstract members of Channel

        protected override void AtStart()
        {
            listener.Start();
            Task.Run((Func<Task>)Listen);
        }

        protected override void AtStop()
        {
            listener.Stop();
        }

        #endregion
    }
}

