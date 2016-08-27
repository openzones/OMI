using OMInsurance.Entities.Core;
using System.Collections.Generic;
using System.Web.Mvc;

namespace OMInsurance.WebApps.Models
{
    public class FundRequestIndexModel
    {
        #region Properties

        public List<ClientVisitFundInfoModel> ClientVisits { get; set; }
        public List<FundResponseModel> FundResponses { get; set; }
        public List<SelectListItem> DeliveryCenters { get; set; }
        public List<SelectListItem> DeliveryPoints { get; set; }
        public List<SelectListItem> ClientVisitStatuses { get; set; }
        public List<SelectListItem> ScenarioList { get; set; }
        public ClientVisitSearchCriteriaModel SearchCriteriaModel { get; set; }
        public List<SelectListItem> UserS { get; set; }

        #endregion

        #region Constructors

        public FundRequestIndexModel()
        {
            ClientVisits = new List<ClientVisitFundInfoModel>();
            FundResponses = new List<FundResponseModel>();
            DeliveryCenters = ReferencesProvider.GetReferences(Constants.DeliveryCenterForOperatorRef, null, true);
            DeliveryPoints = ReferencesProvider.GetReferences(Constants.DeliveryPointRef, null, false);
            ClientVisitStatuses = ReferencesProvider.GetReferences(Constants.ClientVisitStatusRef, null, true);
            SearchCriteriaModel = new ClientVisitSearchCriteriaModel();
            ScenarioList = ReferencesProvider.GetReferences(Constants.ScenarioRef, null, true);
            UserS = ReferencesProvider.GetUsers(null, true);
        }

        #endregion
    }
}