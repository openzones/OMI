using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace OMInsurance.WebApps.Models.PrintedForms
{
    public class AllocationBSOModel
    {
        public AllocationBSOModel()
        {
            AllocationBSODateFrom =  new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            AllocationBSODateTo = DateTime.Now;
            DeliveryCenters = ReferencesProvider.GetReferences(Constants.DeliveryCenterRef, null, false);
            DeliveryPoints = ReferencesProvider.GetReferences(Constants.DeliveryPointRef, null, false);
            DeliveryCenterIds = new List<long>();
            DeliveryPointIds = new List<long>();
        }

        [DisplayName("Дата обращения с")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public DateTime AllocationBSODateFrom { get; set; }

        [DisplayName("Дата обращения по (включительно)")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public DateTime AllocationBSODateTo { get; set; }

        [DisplayName("Номер партии")]
        public string PartyNumber { get; set; }

        [DisplayName("Пункт выдачи")]
        public List<long> DeliveryCenterIds { get; set; }
        public List<SelectListItem> DeliveryCenters { get; set; }

        [DisplayName("Точка выдачи")]
        public List<long> DeliveryPointIds { get; set; }
        public List<SelectListItem> DeliveryPoints { get; set; }
    }
}