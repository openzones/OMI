using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMInsurance.Entities.Check
{
    public class ViewColumn
    {
        public bool IsId { get; set; }
        public bool IsLastname { get; set; }
        public bool IsFirstname { get; set; }
        public bool IsSecondname { get; set; }
        public bool IsBirthday { get; set; }
        public bool IsSex { get; set; }
        public bool IsPolicySeries { get; set; }
        public bool IsPolicyNumber { get; set; }
        public bool IsUnifiedPolicyNumber { get; set; }
        public bool IsDocumentSeries { get; set; }
        public bool IsDocumentNumber { get; set; }
        public bool IsLivingFullAddressString { get; set; }
        public bool IsOfficialFullAddressString { get; set; }
        public bool IsTemporaryPolicyNumber { get; set; }
        public bool IsTemporaryPolicyDate { get; set; }
        public bool IsSNILS { get; set; }
        public bool IsPhone { get; set; }
    }
}
