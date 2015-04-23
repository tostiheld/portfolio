using System;
using System.Collections.Generic;

namespace Roadplus.Server.API
{
    public class Activity
    {
        public ActivityType Type { get; private set; }
        public object[] Payload { get; set; }
        public string SourceAddress { get; private set; }
        public LinkType SourceType { get; private set; }
        public List<Type> TargetTypes { get; set; }

        public Activity(ActivityType type, string sourceaddress, LinkType sourcetype)
        {
            Type = type;
            SourceAddress = sourceaddress;
            SourceType = sourcetype;

            Payload = null;
            TargetTypes = new List<Type>();
        }
    }
}

