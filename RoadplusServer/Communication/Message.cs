using System;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;

namespace Roadplus.Server.Communication
{
    [DataContract]
    public class Message
    {
        public const char MessageStart = '>';
        public const char MessageSplit = ':';
        public const char MessageTerminator = ';';

        [DataMember(Name="command")]
        public CommandType Command { get; private set; }
        [DataMember(Name="payload")]
        public string[] Payload { get; private set; }
        [DataMember(Name="payloadtype")]
        public string PayloadType { get; set; }
        public Source MessageSource { get; set; }

        public Message(CommandType command)
            : this (command, null, "") { }

        public Message(CommandType command, string payloadtype)
            : this(command, null, payloadtype) { }

        public Message(CommandType command, string[] payload)
            : this(command, payload, "") { }

        public Message(CommandType command, string[] payload, string payloadtype)
        {
            if (payloadtype == null)
            {
                throw new ArgumentNullException("payloadtype");
            }

            Command = command;
            Payload = payload;
            PayloadType = payloadtype;
        }

        /// <summary>
        /// Tries to parse a message from ascii or json format
        /// </summary>
        /// <returns>
        /// True if parsing succeeded, falso if not
        /// </returns>
        /// <param name="input">The string to construct a message from</param>
        /// <param name="output">The message that was found</param>
        public static bool TryParse(string input, out Message output)
        {
            if (input.StartsWith(MessageStart.ToString()))
            {
                string clean = input.Trim(MessageStart, MessageTerminator);
                string[] parts = clean.Split(MessageSplit);
                if (parts.Length >= 1)
                {
                    string cmdstr = parts[0].ToUpper();
                    CommandType command;
                    if (Settings.Messages.TryGetValue(cmdstr, out command))
                    {
                        List<string> payload = new List<string>();
                        foreach (string s in parts)
                        {
                            if (s.ToUpper() != cmdstr &&
                                s != "")
                            {
                                payload.Add(s);
                            }
                        }
                        output = new Message(command, payload.ToArray());
                        return true;
                    }
                }
            }
            else
            {
                try
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(input);
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        DataContractJsonSerializer json = 
                            new DataContractJsonSerializer(typeof(Message));
                        output = json.ReadObject(ms) as Message;
                        return true;
                    }
                }
                catch
                {
                    output = null;
                    return false;
                }
            }

            output = null;
            return false;
        }

        public override string ToString()
        {
            return ToString("ascii");
        }

        /// <summary>
        /// Builds a the string representation of the message object 
        /// according to the format string.
        /// </summary>
        /// <param name="format">Valid formats: 'json' or 'ascii'</param>
        /// <returns>The message string</returns>
        public string ToString(string format)
        {
            switch (format)
            {
                case "json":
                    using (MemoryStream ms = new MemoryStream())
                    {
                        DataContractJsonSerializer json = 
                            new DataContractJsonSerializer(typeof(Message));
                        json.WriteObject(ms, this);
                        byte[] bytes = ms.ToArray();
                        string returnval = Encoding.UTF8.GetString(bytes);
                        return returnval;
                    }
                case "ascii":
                default:
                    string payload = "";
                    foreach (string s in Payload)
                    {
                        payload += s + MessageSplit;
                    }
                    string command = "NULL";
                    foreach (KeyValuePair<string, CommandType> pair in Settings.Messages)
                    {
                        if (pair.Value == Command)
                        {
                            command = pair.Key;
                        }
                    }
                    return MessageStart + command + 
                           MessageSplit + payload + MessageTerminator;
            }
        }
    }
}

