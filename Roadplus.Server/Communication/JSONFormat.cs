using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;

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
            result = null;

            try
            {
                JObject json = JObject.Parse(value.Content);

                ActivityType type = ActivityType.Unknown;
                if (Enum.TryParse(json["type"].ToString(), out type))
                {
                    JObject payload = JObject.Parse(
                        json["payload"].ToString());

                    IList<string> keys = payload.Properties()
                        .Select(p => p.Name)
                        .ToList();

                    List<Type> targetTypes = new List<Type>();
                    List<object> parameters = new List<object>();
                    foreach (string s in keys)
                    {
                        Type targetType = Type.GetType(s);
                        if (targetType != null)
                        {
                            targetTypes.Add(targetType);

                            // woahh im tired brb
                            throw new NotImplementedException();
                        }
                    }

                    result = new Activity(
                        type,
                        value.SourceAddress,
                        value.SourceType);
                    result.Payload = parameters.ToArray();
                    result.TargetTypes = targetTypes;

                    return true;
                }
            }
            catch (Exception ex)
            {
                // wtf 
                // what exceptions does this throw
                // nowhere to be found omg
                throw new NotImplementedException();
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

