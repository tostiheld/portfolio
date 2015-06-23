using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWeek15
{
    class MessageBuilder
    {
        private String messageBeginMarker;
        private String messageEndMarker;
        private String bufferedData;

        public MessageBuilder(String messageBeginMarker, String messageEndMarker)
        {
            if (messageBeginMarker == null)
            {
                throw new ArgumentNullException("messageBeginMarker");
            }

            if (messageEndMarker == null)
            {
                throw new ArgumentNullException("messageEndMarker");
            }

            this.messageBeginMarker = messageBeginMarker;
            this.messageEndMarker = messageEndMarker;

            bufferedData = "";
        }

        /// <summary>
        /// Add the data to the end of the buffered data.
        /// </summary>
        /// <param name="data">
        /// The data to add to the builder for later parsing. 
        /// If data == null, nothing is added.
        /// </param>
        public void Append(String data)
        {
            if (data != null)
            {
                bufferedData += data;
            }
        }

        /// <summary>
        /// Find and remove the next (complete) message
        /// from the buffered data (including delimiters).
        /// The beginning and the end of the message are marked 
        /// by 'messageBeginMarker' and 'messageEndMarker' 
        /// (see member variables of this class).
        /// </summary>
        /// <returns>
        /// The next (complete) message (including markers), 
        /// or null if no message was found.
        /// </returns>
        public String FindAndRemoveNextMessage()
        {
            int beginIndex = bufferedData.IndexOf(messageBeginMarker);
            if (beginIndex != -1)
            {
                int endIndex = bufferedData.IndexOf(messageEndMarker, beginIndex);
                if (endIndex != -1)
                {
                    String foundMessage = bufferedData.Substring(
                        beginIndex, (endIndex - beginIndex) + 1);
                    bufferedData = bufferedData.Substring(endIndex + 1);
                    return foundMessage;
                }
            }
            return null;
        }

        /// <summary>
        /// Clear all buffered data
        /// </summary>
        public void Clear()
        {
            bufferedData = "";
        }
    }
}
