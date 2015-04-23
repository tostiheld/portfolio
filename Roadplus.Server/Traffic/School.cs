using System;
using System.Text;
using System.IO;

namespace Roadplus.Server.Traffic
{
    public class School
    {
        public int ID { get; private set; }
		public TimeRange Schedule { get; private set; }
		public string Name { get; set; }
        public Vertex Location { get; set; }

		public School(TimeRange schedule, int id)
        {
            if (schedule == null)
            {
                throw new ArgumentNullException("schedule");
            }

			Schedule = schedule;
            ID = id;
        }
    }
}
