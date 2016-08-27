using OMInsurance.Entities.Core;
using System;
using System.ComponentModel;

namespace OMInsurance.WebApps.Models
{
    public class SvdFundResponseModel : FundResponseModel
    {
        public SvdFundResponseModel()
        {

        }
        #region Properties

        [DisplayName("ENP")]
        public string UnifiedPolicyNumber { get; set; }

        [DisplayName("S_CARD")]
        public string PolicySeries { get; set; }

        [DisplayName("N_CARD")]
        public string PolicyNumber { get; set; }

        [DisplayName("DATE_B")]
        public DateTime? StartDate { get; set; }

        [DisplayName("DATE_E")]
        public DateTime? ExpirationDate { get; set; }

        [DisplayName("Q")]
        public string OmsCode { get; set; }

        [DisplayName("Q_OGRN")]
        public string OGRN { get; set; }

        [DisplayName("IM")]
        public string Firstname { get; set; }

        [DisplayName("OT")]
        public string Secondname { get; set; }

        [DisplayName("FAM")]
        public string Lastname { get; set; }

        [DisplayName("DR")]
        public DateTime? Birthday { get; set; }

        [DisplayName("POL")]
        public string Sex { get; set; }

        [DisplayName("DOC_TYPE")]
        public ReferenceItem DocumentType { get; set; }

        [DisplayName("DOC_SER")]
        public string DocumentSeries { get; set; }

        [DisplayName("DOC_NUM")]
        public string DocumentNumber { get; set; }

        [DisplayName("DOC_DATE")]
        public DateTime? DocumentIssueDate { get; set; }

        [DisplayName("GR")]
        public ReferenceItem Citizenship { get; set; }

        [DisplayName("ERZ")]
        public string ERZ { get; set; }

        [DisplayName("Snils")]
        public string Snils { get; set; }
        
        public EntityType DataType { get; set; }
        #endregion

        #region Constructors


        
        #endregion

    }
}