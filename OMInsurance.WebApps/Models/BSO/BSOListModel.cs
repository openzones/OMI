using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System.Collections.Generic;
using System.Web.Mvc;
using OMInsurance.BusinessLogic;
using OMInsurance.Interfaces;
using System;

namespace OMInsurance.WebApps.Models
{
    public class BSOListModel : PagedList<BSOBaseInfoModel>
    {
        public BSOListModel()
        {
            this.SearchCriteriaModel = new BSOSearchCriteriaModel();
            DeliveryCenters = ReferencesProvider.GetReferences(Constants.DeliveryCenterForOperatorRef, null, false);
            DeliveryPoints = ReferencesProvider.GetReferences(Constants.DeliveryPointRef, null, true);
            listBSOStatuses = StatusBSOProvider.GetBSOListStatus(true);
            listBSOStatusesAvailable = StatusBSOProvider.GetAvailableBSOStatus(null, true);
            listBSOResponsibles = StatusBSOProvider.GetListBSOResponsibles(true);
        }

        public BSOSearchCriteriaModel SearchCriteriaModel { get; set; }
        public List<SelectListItem> DeliveryCenters { get; set; }
        public List<SelectListItem> DeliveryPoints { get; set; }
        public List<SelectListItem> listBSOStatuses { get; set; }
        public List<SelectListItem> listBSOStatusesAvailable { get; set; }
        public List<SelectListItem> listBSOResponsibles { get; set; }

        //используется для передачи количества накладных
        public long FlagPrintReport { get; set; }
        public string SortField { get; set; }
        public string SortOrder { get; set; }

        public BSOListModel GetAvailableBSOStatus(long? statusId, bool withDefaultEmpty = false)
        {
            this.listBSOStatusesAvailable =  StatusBSOProvider.GetAvailableBSOStatus(statusId, withDefaultEmpty);
            return this;
        }
    }
}