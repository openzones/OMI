
using OMInsurance.Entities.Core;
using System;
using System.ComponentModel;
namespace OMInsurance.WebApps.Models
{
    public abstract class ReconciliationFundResponseModel : FundResponseModel
    {
        public ReconciliationFundResponseModel()
        {
        }
        public EntityType DataType { get; set; }

        [DisplayName("ENP")]
        public string UnifiedPolicyNumber { get; set; }

        [DisplayName("TIP_D")]
        public ReferenceItem PolicyType { get; set; }

        [DisplayName("S_POL")]
        public string PolicySeries { get; set; }

        [DisplayName("N_POL")]
        public string PolicyNumber { get; set; }

        [DisplayName("OMS_OKATO")]
        public string OKATO { get; set; }

        [DisplayName("OGRN")]
        public string OGRN { get; set; }

        [DisplayName("DATE_B")]
        public DateTime? StartDate { get; set; }

        [DisplayName("DATE_E")]
        public DateTime? ExpirationDate { get; set; }

        [DisplayName("ANS_FL")]
        public string FundAnswer { get; set; }

        [DisplayName("ERR")]
        public string ErrorMessage { get; set; }

        [DisplayName("NPP")]
        public long Order { get; set; }
    }
}