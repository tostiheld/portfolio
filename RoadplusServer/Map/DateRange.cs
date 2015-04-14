using System;

namespace Roadplus.Server.Map
{
	public class DateRange
	{
		public DateTime BeginDate { get; private set; }
		public DateTime EndDate { get; private set; }

		public DateRange (DateTime beginDate, DateTime endDate)
		{
			if (beginDate > endDate) {
				throw new ArgumentException ("beginDate is later than endDate");
			}
			else if (endDate == default(DateTime)) {
				throw new ArgumentException ("endDate is not set");
			}
			else if (beginDate == default(DateTime)) {
				throw new ArgumentException ("beginDate is not set");
			}

			BeginDate = beginDate;
			EndDate = endDate;
		}

		public Boolean IsActive(DateTime date = DateTime.Now){
			
			if (date > BeginDate && date < EndDate) {
				return true;
			}
			return false;
		}

	}
}
