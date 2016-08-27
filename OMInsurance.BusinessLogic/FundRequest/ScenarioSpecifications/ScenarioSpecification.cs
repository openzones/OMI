using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OMInsurance.BusinessLogic.FundRequest
{
    public abstract class ScenarioSpecification
    {
        protected ClientVisit clientVisit;
        protected List<ReconciliationFundResponse.CreateData> responses;
        protected ReconciliationFundResponse.CreateData newS5;
        protected ReconciliationFundResponse.CreateData oldS5;
        protected ReconciliationFundResponse.CreateData s6;
        protected ReconciliationFundResponse.CreateData s9;
        protected ReconciliationFundResponse.CreateData snils;
        public ScenarioSpecification(ClientVisit clientVisit, List<ReconciliationFundResponse.CreateData> responses)
        {
            this.clientVisit = clientVisit;
            this.responses = responses;
            newS5 = responses.OfType<S5FundResponse.CreateData>().FirstOrDefault(item => item.DataTypeId == (int)EntityType.New && item.Order == 1);
            oldS5 = responses.OfType<S5FundResponse.CreateData>().FirstOrDefault(item => item.DataTypeId == (int)EntityType.Old && item.Order == 1);
            s6 = responses.OfType<S6FundResponse.CreateData>().FirstOrDefault(item => item.Order == 1 && item.FundAnswer == S6FundResponse.OK_Answer);
            s9 = responses.OfType<S9FundResponse.CreateData>().FirstOrDefault(item => item.Order == 1 && item.FundAnswer == S9FundResponse.OK_Answer);

            snils = responses.OfType<SnilsFundResponse.CreateData>().FirstOrDefault(item => item.Order == 1 && item.FundAnswer == SnilsFundResponse.OK_Answer);
        }
        public virtual bool IsSatisfied()
        {
            return IsAvailableScenarioForAutoResolve();
        }

        public abstract ReferenceItem GetScenario();
        protected bool IsTemporaryPolicyNumberEmpty()
        {
            return string.IsNullOrEmpty(clientVisit.TemporaryPolicyNumber);
        }
        protected bool IsUnifiedPolicyNumberEmpty()
        {
            return string.IsNullOrEmpty(clientVisit.NewPolicy.UnifiedPolicyNumber);
        }

        protected bool IsUnifiedPolicyNumberResponseEmpty(ReconciliationFundResponse.CreateData response)
        {
            return string.IsNullOrEmpty(response.UnifiedPolicyNumber);
        }

        protected bool IsPolicyClosed(ReconciliationFundResponse.CreateData response)
        {
            DateTime now = DateTime.Now;
            return response != null && response.ExpirationDate.HasValue && response.ExpirationDate.Value < now;
        }

        protected bool IsPolicyTypeNew(ReconciliationFundResponse.CreateData response)
        {
            return response.StartDate >= new DateTime(2011, 05, 1);
        }

        protected bool IsUralsibPolicy(ReconciliationFundResponse.CreateData response)
        {
            return response.OGRN == "1025002690877";
        }

        protected bool IsMoscowPolicy(ReconciliationFundResponse.CreateData response)
        {
            return response.OKATO == "45000";
        }

        protected bool HasOldClientData()
        {
            return !string.IsNullOrEmpty(clientVisit.OldClientInfo.Lastname);
        }

        protected bool IsAvailableScenarioForAutoResolve()
        {
            return
                clientVisit.Scenario != null &&
                clientVisit.Scenario.Id != ClientVisitScenaries.ChangeDocument.Id &&
                clientVisit.Scenario.Id != ClientVisitScenaries.PolicyExtradition.Id &&
                clientVisit.Scenario.Id != ClientVisitScenaries.PolicyMerge.Id &&
                clientVisit.Scenario.Id != ClientVisitScenaries.PolicySeparation.Id &&
                clientVisit.Scenario.Id != ClientVisitScenaries.RemoveFromRegister.Id &&
                clientVisit.Scenario.Id != ClientVisitScenaries.ChangeSexOrBirthdaySMO.Id &&
                clientVisit.Scenario.Id != ClientVisitScenaries.PolicyRecovery.Id;
        }
    }
}
