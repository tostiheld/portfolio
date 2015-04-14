using System;

namespace Roadplus.Server.Map
{
	public class TimeRange
	{
		public TimeSpan BeginTime { get; private set; }
		public TimeSpan EndTime { get; private set; }

		public TimeRange (TimeSpan beginTime, TimeSpan endTime)
		{
			if (beginTime > endTime) {
				throw new ArgumentException ("beginTime is later than endTime");
			}
			if (beginTime == TimeSpan.Zero || endTime == TimeSpan.Zero) {
				throw new ArgumentNullException ();
			}
			if (beginTime < TimeSpan.Zero || endTime < TimeSpan.Zero) {
				throw new ArgumentOutOfRangeException ();
			}

			BeginTime = beginTime;
			EndTime = endTime;
		}

		public Boolean IsActive(DateTime date = DateTime.Now){
			TimeSpan currentTime = date.Now.TimeOfDay;
			if (currentTime > BeginTime && currentTime < EndTime) {
				return true;
			}
			return false;
		}
			
	}
}

