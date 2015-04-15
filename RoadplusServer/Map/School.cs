using System;

namespace Roadplus.Server.Map
{
    public class School
    {
		public TimeRange Schedule { get; private set; }
		public String Name { get; set; }

		public School(TimeRange schedule, String name)
        {
			if (schedule == null) {
				throw new ArgumentNullException ("schedule");
			} else if (name == null) {
				throw new ArgumentNullException ("name");
			} 

			Schedule = schedule;
			Name = name;
        }
    }
}

