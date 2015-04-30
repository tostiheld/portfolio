using System;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Collections.Generic;

using Roadplus.Server.API;
using Roadplus.Server.Communication;
using Roadplus.Server.Communication.Http;
using Roadplus.Server.Traffic;

namespace Roadplus.Server
{
    public class RoadplusService
    {
        private MessageExchange messageExchange;

        private WSSessionManager websocketService;
        private RoadLinkManager roadLinkService;
        private HttpService httpService;

        private List<Channel> channels;

        private string fileRoot;

        public RoadplusService(Settings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            channels = new List<Channel>();
            fileRoot = settings.FileRoot;

            if (settings.EnableHttp)
            {
                httpService = new HttpService(
                    new IPEndPoint(
                    settings.IP,
                    settings.HttpPort),
                    settings.HttpRoot);
            }

            ActivityValidator validator = new ActivityValidator();
            validator.AllowActivity(LinkType.UI,
                                    ActivityType.Get);
            validator.AllowActivity(LinkType.UI,
                                    ActivityType.Create);
            validator.AllowActivity(LinkType.UI,
                                    ActivityType.Remove);
            validator.AllowActivity(LinkType.UI,
                                    ActivityType.Set);

            messageExchange = new MessageExchange(validator);

            roadLinkService = new RoadLinkManager(
                messageExchange,
                settings.BaudRate,
                settings.RoadDetectTimeOut);
            AddChannel(roadLinkService);

            websocketService = new WSSessionManager(
                messageExchange,
                new IPEndPoint(
                settings.IP,
                settings.Port));
            AddChannel(websocketService);

            EntityManager<Zone> zones = new EntityManager<Zone>(messageExchange);
            EntityManager<School> scbools = new EntityManager<School>(messageExchange);
            EntityManager<RoadConstruction> roads = new EntityManager<RoadConstruction>(messageExchange);
        }

        private void AddChannel(Channel channel)
        {
            channels.Add(channel);
            messageExchange.Register(channel);
        }

        public void Start()
        {
            Trace.WriteLine(
                "Starting RoadPlus service...");

            if (httpService != null)
            {
                httpService.Start();
            }

            foreach (Channel c in channels)
            {
                c.Start();
            }

            Trace.WriteLine(
                "...started.");
        }

        public void Stop()
        {
            Trace.WriteLine(
                "Stopping RoadPlus service...");

            if (httpService != null)
            {
                httpService.Stop();
            }

            foreach (Channel c in channels)
            {
                c.Stop();
            }

            Trace.WriteLine(
                "...stopped.");
        }
    }
}

