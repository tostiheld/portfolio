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

        public int ID
        {
            get
            {
                return -1;
            }
            set{ }
        }

        #endregion
    }
}

