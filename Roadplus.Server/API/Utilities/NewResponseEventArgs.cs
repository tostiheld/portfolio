using System;

namespace Roadplus.Server.API
{
    public class NewResponseEventArgs : EventArgs
    {
        public Response NewResponse { get; private set; }

        public NewResponseEventArgs(Response response) : base()
        {
            NewResponse = response;
        }
    }
}

