using OMInsurance.Entities;
using OMInsurance.Entities.Check;
using OMInsurance.Entities.Core;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Web.Mvc;

namespace OMInsurance.WebApps.Models
{
    public class CheckClientModel
    {
        public CheckClientModel()
        {
        }

        public bool IsHide { get; set; }

        //ClientID
        public long Id { get; set; }

        [DisplayName("Имя")]
        public string Firstname { get; set; }

        [DisplayName("Отчество")]
        public string Secondname { get; set; }

        [DisplayName("Фамилия")]
        public string Lastname { get; set; }

        [DisplayName("Дата рождения")]
        public DateTime? Birthday { get; set; }

        [DisplayName("Пол")]
        public string Sex { get; set; }

        [DisplayName("Серия")]
        public string PolicySeries { get; set; }

        [DisplayName("Номер")]
        public string PolicyNumber { get; set; }

        [DisplayName("ЕНП")]
        public string UnifiedPolicyNumber { get; set; }

        [DisplayName("Серия паспорта")]
        public string DocumentSeries { get; set; }

        [DisplayName("Номер паспорта")]
        public string DocumentNumber { get; set; }

        [DisplayName("Адрес проживания")]
        public string LivingFullAddressString { get; set; }

        [DisplayName("Адрес регистрации")]
        public string OfficialFullAddressString { get; set; }

        [DisplayName("ВС(БСО)")]
        public string TemporaryPolicyNumber { get; set; }

        [DisplayName("Дата обращения")]
        public DateTime? TemporaryPolicyDate { get; set; }

        [DisplayName("СНИЛС")]
        public string SNILS { get; set; }

        [DisplayName("Телефон")]
        public string Phone { get; set; }

        public CheckClientModel(CheckClient check)
        {
            Id = check.Id;
            Firstname = check.Firstname;
            Secondname = check.Secondname;
            Lastname = check.Lastname;
            Birthday = check.Birthday;
            Sex = check.Sex;
            PolicySeries = check.PolicySeries;
            PolicyNumber = check.PolicyNumber;
            UnifiedPolicyNumber = check.UnifiedPolicyNumber;
            DocumentSeries = check.DocumentSeries;
            DocumentNumber = check.DocumentNumber;
            LivingFullAddressString = check.LivingFullAddressString;
            OfficialFullAddressString = check.OfficialFullAddressString;
            TemporaryPolicyNumber = check.TemporaryPolicyNumber;
            TemporaryPolicyDate = check.TemporaryPolicyDate;
            SNILS = check.SNILS;
            Phone = check.Phone;
        }
    }
}