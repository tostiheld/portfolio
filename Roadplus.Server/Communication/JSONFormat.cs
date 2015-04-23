using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Roadplus.Server.API;

namespace Roadplus.Server.Communication
{
    public class JSONFormat : IFormatHandler
    {
        public string MessageFormat
        {
            get
            {
                return "json";
            }
        }

        #region IFormatHandler implementation

        public bool TryParse(RawMessage value, out Activity result)
        {
            throw new NotImplementedException();
        }

        public string Format(Response toformat)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

