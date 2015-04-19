using System;
using Roadplus.Server.Communication.Protocol;

namespace Roadplus.Server.EntityManagement
{
    public class Activity
    {
        public ActivityType Type { get; private set; }
        public Link From { get; private set; }
        public object[] Parameters { get; private set; }

        public Activity(ActivityType type, Link from, object[] parameters)
        {
            Type = type;
            From = from;
            Parameters = parameters;
        }
    }
}

