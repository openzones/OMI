using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System.Collections.Generic;

namespace OMInsurance.BusinessLogic.FundRequest
{
    public class CIScenarioSpecification : ScenarioSpecification
    {
        public CIScenarioSpecification(ClientVisit clientVisit, List<ReconciliationFundResponse.CreateData> responses) : 
            base(clientVisit, responses){ }

        public override bool IsSatisfied()
        {
            return base.IsSatisfied()
                && newS5 != null && s6 != null && newS5.Equals(s6) &&
                (snils != null && newS5.Equals(snils) || snils == null)
                && IsPolicyTypeNew(newS5) && !IsPolicyClosed(newS5) && !IsUralsibPolicy(newS5) && IsMoscowPolicy(newS5)
                && (!IsTemporaryPolicyNumberEmpty() && !IsUnifiedPolicyNumberEmpty() 
                        && newS5.UnifiedPolicyNumber == clientVisit.NewPolicy.UnifiedPolicyNumber
                    || (IsTemporaryPolicyNumberEmpty() && (IsUnifiedPolicyNumberEmpty() || newS5.UnifiedPolicyNumber == clientVisit.NewPolicy.UnifiedPolicyNumber)));
        }

        public override ReferenceItem GetScenario()
        {
            return ClientVisitScenaries.ReregistrationMoscowENPWithoutFIO;
        }
    }
}
