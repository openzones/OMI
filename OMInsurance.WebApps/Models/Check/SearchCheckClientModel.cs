using OMInsurance.Entities;
using OMInsurance.Entities.Check;
using OMInsurance.Entities.Core;
using OMInsurance.Entities.Searching;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Web.Mvc;

namespace OMInsurance.WebApps.Models
{
    /// <summary>
    /// Используется только для ПОИСКА
    /// </summary>
    public class SearchCheckClientModel
    {
        public SearchCheckClientModel()
        {
            //параметры по умолчанию
            IsLastname = true;
            IsFirstname = true;
            IsSecondname = true;
            IsBirthday = true;
        }

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

        public CheckClientSearchCriteria GetCheckClientSearchCriteria()
        {
            CheckClientSearchCriteria criteria = new CheckClientSearchCriteria()
            {
                IsLastname = this.IsLastname,
                IsFirstname = this.IsFirstname,
                IsSecondname = this.IsSecondname,
                IsBirthday = this.IsBirthday,
                IsSex = this.IsSex,
                IsPolicySeries = this.IsPolicySeries,
                IsPolicyNumber = this.IsPolicyNumber,
                IsUnifiedPolicyNumber = this.IsUnifiedPolicyNumber,
                IsDocumentSeries = this.IsDocumentSeries,
                IsDocumentNumber = this.IsDocumentNumber
            };

            return criteria;
        }
    }
}