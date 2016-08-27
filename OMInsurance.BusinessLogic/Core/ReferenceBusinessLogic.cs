using OMInsurance.DataAccess.DAO;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using OMInsurance.Interfaces;
using System;
using System.Collections.Generic;

namespace OMInsurance.BusinessLogic
{
    public class ReferenceBusinessLogic : IReferenceBusinessLogic
    {

        public List<ReferenceUniversalItem> GetUniversalList(string referenceName)
        {
            return ReferencesDao.Instance.GetUniversalList(referenceName);
        }

        public void SaveUniversalReferenceItem(ReferenceUniversalItem item, string referenceName, bool flagUpdateOrInsert = false)
        {
            ReferencesDao.Instance.SaveUniversalReferenceItem(item, referenceName, flagUpdateOrInsert);
        }

        public void DeleteReferenceItem(long id, string referenceName)
        {
            ReferencesDao.Instance.DeleteReferenceItem(id, referenceName);
        }

        /// <summary>
        /// Returns a list of specified reference
        /// </summary>
        /// <param name="referenceName">A name of reference</param>
        /// <returns>list of Reference's items</returns>
        public List<ReferenceItem> GetReferencesList(string referenceName)
        {
            return ReferencesDao.Instance.GetList(referenceName);
        }

        /// <summary>
        /// Returns a list of delivery centers
        /// </summary>
        public List<DeliveryCenter> GetDeliveryCenterList()
        {
            return ReferencesDao.Instance.GetDeliveryCenterList();
        }

        /// <summary>
        /// Returns a list of holidays 
        /// </summary>
        public HashSet<DateTime> GetHolidays(int? year)
        {
            return ReferencesDao.Instance.GetHolidays(year);
        }

        /// <summary>
        /// Returns a list of exceptional working days
        /// </summary>
        public HashSet<DateTime> GetExceptionalWorkingDays(int? year)
        {
            return ReferencesDao.Instance.GetExceptionalWorkingDays(year);
        }
    }
}
