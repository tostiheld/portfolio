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
                { MessageTypes.CreateZone, CreateZoneCallback },
                { MessageTypes.Disconnect, DisconnectCallback },
                { MessageTypes.GetRoads, GetRoadsCallback },
                { MessageTypes.ConnectRoadToZone, ConnectRoadZoneCallback },
                { MessageTypes.SetRoadSign, SetSignCallback },
                { MessageTypes.Temperature, GetTempCallback }
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

        private void RemoveZoneCallback(Message message)
        {
            if (message.MessageSource.Type == SourceTypes.UI)
            {
                WSSession session = FindSessionByIP(
                    message.MessageSource.IP);
                if (session != null)
                {

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
                            id.ToString() + Message.MessageSplit);
                        tosend.DataType = "zone";
                        session.Send(tosend);
                    }
                    else
                    {
                        Message tosend = new Message(
                            source,
                            MessageTypes.Failure, 
                            "Zone with id " + id.ToString() + " already exists");
                        session.Send(tosend);
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
                            Message tosend = new Message(
                                source,
                                MessageTypes.Acknoledge,
                                msg);
                            tosend.DataType = "schools";
                            session.Send(tosend);
                        }
                    }
                }
            }
        }

        private void GetRoadsCallback(Message message)
        {
            if (message.MessageSource.Type == SourceTypes.UI)
            {
                WSSession session = FindSessionByIP(
                    message.MessageSource.IP);
                if (session != null)
                {
                    string[] ports = SerialPort.GetPortNames();
                    List<string> workingports = new List<string>();
                    foreach (string s in ports)
                    {
                        try
                        {
                            using (SerialPort p = new SerialPort(s))
                            {
                                p.Open();
                                p.Close();
                                workingports.Add(s);
                            }
                        }
                        catch
                        {
                            // port is not working
                        }
                    }

                    string reply = "";
                    foreach (string s in workingports)
                    {
                        reply += s + ":";
                    }

                    Message tosend = new Message(
                        source,
                        MessageTypes.Acknoledge,
                        reply);
                    tosend.DataType = "ports";
                    session.Send(tosend);
                }
            }
        }

        private void ConnectRoadZoneCallback(Message message)
        {
            if (message.MessageSource.Type == SourceTypes.UI)
            {
                WSSession session = FindSessionByIP(
                    message.MessageSource.IP);
                if (session != null)
                {
                    try
                    {
                        RoadCommunication roadcom = new RoadCommunication(message.MetaData[1]);
                        int id;
                        if (!Int32.TryParse(message.MetaData[0], out id))
                        {
                            Message tosend = new Message(
                                source,
                                MessageTypes.Failure,
                                "Illegal data");
                            session.Send(tosend);
                            return;
                        }

                        Zone zone = GetZoneByID(id);
                        if (zone != null)
                        {
                            zone.Road = roadcom;
                            logStream.WriteLine(
                                String.Format(
                                "Session at {0} connected road at {1} to zone with id {2}",
                                session.IP.ToString(),
                                roadcom.PortName,
                                zone.ID.ToString()));

                            Message tosend = new Message(
                                source,
                                MessageTypes.Acknoledge,
                                roadcom.PortName);
                            tosend.DataType = "roadconnected";
                            session.Send(tosend);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex is IOException ||
                            ex is ArgumentException || 
                            ex is UnauthorizedAccessException)
                        {
                            Message tosend = new Message(
                                source,
                                MessageTypes.Failure,
                                "Port is invalid, in use or does not exist");
                            session.Send(tosend);
                        }

                        throw;
                    }
                }
            }
        }

        private void DisconnectCallback(Message message)
        {
            WSSession session = FindSessionByIP(message.MessageSource.IP);
            session.End();
        }

        private void SetSignCallback(Message message)
        {
            if (message.MessageSource.Type == SourceTypes.UI)
            {
                WSSession session = FindSessionByIP(
                    message.MessageSource.IP);
                if (session != null)
                {
                    int id;
                    int speed;
                    if (!Int32.TryParse(message.MetaData[0], out speed) ||
                        !Int32.TryParse(message.MetaData[1], out id))
                    {
                        Message tosend = new Message(
                            source,
                            MessageTypes.Failure,
                            "Illegal data");
                        session.Send(tosend);
                        return;
                    }

                    Zone zone = GetZoneByID(id);

                    if (zone == null)
                    {
                        Message tosend = new Message(
                            source,
                            MessageTypes.Failure,
                            "Zone not found");
                        session.Send(tosend);
                        return;
                    }

                    zone.SetSign(speed);

                    Message toSend = new Message(
                        source,
                        MessageTypes.Acknoledge);
                    toSend.DataType = "sign";
                    session.Send(toSend);
                }
            }
        }

        private void GetTempCallback(Message message)
        {
            if (message.MessageSource.Type == SourceTypes.UI)
            {
                WSSession session = FindSessionByIP(
                    message.MessageSource.IP);
                if (session != null)
                {
                    int id;
                    if (!Int32.TryParse(message.MetaData[0], out id))
                    {
                        Message tosend = new Message(
                            source,
                            MessageTypes.Failure,
                            "Illegal data");
                        session.Send(tosend);
                        return;
                    }

                    Zone zone = GetZoneByID(id);

                    if (zone == null)
                    {
                        Message tosend = new Message(
                            source,
                            MessageTypes.Failure,
                            "Zone not found");
                        session.Send(tosend);
                        return;
                    }

                    zone.GetTemp();

                    session.Send(new Message(source, MessageTypes.Acknoledge));
                }
            }
        }
	}
}
