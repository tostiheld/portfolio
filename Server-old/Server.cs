using System;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Collections.Generic;

using ProtoBuf;

using Roadplus.Server.Communication;
using Roadplus.Server.Map;

namespace Roadplus.Server
{
    public partial class Server
    {
        public bool IsRunning { get; private set; }

        private WSSessionManager websockets;
        private HttpService httpService;
        private ZoneManager zoneManager;

        public Server(IPEndPoint endpoint)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException("endpoint");
            }

            if (Settings.EnableHttp)
            {
                httpService = new HttpService(Settings.HttpServiceUrl);
            }

            websockets = new WSSessionManager(endpoint);
            zoneManager = new ZoneManager();
        }

        public void Start()
        {
            if (!IsRunning)
            {
                Trace.WriteLine("Starting server... ");

                zoneManager.Load();

                if (Settings.EnableHttp)
                {
                    httpService.Run();
                }
                websockets.Start();
                IsRunning = true;

                Trace.WriteLine("Server started.");
            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                Trace.WriteLine("Stopping server... ");

                websockets.Stop();

                if (Settings.EnableHttp)
                {
                    httpService.Stop();
                }

                zoneManager.Save();
                IsRunning = false;

                Trace.WriteLine("Server stopped.");
            }
        }
    }
}

