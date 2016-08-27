using System;

namespace OMInsurance.Entities.Searching
{
    public class CheckPretensionSearchCriteria
    {
        public DateTime? M_daktFrom { get; set; }
        public DateTime? M_daktTo { get; set; }
        public DateTime? CreateDateFrom { get; set; }
        public DateTime? CreateDateTo { get; set; }
        public long? UserId { get; set; }
        public long PageSize { get; set; }
    }
}
