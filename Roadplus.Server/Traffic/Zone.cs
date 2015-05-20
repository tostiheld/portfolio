using System;
using System.Collections.Generic;
using Roadplus.Server.Communication;

namespace Roadplus.Server.Traffic
{
    public class Zone
    {
        public int ID { get; private set; }
        public string Name { get; set; }
        public List<School> Schools { get; private set; }
        public List<RoadConstruction> RoadConstructions { get; private set; }

        private Vertex root;
        private Vertex radarLocation;
        private List<Vertex> vertices;
        private List<Edge> edges;

        public Zone(Vertex startingPoint, int id)
        {
			if (startingPoint == null) {
				throw new ArgumentNullException ("startingPoint");
			}
            vertices = new List<Vertex>();
            edges = new List<Edge>();

            ID = id;
            Schools = new List<School>();
            RoadConstructions = new List<RoadConstruction>();

            root = startingPoint;
            vertices.Add(root);
            root.NewConnection += Vertex_NewConnection;
        }

        private void Vertex_NewConnection(object sender, NewConnectionEventArgs e)
        {
            e.End.NewConnection += Vertex_NewConnection;

            vertices.Add(e.End);
            edges.Add(e.Connection);
        }

        /// <summary>
        /// Convert a location in pixels to a vertex, using the 
        /// Vertex tolerance radius as defined in the Settings
        /// </summary>
        /// <returns>A vertex if the location was within the tolerance</returns>
        /// <param name="point">The location to check.</param>
        public Vertex PointToVertex(Point point)
        {
            throw new NotImplementedException();
            /*foreach (Vertex v in vertices)
            {
                // formula to check if point is in circle
                // (x - center_x)^2 + (y - center_y)^2 < radius^2
                if (((point.X - v.Location.X) * (point.X - v.Location.X) +
                     (point.Y - v.Location.Y) * (point.Y - v.Location.Y)) //<
                     )//Settings.VertexRadius * Settings.VertexRadius)
                {
                    return v;
                }

            }

            return null;*/
        }



        public RoadConstruction TEMPGetRCByID(int id)
        {
            foreach (RoadConstruction r in RoadConstructions)
            {
                if (r.ID == id)
                {
                    return r;
                }
            }

            return null;
        }

        public School GetSchoolByID(int id)
        {
            foreach (School s in Schools)
            {
                if (s.ID == id)
                {
                    return s;
                }
            }

            return null;
        }

        public void SetSign(int speed)
        {
            throw new NotImplementedException();
            /*
            Message message = new Message(
                CommandType.SetRoadSign,
                new string[] { speed.ToString() });
            Road.Send(message);*/
        }

        public void GetTemp()
        {
            throw new NotSupportedException();
            /*
            Message message = new Message(
                CommandType.Temperature);
            Road.Send(message);*/
        }
    }
}

