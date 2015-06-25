using System;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Roadplus.Server.API;
using Roadplus.Server.Communication;

namespace Roadplus.Server.Messages.Text
{
    public class DensityMessage : ICommand
    {
        private string JsonLocation;
        private RoadLinkManager Roads;

        public DensityMessage(string jsonlocation, RoadLinkManager roads)
        {
            if (jsonlocation == null ||
                roads == null)
            {
                throw new ArgumentNullException();
            }

            JsonLocation = jsonlocation;
            Roads = roads;
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
                int MaxSpeed = 99;
                string warning = "NoWarning";
                TrafficSignRequest request = new TrafficSignRequest();
                TrafficSignRequest warningrequest = new TrafficSignRequest();

                if (density > 40)
                {
                    MaxSpeed = 30;
                    warning = "Warning";
                }

                request.Payload = new string[]{ MaxSpeed.ToString() };
                warningrequest.Payload = new string[]{ warning };

                Roads.Broadcast(request);
                Roads.Broadcast(warningrequest);

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

                json.Add(new JProperty("maxspeed", MaxSpeed));
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

