﻿using System;
using LinqToDB.Mapping;
using System.Linq;

namespace Roadplus.Server.Data
{
    [Table("School")]
    public class School
    {
        [PrimaryKey, Identity]
        public int SchoolId { get; set; }

        [Column("Name"), NotNull]
        public string Name { get; set; }

        [Column("ZoneId"), NotNull]
        public int ZoneId { get; set; }

        [Column("VertexId"), NotNull]
        public int VertexId { get; set; }

        [Column("OpenTime"), NotNull]
        public DateTime OpenTime { get; set; }

        [Column("CloseTime"), NotNull]
        public DateTime CloseTime { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public Zone Parent
        {
            get
            {
                RoadplusData data = new RoadplusData();
                return data.Zones.FirstOrDefault(
                    z => z.ZoneId == ZoneId);
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        public Vertex LocationVertex
        {
            get
            {
                RoadplusData data = new RoadplusData();
                return data.Vertices.FirstOrDefault(
                    v => v.VertexId == VertexId);
            }
        }
    }
}

