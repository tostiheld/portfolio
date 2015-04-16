using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using vtortola.WebSockets;
using vtortola.WebSockets.Rfc6455;

namespace Roadplus.Server.Communication
{
    public class WSService
    {
        public delegate void OnNewSession(object sender, NewSessionEventArgs e);
        public event OnNewSession NewSession;

        public IPAddress IP { get; private set; }

        private WebSocketListener listener;

        public WSService(IPEndPoint endpoint)
        {
            listener = new WebSocketListener(endpoint);
            WebSocketFactoryRfc6455 standard = new WebSocketFactoryRfc6455(listener);
            listener.Standards.RegisterStandard(standard);
            IP = endpoint.Address;
        }

        private async Task Listen()
        {
            while (listener.IsStarted)
            {
                WebSocket socket = await listener.AcceptWebSocketAsync(CancellationToken.None)
                                                 .ConfigureAwait(false);

                if (socket != null &&
                    NewSession != null)
                {
                    WSSession session = new WSSession(socket);
                    NewSessionEventArgs e = new NewSessionEventArgs(session);
                    NewSession(this, e);
                }
            }
        }

        public void Start()
        {
            listener.Start();
            Task.Run((Func<Task>)Listen);
        }

        public void Stop()
        {
            listener.Stop();
        }
    }
}

