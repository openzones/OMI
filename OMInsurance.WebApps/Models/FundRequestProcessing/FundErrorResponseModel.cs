using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace OMInsurance.WebApps.Models
{
    public class FundErrorResponseModel : FundResponseModel
    {
        public FundErrorResponseModel()
        {

        }
        #region Properties

        [DisplayName("Этап")]
        public ReferenceItem FundResponseStage { get; set; }
        [DisplayName("LID")]
        public long LID { get; set; }
        [DisplayName("NPP")]
        public int OrderNumber { get; set; }
        [DisplayName("ENP")]
        public string UnifiedPolicyNumber { get; set; }
        [DisplayName("SNILS")]
        public string SNILS { get; set; }
        [DisplayName("VID_DOCU")]
        public ReferenceItem PolicyType { get; set; }
        [DisplayName("SER_DOCU")]
        public string PolicySeries { get; set; }
        [DisplayName("NOM_DOCU")]
        public string PolicyNumber { get; set; }
        [DisplayName("DP")]
        public DateTime? TemporaryPolicyDate { get; set; }
        [DisplayName("DT")]
        public DateTime? ExpirationDate { get; set; }
        [DisplayName("SMO_ID")]
        public int SMO_ID { get; set; }
        [DisplayName("IM")]
        public string Firstname { get; set; }
        [DisplayName("OT")]
        public string Secondname { get; set; }
        [DisplayName("FAM")]
        public string Lastname { get; set; }
        [DisplayName("W")]
        public string Sex { get; set; }
        [DisplayName("DR")]
        public DateTime? Birthday { get; set; }
        [DisplayName("PV")]
        public ReferenceItem DeliveryCenter { get; set; }
        [DisplayName("ERR_CODE")]
        public string ErrorCode { get; set; }
        [DisplayName("ERR_TEXT")]
        public string ErrorText { get; set; }
        [DisplayName("TER_STRAH")]
        public string TerritoryCode { get; set; }
        [DisplayName("OGRN")]
        public string OGRN { get; set; }

        #endregion

        #region Constructors


        
        #endregion
    }
}