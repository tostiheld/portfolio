using System;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Threading;
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
                { CommandType.Disconnect, DisconnectCallback },

                { CommandType.RoadDirect, RoadDirectCallback },

                { CommandType.Unknown, UnknownCallback }
            };
        }

        private void RoadDirectCallback(Message message)
        {
            if (message.MessageSource.Type == SourceTypes.UI)
            {
                try
                {
                    string roadpayload = "";
                    for (int i = 3; i < message.Payload.Length; i++)
                    {
                        roadpayload += message.Payload[i] + ":";
                    }
                    string port = message.Payload[0];
                    int rate = Convert.ToInt32(message.Payload[1]);
                    string roadstring = 
                        Message.MessageStart + 
                        message.Payload[2] +
                        Message.MessageSplit +
                        roadpayload +
                        Message.MessageTerminator;

                    Message toRoad;
                    if (Message.TryParse(roadstring, out toRoad))
                    {
                        Thread sendThread = new Thread(new ThreadStart(delegate {
                            using (SerialPort sPort = new SerialPort(port, rate, Parity.None, 8, StopBits.One))
                            {
                                sPort.Open();
                                Thread.Sleep(4000);
                                byte[] bytes = Encoding.ASCII.GetBytes(
                                    toRoad.ToString("ascii"));
                                sPort.Write(bytes, 0, bytes.Length);
                                Thread.Sleep(1000);
                                sPort.Close();
                            }
                        }));
                        sendThread.Start();
                    }
                    else
                    {
                        TrySendFailure(
                            message.MessageSource.IP,
                            "Command parse error");
                    }

                    WriteLineLog(
                        String.Format(
                        "Manual override from {0} to {1}. Command: {2}",
                        message.MessageSource.IP,
                        port,
                        toRoad.ToString("ascii")));
                    TrySendMessage(
                        message.MessageSource.IP,
                        new Message(
                        CommandType.Acknoledge,
                        new string[] { port }, "override"));
                }
                catch (Exception ex)
                {
                    TrySendFailure(
                        message.MessageSource.IP,
                        ex.Message);
                }
            }
        }

        private void UnknownCallback(Message message)
        {
            string payloadstring = "";
            foreach (string s in message.Payload)
            {
                payloadstring = s + ":";
            }

            TrySendFailure(
                message.MessageSource.IP,
                "Command " + message.PayloadType + " unkown. " + payloadstring);
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
                            WriteLineLog(
                                "Session at " + session.IP + " identified as UI");
                            break;
                        case "car":
                            session.SourceType = SourceTypes.Car;
                            WriteLineLog(
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
            if (message.MessageSource.Type == SourceTypes.UI)
            {
                try
                {
                    switch (message.Payload[0].ToLower())
                    {
                        case "ports":
                            string[] portnames = SerialPort.GetPortNames();
                            Message toSend = new Message(
                                CommandType.Acknoledge,
                                portnames,
                                "ports");
                            TrySendMessage(
                                message.MessageSource.IP,
                                toSend);
                            break;
                        default:
                            TrySendFailure(
                                message.MessageSource.IP,
                                "No get callback for " + message.Payload[0]);
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
        
        private void SetCallback(Message message)
        {
            if (message.MessageSource.Type == SourceTypes.UI)
            {
                try
                {
                    switch (message.Payload[0].ToLower())
                    {
                        default:
                            TrySendFailure(
                                message.MessageSource.IP,
                                "No set callback for " + message.Payload[0]);
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

        private void CreateCallback(Message message)
        {
            if (message.MessageSource.Type == SourceTypes.UI)
            {
                try
                {
                    switch (message.Payload[0].ToLower())
                    {
                        case "zone":
                            CreateZone(message);
                            break;
                        case "school":
                            CreateSchool(message);           
                            break;
                        case "roadconstruction":
                            CreateRoadConstruction(message);
                            break;
                        default:
                            TrySendFailure(
                                message.MessageSource.IP,
                                "No create callback for " + message.Payload[0]);
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
            if (message.MessageSource.Type == SourceTypes.UI)
            {
                try
                {
                    switch (message.Payload[0].ToLower())
                    {
                        case "zone":
                            RemoveZone(message);
                            break;
                        case "school":
                            RemoveSchool(message);
                            break;
                        case "roadconstruction":
                            RemoveRoadConstruction(message);
                            break;
                        default:
                            TrySendFailure(
                                message.MessageSource.IP,
                                "No remove callback for " + message.Payload[0]);
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
        
        private void EditCallback(Message message)
        {
            if (message.MessageSource.Type == SourceTypes.UI)
            {
                try
                {
                    switch (message.Payload[0].ToLower())
                    {
                        default:
                            TrySendFailure(
                                message.MessageSource.IP,
                                "No edit callback for " + message.Payload[0]);
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

        private void DisconnectCallback(Message message)
        {
            WSSession session = FindSessionByIP(message.MessageSource.IP);
            session.End();
        }
	}
}
