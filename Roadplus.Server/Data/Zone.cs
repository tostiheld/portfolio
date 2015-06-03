using System;
using LinqToDB.Mapping;
using System.Linq;

namespace Roadplus.Server.Data
{
    [Table("Zone")]
    public class Zone
    {
        [PrimaryKey, Identity]
        public int ZoneId { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("StartVertex")]
        public int StartVertexId { get; set; }

        [Column("RadarVertex")]
        public int RadarVertexId { get; set; }

        public Vertex StartVertex
        {
            get
            {
                RoadplusData data = new RoadplusData();
                return data.Vertices.FirstOrDefault(
                    v => v.VertexId == StartVertexId);
            }
        }

        public Vertex RadarVertex
        {
            get
            {
                RoadplusData data = new RoadplusData();
                return data.Vertices.FirstOrDefault(
                    v => v.VertexId == RadarVertexId);
            }
        }
    }
}

