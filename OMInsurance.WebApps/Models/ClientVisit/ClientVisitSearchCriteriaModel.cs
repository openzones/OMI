using OMInsurance.Entities.Searching;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OMInsurance.WebApps.Models
{
    public class ClientVisitSearchCriteriaModel
    {
        public ClientVisitSearchCriteriaModel(ClientVisitSearchCriteria criteria)
        {
            Firstname = criteria.Firstname;
            Secondname = criteria.Secondname;
            Lastname = criteria.Lastname;
            Birthday = criteria.Birthday;
            TemporaryPolicyNumber = criteria.TemporaryPolicyNumber;
            TemporaryPolicyDateFrom = criteria.TemporaryPolicyDateFrom;
            TemporaryPolicyDateTo = criteria.TemporaryPolicyDateTo;
            IsActualInVisitGroup = criteria.IsActualInVisitGroup;
            UpdateDateFrom = criteria.UpdateDateFrom;
            UpdateDateTo = criteria.UpdateDateTo;
            PolicyDateFrom = criteria.PolicyDateFrom;
            PolicyDateTo = criteria.PolicyDateTo;
            PolicySeries = criteria.PolicySeries;
            PolicyNumber = criteria.PolicyNumber;
            PartyNumber = criteria.PartyNumber;
            StatusIds = criteria.StatusIds;
            ScenarioIds = criteria.ScenarioIds;
            DeliveryCenterIds = criteria.DeliveryCenterIds;
            DeliveryPointIds = criteria.DeliveryPointIds;
            StatusDateFrom = criteria.StatusDateFrom;
            StatusDateTo = criteria.StetusDateTo;
            UserId = criteria.UserId;
        }

        public ClientVisitSearchCriteriaModel()
        {
            StatusIds = new List<long>();
            ScenarioIds = new List<long>();
            DeliveryCenterIds = new List<long>();
            DeliveryPointIds = new List<long>();
        }

        [DisplayName("Имя")]
        [StringLength(50, ErrorMessage = "Максимальная длина - 50 символов")]
        public string Firstname { get; set; }

        [DisplayName("Отчество")]
        [StringLength(50, ErrorMessage = "Максимальная длина - 50 символов")]
        public string Secondname { get; set; }

        [DisplayName("Фамилия")]
        [StringLength(50, ErrorMessage = "Максимальная длина - 50 символов")]
        public string Lastname { get; set; }

        [DisplayName("Дата рожд.")]
        public DateTime? Birthday { get; set; }

        [DisplayName("№ ВС")]
        [StringLength(9, ErrorMessage = "Максимальная длина - 9 символов")]
        public string TemporaryPolicyNumber { get; set; }

        [DisplayName("Дата выдачи ВС c")]
        public DateTime? TemporaryPolicyDateFrom { get; set; }

        [DisplayName("Дата выдачи ВС по")]
        public DateTime? TemporaryPolicyDateTo { get; set; }

        [DisplayName("Серия полиса")]
        [StringLength(9, ErrorMessage = "Максимальная длина - 9 символов")]
        public string PolicySeries { get; set; }

        [Display(Name = "Номер", Prompt = "Номер полиса")]
        [StringLength(32, ErrorMessage = "Максимальная длина - 32 символов")]
        public string PolicyNumber { get; set; }

        [DisplayName("Дата выдачи полиса с")]
        public DateTime? PolicyDateFrom { get; set; }

        [DisplayName("Дата выдачи полиса по")]
        public DateTime? PolicyDateTo { get; set; }

        [DisplayName("Статус")]
        public List<long> StatusIds { get; set; }

        [DisplayName("Сценарий")]
        public List<long> ScenarioIds { get; set; }

        [DisplayName("Пункт")]
        public List<long> DeliveryCenterIds { get; set; }

        [DisplayName("Точка")]
        public List<long> DeliveryPointIds { get; set; }

        [DisplayName("№ партии")]
        public string PartyNumber { get; set; }

        [DisplayName("Дата периода с")]
        public DateTime? UpdateDateFrom { get; set; }

        [DisplayName("Дата периода по")]
        public DateTime? UpdateDateTo { get; set; }

        [DisplayName("Дата статуса с")]
        public DateTime? StatusDateFrom { get; set; }

        [DisplayName("Дата статуса по")]
        public DateTime? StatusDateTo { get; set; }

        [DisplayName("Сотрудник")]
        public long? UserId { get; set; }

        [DisplayName("ВС")]
        public bool? IsTemporaryPolicyNumberNotEmpty { get; set; }

        [DisplayName("Готово к выгрузке")]
        public bool IsReadyToFundSubmitRequest { get; set; }

        [DisplayName("Сложные")]
        public bool IsDifficultCase { get; set; }

        [DisplayName("Актуальные")]
        public bool? IsActualInVisitGroup { get; set; }
        public bool NoIsActualInVisitGroup
        {
            get { return IsActualInVisitGroup ?? false; }
            set { IsActualInVisitGroup = value; }
        }

        public ClientVisitSearchCriteria GetClientSearchCriteria()
        {
            ClientVisitSearchCriteria criteria = new ClientVisitSearchCriteria()
            {
                Firstname = this.Firstname,
                Secondname = this.Secondname,
                Lastname = this.Lastname,
                Birthday = this.Birthday,
                PartyNumber = this.PartyNumber,
                TemporaryPolicyNumber = this.TemporaryPolicyNumber,
                TemporaryPolicyDateFrom = this.TemporaryPolicyDateFrom,
                TemporaryPolicyDateTo = this.TemporaryPolicyDateTo,
                UpdateDateFrom = this.UpdateDateFrom,
                UpdateDateTo = this.UpdateDateTo,
                PolicyDateFrom = this.PolicyDateFrom,
                PolicyDateTo = this.PolicyDateTo,
                PolicySeries = this.PolicySeries,
                PolicyNumber = this.PolicyNumber,
                StatusIds = this.StatusIds,
                ScenarioIds = this.ScenarioIds,
                DeliveryCenterIds = this.DeliveryCenterIds,
                DeliveryPointIds = this.DeliveryPointIds,
                StatusDateFrom = this.StatusDateFrom,
                StetusDateTo = this.StatusDateTo,
                IsTemporaryPolicyNumberNotEmpty = this.IsTemporaryPolicyNumberNotEmpty,
                IsReadyToFundSubmitRequest = this.IsReadyToFundSubmitRequest,
                IsActualInVisitGroup = this.IsActualInVisitGroup,
                UserId = this.UserId,
                IsDifficultCase = this.IsDifficultCase
            };
            return criteria;
        }
    }
}