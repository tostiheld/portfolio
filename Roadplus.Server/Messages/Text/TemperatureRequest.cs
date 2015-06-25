using System;

using Roadplus.Server.API;

namespace Roadplus.Server.Messages.Text
{
    public class TemperatureRequest : IRequest
    {
        #region IRequest implementation
        public string Command
        {
            get
            {
                return "14";
            }
        }
        public string[] Payload
        {
            get
            {
                return new string[0];
            }
        }
        #endregion
        
    }
}

