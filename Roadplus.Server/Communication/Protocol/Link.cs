using System;

namespace Roadplus.Server.Communication.Protocol
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

        public void Send(Response response)
        {
            string data = response.ToString(
                Parent.MessageFormat,
                Parent.MessageFormatter);

            Send(data);
        }

        protected abstract void Send(string data);
        public abstract void Start();
        public abstract void Stop();
    }
}

