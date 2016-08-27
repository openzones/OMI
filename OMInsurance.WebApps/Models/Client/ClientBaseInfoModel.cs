using OMInsurance.Entities;
using System;
using System.ComponentModel;

namespace OMInsurance.WebApps.Models
{
    public class ClientBaseInfoModel
    {
        #region Properties

        [DisplayName("Id")]
        public long Id { get; set; }

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

        [DisplayName("ЕНП")]
        public string UnifiedPolicyNumber { get; set; }

        [DisplayName("Дата выдачи ВС")]
        public DateTime? TemporaryPolicyDate { get; set; }

        [DisplayName("Номер полиса")]
        public string PolicyNumber { get; set; }

        [DisplayName("Серия полиса")]
        public string PolicySeries { get; set; }

        [DisplayName("Дата выдачи полиса")]
        public DateTime? PolicyNumberDate { get; set; }

        #endregion

        #region Constructors

        public ClientBaseInfoModel()
        {
        }

        public ClientBaseInfoModel(ClientBaseInfo baseInfo)
        {
            this.Id = baseInfo.Id;
            this.Firstname = baseInfo.Firstname;
            this.Secondname = baseInfo.Secondname;
            this.Lastname = baseInfo.Lastname;
            this.Birthday = baseInfo.Birthday;
            this.TemporaryPolicyNumber = baseInfo.TemporaryPolicyNumber;
            this.TemporaryPolicyDate = baseInfo.TemporaryPolicyDate;
            this.PolicyNumber = baseInfo.PolicyNumber;
            this.UnifiedPolicyNumber = baseInfo.UnifiedPolicyNumber;
            this.PolicySeries = baseInfo.PolicySeries;
            this.PolicyNumberDate = baseInfo.PolicyDate;
        }

        #endregion

        public ClientBaseInfo GetClientBaseInfo()
        {
            ClientBaseInfo baseInfo = new ClientBaseInfo();
            baseInfo.Id = this.Id;
            baseInfo.Firstname = this.Firstname;
            baseInfo.Secondname = this.Secondname;
            baseInfo.Lastname = this.Lastname;
            baseInfo.Birthday = this.Birthday;
            baseInfo.TemporaryPolicyNumber = this.TemporaryPolicyNumber;
            baseInfo.TemporaryPolicyDate = this.TemporaryPolicyDate;
            baseInfo.UnifiedPolicyNumber = this.UnifiedPolicyNumber;
            baseInfo.PolicySeries = this.PolicySeries;
            baseInfo.PolicyNumber = this.PolicyNumber;
            baseInfo.PolicyDate = this.PolicyNumberDate;

            return baseInfo;
        }

    }
}