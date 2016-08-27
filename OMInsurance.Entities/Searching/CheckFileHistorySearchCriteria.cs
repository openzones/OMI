using System;

namespace OMInsurance.Entities.Searching
{
    public class CheckFileHistorySearchCriteria
    {
        public long? StatusId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public long? UserId { get; set; }
        public long PageSize { get; set; }
    }
}
