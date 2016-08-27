using OMInsurance.Entities;
using OMInsurance.Entities.Searching;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using OMInsurance.WebApps.Validation;
using System.Collections.Generic;

namespace OMInsurance.WebApps.Models
{
    public class BSOSearchCriteriaModel : ValidatableModel<BSOSearchCriteriaModel>
    {
        #region Properties
        //public long BSO_ID { get; set; }
        [DisplayName("Номер БСО от")]
        [StringLength(9, ErrorMessage = "Максимальная длина - 9 символов")]
        public string TemporaryPolicyNumberFrom { get; set; }

        [DisplayName("Номер БСО до")]
        [StringLength(9, ErrorMessage = "Максимальная длина - 9 символов")]
        public string TemporaryPolicyNumberTo { get; set; }

        [DisplayName("Номер партии")]
        [StringLength(3, ErrorMessage = "Максимальная длина - 3 символа")]
        public string PolicyPartyNumber { get; set; }

        [DisplayName("Статус")]
        public long? StatusId { get; set; }

        [DisplayName("Текущий статус")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public long? CurrentStatusId { get; set; }

        [DisplayName("Дата статуса с")]
        public DateTime? StatusDateFrom { get; set; }

        [DisplayName("Дата статуса по")]
        public DateTime? StatusDateTo { get; set; }

        [DisplayName("Пункт выдачи")]
        public List<long> DeliveryCenterIds { get; set; }

        [DisplayName("Ответственный")]
        public long? ResponsibleID { get; set; }

        [DisplayName("Точка выдачи")]
        public List<long> DeliveryPointIds { get; set; }
        public long? NewDeliveryPointId { get; set; }

        [DisplayName("Новый ответственный")]
        public long? NewResponsibleID { get; set; }

        [DisplayName("Новая дата статуса")]
        public DateTime? NewStatusDate { get; set; }

        [DisplayName("Дата изменений от")]
        public DateTime? ChangeDateFrom { get; set; }

        [DisplayName("Дата изменений до")]
        public DateTime? ChangeDateTo { get; set; }

        public bool? IsSuccessfullySaved { get; set; }
        #endregion


        public BSOSearchCriteriaModel(BSOSearchCriteria criteria)
        {
            this.TemporaryPolicyNumberFrom = criteria.TemporaryPolicyNumberFrom;
            this.TemporaryPolicyNumberTo = criteria.TemporaryPolicyNumberTo;
            this.PolicyPartyNumber = criteria.PolicyPartyNumber;
            this.StatusId = criteria.StatusId;
            this.ResponsibleID = criteria.ResponsibleID;
            this.StatusDateFrom = criteria.StatusDateFrom;
            this.StatusDateTo = criteria.StatusDateTo;
            this.DeliveryCenterIds = criteria.DeliveryCenterIds;
            this.DeliveryPointIds = criteria.DeliveryPointIds;
            this.validator = new BSOSearchCriteriaModelValidator();
            this.CurrentStatusId = CurrentStatusId;
            this.NewDeliveryPointId = NewDeliveryPointId;
            this.NewResponsibleID = NewResponsibleID;
            this.ChangeDateFrom = ChangeDateFrom;
            this.ChangeDateTo = ChangeDateTo;
        }

        public BSOSearchCriteriaModel()
        {
            validator = new BSOSearchCriteriaModelValidator();
            DeliveryCenterIds = new List<long>();
            DeliveryPointIds = new List<long>();
        }

        public BSOSearchCriteria GetBSOSearchCriteria()
        {
            BSOSearchCriteria criteria = new BSOSearchCriteria()
            {
                TemporaryPolicyNumberFrom = this.TemporaryPolicyNumberFrom,
                TemporaryPolicyNumberTo = this.TemporaryPolicyNumberTo,
                PolicyPartyNumber = this.PolicyPartyNumber,
                StatusId = this.StatusId == 0 ? new Nullable<long>() : this.StatusId,
                StatusDateFrom = this.StatusDateFrom,
                StatusDateTo = this.StatusDateTo,
                DeliveryCenterIds = DeliveryCenterIds,
                DeliveryPointIds = DeliveryPointIds,
                ResponsibleID = this.ResponsibleID == 0 ? new Nullable<long>() : this.ResponsibleID,
                ChangeDateFrom = this.ChangeDateFrom,
                ChangeDateTo = this.ChangeDateTo
            };
            return criteria;
        }
    }
}