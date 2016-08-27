using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System.Collections.Generic;

namespace OMInsurance.BusinessLogic.FundRequest
{
    public class NBScenarioSpecification : ScenarioSpecification
    {
        public NBScenarioSpecification(ClientVisit clientVisit, List<ReconciliationFundResponse.CreateData> responses) : 
            base(clientVisit, responses){ }
        public override bool IsSatisfied()
        {
            return base.IsSatisfied()
                && (newS5 == null && oldS5 == null && snils == null || IsPolicyClosed(newS5))
                && !IsTemporaryPolicyNumberEmpty()
                && IsUnifiedPolicyNumberEmpty();
        }

        public override ReferenceItem GetScenario()
        {
            return ClientVisitScenaries.FirstRequestENP;
        }
    }
}
