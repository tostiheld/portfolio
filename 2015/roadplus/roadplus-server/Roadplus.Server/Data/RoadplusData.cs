using System;
using System.Linq;
using LinqToDB;

using Roadplus.Server.API;

namespace Roadplus.Server.Data
{
    public class RoadplusData : LinqToDB.Data.DataConnection
    {
        public ITable<Vertex> Vertices
        { 
            get
            {
                return GetTable<Vertex>();
            }
        }

        public ITable<Edge> Edges
        {
            get
            {
                return GetTable<Edge>();
            }
        }

        public ITable<Zone> Zones
        {
            get
            {
                return GetTable<Zone>();
            }
        }

        public ITable<RoadConstruction> RoadConstructions
        {
            get
            {
                return GetTable<RoadConstruction>();
            }
        }

        public ITable<School> Schools
        {
            get
            {
                return GetTable<School>();
            }
        }

        public RoadplusData() 
            : base("RoadplusData")
        { }
    }
}

