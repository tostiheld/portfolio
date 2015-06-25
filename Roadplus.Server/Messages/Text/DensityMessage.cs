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
                int density;
                string strDensity = payload.Split(new char[] {':'}, StringSplitOptions.RemoveEmptyEntries)[0];

                if (!Int32.TryParse(strDensity, out density))
                {
                    return null;
                }

                JToken temp;
                int maxSpeed = 100;

                if (density > 40)
                {
                    maxSpeed = 30;
                }

                if (File.Exists(JsonLocation))
                {
                    json = JsonConvert.DeserializeObject<JObject>(
                        File.ReadAllText(JsonLocation));
                }
                else
                {
                    json = new JObject();
                }

                if (json.TryGetValue("maxspeed", out temp))
                {
                    json.Remove("maxspeed");
                }

                json.Add(new JProperty("maxspeed", maxSpeed));
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

