using System;

using Roadplus.Server.API;

namespace Roadplus.Server.Messages.Text
{
    public class TrafficDensityRequest : IRequest
    {
        public string Command
        {
            get
            {
                return "12";
            }
        }

        public string[] Payload
        {
            get
            {
                return new string[] { "Read" };
            }
        }
    }
}

