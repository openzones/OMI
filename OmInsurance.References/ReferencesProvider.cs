using OMInsurance.DataAccess.DAO;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace OmInsurance.References
{
    public static class ReferencesProvider
    {
        private static ConcurrentDictionary<string, List<ReferenceItem>> referencesPool = new ConcurrentDictionary<string, List<ReferenceItem>>();
        private static HashSet<DateTime> holidays;
        private static HashSet<DateTime> exceptionalWorkingDays;

        public static List<ReferenceItem> GetReferenceItems(string referenceName)
        {
            List<ReferenceItem> items;
            if (referencesPool.ContainsKey(referenceName))
            {
                items = referencesPool[referenceName];
            }
            else
            {
                if (referenceName == Constants.DeliveryCenterForOperatorRef)
                {
                    items = ReferencesDao.Instance.GetDeliveryCenterList()
                        .Where(item => !string.IsNullOrEmpty(item.DisplayName))
                        .Select(item => item as ReferenceItem).ToList();
                }
                else
                {
                    items = ReferencesDao.Instance.GetList(referenceName);
                }
                referencesPool.AddOrUpdate(referenceName, items, (key, value) => value);
            }
            return items;
        }

        public static HashSet<DateTime> GetHolidays()
        {
            if (holidays == null)
            {
                holidays = ReferencesDao.Instance.GetHolidays(null);
            }

            return holidays;
        }

        public static HashSet<DateTime> GetExceptionalWorkingDays()
        {
            if (exceptionalWorkingDays == null)
            {
                exceptionalWorkingDays = ReferencesDao.Instance.GetExceptionalWorkingDays(null);
            }
            return exceptionalWorkingDays;
        }
    }
}
