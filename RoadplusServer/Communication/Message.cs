using System;

namespace Roadplus.Server.Communication
{
    public class Message
    {
        public MessageTypes MessageType { get; private set; }
        public string MetaData { get; private set; }

        public Message(MessageTypes type)
            : this(type, "") { }

        public Message(MessageTypes type, string metadata)
        {
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
            foreach (string s in Settings.Messages.Keys)
            {
                if (input == s)
                {
                    return new Message(Settings.Messages[s], metadata);
                }
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
    }
}

