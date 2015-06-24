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

        private List<Link> links;
        private CommandProcessor commandProcessor;

        public Channel(CommandProcessor commandprocessor, IFormatter formatter)
        {
            if (commandprocessor == null ||
                formatter == null)
            {
                throw new ArgumentNullException();
            }

            commandProcessor = commandprocessor;
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

        /*
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
        }*/

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

        public void Post(Link from, string data)
        {
            Trace.WriteLine("Received from " + from.Address + ": " + data);

            IResponse response = commandProcessor.Process(data);
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
