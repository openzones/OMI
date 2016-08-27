using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace OMInsurance.WebApps.Models
{
    public class ReferenceChoiceModel
    {
        public ReferenceChoiceModel()
        {
            ReferencesDisplayName = ReferencesProvider.GetReferencesDisplayName(true);
            References = ReferencesProvider.GetReferences(Constants.ReferenceRef, null, true);
        }

        [DisplayName("Справочник")]
        public long? ReferenceId { get; set; }
        public string ReferenceName { get; set; }
        public List<SelectListItem> References { get; set; }
        public List<SelectListItem> ReferencesDisplayName { get; set; }

    }
}