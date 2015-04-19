using System;
using System.Text;
using System.IO;
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
                DataContractJsonSerializer json = 
                    new DataContractJsonSerializer(typeof(Message));
                json.WriteObject(ms, toformat);
                byte[] bytes = ms.ToArray();
                string returnval = Encoding.UTF8.GetString(bytes);
                return returnval;
            }
        }

        public override bool TryParse(Message value, out Activity result)
        {
            try
            {
                throw new NotImplementedException();

                /*
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    DataContractJsonSerializer json = 
                        new DataContractJsonSerializer(typeof(Message));
                    output = json.ReadObject(ms) as Message;
                    return true;
                }*/
            }
            catch (Exception ex)
            {
                if (ex is InvalidDataContractException ||
                    ex is SerializationException)
                {
                    result = null;
                    return false;
                }

                throw;
            }
        }

        #endregion
    }
}

