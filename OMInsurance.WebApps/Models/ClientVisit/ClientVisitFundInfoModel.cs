using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System;
using System.ComponentModel;

namespace OMInsurance.WebApps.Models
{
    public class ClientVisitFundInfoModel
    {
        #region Properties
        
        [DisplayName("Номер заявки")]
        public long Id { get; set; }

        [DisplayName("Имя")]
        public string Firstname { get; set; }

        [DisplayName("Отчество")]
        public string Secondname { get; set; }

        [DisplayName("Фамилия")]
        public string Lastname { get; set; }

        [DisplayName("ДР")]
        public DateTime? Birthday { get; set; }

        [DisplayName("Пол")]
        public string Sex { get; set; }

        [DisplayName("Пункт выдачи")]
        public ReferenceItem DeliveryCenter { get; set; }

        [DisplayName("Номер партии")]
        public string PolicyPartyNumber { get; set; }

        [DisplayName("Серия полиса")]
        public string PolicySeries { get; set; }

        [DisplayName("Номер полиса")]
        public string PolicyNumber { get; set; }

        [DisplayName("ЕНП")]
        public string UnifiedPolicyNumber { get; set; }

        [DisplayName("")]
        public string FundResponseApplyingMessage { get; set; }
        public bool IsReadyToFundSubmitRequest { get; set; }
        public bool IsDifficultCase { get; set; }

        [DisplayName("Статус")]
        public ReferenceItem Status { get; set; }

        [DisplayName("SCN")]
        public ReferenceItem Scenario { get; set; }

        [DisplayName("Дата статуса")]
        public DateTime? StatusDate { get; set; }

        [DisplayName("ФИО")]
        public string Fullname
        {
            get
            {
                return string.Format("{0} {1} {2}",
                    Lastname ?? string.Empty,
                    Firstname ?? string.Empty,
                    Secondname ?? string.Empty);
            }
        }

        #endregion

        #region Constructors
        public ClientVisitFundInfoModel()
        {
        }

        public ClientVisitFundInfoModel(ClientVisitInfo item)
        {
            Id = item.Id;
            Firstname = item.Firstname;
            Secondname = item.Secondname;
            Lastname = item.Lastname;
            Lastname = item.Lastname;
            PolicyNumber = item.PolicyNumber;
            PolicySeries = item.PolicySeries;
            PolicyPartyNumber = item.PolicyParty;
            UnifiedPolicyNumber = item.UnifiedPolicyNumber;
            FundResponseApplyingMessage = item.FundResponseApplyingMessage;
            IsReadyToFundSubmitRequest = item.IsReadyToFundSubmitRequest;
            IsDifficultCase = item.IsDifficultCase;
            Birthday = item.Birthday;
            Sex = item.Sex;
            Status = item.Status;
            Scenario = item.Scenario;
            StatusDate = item.StatusDate;
            DeliveryCenter = item.DeliveryCenter;
        }
        #endregion
    }
}