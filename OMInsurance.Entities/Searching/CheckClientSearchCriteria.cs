using System;
using System.Collections.Generic;

namespace OMInsurance.Entities.Searching
{
    public class CheckClientSearchCriteria
    {
        public CheckClientSearchCriteria()
        {
        }

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
    }
}
