using System;
using System.Linq;
using System.Collections.Generic;

namespace Roadplus.Server.API
{
    public class Activity
    {
        public ActivityType Type { get; private set; }
        public string SourceAddress { get; private set; }
        public LinkType SourceType { get; private set; }
        public List<Type> TargetTypes { get; private set; }
        public Dictionary<Type, object> Payload 
        { 
            get
            {
                return payload;
            }
            set
            {
                payload = value;
                TargetTypes = payload.Keys
                    .Distinct()
                    .ToList();
            }
        }

        private Dictionary<Type, object> payload;

        public Activity(ActivityType type, string sourceaddress, LinkType sourcetype)
        {
            Type = type;
            SourceAddress = sourceaddress;
            SourceType = sourcetype;

            Payload = new Dictionary<Type, object>();
            TargetTypes = new List<Type>();
        }
    }
}

