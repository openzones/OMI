using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMInsurance.Interfaces
{
    public interface IReferenceBusinessLogic
    {
        List<ReferenceUniversalItem> GetUniversalList(string referenceName);
        void SaveUniversalReferenceItem(ReferenceUniversalItem item, string referenceName, bool flagUpdateOrInsert = false);
        void DeleteReferenceItem(long id, string referenceName);
        List<ReferenceItem> GetReferencesList(string referenceName);
        List<DeliveryCenter> GetDeliveryCenterList();
        HashSet<DateTime> GetHolidays(int? year);
        HashSet<DateTime> GetExceptionalWorkingDays(int? year);
    }
}
