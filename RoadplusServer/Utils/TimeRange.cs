using System;

namespace Roadplus.Server.Utils
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
			else if (beginTime == TimeSpan.Zero) {
				throw new ArgumentException ("beginTime is not set");
			}
			else if (endTime == TimeSpan.Zero) {
				throw new ArgumentException ("endTime is not set");
			}
			else if (beginTime < TimeSpan.Zero) {
				throw new ArgumentOutOfRangeException ("beginTime");
			}
			else if (endTime < TimeSpan.Zero) {
				throw new ArgumentOutOfRangeException ("endTime");
			}

			BeginTime = beginTime;
			EndTime = endTime;
		}

		public Boolean IsActive(DateTime date){
			TimeSpan currentTime = date.TimeOfDay;
			if (currentTime > BeginTime && currentTime < EndTime) {
				return true;
			}
			return false;
		}

		public Boolean IsActive(){
			return IsActive (DateTime.Now);
		}
			
	}
}

