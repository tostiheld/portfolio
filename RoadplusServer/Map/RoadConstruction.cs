using System;

namespace Roadplus.Server.Map
{
    public class RoadConstruction
    {
        public Edge Location { get; private set; }
        public TimeSpan Duration { get; set; }

        public RoadConstruction(Edge target, TimeSpan duration)
        {
            Location = target;
            Duration = duration;
        }
    }
}

