using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System.Collections.Generic;

namespace OMInsurance.BusinessLogic.FundRequest
{
    public class PTScenarioSpecification : ScenarioSpecification
    {
        public PTScenarioSpecification(ClientVisit clientVisit, List<ReconciliationFundResponse.CreateData> responses) : 
            base(clientVisit, responses){ }
        public override bool IsSatisfied()
        {
            return base.IsSatisfied()
                && !IsTemporaryPolicyNumberEmpty()
                && IsUnifiedPolicyNumberEmpty()
                && newS5 != null
                && (snils != null && newS5.Equals(snils) || snils == null)
                && !IsUralsibPolicy(newS5)
                && !IsPolicyTypeNew(newS5)
                && !IsPolicyClosed(newS5)
                && !IsMoscowPolicy(newS5);
        }

        public override ReferenceItem GetScenario()
        {
            return ClientVisitScenaries.ReregistrationRegionalOldPolicyWithoutFIO;
        }
    }
}
