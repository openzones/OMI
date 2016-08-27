using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMInsurance.Entities
{
    public abstract class ReconciliationFundResponse : FundResponse, IReconciliationFundResponse
    {
        #region Properties

        public string UnifiedPolicyNumber { get; set; }
        public ReferenceItem PolicyType { get; set; }
        public string PolicySeries { get; set; }
        public string PolicyNumber { get; set; }
        public string OKATO { get; set; }
        public string OGRN { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string FundAnswer { get; set; }
        public int Order { get; set; }
        public string ErrorMessage { get; set; }
        public EntityType DataType { get; set; }
        public long? PolicyTypeId
        {
            get
            {
                return PolicyType.Id;
            }
            set
            {
                PolicyType.Id = value.Value;
            }
        }

        #endregion

        public new abstract class CreateData : FundResponse.CreateData, IReconciliationFundResponse, IEqualityComparer<ReconciliationFundResponse.CreateData>
        {
            public string UnifiedPolicyNumber { get; set; }
            public long? PolicyTypeId { get; set; }
            public string PolicySeries { get; set; }
            public string PolicyNumber { get; set; }
            public string OKATO { get; set; }
            public string OGRN { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? ExpirationDate { get; set; }
            public string FundAnswer { get; set; }
            public string ErrorMessage { get; set; }

            public override bool Equals(object obj)
            {
                ReconciliationFundResponse.CreateData response = obj as ReconciliationFundResponse.CreateData;
                return response != null && Equals(this, response);
            }

            public override int GetHashCode()
            {
                return GetHashCode(this);
            }

            public bool Equals(CreateData x, CreateData y)
            {
                return x.ClientVisitId == y.ClientVisitId
                    && x.ErrorMessage == y.ErrorMessage
                    && x.ExpirationDate == y.ExpirationDate
                    && x.FundAnswer == y.FundAnswer
                    && x.OGRN == y.OGRN
                    && x.OKATO == y.OKATO
                    && x.PolicyNumber == y.PolicyNumber
                    && x.PolicySeries == y.PolicySeries
                    && x.PolicyTypeId == y.PolicyTypeId
                    && x.StartDate == y.StartDate
                    && x.UnifiedPolicyNumber == y.UnifiedPolicyNumber; 
            }

            public int GetHashCode(CreateData obj)
            {
                return
                    obj.ClientVisitId.GetHashCode() ^
                    (obj.DataTypeId ?? 0).GetHashCode() ^
                    (obj.ErrorMessage ?? string.Empty).GetHashCode() ^
                    (obj.ExpirationDate ?? default(DateTime)).GetHashCode() ^
                    (obj.FundAnswer ?? string.Empty).GetHashCode() ^
                    (obj.OGRN ?? string.Empty).GetHashCode() ^
                    (obj.OKATO ?? string.Empty).GetHashCode() ^
                    (obj.PolicyNumber ?? string.Empty).GetHashCode() ^
                    (obj.PolicySeries ?? string.Empty).GetHashCode() ^
                    (obj.PolicyTypeId ?? 0).GetHashCode() ^
                    (obj.StartDate ?? default(DateTime)).GetHashCode() ^
                    (obj.UnifiedPolicyNumber ?? string.Empty).GetHashCode();
            }

            public EntityType DataType
            {
                get
                {
                    return (EntityType)DataTypeId.Value;
                }
                set
                {
                    DataTypeId = (int)value;
                }
            }
        }
    }
}
