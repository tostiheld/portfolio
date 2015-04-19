using System;
using Roadplus.Server.EntityManagement;

namespace Roadplus.Server.Communication.Protocol
{
    public abstract class FormatHandler : IFormatProvider, ICustomFormatter
    {
        public string MessageFormat { get; private set;}

        public FormatHandler(string messageformat)
        {
            MessageFormat = messageformat;
        }

        protected abstract string FormatResponse(Response toformat);
        public abstract bool TryParse(Message value, out Activity result);

        #region IFormatProvider implementation

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
            {
                return this;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region ICustomFormatter implementation

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (format != MessageFormat)
            {
                throw new NotSupportedException("Wrong formatter");
            }

            else if (arg is Response)
            {
                return FormatResponse(arg as Response);
            }
            else
            {
                throw new NotImplementedException(
                    "This formatter is only meant to format Response types");
            }
        }

        #endregion


    }
}

