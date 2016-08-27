using OMInsurance.Entities.Core;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace OMInsurance.WebApps.Models
{
    public class BaseCheckPretensionModel
    {
        public BaseCheckPretensionModel()
        {
            ListClientPretensionModel = new List<ClientPretensionModel>();
            Search = new SearchCheckPretensionModel();
            listUsers = ReferencesProvider.GetUsers(null, true);
            PageSize = 100;
        }

        public List<ClientPretensionModel> ListClientPretensionModel { get; set; }
        public SearchCheckPretensionModel Search { get; set; }
        public List<SelectListItem> listUsers { get; set; }

        [DisplayName("Отображать")]
        public long PageSize { get; set; }
    }
}