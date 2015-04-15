using System;

using Roadplus.Server.Map;

namespace Roadplus.Server
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Vertex start = new Vertex(new Point(0, 0));
            Zone zone = new Zone(start);

            Point point = new Point(10, 10);
            start.Connect(point);
        }
    }
}
