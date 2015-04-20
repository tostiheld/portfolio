using System;
using Roadplus.Server.EntityManagement;

namespace Roadplus.Server.Communication.Protocol
{
    public abstract class FormatHandler
    {
        public string MessageFormat { get; private set;}

        public FormatHandler(string messageformat)
        {
            MessageFormat = messageformat;
        }

        protected abstract string FormatResponse(Response toformat);
        public abstract bool TryParse(Message value, out Activity result);

        public string Format(Response toformat)
        {
            if (toformat == null)
            {
                throw new ArgumentNullException("toformat");
            }

            return FormatResponse(toformat);
        }
    }
}

