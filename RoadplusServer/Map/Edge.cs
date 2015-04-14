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
				ArgumentNullException ("start");
			} else if (end == null) {
				ArgumentNullException ("end");
			}

            Start = start;
            End = end;
            Direction = DirectionType.Both;
        }
    }
}

