using System;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Collections.Generic;

using Roadplus.Server.API;
using Roadplus.Server.Communication;
using Roadplus.Server.Communication.Http;
using Roadplus.Server.Data;
using Roadplus.Server.Messages.Json;
using Roadplus.Server.Messages.Text;

namespace Roadplus.Server
{
    public class RoadplusService
    {
        public ZoneChecker Checker { get; set; }

        private WSSessionManager websocketService;
        private RoadLinkManager roadLinkService;
        private HttpService httpService;

        private List<Channel> channels;

        public RoadplusService(Settings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            else if (!Directory.Exists(settings.HttpRoot))
            {
                Directory.CreateDirectory(settings.HttpRoot);
            }

            IPAddress ip = IPAddress.Parse(settings.IP);

            channels = new List<Channel>();

            if (settings.EnableHttp)
            {
                httpService = new HttpService(
                    new IPEndPoint(
                    ip,
                    settings.HttpPort),
                    settings.HttpRoot);
            }

            roadLinkService = new RoadLinkManager(
                new CommandProcessorText(),
                new TextFormatter(),
                settings.BaudRate,
                settings.RoadDetectTimeOut);

            roadLinkService.CommandProcessor.RegisteredCommands.AddRange(
            new ICommand[]
            {
                new TemperatureMessage(Path.Combine(settings.HttpRoot, "data.json")),
                    new DensityMessage(Path.Combine(settings.HttpRoot, "data.json"), roadLinkService)
            }); 

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

                new ConnectRoadToZoneCommand(roadLinkService, this),
                new GetConnectedRoadsCommand(roadLinkService),

                new GetMapCommand()
            });

            websocketService = new WSSessionManager(
                new IPEndPoint(
                    ip,
                    settings.Port),
                processorJson,
                new JsonFormatter());

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

