using OMInsurance.Entities.Core;
using System;

namespace OMInsurance.Entities
{
    public class PolicyInfo : DataObject
    {
        #region Properties

        public ReferenceItem PolicyType { get; set; }
        public string Series { get; set; }
        public string Number { get; set; }
        public string UnifiedPolicyNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string OGRN { get; set; }
        public string OKATO { get; set; }
        public long? SmoId { get; set; }
        public string SmoName { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        #endregion

        public class SaveData
        {
            public SaveData()
            {
            }
            public SaveData(PolicyInfo policyInfo)
            {
                this.PolicyTypeId = policyInfo.PolicyType.Id == 0 ? new long?() : policyInfo.PolicyType.Id;
                this.Series = policyInfo.Series;
                this.Number = policyInfo.Number;
                this.UnifiedPolicyNumber = policyInfo.UnifiedPolicyNumber;
                this.StartDate = policyInfo.StartDate;
                this.EndDate = policyInfo.EndDate;
                this.OGRN = policyInfo.OGRN;
                this.OKATO = policyInfo.OKATO;
            }
            public long? Id { get; set; }
            public long? PolicyTypeId { get; set; }
            public string Series { get; set; }
            public string Number { get; set; }
            public string UnifiedPolicyNumber { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public string OGRN { get; set; }
            public string OKATO { get; set; }
            public long? SmoId { get; set; }
            public string SmoName { get; set; }
        }
    }
}
