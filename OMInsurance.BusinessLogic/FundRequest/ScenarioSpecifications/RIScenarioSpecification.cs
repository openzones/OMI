using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System.Collections.Generic;

namespace OMInsurance.BusinessLogic.FundRequest
{
    public class RIScenarioSpecification : ScenarioSpecification
    {
        public RIScenarioSpecification(ClientVisit clientVisit, List<ReconciliationFundResponse.CreateData> responses) : 
            base(clientVisit, responses){ }
        public override bool IsSatisfied()
        {
            return base.IsSatisfied()
                && !IsTemporaryPolicyNumberEmpty() && HasOldClientData() && oldS5 != null && s6 != null && oldS5.Equals(s6) 
                && (snils != null && oldS5.Equals(snils) || snils == null)
                && IsPolicyTypeNew(oldS5) && !IsPolicyClosed(oldS5) && !IsUralsibPolicy(oldS5) && IsMoscowPolicy(oldS5)
                && (IsUnifiedPolicyNumberEmpty() || oldS5.UnifiedPolicyNumber == clientVisit.NewPolicy.UnifiedPolicyNumber);
        }

        public override ReferenceItem GetScenario()
        {
            return ClientVisitScenaries.ReregistrationMoscowENPWithFIO;
        }
    }
}
