using System;
using System.Collections.Generic;

namespace Roadplus.Server.API
{
    public class MessageExchange
    {
        public event EventHandler<NewResponseEventArgs> NewResponse;
        public event EventHandler<NewActivityEventArgs> NewActivity;

        private ActivityValidator Validator;
        private List<IFormatHandler> Formats;

        public MessageExchange(ActivityValidator validator)
        {
            Validator = validator;
            Formats = new List<IFormatHandler>();
        }

        private void Channel_NewMessage(object sender, MessageFoundEventArgs e)
        {
            IFormatHandler handler = GetHandler(e.FoundMessage.Format);
            if (handler == null)
            {
                Post(QuickFailure(
                    "Unsupported format",
                    e.FoundMessage.SourceAddress));
                return;
            }

            Activity newActivity = null;

            if (handler.TryParse(e.FoundMessage, out newActivity))
            {
                if ((newActivity.Type == ActivityType.Identify &&
                    newActivity.SourceType == LinkType.Unidentified ) ||
                    Validator.IsAllowed(newActivity))
                {
                    OnNewActivity(newActivity);
                }
                else
                {
                    Post(QuickFailure(
                        "Activity not allowed",
                        e.FoundMessage.SourceAddress));
                }
            }
            else
            {
                Post(QuickFailure(
                    "Error parsing message",
                    e.FoundMessage.SourceAddress));
            }
        }

        private void OnNewActivity(Activity activity)
        {
            if (NewActivity != null &&
                activity != null)
            {
                NewActivityEventArgs e = 
                    new NewActivityEventArgs(activity);
                NewActivity(this, e);
            }
        }

        private Response QuickFailure(string message, string address)
        {
            Response response = new Response(
                address);
            response.Type = ResponseType.Failure;
            response.Message = message;
            return response;
        }

        private Response QuickInfo(string message, string address)
        {
            Response response = new Response(
                address);
            response.Type = ResponseType.Information;
            response.Message = message;
            return response;
        }

        private IFormatHandler GetHandler(string format)
        {
            foreach (IFormatHandler f in Formats)
            {
                if (f.MessageFormat == format)
                {
                    return f;
                }
            }

            return null;
        }

        public void Register(Channel channel)
        {
            channel.MessageFound += Channel_NewMessage;
            Formats.Add(channel.MessageFormatter);
        }

        public void Post(Response response)
        {
            if (response == null)
            {
                throw new ArgumentNullException("response");
            }

            if (NewResponse != null)
            {
                NewResponseEventArgs e = 
                    new NewResponseEventArgs(response);
                NewResponse(this, e);
            }
        }
    }
}

