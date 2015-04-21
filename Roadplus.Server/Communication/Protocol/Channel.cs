using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Roadplus.Server.EntityManagement;

namespace Roadplus.Server.Communication.Protocol
{
    public abstract class Channel
    {
        public event EventHandler<MessageFoundEventArgs> MessageFound;

        public string MessageFormat { get; private set; }
        public FormatHandler MessageFormatter { get; private set; }

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
            string messageformat,
            FormatHandler messageformatter)
        {
            MessageFormat = messageformat;
            MessageFormatter = messageformatter;
            links = new List<Link>();

            exchange.NewResponse += Exchange_NewResponse;
            exchange.NewActivity += Exchange_NewActivity;
        }

        // TODO: this method probably doesn't belong here.
        private void Exchange_NewActivity(object sender, NewActivityEventArgs e)
        {
            if (e.NewActivity.From.Parent == this &&
                e.NewActivity.Type == ActivityType.Identify)
            {
                LinkType linkType = LinkType.Unidentified;
                if (Enum.TryParse(e.NewActivity.Parameters[0].ToString(), out linkType))
                {
                    e.NewActivity.From.Type = linkType;
                    Trace.WriteLine(
                        "Link at " + e.NewActivity.From.Address +
                        " identified as " + linkType.ToString("G"));
                }
            }
        }

        private void Exchange_NewResponse(object sender, NewResponseEventArgs e)
        {
            Response response = e.NewResponse;

            if (!response.Broadcast &&
                response.To.Parent == this)
            {
                response.To.Send(response);
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

        private void Broadcast(Response response)
        {
            foreach (Link l in links)
            {
                l.Send(response);
            }
        }

        private void Broadcast(Response response, LinkType destination)
        {
            foreach (Link l in links)
            {
                if (l.Type == destination)
                {
                    l.Send(response);
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
                Message newMessage = new Message(
                    from,
                    MessageFormat,
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
                this.GetType(),
                new string[] { "Channel closing" });

            // clone so we can modify
            List<Link> temp = new List<Link>(links);
            foreach (Link l in temp)
            {
                l.Send(shutdown);
                l.Stop();
            }

            AtStop();
        }

        protected abstract void AtStart();
        protected abstract void AtStop();
    }
}

