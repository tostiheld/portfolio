using System;
using System.Collections.Generic;

using Roadplus.Server.Communication.Protocol;

namespace Roadplus.Server.EntityManagement
{
    public class ActivityFactory
    {
        private List<FormatHandler> formats;
        private MessageExchange messageExchange;

        public ActivityFactory(MessageExchange exchange)
        {
            formats = new List<FormatHandler>();
            messageExchange = exchange;
        }

        private void Channel_MessageFound(object sender, MessageFoundEventArgs e)
        {
            FormatHandler handler = null;
            foreach (FormatHandler format in formats)
            {
                if (e.FoundMessage.Format == format.MessageFormat)
                {
                    handler = format;
                    break;
                }
            }

            Activity newActivity;
            if (handler != null &&
                handler.TryParse(e.FoundMessage, out newActivity))
            {
                messageExchange.Post(newActivity);
            }
        }

        public void Register(FormatHandler handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }

            foreach (FormatHandler f in formats)
            {
                if (f.MessageFormat == handler.MessageFormat)
                {
                    return;
                }
            }

            formats.Add(handler);
        }

        public void Register(Channel channel)
        {
            channel.MessageFound += Channel_MessageFound;
        }
    }
}

