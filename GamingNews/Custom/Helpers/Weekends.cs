using System;

namespace GamingWeb.Custom.Helpers
{
    public static class Weekends
    {
        public static int WeekendsBetween(this DateTime firstDay, DateTime lastDay)
        {
            firstDay = firstDay.Date;
            lastDay = lastDay.Date;

            if (firstDay > lastDay)
                throw new ArgumentException("Incorrect last day " + lastDay);

            TimeSpan span = lastDay - firstDay;
            int totalDays = span.Days + 1;
            int weekends = 0;

            for (int i = 0; i < totalDays; i++)
            {
                DateTime currentDay = firstDay.AddDays(i);
                if (currentDay.DayOfWeek == DayOfWeek.Saturday || currentDay.DayOfWeek == DayOfWeek.Sunday)
                {
                    weekends++;
                }
            }

            return weekends;
        }
    }
}
