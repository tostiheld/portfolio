using System;
using Roadplus.Server.Communication.Protocol;

namespace Roadplus.Server
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

