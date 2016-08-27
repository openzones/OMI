using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
namespace OMInsurance.Entities.Searching
{
    public class ClientVisitSearchCriteria
    {
        public ClientVisitSearchCriteria()
        {
            StatusIds = new List<long>();
            ScenarioIds = new List<long>();
            DeliveryCenterIds = new List<long>();
            DeliveryPointIds = new List<long>();
        }

        #region Properties

        public long? Id { get; set; }
        public string Firstname { get; set; }
        public string Secondname { get; set; }
        public string Lastname { get; set; }
        public DateTime? Birthday { get; set; }
        public string UnifiedPolicyNumber { get; set; }
        public string SNILS { get; set; }
        public string PartyNumber { get; set; }
        public long? DocumentTypeId { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentSeries { get; set; }
        public string TemporaryPolicyNumber { get; set; }
        public DateTime? TemporaryPolicyDateFrom { get; set; }
        public DateTime? TemporaryPolicyDateTo { get; set; }
        public string PolicySeries { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime? PolicyDateFrom { get; set; }
        public DateTime? PolicyDateTo { get; set; }
        public List<long> StatusIds { get; set; }
        public List<long> ScenarioIds { get; set; }
        public List<long> DeliveryCenterIds { get; set; }
        public List<long> DeliveryPointIds { get; set; }
        public bool? IsActualInVisitGroup { get; set; }
        public DateTime? UpdateDateFrom { get; set; }
        public DateTime? UpdateDateTo { get; set; }
        public DateTime? StatusDateFrom { get; set; }
        public DateTime? StetusDateTo { get; set; }
        public long? UserId { get; set; }
        public bool? IsTemporaryPolicyNumberNotEmpty { get; set; }
        public bool IsReadyToFundSubmitRequest { get; set; }
        public bool IsDifficultCase { get; set; }

        #endregion

    }
}
