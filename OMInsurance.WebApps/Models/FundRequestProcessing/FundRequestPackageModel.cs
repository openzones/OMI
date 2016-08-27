using System;
using System.Collections.Generic;

namespace OMInsurance.WebApps.Models
{
    public class FundResponsePackageModel
    {
        public DateTime ImportDate { get; set; }
        public List<FundResponseModel> Responses { get; set; }
    }
}