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

        public abstract void Send(string data);
        public abstract void Start();
        public abstract void Stop();
    }
}

