using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace OMInsurance.WebApps.Models.PrintedForms
{
    public class StatusReportModel
    {
        public StatusReportModel()
        {
            StatusDateFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            StatusDateTo = DateTime.Now;
            ClientVisitStatuses = ReferencesProvider.GetReferences(Constants.ClientVisitStatusRef, null, false);
            StatusIds = new List<long>();
        }

        [DisplayName("Дата статуса с")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public DateTime StatusDateFrom { get; set; }

        [DisplayName("Дата статуса по (включительно)")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public DateTime StatusDateTo { get; set; }

        [DisplayName("Статус")]
        public List<long> StatusIds { get; set; }
        public List<SelectListItem> ClientVisitStatuses { get; set; }


    }
}