using System;

using Newtonsoft.Json;

using Roadplus.Server.API;

namespace Roadplus.Server.Communication
{
    public class JsonFormatter : IFormatter
    {
        public string Format(IResponse toformat)
        {
            return JsonConvert.SerializeObject(toformat);
        }

        public string Format(IRequest toformat)
        {
            return JsonConvert.SerializeObject(toformat);
        }
    }
}

