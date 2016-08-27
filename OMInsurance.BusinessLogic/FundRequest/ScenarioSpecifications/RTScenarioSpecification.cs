using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System.Collections.Generic;

namespace OMInsurance.BusinessLogic.FundRequest
{
    public class RTScenarioSpecification : ScenarioSpecification
    {
        public RTScenarioSpecification(ClientVisit clientVisit, List<ReconciliationFundResponse.CreateData> responses) : 
            base(clientVisit, responses){ }
        public override bool IsSatisfied()
        {
            return base.IsSatisfied()
                && !IsTemporaryPolicyNumberEmpty()
                && HasOldClientData()
                && oldS5 != null && s6 != null && s6.Equals(oldS5)
                && (snils != null && oldS5.Equals(snils) || snils == null)
                && (oldS5.UnifiedPolicyNumber == clientVisit.NewPolicy.UnifiedPolicyNumber || IsUnifiedPolicyNumberEmpty())
                && !IsPolicyTypeNew(oldS5)
                && !IsPolicyClosed(oldS5)
                && !IsMoscowPolicy(oldS5);
        }

        public override ReferenceItem GetScenario()
        {
            return ClientVisitScenaries.ReregistrationRegionalENPWithFIO;
        }
    }
}
