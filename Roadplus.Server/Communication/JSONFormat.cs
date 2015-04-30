using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Roadplus.Server.API;
using Roadplus.Server.Traffic;

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

        private Dictionary<Type, string> IdentifyTypes(string payloadjson)
        {
            IDictionary <string, JToken> jsondata = JObject.Parse(payloadjson);
            Dictionary<Type, string> output = new Dictionary<Type, string>();

            foreach (KeyValuePair<string, JToken> pair in jsondata)
            {
                Type targetType = Type.GetType(pair.Key);
                if (targetType != null ||
                    UtilityMethods.TryFindType(pair.Key, out targetType))
                {
                    output.Add(targetType, pair.Value.ToString());
                }
                else
                {
                    // TODO: bit of a hack, make better
                    throw new JsonReaderException("Invalid type encountered");
                }
            }

            return output;
        }

        public Dictionary<Type, object> ParseParameters(Dictionary<Type, string> payload, ActivityType activity)
        {
            Dictionary<Type, object> output = new Dictionary<Type, object>();

            foreach (KeyValuePair<Type, string> pair in payload)
            {
                if (activity == ActivityType.Get)
                {
                    int tmp = 0;
                    if (Int32.TryParse(pair.Value, out tmp))
                    {
                        output.Add(pair.Key, tmp);
                    }
                    else
                    {
                        output.Add(pair.Key, pair.Value.ToString());
                    }
                }
                else if (activity == ActivityType.Identify)
                {
                    if (pair.Key == typeof(LinkType))
                    {
                        LinkType type = 
                            JsonConvert.DeserializeObject<LinkType>(pair.Value);
                        output.Add(pair.Key,
                                   type);
                    }
                    else
                    {
                        throw new JsonReaderException("Invalid type while identifying");
                    }
                }
            }

            return output;
        }

        #region IFormatHandler implementation

        public bool TryParse(RawMessage value, out Activity result)
        {
            result = null;

            try
            {
                JObject json = JObject.Parse(value.Content);

                ActivityType type = ActivityType.Unknown;
                if (Enum.TryParse(json["type"].ToString(), out type))
                {
                    Dictionary<Type, string> payload = IdentifyTypes(
                        json["payload"].ToString());

                    Dictionary<Type, object> parameters = ParseParameters(
                        payload, type);

                    result = new Activity(
                        type,
                        value.SourceAddress,
                        value.SourceType);
                    result.Payload = parameters;
                    return true;
                }
            }
            catch (JsonReaderException ex)
            {
                return false;
            }

            return false;

        }

        public string Format(Response toformat)
        {
            try
            {
                // will this work? lawl
                return JsonConvert.SerializeObject(toformat);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}

