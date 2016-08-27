using OMInsurance.Entities.Core;
using System;

namespace OMInsurance.Entities
{
    public class ClientBaseInfo : DataObject
    {
        #region Properties

        public string Firstname { get; set; }
        public string Secondname { get; set; }
        public string Lastname { get; set; }
        public DateTime? Birthday { get; set; }
        public string TemporaryPolicyNumber { get; set; }
        public DateTime? TemporaryPolicyDate { get; set; }
        public string PolicySeries { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime? PolicyDate { get; set; }
        public string UnifiedPolicyNumber { get; set; }

        #endregion

        #region Constructors

        #endregion
    }
}
