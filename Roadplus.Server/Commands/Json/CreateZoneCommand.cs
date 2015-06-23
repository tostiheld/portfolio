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

            RoadplusData data = new RoadplusData();

            Zone newZone = new Zone()
            {
                Name = name,
                StartVertexId = 0,
                RadarVertexId = 0
            };

            int newZoneId = Convert.ToInt32(
                data.InsertWithIdentity<Zone>(newZone));

            Vertex startVertex = new Vertex()
            {
                X = 0,
                Y = 0,
                ZoneId = newZoneId
            };

            int startVertexId = Convert.ToInt32(
                data.InsertWithIdentity<Vertex>(startVertex));

            newZone.ZoneId = newZoneId;
            newZone.StartVertexId = startVertexId;

            data.InsertOrReplace<Zone>(newZone);

            Zone createdZone = data.Zones.First<Zone>(
                z => z.ZoneId == newZoneId);
            
            CreateResponse response = new CreateResponse()
            {
                Command = Name,
                CreatedObject = createdZone
            };

            return response;
        }
    }
}

