using System;

namespace Roadplus.Server.Map
{
    public class School
    {
		public TimeRange TimeRange { get; private set; }
		public String Name { get; private set; }

		public School(TimeRange timeRange, String name)
        {
			if (timeRange == null) {
				throw new ArgumentNullException ("timeRange");
			} else if (name == null) {
				throw new ArgumentNullException ("name");
			} 

			TimeRange = timeRange;
			Name = name;
        }
    }
}

