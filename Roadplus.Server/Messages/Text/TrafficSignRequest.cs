using System;

using Roadplus.Server.API;

namespace Roadplus.Server.Messages.Text
{
    public class TrafficSignRequest : IRequest
    {
        public string Command
        {
            get
            {
                return "15";
            }
        }

        public string[] Payload
        {
            get;
            set;
        }
    }
}

