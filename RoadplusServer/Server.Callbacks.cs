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

                { CommandType.Disconnect, DisconnectCallback },


                { CommandType.Unknown, UnknownCallback }
            };
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
            if (message.MessageSource.Type == SourceTypes.UI)
            {
                try
                {
                    switch (message.Payload[0])
                    {
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
                    switch (message.Payload[0])
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
                    switch (message.Payload[0])
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
                    switch (message.Payload[0])
                    {
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
                    switch (message.Payload[0])
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
