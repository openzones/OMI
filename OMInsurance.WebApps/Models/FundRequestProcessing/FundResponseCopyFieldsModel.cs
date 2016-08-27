using OMInsurance.Entities;
using System.Collections.Generic;

namespace OMInsurance.WebApps.Models
{
    public class FundResponseCopyFieldsModel
    {
        public long ResponseId { get; set; }
        public long ClientVisitGroupId { get; set; }
        public List<FundResponseFields> OldFields { get; set; }
        public List<FundResponseFields> NewFields { get; set; }

        public FundResponseCopyFieldsModel()
        {
            OldFields = new List<FundResponseFields>();
            NewFields = new List<FundResponseFields>();
        }

        public FundResponseCopyFields GetForBll()
        {
            FundResponseCopyFields data = new FundResponseCopyFields();
            data.ClientVisitGroupId = this.ClientVisitGroupId;
            data.NewFields = this.NewFields;
            data.OldFields = this.OldFields;
            data.ResponseId = this.ResponseId;
            return data;
        }
    }
}