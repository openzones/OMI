using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace OMInsurance.Entities
{
    public class S5FundResponse : ReconciliationFundResponse
    {
        public const string Name = "s5";
     
        public override void Apply(
            ClientVisit.SaveData data,
            List<FundResponseFields> newFields,
            List<FundResponseFields> oldFields)
        {
            UpdatePolicy(data.NewPolicy, newFields);
            UpdatePolicy(data.OldPolicy, oldFields);
        }

        private void UpdatePolicy(PolicyInfo.SaveData saveData, List<FundResponseFields> fields)
        {
            if (fields.Contains(FundResponseFields.ExpirationDate))
            {
                saveData.EndDate = ExpirationDate ?? new DateTime(2099, 12, 31);
            }
            if (fields.Contains(FundResponseFields.OGRN))
            {
                saveData.OGRN = OGRN;
            }
            if (fields.Contains(FundResponseFields.OKATO))
            {
                saveData.OKATO = OKATO;
            }
            if (fields.Contains(FundResponseFields.PolicyNumber))
            {
                saveData.Number = PolicyNumber;
            }
            if (fields.Contains(FundResponseFields.UnifierPolicyNumberToPolicyNumber))
            {
                saveData.Number = UnifiedPolicyNumber;
            }
            if (fields.Contains(FundResponseFields.PolicySeries))
            {
                saveData.Series = PolicySeries;
            }
            if (fields.Contains(FundResponseFields.PolicyType))
            {
                saveData.PolicyTypeId = PolicyType.Id != 0 ? PolicyType.Id : new long?();
            }
            if (fields.Contains(FundResponseFields.StartDate))
            {
                saveData.StartDate = StartDate;
            }
            if (fields.Contains(FundResponseFields.UnifiedPolicyNumber))
            {
                saveData.UnifiedPolicyNumber = UnifiedPolicyNumber;
            }
        }
         
        #region Constructors
        #endregion

        public new class CreateData : ReconciliationFundResponse.CreateData
        {
            public override long Create(IFundResponseCreator creator, DateTime date)
            {
                return creator.Create(this, date);
            }

            public override string GetResponseTypeName()
            {
                return "Сверка 5";
            }
        }
    }
}