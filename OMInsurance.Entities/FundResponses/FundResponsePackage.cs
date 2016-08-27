using System;
using System.Collections.Generic;

namespace OMInsurance.Entities
{
    public class FundResponsePackage
    {
        public DateTime ImportDate { get; set; }
        public List<FundResponse> Responses { get; set; }

        public FundResponsePackage()
        {
            Responses = new List<FundResponse>();
        }
    }
}
