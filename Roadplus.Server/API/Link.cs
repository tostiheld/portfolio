using System;

namespace Roadplus.Server.API
{
    public abstract class Link
    {
        public event EventHandler<EventArgs> Disconnected;

        public abstract string Address { get; }
        public LinkType Type { get; set; }
        public Channel Parent { get; private set; }

        public Link(Channel parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException();
            }

            Parent = parent;
            Type = LinkType.Unidentified;
        }

        protected virtual void OnDisconnected()
        {
            if (Disconnected != null)
            {
                Disconnected(this, EventArgs.Empty);
            }
        }

        public void Send(IResponse response)
        {
            if (response == null)
            {
                return;
            }

            string data = Parent.Formatter.Format(response);
            DoSend(data);
        }

        public void Send(IRequest request)
        {
            if (request == null)
            {
                return;
            }

            string data = Parent.Formatter.Format(request);
            DoSend(data);
        }

        protected abstract void DoSend(string data);
        public abstract void Start();
        public abstract void Stop();
    }
}

