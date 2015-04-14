using System;

namespace Roadplus.Server.Map
{
    public class Edge
    {
        public Vertex Start { get; private set; }
        public Vertex End { get; private set; }
        public double Weight { get; set; }
        public int MaxSpeed { get; set; }
        public DirectionType Direction { get; set; }

        public Edge(Vertex start, Vertex end)
        {
			if (start == null) {
				throw new ArgumentNullException ("start");
			} else if (end == null) {
				throw new ArgumentNullException ("end");
			}

            Start = start;
            End = end;
            Direction = DirectionType.Both;
        }
    }
}

