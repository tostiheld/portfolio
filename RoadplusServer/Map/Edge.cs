using System;
using ProtoBuf;

namespace Roadplus.Server.Map
{
    [ProtoContract]
    public class Edge
    {
        [ProtoMember(1, AsReference=true)]
        public Vertex Start { get; private set; }
        [ProtoMember(2, AsReference=true)]
        public Vertex End { get; private set; }
        [ProtoMember(3)]
        public double Weight { get; set; }
        [ProtoMember(4)]
        public int MaxSpeed { get; set; }
        [ProtoMember(5)]
        public DirectionType Direction { get; set; }

        // USED BY PROTOBUF-NET
        // DO NOT EXPOSE
        private Edge() { }

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

