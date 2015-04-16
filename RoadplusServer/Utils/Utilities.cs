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
        public static Message ProcessMessages(ref string buffer)
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

                    Message returnval;
                    if (Message.TryParse(msg, out returnval))
                    {
                        return returnval;
                    }
                }
            }

            return null;
        }
    }
}

