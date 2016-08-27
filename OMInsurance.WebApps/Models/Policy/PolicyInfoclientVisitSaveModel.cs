using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using OMInsurance.WebApps.Models.Core;
using OMInsurance.WebApps.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace OMInsurance.WebApps.Models
{
    public class PolicyInfoClientVisitSaveModel : ValidatableModel<PolicyInfoClientVisitSaveModel>
    {
        #region Properties

        public long? Id { get; set; }

        [DisplayName("Тип полиса")]
        public long? PolicyTypeId { get; set; }
        public List<SelectListItem> PolicyTypes { get; set; }

        public EntityType EntityType { get; set; }

        [DisplayName("Серия")]
        [StringLength(12, ErrorMessage = "Максимальная длина - 12 символов")]
        public string Series { get; set; }

        [DisplayName("Номер")]
        [StringLength(32, ErrorMessage = "Максимальная длина - 32 символов")]
        public string Number { get; set; }

        [DisplayName("ЕНП")]
        [StringLength(16, ErrorMessage = "Максимальная длина - 16 символов")]
        public string UnifiedPolicyNumber { get; set; }

        [DisplayName("Дата начала действия")]
        public DateTime? StartDate { get; set; }

        [DisplayName("Дата окончания действия")]
        public DateTime? EndDate { get; set; }

        [DisplayName("Страховая медицинская организация")]
        public string SmoName { get; set; }
        public long? SmoId { get; set; }

        [DisplayName("ОГРН")]
        [StringLength(13, ErrorMessage = "Максимальная длина - 13 символов")]
        public string OGRN { get; set; }

        [DisplayName("ОКАТО")]
        [StringLength(5, ErrorMessage = "Максимальная длина - 5 символов")]
        public string OKATO { get; set; }

        #endregion

        #region Constructors
				
        public PolicyInfoClientVisitSaveModel()
        {
            validator = new PolicyInfoValidator();
            PolicyTypes = ReferencesProvider.GetReferences(Constants.PolicyTypeRef, null, true);
        }

        public PolicyInfoClientVisitSaveModel(EntityType type)
            : this()
        {
            EntityType = type;
        }

        public PolicyInfoClientVisitSaveModel(PolicyInfo policyInfo, EntityType type) : this()
        {
            Id = policyInfo.Id;
            EntityType = type;
            PolicyTypeId = policyInfo.PolicyType != null ? policyInfo.PolicyType.Id : 0;
            Series = policyInfo.Series;
            Number = policyInfo.Number;
            UnifiedPolicyNumber = policyInfo.UnifiedPolicyNumber;
            StartDate = policyInfo.StartDate;
            EndDate = policyInfo.EndDate;
            OGRN = policyInfo.OGRN;
            OKATO = policyInfo.OKATO;
            SmoId = policyInfo.SmoId;
            SmoName = policyInfo.SmoName;
        }
        #endregion

        #region Methods

        internal PolicyInfo.SaveData GetForBLL()
        {
            PolicyInfo.SaveData data = new PolicyInfo.SaveData();
            data.Id = this.Id;
            data.PolicyTypeId = this.PolicyTypeId == 0 ? new long?() : this.PolicyTypeId;
            data.Series = this.Series;
            data.Number = this.Number;
            data.UnifiedPolicyNumber = this.UnifiedPolicyNumber;
            data.StartDate = this.StartDate.HasValue && this.StartDate.Equals(default(DateTime)) ? null : this.StartDate;
            data.EndDate = this.EndDate.HasValue && this.EndDate.Equals(default(DateTime)) ? null : this.EndDate;
            data.OGRN = this.OGRN;
            data.OKATO = this.OKATO;
            data.SmoId = this.SmoId;
            data.SmoName = this.SmoName;
            return data;
        }
        
        #endregion
    }
}