using System;
using System.Collections.Generic;
using System.ComponentModel;
using OMInsurance.Entities.Check;
using System.Linq;
using System.Web;

namespace OMInsurance.WebApps.Models
{
    /// <summary>
    /// Используется для отображения данных
    /// </summary>
    public class ViewColumnModel
    {
        public ViewColumnModel()
        {
            IsId = true;
            IsLastname = true;
            IsFirstname = true;
            IsSecondname = true;
            IsBirthday = true;
            IsSex = true;
            IsPolicySeries = true;
            IsPolicyNumber = true;
            IsUnifiedPolicyNumber = true;
            IsDocumentSeries = true;
            IsDocumentNumber = true;

            IsSNILS = false;
            IsLivingFullAddressString = false;
            IsOfficialFullAddressString = false;
            IsTemporaryPolicyNumber = false;
            IsTemporaryPolicyDate = false;
            IsPhone = false;
        }

        [DisplayName("Id")]
        public bool IsId { get; set; }

        [DisplayName("Фамилия")]
        public bool IsLastname { get; set; }

        [DisplayName("Имя")]
        public bool IsFirstname { get; set; }

        [DisplayName("Отчество")]
        public bool IsSecondname { get; set; }

        [DisplayName("Дата рождения")]
        public bool IsBirthday { get; set; }

        [DisplayName("Пол")]
        public bool IsSex { get; set; }

        [DisplayName("Серия полиса")]
        public bool IsPolicySeries { get; set; }

        [DisplayName("Номер полиса")]
        public bool IsPolicyNumber { get; set; }

        [DisplayName("ЕНП")]
        public bool IsUnifiedPolicyNumber { get; set; }

        [DisplayName("Серия паспорта")]
        public bool IsDocumentSeries { get; set; }

        [DisplayName("Номер паспорта")]
        public bool IsDocumentNumber { get; set; }

        [DisplayName("Адрес проживания")]
        public bool IsLivingFullAddressString { get; set; }

        [DisplayName("Адрес регистрации")]
        public bool IsOfficialFullAddressString { get; set; }

        [DisplayName("ВС(БСО)")]
        public bool IsTemporaryPolicyNumber { get; set; }

        [DisplayName("Дата обращения")]
        public bool IsTemporaryPolicyDate { get; set; }

        [DisplayName("СНИЛС")]
        public bool IsSNILS { get; set; }

        [DisplayName("Телефон")]
        public bool IsPhone { get; set; }

        public ViewColumn GetViewColumn()
        {
            ViewColumn data = new ViewColumn()
            {
                IsId = this.IsId,
                IsLastname = this.IsLastname,
                IsFirstname = this.IsFirstname,
                IsSecondname = this.IsSecondname,
                IsBirthday = this.IsBirthday,
                IsSex = this.IsSex,
                IsPolicySeries = this.IsPolicySeries,
                IsPolicyNumber = this.IsPolicyNumber,
                IsUnifiedPolicyNumber = this.IsUnifiedPolicyNumber,
                IsDocumentSeries = this.IsDocumentSeries,
                IsDocumentNumber = this.IsDocumentNumber,
                IsSNILS = this.IsSNILS,
                IsLivingFullAddressString = this.IsLivingFullAddressString,
                IsOfficialFullAddressString = this.IsOfficialFullAddressString,
                IsTemporaryPolicyNumber = this.IsTemporaryPolicyNumber,
                IsTemporaryPolicyDate = this.IsTemporaryPolicyDate,
                IsPhone = this.IsPhone
            };

            return data;
        }
    }
}