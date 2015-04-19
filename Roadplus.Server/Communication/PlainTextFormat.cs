using System;
using System.Collections.Generic;

using Roadplus.Server.Communication.Protocol;
using Roadplus.Server.EntityManagement;

namespace Roadplus.Server.Communication
{
    public class PlainTextFormat : FormatHandler
    {
        public const char MessageStart = '>';
        public const char MessageSplit = ':';
        public const char MessageTerminator = ';';

        public PlainTextFormat()
            : base("text")
        { }

        #region implemented abstract members of FormatHandler

        protected override string FormatResponse(Response toformat)
        {
            string type = toformat.Type.ToString("G");
            string parameters = "";
            foreach (string s in toformat.Content)
            {
                parameters += s + MessageSplit;
            }

            return MessageStart + type + MessageSplit + parameters + MessageTerminator;

        }

        public override bool TryParse(Message value, out Activity result)
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
                value.From,
                parameters.ToArray());
            return true;
        }

        #endregion
    }
}
