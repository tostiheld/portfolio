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

        private List<Link> links;
        private CommandProcessor commandProcessor;

        public Channel(CommandProcessor commandprocessor)
        {
            if (commandprocessor == null)
            {
                throw new ArgumentNullException("commandprocessor");
            }

            commandProcessor = commandprocessor;
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
            Trace.WriteLine("Received from " + from.Address + ": " + data);

            IResponse response = commandProcessor.Process(data);
            from.Send(response.ToString());
        }

        public void Start()
        {
            links = new List<Link>();
            AtStart();
        }

        public void Stop()
        {
            /*
            Response shutdown = new Response();
            shutdown.Type = ResponseType.Information;
            shutdown.Message = "Channel closing";

            // TODO: which formatter to use? - response's own or channel's?
            string message = MessageFormatter.Format(shutdown);

            // clone so we can modify
            List<Link> temp = new List<Link>(links);
            foreach (Link l in temp)
            {
                l.Send(message);
                l.Stop();
            }*/

            AtStop();
        }

        protected abstract void AtStart();
        protected abstract void AtStop();
    }
}
