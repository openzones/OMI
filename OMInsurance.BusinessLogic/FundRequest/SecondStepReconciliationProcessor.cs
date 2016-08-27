using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMInsurance.BusinessLogic.FundRequest
{
    public class SecondStepReconciliationProcessor
    {
        protected ClientVisit.SaveData clientVisit;
        protected List<ReconciliationFundResponse.CreateData> responses;
        protected ReconciliationFundResponse.CreateData newS5;
        protected ReconciliationFundResponse.CreateData oldS5;
        protected ReconciliationFundResponse.CreateData s6;
        protected ReconciliationFundResponse.CreateData s9;
        protected ReconciliationFundResponse.CreateData snils;
        bool isChanged;
        public SecondStepReconciliationProcessor(ClientVisit.SaveData clientVisit, List<ReconciliationFundResponse.CreateData> responses)
        {
            this.clientVisit = clientVisit;
            this.responses = responses;
            newS5 = responses.OfType<S5FundResponse.CreateData>().FirstOrDefault(item => item.DataTypeId == (int)EntityType.New && item.Order == 1);
            oldS5 = responses.OfType<S5FundResponse.CreateData>().FirstOrDefault(item => item.DataTypeId == (int)EntityType.Old && item.Order == 1);
            s6 = responses.OfType<S6FundResponse.CreateData>().FirstOrDefault(item => item.Order == 1 && item.FundAnswer == S6FundResponse.OK_Answer);
            s9 = responses.OfType<S9FundResponse.CreateData>().FirstOrDefault(item => item.Order == 1 && item.FundAnswer == S9FundResponse.OK_Answer);
            snils = responses.OfType<SnilsFundResponse.CreateData>().FirstOrDefault(item => item.Order == 1 && item.FundAnswer == SnilsFundResponse.OK_Answer);
        }

        public void Process()
        {
            //Reregistration confirmation
            if (newS5 != null && (clientVisit.Status == ClientVisitStatuses.Processed.Id || clientVisit.Status == ClientVisitStatuses.AnswerPending.Id)
                && IsTemporaryPolicyNumberEmpty() && IsUralsibPolicy(newS5) && IsMoscowPolicy(newS5)
                && (clientVisit.ScenarioId == ClientVisitScenaries.ReregistrationMoscowENPWithoutFIO.Id || clientVisit.ScenarioId == ClientVisitScenaries.ReregistrationRegionalENPWithoutFIO.Id))
            {
                clientVisit.Status = ClientVisitStatuses.ReregistrationDone.Id;
                clientVisit.StatusDate = DateTime.Now;
                clientVisit.IsActual = true;
                isChanged = true;
            }
        }

        public bool IsChanged { get { return isChanged; } }

        protected bool IsTemporaryPolicyNumberEmpty()
        {
            return string.IsNullOrEmpty(clientVisit.TemporaryPolicyNumber);
        } 

        protected bool IsUnifiedPolicyNumberEmpty()
        {
            return string.IsNullOrEmpty(clientVisit.NewPolicy.UnifiedPolicyNumber);
        }

        protected bool IsUralsibPolicy(ReconciliationFundResponse.CreateData response)
        {
            return response.OGRN == "1025002690877";
        }

        protected bool IsMoscowPolicy(ReconciliationFundResponse.CreateData response)
        {
            return response.OKATO == "45000";
        }

    }
}
