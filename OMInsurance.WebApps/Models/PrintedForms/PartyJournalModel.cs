using OMInsurance.Entities.Core;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace OMInsurance.WebApps.Models.PrintedForms
{
    public class PartyJournalModel
    {
        #region Constructors

        public PartyJournalModel()
        {
            DeliveryCenters = ReferencesProvider.GetReferences(Constants.DeliveryCenterRef, null, true);
            DeliveryPoints = ReferencesProvider.GetReferences(Constants.DeliveryPointRef, null, true);
        }

        #endregion

        #region Properties

        [DisplayName("Номер партии")]
        public string PartyNumber { get; set; }

        [DisplayName("Пункт выдачи")]
        public long? DeliveryCenterId { get; set; }
        public List<SelectListItem> DeliveryCenters { get; set; }

        [DisplayName("Точка выдачи")]
        public long? DeliveryPointId { get; set; }
        public List<SelectListItem> DeliveryPoints { get; set; }

        #endregion
    }
}