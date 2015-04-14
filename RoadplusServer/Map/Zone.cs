using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Roadplus.Server.Communication;

namespace Roadplus.Server.Map
{
    public class Zone
    {
        private Vertex root;
        private Vertex radarLocation;
        private List<Vertex> vertices;
        private List<Edge> edges;

        public Zone(Vertex startingPoint)
        {
			if (startingPoint == null) {
				throw new ArgumentNullException ("startingPoint");
			}
            vertices = new List<Vertex>();
            edges = new List<Edge>();

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
            foreach (Vertex v in vertices)
            {
                // formula to check if point is in circle
                // (x - center_x)^2 + (y - center_y)^2 < radius^2
                if (((point.X - v.Location.X) * (point.X - v.Location.X) +
                     (point.Y - v.Location.Y) * (point.Y - v.Location.Y)) <
                     Settings.VertexRadius * Settings.VertexRadius)
                {
                    return v;
                }

            }

            return null;
        }

        /// <summary>
        /// Connect to a real life road
        /// </summary>
        /// <param name="comms">The communicator to pair with</param>
        /// <param name="location">The location of the radar on the map</param> 
        public void Connect(RoadCommunicator comms, Point location)
        {
			if (RoadCommunicator == null) {
				throw new ArgumentNullException ("comms");
			}
            Vertex radarlocation = PointToVertex(location);
            if (radarlocation == null)
            {
                throw new VertexNotFoundException();
            }
            else
            {
                radarLocation = radarlocation;

                // further implementation down here
            }
        }
    }
}

