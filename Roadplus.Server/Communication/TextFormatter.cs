using System;

using Roadplus.Server.API;

namespace Roadplus.Server.Communication
{
    public class TextFormatter : IFormatter
    {
        public string Format(IResponse toformat)
        {
            throw new NotImplementedException();
        }

        public string Format(IRequest toformat)
        {
            string request = String.Format(">{0}:", toformat.Command);

            foreach (string s in toformat.Payload)
            {
                request += s + ":";
            }
            request += ";";

            return request;
        }
    }
}

