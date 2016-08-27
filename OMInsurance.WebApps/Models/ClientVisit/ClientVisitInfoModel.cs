using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System;
using System.ComponentModel;

namespace OMInsurance.WebApps.Models
{
    public class ClientVisitInfoModel
    {
        [DisplayName("Id")]
        public long Id { get; set; }

        [DisplayName("Id")]
        public long VisitGroupId { get; set; }

        [DisplayName("Дата создания")]
        public DateTime Date { get; set; }
        
        [DisplayName("Дата статуса")]
        public DateTime StatusDate { get; set; }

        [DisplayName("Имя")]
        public string Firstname { get; set; }

        [DisplayName("Отчество")]
        public string Secondname { get; set; }

        [DisplayName("Фамилия")]
        public string Lastname { get; set; }

        [DisplayName("Дата рождения")]
        public DateTime? Birthday { get; set; }

        [DisplayName("Временное свидетельство")]
        public string TemporaryPolicyNumber { get; set; }

        [DisplayName("Дата выдачи ВС")]
        public DateTime? TemporaryPolicyDate { get; set; }

        [DisplayName("Серия полиса")]
        public string PolicySeries { get; set; }

        [DisplayName("Номер полиса")]
        public string PolicyNumber { get; set; }

        [DisplayName("Дата выдачи полиса")]
        public DateTime? PolicyNumberDate { get; set; }

        [DisplayName("ЕНП")]
        public string UnifiedPolicyNumber { get; set; }

        [DisplayName("SCN")]
        public string FundResponseApplyingMessage { get; set; }
        
        [DisplayName("Партия")]
        public string PolicyParty { get; set; }
        
        [DisplayName("СНИЛС")]
        public string SNILS { get; set; }

        [DisplayName("Пол")]
        public string Sex { get; set; }

        [DisplayName("Scn")]
        public ReferenceItem Scenario { get; set; }

        [DisplayName("Статус обращения")]
        public ReferenceItem Status { get; set; }

        [DisplayName("Сложный случай")]
        public bool IsDifficultCase { get; set; }

        public ClientVisitInfoModel()
        {
        }

        public ClientVisitInfoModel(ClientVisitInfo clientVisit)
        {
            this.Id = clientVisit.Id;
            this.VisitGroupId = clientVisit.VisitGroupId;
            this.Date = clientVisit.StatusDate;
            this.Firstname = clientVisit.Firstname;
            this.Secondname = clientVisit.Secondname;
            this.Lastname = clientVisit.Lastname;
            this.Birthday = clientVisit.Birthday;
            this.TemporaryPolicyNumber = clientVisit.TemporaryPolicyNumber;
            this.TemporaryPolicyDate = clientVisit.TemporaryPolicyDate;
            this.PolicyNumber = clientVisit.PolicyNumber;
            this.PolicySeries = clientVisit.PolicySeries;
            this.UnifiedPolicyNumber = clientVisit.UnifiedPolicyNumber;
            this.PolicyNumberDate = clientVisit.PolicyIssueDate;
            this.SNILS = clientVisit.SNILS;
            this.Secondname = clientVisit.Secondname;
            this.Status = clientVisit.Status;
            this.PolicyParty = clientVisit.PolicyParty;
            this.Sex = clientVisit.Sex;
            this.Scenario = clientVisit.Scenario;
            this.FundResponseApplyingMessage = clientVisit.FundResponseApplyingMessage;
            this.StatusDate = clientVisit.StatusDate;
            this.IsDifficultCase = clientVisit.IsDifficultCase;
        }
    }
}