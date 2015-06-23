using System;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LinqToDB;

using Roadplus.Server.API;
using Roadplus.Server.Communication;
using Roadplus.Server.Data;

namespace Roadplus.Server.Commands.Json
{
    public class ConnectRoadToZoneCommand : ICommand
    {
        public string Name { get { return "connectRoad"; } }

        private const string ZoneIdKey = "zoneId";
        private const string PortNameKey = "roadPort";

        private RoadLinkManager Source;

        public ConnectRoadToZoneCommand(RoadLinkManager source)
        {
            if (source == null)
            {
                throw new ArgumentNullException();
            }

            Source = source;
        }

        public IResponse Execute(string payload)
        {
            JObject o = JsonConvert.DeserializeObject<JObject>(payload);

            int id;

            if (!Int32.TryParse(o[ZoneIdKey].ToString(), out id))
            {
                return new ResponseFailure(Name, "Parse error");
            }

            if (!Source.ConnectedPorts.Contains(o[PortNameKey].ToString()))
            {
                return new ResponseFailure(Name, "Port not available");
            }

            RoadplusData data = new RoadplusData();
            Zone target;
            try
            {
                target = data.Zones.Single(z => z.ZoneId == id);
            }
            catch (InvalidOperationException)
            {
                return new ResponseFailure(Name, "Zone not found");
            }

            target.ArduinoPort = o[PortNameKey].ToString();

            data.Update<Zone>(target);

            return new ConnectRoadToZoneResponse()
            {
                ZoneId = id,
                RoadPort = o[PortNameKey].ToString()
            };
        }

    }
}

