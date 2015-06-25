using System;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LinqToDB;

using Roadplus.Server.API;
using Roadplus.Server.Data;

namespace Roadplus.Server.Messages.Json
{
    public class GetMapCommand : ICommand
    {
        public string Name { get { return "requestMap"; } }

        private const string ZoneIdKey = "zoneId";

        public IResponse Execute(string payload)
        {
            RoadplusData data = new RoadplusData();

            return new GetMapResponse()
            {
                Vertices = data.Vertices.ToArray(),
                Edges = data.Edges.ToArray()
            };
        }
    }
}

