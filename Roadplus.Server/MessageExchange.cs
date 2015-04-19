using System;
using System.Collections.Generic;

using Roadplus.Server.Communication.Protocol;
using Roadplus.Server.EntityManagement;

namespace Roadplus.Server
{
    public class MessageExchange
    {
        public event EventHandler<NewResponseEventArgs> NewResponse;
        public event EventHandler<NewActivityEventArgs> NewActivity;

        public MessageExchange()
        {
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

        public void Post(Activity activity)
        {
            if (activity == null)
            {
                throw new ArgumentNullException("activity");
            }

            if (NewActivity != null)
            {
                NewActivityEventArgs e =
                    new NewActivityEventArgs(activity);
                NewActivity(this, e);
            }
        }
    }
}

