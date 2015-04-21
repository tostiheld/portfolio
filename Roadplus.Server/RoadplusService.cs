using System;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Collections.Generic;

using Roadplus.Server.Communication.Protocol;
using Roadplus.Server.Communication;
using Roadplus.Server.EntityManagement;
using Roadplus.Server.Traffic;

namespace Roadplus.Server
{
    public class RoadplusService
    {
        private ActivityFactory activityFactory;

        private WSSessionManager websocketService;
        private RoadLinkManager roadLinkService;
        private HttpService httpService;

        private List<Channel> channels;
        private List<EntityManager> entityManagers;

        private string fileRoot;

        public RoadplusService(Settings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            channels = new List<Channel>();
            entityManagers = new List<EntityManager>();
            fileRoot = settings.FileRoot;

            if (settings.EnableHttp)
            {
                httpService = new HttpService(
                    new IPEndPoint(
                    settings.IP,
                    settings.HttpPort),
                    settings.HttpRoot);
            }

            IPEndPoint wsendpoint = new IPEndPoint(
                settings.IP,
                settings.Port);

            MessageExchange messageExchange = new MessageExchange();

            activityFactory = new ActivityFactory(messageExchange);
            activityFactory.Register(new JSONFormat());
            activityFactory.Register(new PlainTextFormat());

            websocketService = new WSSessionManager(messageExchange,
                                                    wsendpoint);
            activityFactory.Register(websocketService);
            channels.Add(websocketService);

            /*
            roadLinkService = new RoadLinkManager(messageExchange,
                                                  settings.BaudRate,
                                                  settings.RoadDetectTimeOut);
            activityFactory.Register(roadLinkService);
            channels.Add(roadLinkService);*/
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

            foreach (EntityManager e in entityManagers)
            {
                //e.Load();
            }

            Trace.WriteLine(
                "... started");
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

            foreach (EntityManager e in entityManagers)
            {
                // e.Save();
            }

            Trace.WriteLine(
                "... stopped");
        }
    }
}

