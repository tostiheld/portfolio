using System;
using System.Collections.Generic;

using Roadplus.Server.API;

namespace Roadplus.Server.Communication
{
    public class PlainTextFormat : IFormatHandler
    {
        public const char MessageStart = '>';
        public const char MessageSplit = ':';
        public const char MessageTerminator = ';';

        public string MessageFormat
        {
            get
            {
                return "text";
            }
        }

        public PlainTextFormat()
        { }

        #region IFormatHandler implementation

        public bool TryParse(RawMessage value, out Activity result)
        {
            if (!value.Content.StartsWith(MessageStart.ToString()) ||
                !value.Content.EndsWith(MessageTerminator.ToString()) ||
                !value.Content.Contains(MessageSplit.ToString()))
            {
                // invalid format
                result = null;
                return false;
            }

            string stripped = value.Content.TrimStart(MessageStart);
            stripped = stripped.TrimEnd(MessageTerminator);
            stripped = stripped.TrimEnd(MessageSplit);

            string[] parts = stripped.Split(MessageSplit);
            List<object> parameters = new List<object>();
            ActivityType type;
            if (!Enum.TryParse(parts[0], out type))
            {
                type = ActivityType.Unknown;
            }
            if (parts.Length > 1)
            {
                for (int i = 1; i < parts.Length; i++)
                {
                    int parameter;
                    if (Int32.TryParse(parts[i], out parameter))
                    {
                        parameters.Add(parameter);
                    }
                    else
                    {
                        parameters.Add(parts[i]);
                    }
                }
            }

            result = new Activity(
                type,
                value.SourceAddress,
                value.SourceType);
            //result.Payload = parameters.ToArray();
            return true;
        }

        public string Format(Response toformat)
        {
            if (toformat == null)
            {
                throw new ArgumentNullException("toformat");
            }

            string type = toformat.Type.ToString("G");
            string parameters = "";
            if (toformat.Payload != null &&
                toformat.Payload.Length > 0)
            {
                foreach (object o in toformat.Payload)
                {
                    parameters += o.ToString() + MessageSplit;
                }
            }

            return MessageStart + type + MessageSplit + parameters + MessageTerminator;
        }

        #endregion
    }
}
