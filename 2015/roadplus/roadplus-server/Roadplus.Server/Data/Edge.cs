using System;
using LinqToDB.Mapping;
using System.Linq;
using Newtonsoft.Json;

namespace Roadplus.Server.Data
{
    [Table("Edge"), JsonObject]
    public class Edge
    {
        [PrimaryKey, Identity]
        public int EdgeId { get; set; }

        [Column("ZoneId"), NotNull]
        public int ZoneId { get; set; }

        [Column("StartVertex"), NotNull]
        public int StartVertexId { get; set; }

        [Column("EndVertex"), NotNull]
        public int EndVertexId { get; set; }

        [Column("Weight")]
        public Nullable<double> Weight { get; set; }

        [Column("MaxSpeed")]
        public int MaxSpeed { get; set; }

        [JsonIgnore]
        public Zone Parent
        {
            get
            {
                RoadplusData data = new RoadplusData();
                return data.Zones.FirstOrDefault(
                    z => z.ZoneId == ZoneId);
            }
        }

        [JsonIgnore]
        public Vertex StartVertex
        {
            get
            {
                RoadplusData data = new RoadplusData();
                return data.Vertices.FirstOrDefault(
                    v => v.VertexId == StartVertexId);
            }
        }

        [JsonIgnore]
        public Vertex EndVertex
        {
            get
            {
                RoadplusData data = new RoadplusData();
                return data.Vertices.FirstOrDefault(
                    v => v.VertexId == EndVertexId);
            }
        }
    }
}

