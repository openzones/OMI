using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System.Collections.Generic;

namespace OMInsurance.BusinessLogic.FundRequest
{
    public class ScenarioResolver
    {
        private List<ScenarioSpecification> specs = new List<ScenarioSpecification>();
        public ScenarioResolver(ClientVisit clientVisit, List<ReconciliationFundResponse.CreateData> responses)
        {
            specs.Add(new NBScenarioSpecification(clientVisit, responses));
            specs.Add(new CIScenarioSpecification(clientVisit, responses));
            specs.Add(new RIScenarioSpecification(clientVisit, responses));
            specs.Add(new PIScenarioSpecification(clientVisit, responses));
            specs.Add(new PRIScenarioSpecification(clientVisit, responses));
            specs.Add(new CTScenarioSpecification(clientVisit, responses));
            specs.Add(new RTScenarioSpecification(clientVisit, responses));
            specs.Add(new PTScenarioSpecification(clientVisit, responses));
            specs.Add(new PRTScenarioSpecification(clientVisit, responses));
            specs.Add(new DPScenarioSpecification(clientVisit, responses));
            specs.Add(new CRScenarioSpecification(clientVisit, responses));
            specs.Add(new CPScenarioSpecification(clientVisit, responses));
            specs.Add(new PRScenarioSpecification(clientVisit, responses));
        }

        public ReferenceItem GetResolvedScenario()
        {
            foreach (var spec in specs)
	        {
                if (spec.IsSatisfied())
                {
                    return spec.GetScenario();
                }
	        }
            return null;
        }
    }
}
