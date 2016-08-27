using OMInsurance.Entities.Core;
using System;
namespace OMInsurance.Entities
{
    public interface IReconciliationFundResponse
    {
        EntityType DataType { get; set; }
        string ErrorMessage { get; set; }
        DateTime? ExpirationDate { get; set; }
        string FundAnswer { get; set; }
        string OGRN { get; set; }
        long? PolicyTypeId { get; set; }
        string OKATO { get; set; }
        int Order { get; set; }
        string PolicyNumber { get; set; }
        string PolicySeries { get; set; }
        DateTime? StartDate { get; set; }
        string UnifiedPolicyNumber { get; set; }
    }
}
