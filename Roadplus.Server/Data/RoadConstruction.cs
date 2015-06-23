using System;
using LinqToDB.Mapping;
using System.Linq;
using Newtonsoft.Json;

namespace Roadplus.Server.Data
{
    [Table("RoadConstruction"), JsonObject]
    public class RoadConstruction
    {
        [PrimaryKey, Identity]
        public int RoadConstructionId { get; set; }

        [Column("ZoneId"), NotNull]
        public int ZoneId { get; set; }

        [Column("EdgeId"), NotNull]
        public int EdgeId { get; set; }

        [Column("DateStart"), NotNull]
        public DateTime DateStart { get; set; }

        [Column("DateEnd"), NotNull]
        public DateTime DateEnd { get; set; }

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
        public Edge LocationEdge
        {
            get
            {
                RoadplusData data = new RoadplusData();
                return data.Edges.FirstOrDefault(
                    e => e.EdgeId == EdgeId);
            }
        }
    }
}

