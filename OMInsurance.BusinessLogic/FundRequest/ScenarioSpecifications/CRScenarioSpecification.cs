using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System.Collections.Generic;

namespace OMInsurance.BusinessLogic.FundRequest
{
    public class CRScenarioSpecification : ScenarioSpecification
    {
        public CRScenarioSpecification(ClientVisit clientVisit, List<ReconciliationFundResponse.CreateData> responses) : 
            base(clientVisit, responses){ }
        public override bool IsSatisfied()
        {
            return base.IsSatisfied()
                && !IsTemporaryPolicyNumberEmpty()
                && HasOldClientData()
                && newS5 != null && s6 != null && s6.Equals(newS5)
                && (snils != null && newS5.Equals(snils) || snils == null)
                && (newS5.UnifiedPolicyNumber == clientVisit.NewPolicy.UnifiedPolicyNumber || IsUnifiedPolicyNumberEmpty())
                && IsPolicyTypeNew(newS5)
                && !IsPolicyClosed(newS5)
                && IsUralsibPolicy(newS5)
                && IsMoscowPolicy(newS5);
        }

        public override ReferenceItem GetScenario()
        {
            return ClientVisitScenaries.RequestENPSameSMOChangeFIO;
        }
    }
}
