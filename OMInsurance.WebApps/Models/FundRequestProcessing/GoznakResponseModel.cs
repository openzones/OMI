using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace OMInsurance.WebApps.Models
{
    public class GoznakResponseModel : FundResponseModel
    {
        public GoznakResponseModel()
        {

        }
        #region Properties

        [DisplayName("ENP")]
        public string UnifiedPolicyNumber { get; set; }
        
        [DisplayName("FAM")]
        public string Lastname { get; set; }

        [DisplayName("IM")]
        public string Firstname { get; set; }

        [DisplayName("OT")]
        public string Secondname { get; set; }

        [DisplayName("DR")]
        public DateTime? Birthday { get; set; }
        
        [DisplayName("W")]
        public string Sex { get; set; }
        
        [DisplayName("PV")]
        public ReferenceItem DeliveryCenter { get; set; }

        [DisplayName("VSN")]
        public string TemporaryPolicyNumber { get; set; }

        [DisplayName("Дата обновления")]
        public DateTime UpdateDate { get; set; }

        #endregion

        #region Constructors


        
        #endregion
    }
}