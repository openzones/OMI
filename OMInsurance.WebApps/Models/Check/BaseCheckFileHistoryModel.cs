using OMInsurance.Entities.Core;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace OMInsurance.WebApps.Models
{
    public class BaseCheckFileHistoryModel
    {
        public BaseCheckFileHistoryModel()
        {
            ListFundFileHistoryModel = new List<FundFileHistoryModel>();
            Search = new SearchCheckFileHistoryModel();
            listStatuses = ReferencesProvider.GetReferences(Constants.FundFileHistoryStatusRef, null, true);
            listUsers = ReferencesProvider.GetUsers(null, true);
            PageSize = 100;
        }

        public List<FundFileHistoryModel> ListFundFileHistoryModel { get; set; }
        public SearchCheckFileHistoryModel Search { get; set; }

        public List<SelectListItem> listStatuses { get; set; }
        public List<SelectListItem> listUsers { get; set; }

        [DisplayName("Отображать")]
        public long PageSize { get; set; }
    }
}