using System;

namespace Roadplus.Server.API
{
    public class ResponseStop : IResponse
    {
        #region IResponse implementation

        public string ResponseString
        {
            get
            {
                return "server-closing";
            }
        }

        public string Command
        {
            get
            {
                return "server-closing";
            }
            set{ }
        }

        #endregion
    }
}

