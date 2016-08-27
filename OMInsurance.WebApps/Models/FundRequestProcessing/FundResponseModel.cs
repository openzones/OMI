
using OMInsurance.Entities.Core;
using System;
using System.ComponentModel;
namespace OMInsurance.WebApps.Models
{
    public abstract class FundResponseModel
    {
        public FundResponseModel()
        {
        }
        /// <summary>
        /// Response Identifier
        /// </summary>
        public long Id { get; set; }

        /// <summary>       
        /// Identifier that is equal to identifier of corresponding client visit
        /// </summary>
        public long ClientVisitId { get; set; }

        /// <summary>
        /// Identifier that is equal to identifier of corresponding client visit
        /// </summary>
        public long ClientVisitGroupId { get; set; }

        public DateTime CreateDate { get; set; }
    }
}