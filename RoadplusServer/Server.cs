using System;
using System.IO;
using System.Net;
using System.Collections.Generic;

using ProtoBuf;

using Roadplus.Server.Communication;
using Roadplus.Server.Map;

namespace Roadplus.Server
{
    public class Server
    {
        public bool IsRunning { get; private set; }

        private IPEndPoint endPoint;
        private WSService service;
        private StreamWriter logStream;
        private List<WSSession> sessions;
        private List<Zone> zones;

        public Server(IPEndPoint endpoint, StreamWriter logstream)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException("endpoint");
            }

            logStream = logstream;
            service = new WSService(endpoint);
        }

        private void Mockups()
        {
            Vertex start = new Vertex(new Point(0, 0));
            zones.Add(new Zone(start));
            start.Connect(new Point(20, 20));
        }

        private void LoadZones()
        {
            try
            {
                using (FileStream fs = new FileStream(Settings.ZoneFilePath,
                                                      FileMode.Open))
                {
                    zones = Serializer.Deserialize<List<Zone>>(fs);
                }

                logStream.WriteLine("Zones loaded.");
            }
            catch (FileNotFoundException)
            {
                zones = new List<Zone>();
                logStream.WriteLine("No zones file found, so new a collection was created.");
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        private void SaveZones()
        {
            try
            {
                using (FileStream fs = new FileStream(Settings.ZoneFilePath,
                                                      FileMode.Create))
                {
                    Serializer.Serialize<List<Zone>>(fs, zones);
                }

                logStream.WriteLine("Zones saved to file.");
            }
            catch(Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public void Start()
        {
            if (!IsRunning)
            {
                logStream.WriteLine("Starting server... ");

                LoadZones();
                sessions = new List<WSSession>();
                service.Start();
                IsRunning = true;

                Mockups();

                logStream.WriteLine("Server started.");
            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                logStream.WriteLine("Stopping server... ");

                // first ensure nobody is coming in anymore...
                service.Stop();

                // ...then kick everyone out.
                Message msg = new Message(MessageTypes.ServerOffline);
                foreach (WSSession session in sessions)
                {
                    session.Send(msg);
                    session.End();
                }

                SaveZones();
                IsRunning = false;

                logStream.WriteLine("Server stopped.");
            }
        }
    }
}

