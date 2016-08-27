using OmInsurance.References;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OMInsurance.Utils
{
    public static class DateHelper
    {
        public static bool IsHoliday(DateTime date)
        {
            return ReferencesProvider.GetHolidays().Contains(date.Date);
        }

        public static bool IsExceptionalWorkingDay(DateTime date)
        {
            return ReferencesProvider.GetExceptionalWorkingDays().Contains(date.Date);
        }

        public static bool IsWorkingDay(this DateTime date)
        {
            return date.DayOfWeek != DayOfWeek.Saturday &&
                date.DayOfWeek != DayOfWeek.Sunday &&
                !ReferencesProvider.GetHolidays().Contains(date.Date) ||
                ReferencesProvider.GetExceptionalWorkingDays().Contains(date.Date);
        }

        public static DateTime AddWorkingDays(this DateTime date, int workingDaysCount)
        {
            DateTime resultDate = date;

            if (workingDaysCount > 0)
            {
                while (workingDaysCount > 0)
                {
                    resultDate = resultDate.AddDays(1);
                    if (resultDate.IsWorkingDay())
                    {
                        workingDaysCount--;
                    }
                }
            }
            else
            {
                while (workingDaysCount < 0)
                {
                    resultDate = resultDate.AddDays(-1);
                    if (resultDate.IsWorkingDay())
                    {
                        workingDaysCount++;
                    }
                }
            }

            return resultDate;
        }
    }
}