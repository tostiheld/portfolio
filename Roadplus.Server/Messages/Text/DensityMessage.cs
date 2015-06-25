using System;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Roadplus.Server.API;

namespace Roadplus.Server.Messages.Text
{
    public class DensityMessage : ICommand
    {
        private string JsonLocation;

        public DensityMessage(string jsonlocation)
        {
            JsonLocation = jsonlocation;
        }

        public IResponse Execute(string payload)
        {
            JObject json;

            try
            {
                if (File.Exists(JsonLocation))
                {
                    json = JsonConvert.DeserializeObject<JObject>(
                        File.ReadAllText(JsonLocation));
                }
                else
                {
                    json = new JObject();
                }

                JToken temp;
                int density;
                string strDensity = payload.Split(new char[] {':'}, StringSplitOptions.RemoveEmptyEntries)[0];

                if (!Int32.TryParse(strDensity, out density))
                {
                    return null;
                }

                if (json.TryGetValue("density", out temp))
                {
                    json.Remove("density");
                }

                json.Add(new JProperty("density", density));
                File.WriteAllText(JsonLocation, json.ToString());
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while writing JSON: " + ex.Message);
                return null;
            }

            return null;
        }

        public string Name
        {
            get
            {
                return "dens";
            }
        }
    }
}

