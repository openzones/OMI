using System;

namespace OMInsurance.Entities
{
    public class FundRequestRecid
    {
        public long ClientVisitId { get; set; }
        public long Recid { get; set; }
        public int DataTypeId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
