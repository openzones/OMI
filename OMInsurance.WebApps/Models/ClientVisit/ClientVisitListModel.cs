using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System.Collections.Generic;
using System.Web.Mvc;
namespace OMInsurance.WebApps.Models
{
    public class ClientVisitListModel : PagedList<ClientVisitInfoModel>
    {
        public ClientVisitListModel()
        {
            this.SearchCriteriaModel = new ClientVisitSearchCriteriaModel();
            DeliveryCenters = ReferencesProvider.GetReferences(Constants.DeliveryCenterRef, null, false);
            DeliveryPoints = ReferencesProvider.GetReferences(Constants.DeliveryPointRef,null,false);
            ClientVisitStatuses = ReferencesProvider.GetReferences(Constants.ClientVisitStatusRef, null, false);
            Scenaries = ReferencesProvider.GetReferences(Constants.ScenarioRef, null, false);
            UserS = ReferencesProvider.GetUsers(null, true);
        }

        public ClientVisitListModel(User user) : this()
        {
            IsDbfDownloadAvailable = user.Roles.Contains(Role.OperatorSG) || user.Roles.Contains(Role.Administrator);
            DeliveryCenters = ReferencesProvider.GetReferences(Constants.DeliveryCenterRef, user.Roles, null, null, true);
            DeliveryPoints = ReferencesProvider.GetReferences(Constants.DeliveryPointRef, user.Roles, null, null, true);
        }

        public bool? IsDbfDownloadAvailable { get; set; }
        public List<SelectListItem> DeliveryCenters { get; set; }
        public List<SelectListItem> DeliveryPoints { get; set; }
        public List<SelectListItem> Scenaries { get; set; }
        public List<SelectListItem> ClientVisitStatuses { get; set; }
        public List<SelectListItem> UserS { get; set; }
        public ClientVisitSearchCriteriaModel SearchCriteriaModel { get; set; }
    }
}