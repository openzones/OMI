using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System.Collections.Generic;

namespace OMInsurance.BusinessLogic.FundRequest
{
    public class PRScenarioSpecification : ScenarioSpecification
    {
        public PRScenarioSpecification(ClientVisit clientVisit, List<ReconciliationFundResponse.CreateData> responses) : 
            base(clientVisit, responses){ }
        public override bool IsSatisfied()
        {
            return base.IsSatisfied()
                && !IsTemporaryPolicyNumberEmpty()
                && IsUnifiedPolicyNumberEmpty()
                && HasOldClientData()
                && newS5 != null 
                && (snils != null && newS5.Equals(snils) || snils == null)
                && !IsPolicyTypeNew(newS5)
                && !IsPolicyClosed(newS5)
                && IsUralsibPolicy(newS5)
                && IsMoscowPolicy(newS5);
        }

        public override ReferenceItem GetScenario()
        {
            return ClientVisitScenaries.NewUnifiedPolicyNumberByOldPolicy;
        }
    }
}
