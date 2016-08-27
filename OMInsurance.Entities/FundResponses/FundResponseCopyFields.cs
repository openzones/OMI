using System.Collections.Generic;

namespace OMInsurance.Entities
{
    public class FundResponseCopyFields
    {
        public long ResponseId { get; set; }
        public long ClientVisitGroupId { get; set; }
        public List<FundResponseFields> OldFields { get; set; }
        public List<FundResponseFields> NewFields { get; set; }

        public FundResponseCopyFields()
        {
            OldFields = new List<FundResponseFields>();
            NewFields = new List<FundResponseFields>();
        }
    }
}
