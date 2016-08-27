using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System.Collections.Generic;

namespace OMInsurance.BusinessLogic.FundRequest
{
    public class CTScenarioSpecification : ScenarioSpecification
    {
        public CTScenarioSpecification(ClientVisit clientVisit, List<ReconciliationFundResponse.CreateData> responses) : 
            base(clientVisit, responses){ }
        public override bool IsSatisfied()
        {
            return base.IsSatisfied()
                && newS5 != null && s6 != null && newS5.Equals(s6) &&
                (snils != null && newS5.Equals(snils) || snils == null)
                && !IsPolicyClosed(newS5)
                && !IsMoscowPolicy(newS5)
                && IsPolicyTypeNew(newS5)
                && ((!IsTemporaryPolicyNumberEmpty() && (newS5.UnifiedPolicyNumber == clientVisit.NewPolicy.UnifiedPolicyNumber || IsUnifiedPolicyNumberEmpty())
                    || (IsTemporaryPolicyNumberEmpty() && !IsUnifiedPolicyNumberEmpty() && newS5.UnifiedPolicyNumber == clientVisit.NewPolicy.UnifiedPolicyNumber)));
        }

        public override ReferenceItem GetScenario()
        {
            return ClientVisitScenaries.ReregistrationRegionalENPWithoutFIO;
        }
    }
}
