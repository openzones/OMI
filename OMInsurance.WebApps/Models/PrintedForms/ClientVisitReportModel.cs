using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OMInsurance.WebApps.Models.PrintedForms
{
    public class ClientVisitReportModel
    {
        public ClientVisitReportModel()
        {
            CVDateFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            CVDateTo = DateTime.Now;
        }

        [DisplayName("Дата обращения с")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public DateTime CVDateFrom { get; set; }

        [DisplayName("Дата обращения по (включительно)")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public DateTime CVDateTo { get; set; }
    }
}