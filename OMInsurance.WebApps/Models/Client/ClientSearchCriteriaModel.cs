using OMInsurance.Entities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OMInsurance.WebApps.Models
{
    public class ClientSearchCriteriaModel
    {
        public ClientSearchCriteriaModel(ClientSearchCriteria criteria)
        {
            this.Firstname = criteria.Firstname;
            this.Secondname = criteria.Secondname;
            this.Lastname = criteria.Lastname;
            this.Birthday = criteria.Birthday;
            this.TemporaryPolicyNumber = criteria.TemporaryPolicyNumber;
            this.PolicySeries = criteria.PolicySeries;
            this.PolicyNumber = criteria.PolicyNumber;
            this.PolicyDateFrom = criteria.PolicyDateFrom;
            this.PolicyDateTo = criteria.PolicyDateTo;
            this.TemporaryPolicyDateTo = criteria.TemporaryPolicyDateTo;
            this.TemporaryPolicyDateFrom = criteria.TemporaryPolicyDateFrom;
            this.PolicyNumber = criteria.PolicyNumber;
            this.UnifiedPolicyNumber = criteria.UnifiedPolicyNumber;
        }

        public ClientSearchCriteriaModel()
        {
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

        [DisplayName("Дата рождения")]
        public DateTime? Birthday { get; set; }

        [DisplayName("Номер ВС")]
        [StringLength(9, ErrorMessage = "Максимальная длина - 9 символов")]
        public string TemporaryPolicyNumber { get; set; }

        [DisplayName("Дата выдачи временного свидетельства с ")]
        public DateTime? TemporaryPolicyDateFrom { get; set; }

        [DisplayName("Дата выдачи временного свидетельства по ")]
        public DateTime? TemporaryPolicyDateTo { get; set; }

        [DisplayName("Серия полиса")]
        [StringLength(12, ErrorMessage = "Максимальная длина - 12 символов")]
        public string PolicySeries { get; set; }

        [DisplayName("Номер полиса")]
        [StringLength(32, ErrorMessage = "Максимальная длина - 32 символов")]
        public string PolicyNumber { get; set; }

        [DisplayName("Дата выдачи полиса с")]
        public DateTime? PolicyDateFrom { get; set; }

        [DisplayName("Дата выдачи полиса по")]
        public DateTime? PolicyDateTo { get; set; }

        [DisplayName("ЕНП")]
        [StringLength(16, ErrorMessage = "Максимальная длина - 16 символов")]
        public string UnifiedPolicyNumber { get; set; }

        public ClientSearchCriteria GetClientSearchCriteria()
        {
            ClientSearchCriteria criteria = new ClientSearchCriteria()
            {
                Birthday = this.Birthday,
                Firstname = this.Firstname,
                Lastname = this.Lastname,
                PolicySeries = this.PolicySeries,
                PolicyNumber = this.PolicyNumber,
                PolicyDateFrom = this.PolicyDateFrom,
                PolicyDateTo = this.PolicyDateTo,
                Secondname = this.Secondname,
                TemporaryPolicyDateTo = this.TemporaryPolicyDateTo,
                TemporaryPolicyDateFrom = this.TemporaryPolicyDateFrom,
                TemporaryPolicyNumber = this.TemporaryPolicyNumber,
                UnifiedPolicyNumber = this.UnifiedPolicyNumber
            };
            return criteria;
        }
    }
}