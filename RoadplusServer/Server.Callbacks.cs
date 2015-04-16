using System;
using System.Text;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;

using Roadplus.Server.Communication;
using Roadplus.Server.Map;

namespace Roadplus.Server
{
	public partial class Server
	{
        private Dictionary<MessageTypes, Action<Message>> MessageCallbacks;

        private WSSession FindSessionByIP(IPAddress ip)
        {
            foreach (WSSession session in sessions)
            {
                if (session.IP == ip)
                {
                    return session;
                }
            }

            return null;
        }

        private void AttachCallbacks()
        {
            MessageCallbacks = new Dictionary<MessageTypes, Action<Message>>()
            {
                { MessageTypes.Identification, IdentificationCallback },
                { MessageTypes.GetSchools, GetSchoolsCallback },
                { MessageTypes.CreateZone, CreateZoneCallback }
            };
        }

        private void IdentificationCallback(Message message)
        {
            if (message.MessageSource.Type == SourceTypes.Unidentified)
            {
                WSSession session = FindSessionByIP(
                    message.MessageSource.IP);
                if (session != null)
                {
                    switch (message.MetaData[0].ToLower())
                    {
                        case "ui":
                            session.SourceType = SourceTypes.UI;
                            logStream.WriteLine(
                                "Session at " + session.IP + " identified as UI");
                            break;
                        case "car":
                            session.SourceType = SourceTypes.Car;
                            logStream.WriteLine(
                                "Session at " + session.IP + " identified as car");
                            break;
                        default:
                            // do nothing
                            break;
                    }
                }
            }
        }

        private void CreateZoneCallback(Message message)
        {
            if (message.MessageSource.Type == SourceTypes.UI)
            {
                WSSession session = FindSessionByIP(
                    message.MessageSource.IP);
                if (session != null)
                {
                    int id;
                    int x;
                    int y;
                    if (!Int32.TryParse(message.MetaData[0], out id) ||
                        !Int32.TryParse(message.MetaData[1], out x) ||
                        !Int32.TryParse(message.MetaData[2], out y))
                    {
                        logStream.WriteLine("Illegal CZONE message from " + session.IP);
                        Message tosend = new Message(
                            source,
                            MessageTypes.Failure,
                            "Illegal data");
                        session.Send(tosend);
                        return;
                    }

                    if (GetZoneByID(id) == null)
                    {
                        Vertex start = new Vertex(new Point(x, y));
                        Zone newzone = new Zone(start, id);
                        zones.Add(newzone);

                        logStream.WriteLine(
                            "Session at " + session.IP.ToString() + " created new zone with id " + id.ToString());
                        Message tosend = new Message(
                            source,
                            MessageTypes.Acknoledge,
                            "zone:" + id.ToString());
                        session.Send(tosend);
                    }
                    else
                    {
                        Message tosend = new Message(
                            source,
                            MessageTypes.Failure, 
                            "Zone with id " + id.ToString() + " already exists");
                        session.Send(message);
                    }
                }
            }
        }

        private void GetSchoolsCallback(Message message)
        {
            if (message.MessageSource.Type == SourceTypes.UI)
            {
                WSSession session = FindSessionByIP(
                    message.MessageSource.IP);
                if (session != null)
                {
                    int id;
                    if (Int32.TryParse(message.MetaData[0], out id))
                    {
                        Zone zone = GetZoneByID(id);
                        if (zone != null)
                        {
                            DataContractJsonSerializer json = 
                                new DataContractJsonSerializer(typeof(List<School>));

                            byte[] data;
                            using (MemoryStream ms = new MemoryStream())
                            {
                                json.WriteObject(ms, zone.Schools);
                                data = ms.ToArray();
                            }

                            logStream.WriteLine(
                                String.Format("Sending schools from zone {0} to {1}",
                                              id.ToString(),
                                              session.IP.ToString()));

                            UTF8Encoding encoder = new UTF8Encoding();
                            string msg = encoder.GetString(data);
                            session.Send(msg);
                        }
                    }
                }
            }
        }
	}
}
