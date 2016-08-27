using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace OMInsurance.WebApps.Models.PrintedForms
{
    public class SNILSReportModel
    {
        public SNILSReportModel()
        {
            DateSnilsFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateSnilsTo = DateTime.Now;
            DeliveryPoints = ReferencesProvider.GetReferences(Constants.DeliveryPointRef, null, false);
            DeliveryPointIds = new List<long>();
        }

        [DisplayName("Дата статуса с")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public DateTime DateSnilsFrom { get; set; }

        [DisplayName("Дата статуса по (включительно)")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public DateTime DateSnilsTo { get; set; }

        [DisplayName("Точка выдачи")]
        public List<long> DeliveryPointIds { get; set; }
        public List<SelectListItem> DeliveryPoints { get; set; }

        [DisplayName("СНИЛС")]
        public bool? IsSnilsNotEmpty { get; set; }
    }
}