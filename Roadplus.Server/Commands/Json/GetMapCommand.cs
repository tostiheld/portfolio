using System;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LinqToDB;

using Roadplus.Server.API;
using Roadplus.Server.Data;

namespace Roadplus.Server.Commands.Json
{
    public class GetMapCommand : ICommand
    {
        public string Name { get { return "requestMap"; } }

        private const string ZoneIdKey = "zoneId";

        public IResponse Execute(string payload)
        {
            JObject o = JsonConvert.DeserializeObject<JObject>(payload);

            int id;

            if (!Int32.TryParse(o[ZoneIdKey].ToString(), out id))
            {
                return new ResponseFailure(Name, "Parse error");
            }

            RoadplusData data = new RoadplusData();

            return new GetMapResponse()
            {
                ZoneId = id,
                Vertices = data.Vertices.Where(v => v.ZoneId == id).ToArray(),
                Edges = data.Edges.Where(e => e.ZoneId == id).ToArray()
            };
        }
    }
}

