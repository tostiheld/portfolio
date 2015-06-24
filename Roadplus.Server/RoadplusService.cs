using System;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Collections.Generic;

using Roadplus.Server.API;
using Roadplus.Server.Communication;
using Roadplus.Server.Communication.Http;
using Roadplus.Server.Data;
using Roadplus.Server.Commands.Json;


namespace Roadplus.Server
{
    public class RoadplusService
    {
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

            roadLinkService = new RoadLinkManager(
                new CommandProcessorText(),
                settings.BaudRate,
                settings.RoadDetectTimeOut);

            channels.Add(roadLinkService);

            CommandProcessorJson processorJson = new CommandProcessorJson();
            processorJson.RegisteredCommands.AddRange(
            new ICommand[]
            {
                new CreateZoneCommand(),
                new CreateSchoolCommand(),
                new CreateRoadConstructionCommand(),
                new EdgeSetCommand(),

                new GetZonesCommand(),
                new GetSchoolsCommand(),
                new GetRoadconstructionsCommand(),

                new RemoveZoneCommand(),
                new RemoveSchoolCommand(),
                new RemoveRoadConstructionCommand(),

                new ConnectRoadToZoneCommand(roadLinkService),
                new GetConnectedRoadsCommand(roadLinkService),

                new GetMapCommand()
            });

            websocketService = new WSSessionManager(
                new IPEndPoint(
                    settings.IP,
                    settings.Port),
                processorJson);

            channels.Add(websocketService);
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

