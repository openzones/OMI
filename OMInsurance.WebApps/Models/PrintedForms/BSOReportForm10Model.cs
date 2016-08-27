using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace OMInsurance.WebApps.Models.PrintedForms
{
    public class BSOReportForm10Model
    {
        public BSOReportForm10Model()
        {
            DateForm10From = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateForm10To = DateTime.Now;
            DeliveryPoints = ReferencesProvider.GetReferences(Constants.DeliveryPointRef, null, false);
            DeliveryPointIds = new List<long>();
        }

        [DisplayName("Дата статуса с")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public DateTime DateForm10From { get; set; }

        [DisplayName("Дата статуса по (включительно)")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public DateTime DateForm10To { get; set; }

        [DisplayName("Точка выдачи")]
        public List<long> DeliveryPointIds { get; set; }
        public List<SelectListItem> DeliveryPoints { get; set; }
    }
}