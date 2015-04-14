using System;

namespace Roadplus.Server.Map
{
    public class RoadConstruction
    {
        public Edge Location { get; private set; }
        public DateRange DateRange { get; set; }
        public DirectionType Direction { get; set; }

		public RoadConstruction(Edge target, DateRange dateRange)
        {
			if (target == null) {
				throw new ArgumentNullException ("target");
			} else if (dateRange == null) {
				throw new ArgumentNullException ("dateRange");
			}

            Location = target;
			DateRange = dateRange;

			//Is this needed? if direction is always both/same as edge?
			Direction = target.Direction;
        }
    }
}

