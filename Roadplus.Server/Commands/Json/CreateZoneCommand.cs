using System;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LinqToDB;

using Roadplus.Server.API;
using Roadplus.Server.Data;

namespace Roadplus.Server.Commands.Json
{
    public class CreateZoneCommand : ICommand
    {
        private const string NameKey = "name";

        public string Name { get { return "createZone"; } }

        public IResponse Execute(string payload)
        {
            JObject json = JsonConvert.DeserializeObject<JObject>(payload);
            string name = json[NameKey].ToString();

            Vertex start = new Vertex()
            {
                X = 0,
                Y = 0
            };

            RoadplusData data = new RoadplusData();
            int startId = Convert.ToInt32(
                data.InsertWithIdentity<Vertex>(start));

            Zone newZone = new Zone()
            {
                Name = name,
                StartVertexId = startId,
                RadarVertexId = 0
            };

            int newZoneId = Convert.ToInt32(
                data.InsertWithIdentity<Zone>(newZone));

            Zone createdZone = data.Zones.First<Zone>(
                z => z.ZoneId == newZoneId);

            int id = Convert.ToInt32(json["id"]);
            CreateResponse response = new CreateResponse()
            {
                ID = id,
                CreatedObject = createdZone
            };

            return response;
        }
    }
}

