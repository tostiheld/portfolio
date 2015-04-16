using System;
using Roadplus.Server.Communication;

namespace Roadplus.Server
{
    public static class Utilities
    {
        /// <summary>
        /// Only processes new string messages.
        /// Expects messages in simple ASCII protocol format.
        /// </summary>
        /// <returns>
        /// A message if one is found, null if nothing is found
        /// </returns>
        public static Message ProcessMessages(Source source, string buffer)
        {
            int start = buffer.IndexOf(Message.MessageStart);
            if (start != -1)
            {
                int end = buffer.IndexOf(Message.MessageTerminator);
                if (end != -1)
                {
                    string msg = buffer.Substring(
                        start, (end - start) + 1);
                    buffer = buffer.Substring(end + 1);

                    string cmd = msg.Substring(1, 4);
                    string data = msg.Substring(4, msg.Length - 6);

                    return Message.FromString(source, cmd, data);
                }
            }

            return null;
        }
    }
}

