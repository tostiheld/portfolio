using System;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LinqToDB;

using Roadplus.Server.API;
using Roadplus.Server.Data;
using System.Globalization;

namespace Roadplus.Server.Commands.Json
{
    public class CreateRoadConstructionCommand : ICommand
    { 
        public string Name { get { return "createRoadconstruction"; } }

        private const string NameKey = "name";
        private const string ZoneIdKey = "zoneId";
        private const string LocationKey = "location";
        private const string StartDateKey = "startDate";
        private const string EndDateKey = "endDate";

        public IResponse Execute(string payload)
        {
            JObject o = JsonConvert.DeserializeObject<JObject>(payload);

            int zoneId;
            int locationId;
            DateTime startDate = DateTime.ParseExact(
                                     o[StartDateKey].ToString(),
                                     "yyyy-MM-dd",
                                     CultureInfo.InvariantCulture);
            DateTime endDate = DateTime.ParseExact(
                                   o[EndDateKey].ToString(), 
                                   "yyyy-MM-dd",
                                   CultureInfo.InvariantCulture);

            if (!Int32.TryParse(o[ZoneIdKey].ToString(), out zoneId) ||
                !Int32.TryParse(o[LocationKey].ToString(), out locationId))
            {
                return new ResponseFailure(Name, "Parse error");
            }

            RoadplusData data = new RoadplusData();

            if (!data.Zones.Any(z => z.ZoneId == zoneId))
            {
                return new ResponseFailure(Name, "Zone does not exist");
            }
            else if (!data.Edges.Any(e => e.EdgeId == locationId))
            {
                return new ResponseFailure(Name, "Edge does not exist");
            }

            RoadConstruction newRC = new RoadConstruction()
            {
                    Name = o[NameKey].ToString(),
                    ZoneId = zoneId,
                    EdgeId = locationId,
                    DateStart = startDate,
                    DateEnd = endDate
            };

            int id = Convert.ToInt32(
                data.InsertWithIdentity<RoadConstruction>(newRC));

            newRC.RoadConstructionId = id;

            return new CreateResponse()
            {
                Command = Name,
                CreatedObject = newRC
            };
        }
    }
}

