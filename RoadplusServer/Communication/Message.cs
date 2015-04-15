using System;
using System.Collections.Generic;

namespace Roadplus.Server.Communication
{
    public class Message
    {
        public const char MessageStart = '>';
        public const char MessageSplit = ':';
        public const char MessageTerminator = ';';

        public MessageTypes MessageType { get; private set; }
        public string MetaData { get; private set; }
        public Source MessageSource { get; private set; }

        public Message(MessageTypes type)
            : this(type, "") { }

        public Message(MessageTypes type, string metadata)
        {
            if (metadata == null)
            {
                metadata = "";
            }

            MessageType = type;
            MetaData = metadata;
        }

        /// <summary>
        /// Constructs a message from the string against the 
        /// dictionary of known messages
        /// </summary>
        /// <returns>
        /// A message if the string was found, or null if 
        /// nothing was found.
        /// </returns>
        /// <param name="input">The string to construct a message from</param>
        /// <param name="metadata">Extra data</param>
        public static Message FromString(string input, string metadata)
        {
            MessageTypes type;
            if (Settings.Messages.TryGetValue(input, out type))
            {
                return new Message(type, metadata);
            }

            return null;
        }

        /// <summary>
        /// Constructs a message from the string against the 
        /// dictionary of known messages
        /// </summary>
        /// <returns>
        /// A message if the string was found, or null if 
        /// nothing was found.
        /// </returns>
        /// <param name="input">The string to construct a message from</param>
        public static Message FromString(string input)
        {
            return FromString(input, "");
        }

        /// <summary>
        /// Builds a the string representation of the message object 
        /// as specified in the communication protocol
        /// </summary>
        /// <returns>The message string</returns>
        public override string ToString()
        {
            string type = "NULL";
            foreach (KeyValuePair<string, MessageTypes> pair in Settings.Messages)
            {
                if (MessageType == pair.Value)
                {
                    type = pair.Key;
                }
            }

            string format = MessageStart + "{0}" + MessageSplit + 
                            "{1}" + MessageTerminator;
            return String.Format(format, type, MetaData);
        }
    }
}

