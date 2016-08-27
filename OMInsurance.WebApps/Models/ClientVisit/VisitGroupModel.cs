using System;
using System.Collections.Generic;

namespace OMInsurance.WebApps.Models
{
    public class VisitGroupModel : IEqualityComparer<VisitGroupModel>
    {
        public long Id { get; set; }
        public string ClientFullname { get; set; }
        public string Status { get; set; }
        public DateTime? TemporaryPolicyDate { get; set; }
        public DateTime StatusDate { get; set; }
        
        public bool Equals(VisitGroupModel x, VisitGroupModel y)
        {
            return x.Id == y.Id ;
        }

        public int GetHashCode(VisitGroupModel obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}