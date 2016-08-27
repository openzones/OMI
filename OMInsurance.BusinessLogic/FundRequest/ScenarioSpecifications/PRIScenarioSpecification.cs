using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System.Collections.Generic;

namespace OMInsurance.BusinessLogic.FundRequest
{
    public class PRIScenarioSpecification : ScenarioSpecification
    {
        public PRIScenarioSpecification(ClientVisit clientVisit, List<ReconciliationFundResponse.CreateData> responses) : 
            base(clientVisit, responses){ }
        public override bool IsSatisfied()
        {
            return base.IsSatisfied()
                && !IsTemporaryPolicyNumberEmpty()
                && IsUnifiedPolicyNumberEmpty()
                && HasOldClientData()
                && oldS5 != null
                && (snils != null && oldS5.Equals(snils) || snils == null)
                && !IsUralsibPolicy(oldS5) && IsMoscowPolicy(oldS5) && !IsPolicyTypeNew(oldS5) && !IsPolicyClosed(oldS5);
        }

        public override ReferenceItem GetScenario()
        {
            return ClientVisitScenaries.NewUnifiedPolicyNumberByKMSOtherSMOWithFIO;
        }
    }
}
