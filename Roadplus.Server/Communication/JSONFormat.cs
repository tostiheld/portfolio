using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using Roadplus.Server.Communication.Protocol;
using Roadplus.Server.EntityManagement;

namespace Roadplus.Server.Communication
{
    public class JSONFormat : FormatHandler
    {
        public JSONFormat()
            : base("json")
        { }

        #region implemented abstract members of FormatHandler

        protected override string FormatResponse(Response toformat)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                try
                {
                    DataContractJsonSerializer json = 
                        new DataContractJsonSerializer(typeof(Response));
                    json.WriteObject(ms, toformat);
                    byte[] bytes = ms.ToArray();
                    string returnval = Encoding.UTF8.GetString(bytes);
                    return returnval;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public override bool TryParse(Message value, out Activity result)
        {
            result = null;

            if (value.Format != MessageFormat)
            {
                return false;
            }

            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(value.Content);
                JSONMessage output = null;
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    DataContractJsonSerializer json = 
                        new DataContractJsonSerializer(typeof(JSONMessage));
                    output = json.ReadObject(ms) as JSONMessage;
                    return true;
                }

                if (output != null)
                {
                    List<object> parameters = new List<object>();

                    foreach (string s in output.Parameters)
                    {
                        int parameter;
                        if (Int32.TryParse(s, out parameter))
                        {
                            parameters.Add(parameter);
                        }
                        else
                        {
                            parameters.Add(s);
                        }
                    }

                    result = new Activity(
                        output.Type,
                        value.From,
                        parameters.ToArray());
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (ex is InvalidDataContractException ||
                    ex is SerializationException)
                {
                    return false;
                }

                throw;
            }
        }

        #endregion
    }
}

