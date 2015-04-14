using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Roadplus.Server.Map
{
    public class Vertex
    {
        public delegate void OnNewConnection(object sender, NewConnectionEventArgs e);
        public event OnNewConnection NewConnection;

        public Point Location { get; private set; }
        public List<Edge> Edges { get; set; }

        public Vertex(Point location)
        {
			Location = location;
            Edges = new List<Edge>();
        }

        /// <summary>
        /// Creates a new Vertex from the point and creates a new 
        /// Edge inbetween.
        /// </summary>
        /// <param name="end">The point to connect to</param>
        public void Connect(Point end)
        {
            Vertex endVertex = new Vertex(end);
            Edge newEdge = new Edge(this, endVertex);

            Edges.Add(newEdge);
            endVertex.Edges.Add(newEdge);

            if (NewConnection != null)
            {
                NewConnectionEventArgs e = new NewConnectionEventArgs(
                    endVertex,
                    newEdge);
                NewConnection(this, e);
            }
        }

        // MONODEVELOP IS INTELLIGENT AAHHHH
        // IT KNOWS
        // BURN ALL THE ELECTRONICS
        // (my freakout over auto doc generation)
        // (its really nice ahhh yeah)

        /// <summary>
        /// Obstructs the road.
        /// </summary>
        /// <returns>
        /// A new RoadConstruction if the edge is found, or null if the
        /// edge is not found
        /// </returns>
        /// <param name="second">The second vertex to look for the road 
        /// inbetween</param>
        /// <param name="dateRange">From when to when is the road obstructed?</param>
		public RoadConstruction ObstructRoad(Vertex second, Utils.DateRange dateRange)
        {
			if (second == null) {
				throw new ArgumentNullException ("second");
			} else if (dateRange == null) {
				throw new ArgumentNullException ("dateRange");
			}

            foreach (Edge e in Edges)
            {
                if (Object.ReferenceEquals(e.End, second))
                {
                    RoadConstruction rc = new RoadConstruction(e, dateRange);
                    return rc;
                }
            }

            return null;
        }
    }
}

