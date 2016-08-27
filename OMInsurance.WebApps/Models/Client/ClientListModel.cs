namespace OMInsurance.WebApps.Models
{
    public class ClientListModel : PagedList<ClientBaseInfoModel>
    {
        public ClientListModel()
        {
            this.SearchCriteriaModel = new ClientSearchCriteriaModel();
        }
        
        public ClientSearchCriteriaModel SearchCriteriaModel { get; set; }
    }
}