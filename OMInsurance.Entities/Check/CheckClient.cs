using OMInsurance.Entities.Core;
using OMInsurance.Entities.SMS;
using System;
using System.Collections.Generic;

namespace OMInsurance.Entities.Check
{
    public class CheckClient : DataObject
    {
        public string Firstname { get; set; }
        public string Secondname { get; set; }
        public string Lastname { get; set; }
        public DateTime? Birthday { get; set; }
        public string Sex { get; set; }
        public string PolicySeries { get; set; }
        public string PolicyNumber { get; set; }
        public string UnifiedPolicyNumber { get; set; }
        public string DocumentSeries { get; set; }
        public string DocumentNumber { get; set; }
        public string LivingFullAddressString { get; set; }
        public string OfficialFullAddressString { get; set; }
        public string TemporaryPolicyNumber { get; set; }
        public DateTime? TemporaryPolicyDate { get; set; }
        public string SNILS { get; set; }
        public string Phone { get; set; }
    }
}
