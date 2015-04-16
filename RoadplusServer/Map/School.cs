using System;
using System.Runtime.Serialization;

namespace Roadplus.Server.Map
{
    [DataContract]
    public class School
    {
        [DataMember(Name="id")]
        public int ID { get; private set; }
        [DataMember(Name="schedule")]
		public TimeRange Schedule { get; private set; }
        [DataMember(Name="name")]
		public string Name { get; set; }
        [DataMember(Name="location")]
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
