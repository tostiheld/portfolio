using System;
using System.Collections.Generic;

namespace Roadplus.Server.API
{
    public class ActivityValidator
    {
        private List<Tuple<LinkType, ActivityType>> table;

        public ActivityValidator()
        {
            table = new List<Tuple<LinkType, ActivityType>>();
        }

        public bool IsAllowed(Activity activity)
        {
            foreach (Tuple<LinkType, ActivityType> t in table)
            {
                if (activity.SourceType == t.Item1 &&
                    activity.Type == t.Item2)
                {
                    return true;
                }
            }

            return false;
        }

        public void AllowActivity(LinkType linktype, ActivityType activity)
        {
            Tuple<LinkType, ActivityType> t = 
                new Tuple<LinkType, ActivityType>(linktype, activity);

            table.Add(t);
        }

        public void DisllowActivity(LinkType linktype, ActivityType activity)
        {
            List<Tuple<LinkType, ActivityType>> clone = 
                new List<Tuple<LinkType, ActivityType>>(table);

            foreach (Tuple<LinkType, ActivityType> t in clone)
            {
                if (t.Item1 == linktype &&
                    t.Item2 == activity)
                {
                    table.Remove(t);
                }
            }
        }
    }
}

