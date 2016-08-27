using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OMInsurance.WebApps.Models
{
    public class FundResponseUploadReportModel
    {
        [Display(Name="Заявка")]
        public long ClientVisitId { get; set; }

        [Display(Name = "RECID")]
        public long? Recid { get; set; }

        [Display(Name = "Тип")]
        public string ResponseTypeName { get; set; }

        [Display(Name = "Результат")]
        public string UploadResult { get; set; }

        [Display(Name = "Идентификатор клиента")]
        public long ClientId { get; set; }

        [Display(Name = "ФИО")]
        public string Fullname { get; set; }
        
        [Display(Name = "Пол")]
        public string Sex { get; set; }

        [Display(Name = "Дата рождения")]
        public DateTime? Birthday { get; set; }

        [Display(Name = "ВС")]
        public string TemporaryPolicyNumber { get; set; }

        [Display(Name = "Дата ВС")]
        public DateTime? TemporaryPolicyDate { get; set; }

        [Display(Name = "Серия полиса")]
        public string PolicySeries { get; set; }

        [Display(Name = "Номер")]
        public string PolicyNumber { get; set; }

        [Display(Name = "ЕНП")]
        public string UnifiedPolicyNumber { get; set; }

        [Display(Name = "Статус")]
        public ReferenceItem Status { get; set; }

        [Display(Name = "Дата статуса")]
        public DateTime StatusDate { get; set; }

        [Display(Name = "Партия")]
        public string PolicyParty { get; set; }

        [Display(Name = "Пункт выдачи")]
        public ReferenceItem DeliveryCenter { get; set; }
    }
}