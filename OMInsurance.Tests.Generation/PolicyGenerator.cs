using OMInsurance.BusinessLogic;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMInsurance.Tests.Generation
{
    public static class PolicyGenerator
    {
        static ReferenceBusinessLogic bll = new ReferenceBusinessLogic();
        public static PolicyInfo.SaveData GetPolicyInfoSaveData(long? id)
        {
            PolicyInfo.SaveData data = new PolicyInfo.SaveData();
            data.Id = id;
            data.EndDate = DateTime.Now;
            data.Series = "1233";
            data.Number = "0000123123";
            data.UnifiedPolicyNumber = "123123123123";
            data.StartDate = DateTime.Now;
            data.PolicyTypeId = bll.GetReferencesList(Constants.PolicyTypeRef).FirstOrDefault().Id; 
            return data;
        }
    }
}
