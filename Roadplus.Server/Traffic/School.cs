using System;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Roadplus.Server.Traffic
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

        public override string ToString()
        {
            return string.Format("[School: ID={0}, Schedule={1}, Name={2}, Location={3}]", ID, Schedule, Name, Location);
        } 

        public string ToString(string format)
        {
            if (format == "json")
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    DataContractJsonSerializer json = 
                        new DataContractJsonSerializer(typeof(School));
                    json.WriteObject(ms, this);
                    using (StreamReader sr = new StreamReader(ms, Encoding.UTF8))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
            return ToString();
        }
    }
}
