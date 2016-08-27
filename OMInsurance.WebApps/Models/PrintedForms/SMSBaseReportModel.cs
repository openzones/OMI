using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OMInsurance.WebApps.Models.PrintedForms
{
    public class SMSBaseReportModel
    {
        public SMSBaseReportModel()
        {
            SmsBaseDateFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            SmsBaseDateTo = DateTime.Now;
        }

        [DisplayName("Дата создания SMS с")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public DateTime SmsBaseDateFrom { get; set; }

        [DisplayName("Дата создания SMS по (включительно)")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public DateTime SmsBaseDateTo { get; set; }
    }
}