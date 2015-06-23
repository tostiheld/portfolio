using System;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LinqToDB;

using Roadplus.Server.API;
using Roadplus.Server.Data;

namespace Roadplus.Server.Commands.Json
{
    public class RemoveZoneCommand : ICommand
    {
        public string Name { get { return "removeZone"; } }

        private const string ZoneIdKey = "zoneId";

        public IResponse Execute(string payload)
        {
            JObject o = JsonConvert.DeserializeObject<JObject>(payload);
            int id;

            if (!Int32.TryParse(o[ZoneIdKey].ToString(), out id))
            {
                return new ResponseFailure(Name, "Int32 parse error");
            }

            RoadplusData data = new RoadplusData();

            data.Zones.Where(z => z.ZoneId == id).Delete();

            return new RemoveResponse()
            {
                Command = Name,
                RemovedObjectId = id
            };
        }
    }
}

