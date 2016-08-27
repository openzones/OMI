using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMInsurance.BusinessLogic.FundRequest
{
    public class POKProcessor
    {
        protected ClientVisit.SaveData clientVisit;
        protected List<ReconciliationFundResponse.CreateData> responses;
        protected ReconciliationFundResponse.CreateData newS5;
        protected ReconciliationFundResponse.CreateData oldS5;
        bool isChanged;

        public POKProcessor(ClientVisit.SaveData clientVisit, List<ReconciliationFundResponse.CreateData> responses)
        {
            this.clientVisit = clientVisit;
            this.responses = responses;
            newS5 = responses.OfType<S5FundResponse.CreateData>().FirstOrDefault(item => item.DataTypeId == (int)EntityType.New && item.Order == 1);
            oldS5 = responses.OfType<S5FundResponse.CreateData>().FirstOrDefault(item => item.DataTypeId == (int)EntityType.Old && item.Order == 1);
        }

        public void Process()
        {
            // Если у заявки сценарий POK, а в сверке 5 ОГРН Уралсиб и ОКАТО 45000 (task - 9765)
            if (newS5 != null &&
                IsUralsibPolicy(newS5) &&
                IsMoscowPolicy(newS5) &&
                clientVisit.ScenarioId == ClientVisitScenaries.PolicyExtradition.Id)
            {
                clientVisit.NewPolicy.UnifiedPolicyNumber = newS5.UnifiedPolicyNumber;
                clientVisit.NewPolicy.PolicyTypeId = newS5.PolicyTypeId;
                clientVisit.NewPolicy.Series = newS5.PolicySeries;
                clientVisit.NewPolicy.Number = newS5.PolicyNumber;
                clientVisit.NewPolicy.OGRN = newS5.OGRN;
                clientVisit.NewPolicy.OKATO = newS5.OKATO;
                clientVisit.NewPolicy.StartDate = newS5.StartDate;
                clientVisit.IsActual = true;
                isChanged = true;
            }
        }

        public bool IsChanged { get { return isChanged; } }

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
