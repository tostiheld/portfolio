using System;
using LinqToDB.Mapping;
using System.Linq;

using Newtonsoft.Json;

namespace Roadplus.Server.Data
{
    [Table("Vertex"), JsonObject]
    public class Vertex
    {
        [PrimaryKey, Identity]
        public int VertexId { get; set; }

        [Column("ZoneId"), NotNull]
        public int ZoneId { get; set; }

        [Column("X"), NotNull]
        public int X { get; set; }

        [Column("Y"), NotNull]
        public int Y { get; set; }

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
    }
}

