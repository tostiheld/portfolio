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
    public class CreateSchoolCommand : ICommand
    {
        public string Name { get { return "createSchool"; } }

        private const string NameKey = "name";
        private const string ZoneIdKey = "zoneId";
        private const string LocationKey = "location";
        private const string OpenTimeKey = "openTime";
        private const string CloseTimekey = "closeTime";

        public IResponse Execute(string payload)
        {
            JObject o = JsonConvert.DeserializeObject<JObject>(payload);

            int zoneId;
            int locationId;
            DateTime openTime;
            DateTime closeTime;

            if (!Int32.TryParse(o[ZoneIdKey].ToString(), out zoneId) ||
                !Int32.TryParse(o[LocationKey].ToString(), out locationId) ||
                !DateTime.TryParseExact(
                    o[OpenTimeKey].ToString(), "HH:mm", CultureInfo.InvariantCulture, 
                    DateTimeStyles.None, out openTime) ||
                !DateTime.TryParseExact(
                    o[CloseTimekey].ToString(), "HH:mm", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out closeTime))
            {
                return new ResponseFailure(Name, "Parse error");
            }

            RoadplusData data = new RoadplusData();

            if (!data.Vertices.Any(v => v.VertexId == locationId) ||
                !data.Zones.Any(z => z.ZoneId == zoneId))
            {
                return new ResponseFailure(Name, "Nonexisting parent");
            }

            School newSchool = new School()
            {
                Name = o[NameKey].ToString(),
                ZoneId = zoneId,
                VertexId = locationId,
                OpenTime = openTime,
                CloseTime = closeTime
            };

            int id = Convert.ToInt32(
                data.InsertWithIdentity<School>(newSchool));

            newSchool.SchoolId = id;

            return new CreateResponse()
            {
                Command = Name,
                CreatedObject = newSchool
            };
        }
    }
}

