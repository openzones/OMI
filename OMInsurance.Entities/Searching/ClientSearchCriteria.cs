using System;

namespace OMInsurance.Entities
{
    public class ClientSearchCriteria
    {
        #region Properties

        public long Id { get; set; }
        public string Firstname { get; set; }
        public string Secondname { get; set; }
        public string Lastname { get; set; }
        public DateTime? Birthday { get; set; }
        public string TemporaryPolicyNumber { get; set; }
        public DateTime? TemporaryPolicyDateFrom { get; set; }
        public DateTime? TemporaryPolicyDateTo { get; set; }
        public string PolicySeries { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime? PolicyDateFrom { get; set; }
        public DateTime? PolicyDateTo { get; set; }
        public string UnifiedPolicyNumber { get; set; }

        #endregion

    }
}
