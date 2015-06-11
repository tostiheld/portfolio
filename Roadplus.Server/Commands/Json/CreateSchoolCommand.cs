using System;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LinqToDB;

using Roadplus.Server.API;
using Roadplus.Server.Data;

namespace Roadplus.Server.Commands.Json
{
    public class CreateSchoolCommand : ICommand
    {
        public string Name { get { return "createSchool"; } }

        public IResponse Execute(string payload)
        {
            JObject json = JsonConvert.DeserializeObject<JObject>(payload);
            int requestId = Convert.ToInt32(json["id"]);
            int zoneId = Convert.ToInt32(json["zoneId"]);
            int location = Convert.ToInt32(json["location"]);

            RoadplusData data = new RoadplusData();
            if (!data.Zones.Any<Zone>(z => z.ZoneId == zoneId))
            {
                return new ResponseFailure(
                    requestId,
                    "Specified zone does not exist");
            }
            else if (!data.Vertices.Any<Vertex>(v => v.VertexId == location))
            {
                return new ResponseFailure(
                    requestId,
                    "Specified vertex does not exist");
            }

            DateTime openTime = Convert.ToDateTime(json["openTime"]);
            DateTime closeTime = Convert.ToDateTime(json["closeTime"]);

            School newSchool = new School()
            {
                ZoneId = zoneId,
                VertexId = location,
                OpenTime = openTime,
                CloseTime = closeTime
            };

            int schoolId = Convert.ToInt32(
                data.InsertWithIdentity<School>(newSchool));

            newSchool.SchoolId = schoolId;

            CreateResponse response = new CreateResponse()
            {
                ID = requestId,
                CreatedObject = newSchool
            };

            return response;
        }
    }
}

