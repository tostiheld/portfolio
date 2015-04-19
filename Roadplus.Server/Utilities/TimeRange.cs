using System;

namespace Roadplus.Server
{
    public class TimeRange
    {
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public TimeRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public TimeRange(DateTime end)
            : this(DateTime.Now, end)
        { }

        public bool IsInRange(DateTime datetime)
        {
            if (datetime >= Start &&
                datetime <= End)
            {
                return true;
            }

            return false;
        }

        public bool IsInRange()
        {
            return IsInRange(DateTime.Now);
        }
    }
}

