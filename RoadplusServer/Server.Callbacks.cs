using System;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;

using Roadplus.Server.Communication;
using Roadplus.Server.Map;

namespace Roadplus.Server
{
	public partial class Server
	{
        private Dictionary<CommandType, Action<Message>> MessageCallbacks;

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
            MessageCallbacks = new Dictionary<CommandType, Action<Message>>()
            {
                { CommandType.Identification, IdentificationCallback },
                { CommandType.Get, GetCallback },
                { CommandType.Set, SetCallback },
                { CommandType.Create, CreateCallback },
                { CommandType.Remove, RemoveCallback },
                { CommandType.Edit, EditCallback },

                { CommandType.Disconnect, DisconnectCallback }

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
                    switch (message.Payload[0].ToLower())
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

        private bool TrySendFailure(IPAddress to, string reason)
        {
            return TrySendMessage(
                        to, 
                        new Message(
                            CommandType.Failure,
                            new string[] { reason }));
        }

        private bool TrySendMessage(IPAddress to, Message message)
        {
            WSSession session = FindSessionByIP(to);

            if (session != null)
            {
                session.Send(message);
                return true;
            }

            return false;
        }

        private void GetCallback(Message message)
        {
        }
        
        private void SetCallback(Message message)
        {

        }

        private void CreateCallback(Message message)
        {
            if (message.MessageSource.Type == SourceTypes.UI)
            {
                try
                {
                    int id = Convert.ToInt32(message.Payload[1]);

                    switch (message.Payload[0])
                    {
                        case "zone":
                            int x = Convert.ToInt32(message.Payload[2]);
                            int y = Convert.ToInt32(message.Payload[3]);

                            if (GetZoneByID(id) != null)
                            {
                                TrySendFailure(
                                    message.MessageSource.IP,
                                    "Zone with id " + id.ToString() + " already exists");
                                return;
                            }

                            Vertex start = new Vertex(new Point(x, y));
                            Zone zone = new Zone(start, id);
                            zones.Add(zone);

                            logStream.WriteLine(
                                "Session at {0} created a zone with id {1}",
                                message.MessageSource.IP.ToString(),
                                id.ToString());
                            Message success = new Message(
                                CommandType.Acknoledge,
                                new string[] { id.ToString() },
                                "zone");
                            TrySendMessage(
                                message.MessageSource.IP,
                                success);

                            break;
                        case "school":
                            int sid = Convert.ToInt32(message.Payload[2]);
                            string name = message.Payload[3];
                            int hStart = Convert.ToInt32(message.Payload[4].Split('-')[0]);
                            int mStart = Convert.ToInt32(message.Payload[4].Split('-')[1]);
                            int hEnd = Convert.ToInt32(message.Payload[5].Split('-')[0]);
                            int mEnd = Convert.ToInt32(message.Payload[5].Split('-')[1]);

                            Zone target = GetZoneByID(id);
                            if (target == null)
                            {
                                TrySendFailure(
                                    message.MessageSource.IP,
                                    "Zone with id " + id.ToString() + " not found");
                                return;
                            }

                            if (target.GetSchoolByID(sid) != null)
                            {
                                TrySendFailure(
                                    message.MessageSource.IP,
                                    String.Format(
                                        "School with id {0} in zone {1} already exists",
                                        sid.ToString(),
                                        id.ToString()));
                                return;
                            }

                            DateTime sTime = new DateTime(1, 1, 1, hStart, mStart, 0);
                            DateTime eTime = new DateTime(1, 1, 1, hEnd, mEnd, 0);
                            School school = new School(
                                new TimeRange(sTime, eTime),
                                sid);
                            school.Name = name;
                            target.Schools.Add(school);

                            logStream.WriteLine(
                                String.Format(
                                "Session at {0} added school with id {1} to zone {2}",
                                message.MessageSource.IP.ToString(),
                                sid.ToString(),
                                id.ToString()));
                            success = new Message(
                                CommandType.Acknoledge,
                                new string[] { sid.ToString() },
                                "school");
                            TrySendMessage(
                                message.MessageSource.IP,
                                success);

                            break;
                        case "roadconstruction":
                            int rid = Convert.ToInt32(message.Payload[2]);


                            break;
                    }
                }
                catch (Exception ex)
                {
                    TrySendFailure(
                        message.MessageSource.IP,
                        ex.Message);
                }
            }
        }
        
        private void RemoveCallback(Message message)
        {

        }
        
        private void EditCallback(Message message)
        {

        }

        private void DisconnectCallback(Message message)
        {
            WSSession session = FindSessionByIP(message.MessageSource.IP);
            session.End();
        }
	}
}
