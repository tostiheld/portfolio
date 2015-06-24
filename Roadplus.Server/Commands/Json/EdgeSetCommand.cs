using System;
using System.Linq;

using Roadplus.Server.API;
using Roadplus.Server.Data;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using LinqToDB;

namespace Roadplus.Server.Commands.Json
{
    public class EdgeSetCommand : ICommand
    {
        private const string ZoneIdKey = "zoneId";
        private const string startVertexXKey = "startVertexX";
        private const string startVertexYKey = "startVertexY";
        private const string endVertexXKey = "endVertexX";
        private const string endVertexYKey = "endVertexY";

        public string Name { get { return "createEdgeSet"; } }

        public IResponse Execute(string payload)
        {
            JObject o = JsonConvert.DeserializeObject<JObject>(payload);

            int zoneId, startVertexX, startVertexY, endVertexX, endVertexY;

            if (!Int32.TryParse(o[ZoneIdKey].ToString(), out zoneId) ||
                !Int32.TryParse(o[startVertexXKey].ToString(), out startVertexX) ||
                !Int32.TryParse(o[startVertexYKey].ToString(), out startVertexY) ||
                !Int32.TryParse(o[endVertexXKey].ToString(), out endVertexX) ||
                !Int32.TryParse(o[endVertexYKey].ToString(), out endVertexY))
            {
                return new ResponseFailure(
                    Name,
                    "Int32 Parse error");
            }

            RoadplusData data = new RoadplusData();

            if (!data.Zones.Any(z => z.ZoneId == zoneId))
            {
                return new ResponseFailure(
                    Name,
                    "Zone does not exist");
            }

            Vertex startVertex = new Vertex()
            {
                    ZoneId = zoneId,
                    X = startVertexX,
                    Y = startVertexY
            };
            
            Vertex endVertex = new Vertex()
            {
                    ZoneId = zoneId,
                    X = endVertexX,
                    Y = endVertexY
            };

            int startVertexId = Convert.ToInt32(
                data.InsertWithIdentity<Vertex>(startVertex));

            int endVertexId = Convert.ToInt32(
                data.InsertWithIdentity<Vertex>(endVertex));

            startVertex.VertexId = startVertexId;
            endVertex.VertexId = endVertexId;

            Edge newEdge = new Edge()
            {
                    ZoneId = zoneId,
                    StartVertexId = startVertexId,
                    EndVertexId = endVertexId,
                    Weight = 0.0,
                    MaxSpeed = 0
            };

            int edgeId = Convert.ToInt32(
                data.InsertWithIdentity<Edge>(newEdge));

            newEdge.EdgeId = edgeId;

            return new EdgeSetResponse()
            {
                ZoneId = zoneId,
                CreatedEdge = newEdge,
                StartVertex = startVertex,
                EndVertex = endVertex
            };
        }
    }
}

