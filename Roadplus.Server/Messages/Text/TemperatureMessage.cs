using System;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Roadplus.Server.API;

namespace Roadplus.Server.Messages.Text
{
    public class TemperatureMessage : ICommand
    {
        public string Name { get { return "TEMP"; } }

        private string JsonLocation;

        public TemperatureMessage(string jsonlocation)
        {
            if (jsonlocation == null)
            {
                throw new ArgumentNullException();
            }

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
                double temperature;
                string strTemp = payload.Split(new char[] {':'}, StringSplitOptions.RemoveEmptyEntries)[0];

                if (!Double.TryParse(strTemp, out temperature))
                {
                    return null;
                }

                if (json.TryGetValue("temperature", out temp))
                {
                    json.Remove("temperature");
                }

                json.Add(new JProperty("temperature", temperature));
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

    }
}

