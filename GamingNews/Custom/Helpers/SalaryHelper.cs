using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingWeb.Custom.Helpers
{
	public class SalaryHelper
	{
        public int WorkDays(int month, int year, Boolean excludeWeekends)
        {
            var startDate = new DateTime(year, month, 1);
            var daysInMonth = DateTime.DaysInMonth(year, month);
            var endDate = new DateTime(year, month, daysInMonth);
            int count = 0;
            for (DateTime index = startDate; index <= endDate; index = index.AddDays(1))
            {
                if (excludeWeekends && (index.DayOfWeek == DayOfWeek.Sunday || index.DayOfWeek == DayOfWeek.Saturday))
                    continue;
                count++;
            }
            return count;
        }
    }
}
