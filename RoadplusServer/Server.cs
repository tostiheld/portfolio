using System;
using System.IO;
using System.Net;
using System.Collections.Generic;

using ProtoBuf;

using Roadplus.Server.Communication;
using Roadplus.Server.Map;

namespace Roadplus.Server
{
    public partial class Server
    {
        public bool IsRunning { get; private set; }

        private WSService service;
        private HttpService httpService;
        private StreamWriter logStream;
        private List<WSSession> sessions;
        private List<Zone> zones;

        public Server(IPEndPoint endpoint, StreamWriter logstream)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException("endpoint");
            }

            AttachCallbacks();

            logStream = logstream;
            if (Settings.EnableHttp)
            {
                httpService = new HttpService(Settings.HttpServiceUrl);
            }
            service = new WSService(endpoint);
            service.NewSession += Service_NewSession;
        }

        private void Service_NewSession(object sender, NewSessionEventArgs e)
        {
            foreach (WSSession s in sessions)
            {
                if (s.IP == e.Session.IP)
                {
                    sessions.Remove(s);
                }
            }

            sessions.Add(e.Session);
            e.Session.MessageReceived += Session_MessageReceived;
            e.Session.Disconnected += Session_Disconnected;
            logStream.WriteLine("New session with " + e.Session.IP.ToString());
        }

        private void Session_Disconnected(object sender, EventArgs e)
        {
            if (sender is WSSession)
            {
                WSSession session = sender as WSSession;
                logStream.WriteLine("Client at " + session.IP.ToString() + " disconnected");
                sessions.Remove(session);
            }
        }

        private void Session_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Action<Message> callback;
            if (MessageCallbacks.TryGetValue(
                e.Received.Command,
                out callback))
            {
                callback(e.Received);
            }
            else
            {
                logStream.WriteLine(
                    "Message does not have a callback: " + e.Received.ToString());
            }
        }

        private void LoadZones()
        {
            try
            {
                throw new FileNotFoundException();
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

        private Zone GetZoneByID(int id)
        {
            foreach (Zone z in zones)
            {
                if (z.ID == id)
                {
                    return z;
                }
            }

            return null;
        }

        public void Start()
        {
            if (!IsRunning)
            {
                logStream.WriteLine("Starting server... ");

                LoadZones();
                if (Settings.EnableHttp)
                {
                    httpService.Run();
                }
                sessions = new List<WSSession>();
                service.Start();
                IsRunning = true;

                logStream.WriteLine("Server started.");
            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                logStream.WriteLine("Stopping server... ");

                foreach (Zone z in zones)
                {
                    if (z.Road != null)
                    {
                        z.Road.Stop();
                    }
                }

                // first ensure nobody is coming in anymore...
                service.Stop();

                // ...then kick everyone out.
                Message msg = new Message(CommandType.ServerOffline);
                // clone list because we're going to modify the other one
                List<WSSession> temp = new List<WSSession>(sessions);
                foreach (WSSession session in temp)
                {
                    session.Send(msg);
                    session.End();
                }

                if (Settings.EnableHttp)
                {
                    httpService.Stop();
                }
                SaveZones();
                IsRunning = false;

                logStream.WriteLine("Server stopped.");
            }
        }
    }
}

