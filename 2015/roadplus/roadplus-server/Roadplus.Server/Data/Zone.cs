using System;
using LinqToDB.Mapping;
using Newtonsoft.Json;

using System.Linq;

namespace Roadplus.Server.Data
{
    [Table("Zone"), JsonObject]
    public class Zone
    {
        [PrimaryKey, Identity]
        public int ZoneId { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("StartVertex")]
        public int StartVertexId { get; set; }

        [Column("RadarVertex"), JsonIgnore]
        public int RadarVertexId { get; set; }

        [Column("ArduinoPort"), JsonIgnore]
        public string ArduinoPort { get; set; }

        [Column("CurrentTemp")]
        public double CurrentTemp { get; set; }

        [Column("TrafficDensity")]
        public int TrafficDensity { get; set; }

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

