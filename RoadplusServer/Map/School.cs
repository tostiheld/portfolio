using System;

namespace Roadplus.Server.Map
{
    public class School
    {
		public Utils.TimeRange TimeRange { get; set; }
		public String Name { get; set; }

		public School(Utils.TimeRange timeRange, String name)
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

