using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Roadplus.Server.API
{
    public abstract class Channel
    {
        public event EventHandler<MessageFoundEventArgs> MessageFound;

        public IFormatHandler MessageFormatter { get; private set; }

        protected ReadOnlyCollection<Link> Links 
        {
            get
            {
                return links.AsReadOnly();
            }
        }

        private List<Link> links;

        public Channel(
            MessageExchange exchange,
            IFormatHandler messageformatter)
        {
            MessageFormatter = messageformatter;
            links = new List<Link>();

            exchange.NewResponse += Exchange_NewResponse;
            exchange.NewActivity += Exchange_NewActivity;
        }

        // TODO: this method probably doesn't belong here.
        private void Exchange_NewActivity(object sender, NewActivityEventArgs e)
        {
            Link targetLink = GetLinkAt(e.NewActivity.SourceAddress);
            if (targetLink == null)
            {
                return;
            }

            LinkType linktype = LinkType.Unidentified;
            if (e.NewActivity.SourceType == LinkType.Unidentified &&
                Enum.TryParse(e.NewActivity.Payload[0].ToString(), out linktype))
            {
                targetLink.Type = linktype;
                Trace.WriteLine(
                    "Link at " + e.NewActivity.SourceAddress +
                    " identified as " + linktype.ToString("G"));
            }
        }

        private void Exchange_NewResponse(object sender, NewResponseEventArgs e)
        {
            Response response = e.NewResponse;
            Link targetLink = GetLinkAt(response.DestinationAddress);
            if (targetLink == null)
            {
                return;
            }

            if (!response.Broadcast)
            {
                targetLink.Send(
                    MessageFormatter.Format(response));
            }
            else
            {
                if (response.BroadcastTo == null)
                {
                    Broadcast(response);
                }
                else
                {
                    Broadcast(response,
                              response.BroadcastTo.Value);
                }
            }
        }

        private void Link_Disconnected(object sender, EventArgs e)
        {
            if (sender is Link)
            {
                Link link = sender as Link;
                links.Remove(link);
                Trace.WriteLine(
                    "Link at " + link.Address + " disconnected");
            }
        }

        private Link GetLinkAt(string address)
        {
            foreach (Link l in links)
            {
                if (l.Address == address)
                {
                    return l;
                }
            }

            return null;
        }

        private void Broadcast(Response response)
        {
            foreach (Link l in links)
            {
                l.Send(MessageFormatter.Format(response));
            }
        }

        private void Broadcast(Response response, LinkType destination)
        {
            foreach (Link l in links)
            {
                if (l.Type == destination)
                {
                    l.Send(MessageFormatter.Format(response));
                }
            }
        }

        protected void NewLink(Link link)
        {
            foreach (Link l in links)
            {
                if (l.Address == link.Address)
                {
                    l.Stop();
                }
            }

            link.Disconnected += Link_Disconnected;
            links.Add(link);
            link.Start();
            Trace.WriteLine(
                "New link at " + link.Address);
        }

        public void Post(Link from, string data)
        {
            if (MessageFound != null)
            {
                RawMessage newMessage = new RawMessage(
                    from.Address,
                    from.Type,
                    MessageFormatter.MessageFormat,
                    data);

                MessageFoundEventArgs e = 
                    new MessageFoundEventArgs(newMessage);
                MessageFound(this, e);
            }
        }

        public void Start()
        {
            links = new List<Link>();
            AtStart();
        }

        public void Stop()
        {
            Response shutdown = new Response(
                ResponseType.Information,
                ActivityType.Unknown,
                "Channel closing",
                LinkType.Unidentified);

            string message = MessageFormatter.Format(shutdown);

            // clone so we can modify
            List<Link> temp = new List<Link>(links);
            foreach (Link l in temp)
            {
                l.Send(message);
                l.Stop();
            }

            AtStop();
        }

        protected abstract void AtStart();
        protected abstract void AtStop();
    }
}

