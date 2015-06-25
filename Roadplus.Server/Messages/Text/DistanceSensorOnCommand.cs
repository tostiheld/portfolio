using System;

using Roadplus.Server.API;

namespace Roadplus.Server.Messages.Text
{
    public class DistanceSensorOnCommand : IRequest
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
                return new string[] { "On" };
            }
        }
    }
}

