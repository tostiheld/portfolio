using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Roadplus.Server.API
{
    public abstract class Channel
    {
        public ReadOnlyCollection<Link> Links 
        {
            get
            {
                return links.AsReadOnly();
            }
        }

        public IFormatter Formatter { get; private set; }

        public CommandProcessor CommandProcessor { get; private set; }

        private List<Link> links;

        public Channel(CommandProcessor commandprocessor, IFormatter formatter)
        {
            if (commandprocessor == null ||
                formatter == null)
            {
                throw new ArgumentNullException();
            }

            CommandProcessor = commandprocessor;
            Formatter = formatter;
            links = new List<Link>();
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

        public void Broadcast(IResponse response)
        {
            foreach (Link l in links)
            {
                l.Send(response);
            }
        }

        public void Broadcast(IRequest request)
        {
            foreach (Link l in links)
            {
                l.Send(request);
            }
        }

        protected void NewLink(Link link)
        {
            // clone to be able to edit original list
            List<Link> temp = new List<Link>(links);

            foreach (Link l in temp)
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

        public Link GetLinkByAddress(string address)
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

        public void Post(Link from, string data)
        {
            Trace.WriteLine("Received from " + from.Address + ": " + data);

            IResponse response = CommandProcessor.Process(data);
            from.Send(response);
        }

        public void Start()
        {
            links = new List<Link>();
            AtStart();
        }

        public void Stop()
        {
            AtStop();
        }

        protected abstract void AtStart();
        protected abstract void AtStop();
    }
}
