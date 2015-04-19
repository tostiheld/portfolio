using ProtoBuf;

namespace Roadplus.Server
{
    [ProtoContract]
    public struct Point
    {
        [ProtoMember(1)]
        public int X { get; set; }
        [ProtoMember(2)]
        public int Y { get; set; }

        public Point(int x, int y)
        : this()
        {
            X = x;
            Y = y;
        }
    }
}

