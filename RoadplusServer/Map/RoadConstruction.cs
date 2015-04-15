using System;

namespace Roadplus.Server.Map
{
    public class RoadConstruction
    {
        public Edge Location { get; private set; }
        public TimeRange Duration { get; private set; }
        public DirectionType Direction { get; set; }

		public RoadConstruction(Edge target, TimeRange duration)
        {
			if (target == null) {
				throw new ArgumentNullException ("target");
			} else if (duration == null) {
				throw new ArgumentNullException ("duration");
			}

            Location = target;
			Duration = duration;

			//STIJN: Is this needed? if direction is always both/same as edge?
            // THOMAS: you proposed this yourself. the case was that a road
            //         can be under construction in only one direction.
            //         We should adjust the ctor accordingly

            // also: english lol.
			Direction = target.Direction;
        }
    }
}

