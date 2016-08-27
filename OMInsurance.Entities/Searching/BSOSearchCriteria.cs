using System;
using System.Collections.Generic;

namespace OMInsurance.Entities.Searching
{
    public class BSOSearchCriteria
    {
        public BSOSearchCriteria()
        {
            StatusIds = new List<long>();
            DeliveryCenterIds = new List<long>();
            DeliveryPointIds = new List<long>();
            ResponsibleIDs = new List<long>();
        }

        public long BSO_ID { get; set; }
        public string TemporaryPolicyNumberFrom { get; set; }
        public string TemporaryPolicyNumberTo { get; set; }
        public string PolicyPartyNumber { get; set; }
        public long? StatusId { get; set; }
        public DateTime? StatusDateFrom { get; set; }
        public DateTime? StatusDateTo { get; set; }
        public List<long> DeliveryCenterIds { get; set; }
        public List<long> DeliveryPointIds { get; set; }
        public long? ResponsibleID { get; set; }    
        public DateTime? ChangeDateFrom { get; set; }
        public DateTime? ChangeDateTo { get; set; }

        public List<long> StatusIds { get; set; }
        public List<long> ResponsibleIDs { get; set; }
    }
}
