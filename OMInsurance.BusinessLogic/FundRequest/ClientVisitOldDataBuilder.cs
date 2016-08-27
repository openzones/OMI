using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OMInsurance.BusinessLogic
{
    public class ClientVisitOldDataBuilder
    {
        protected ClientVisit.SaveData clientVisit;
        protected IReconciliationFundResponse newS5;
        protected IReconciliationFundResponse oldS5;
        protected IReconciliationFundResponse s6;
        protected IReconciliationFundResponse s9;
        protected IReconciliationFundResponse snils;
        protected static Dictionary<long, Action> oldDataProcessors;

        public ClientVisitOldDataBuilder(ClientVisit.SaveData clientVisit, List<IReconciliationFundResponse> responses)
        {
            this.clientVisit = clientVisit;
            newS5 = responses.FirstOrDefault(item =>
                (item.GetType() == typeof(S5FundResponse.CreateData) || item.GetType() == typeof(S5FundResponse)) && item.DataType == EntityType.New && item.Order == 1);
            oldS5 = responses.FirstOrDefault(item =>
                (item.GetType() == typeof(S5FundResponse.CreateData) || item.GetType() == typeof(S5FundResponse)) && item.DataType == EntityType.Old && item.Order == 1);
            s6 = responses.FirstOrDefault(item =>
                (item.GetType() == typeof(S6FundResponse.CreateData) || item.GetType() == typeof(S6FundResponse)) && item.Order == 1);
            s9 = responses.FirstOrDefault(item =>
                (item.GetType() == typeof(S9FundResponse.CreateData) || item.GetType() == typeof(S9FundResponse)) && item.Order == 1);
            snils = responses.FirstOrDefault(item =>
                (item.GetType() == typeof(SnilsFundResponse.CreateData) || item.GetType() == typeof(SnilsFundResponse)) && item.Order == 1);
            oldDataProcessors = new Dictionary<long, Action>();
            oldDataProcessors.Add(ClientVisitScenaries.FirstRequestENP.Id, ProcessNB);
            oldDataProcessors.Add(ClientVisitScenaries.ReregistrationMoscowENPWithoutFIO.Id, ProcessCI);
            oldDataProcessors.Add(ClientVisitScenaries.ReregistrationRegionalENPWithoutFIO.Id, ProcessCT);
            oldDataProcessors.Add(ClientVisitScenaries.NewUnifiedPolicyNumberByKMSOtherSMO.Id, ProcessPI);
            oldDataProcessors.Add(ClientVisitScenaries.ReregistrationRegionalOldPolicyWithoutFIO.Id, ProcessPT);
            oldDataProcessors.Add(ClientVisitScenaries.ReregistrationMoscowENPWithFIO.Id, ProcessRI);
            oldDataProcessors.Add(ClientVisitScenaries.ReregistrationRegionalENPWithFIO.Id, ProcessRT);
            oldDataProcessors.Add(ClientVisitScenaries.NewUnifiedPolicyNumberByKMSOtherSMOWithFIO.Id, ProcessPRI);
            oldDataProcessors.Add(ClientVisitScenaries.ReregistrationRegionalOldPolicyWithFIO.Id, ProcessPRT);
            oldDataProcessors.Add(ClientVisitScenaries.RequestENPSameSMOChangeFIO.Id, ProcessCR);
            oldDataProcessors.Add(ClientVisitScenaries.NewUnifiedPolicyNumberByKMS.Id, ProcessCP);
            oldDataProcessors.Add(ClientVisitScenaries.NewUnifiedPolicyNumberByOldPolicy.Id, ProcessPR);
            oldDataProcessors.Add(ClientVisitScenaries.PolicyMerge.Id, ProcessMP);
            oldDataProcessors.Add(ClientVisitScenaries.PolicySeparation.Id, ProcessRD);
            oldDataProcessors.Add(ClientVisitScenaries.ChangeDocument.Id, ProcessCD);
            oldDataProcessors.Add(ClientVisitScenaries.RemoveFromRegister.Id, ProcessCLR);
            oldDataProcessors.Add(ClientVisitScenaries.PolicyExtradition.Id, ProcessPOK);
            oldDataProcessors.Add(ClientVisitScenaries.PolicyRecovery.Id, ProcessAD);
            oldDataProcessors.Add(ClientVisitScenaries.LostENPWithoutFIO.Id, ProcessDP);
        }

        private void ProcessDP()
        {
            ClearOldDocument();
        }

        public ClientVisit.SaveData Process()
        {
            if (oldDataProcessors.ContainsKey(clientVisit.ScenarioId.Value))
            {
                oldDataProcessors[clientVisit.ScenarioId.Value]();
            }
            return clientVisit;
        }

        protected void ProcessCI()
        {
            if (newS5 != null)
            {
                clientVisit.OldPolicy.UnifiedPolicyNumber = newS5.UnifiedPolicyNumber;
                clientVisit.OldPolicy.OGRN = newS5.OGRN;
                clientVisit.OldPolicy.OKATO = Constants.MoscowOKATO;
                clientVisit.OldPolicy.Series = string.Empty;
                clientVisit.OldPolicy.Number = newS5.UnifiedPolicyNumber;
                clientVisit.OldPolicy.PolicyTypeId = newS5.PolicyTypeId;
                clientVisit.OldPolicy.StartDate = newS5.StartDate;
                clientVisit.OldPolicy.EndDate = newS5.ExpirationDate ?? new DateTime(2099, 12, 31);
            }
            CopyNewDocToOld();
        }

        #region OldDataProcessors
        protected void ProcessNB()
        {
            clientVisit.OldPolicy = new PolicyInfo.SaveData() { Id = clientVisit.OldPolicy.Id };
            ClearOldDocument();
        }

        protected void ProcessCT()
        {
            if (newS5 != null)
            {
                clientVisit.OldPolicy.UnifiedPolicyNumber = newS5.UnifiedPolicyNumber;
                clientVisit.OldPolicy.OGRN = newS5.OGRN;
                clientVisit.OldPolicy.OKATO = newS5.OKATO;
                clientVisit.OldPolicy.Series = string.Empty;
                clientVisit.OldPolicy.Number = newS5.UnifiedPolicyNumber;
                clientVisit.OldPolicy.PolicyTypeId = newS5.PolicyTypeId;
                clientVisit.OldPolicy.StartDate = newS5.StartDate;
                clientVisit.OldPolicy.EndDate = newS5.ExpirationDate ?? new DateTime(2099, 12, 31);
            }
            CopyNewDocToOld();
        }

        protected void ProcessPI()
        {
            if (newS5 != null)
            {
                clientVisit.OldPolicy.UnifiedPolicyNumber = newS5.UnifiedPolicyNumber;
                clientVisit.OldPolicy.OGRN = newS5.OGRN;
                clientVisit.OldPolicy.OKATO = Constants.MoscowOKATO;
                clientVisit.OldPolicy.Series = newS5.PolicySeries;
                clientVisit.OldPolicy.Number = newS5.PolicyNumber;
                clientVisit.OldPolicy.PolicyTypeId = newS5.PolicyTypeId;
                clientVisit.OldPolicy.StartDate = newS5.StartDate;
                clientVisit.OldPolicy.EndDate = newS5.ExpirationDate ?? new DateTime(2099, 12, 31);
            }
            CopyNewDocToOld();
        }

        protected void ProcessPT()
        {
            if (newS5 != null)
            {
                clientVisit.OldPolicy.UnifiedPolicyNumber = newS5.UnifiedPolicyNumber;
                clientVisit.OldPolicy.OGRN = newS5.OGRN;
                clientVisit.OldPolicy.OKATO = newS5.OKATO;
                clientVisit.OldPolicy.Series = newS5.PolicySeries;
                clientVisit.OldPolicy.Number = newS5.PolicyNumber;
                clientVisit.OldPolicy.PolicyTypeId = newS5.PolicyTypeId;
                clientVisit.OldPolicy.StartDate = newS5.StartDate;
                clientVisit.OldPolicy.EndDate = newS5.ExpirationDate ?? new DateTime(2099, 12, 31);
            }
            CopyNewDocToOld();
        }

        protected void ProcessRI()
        {
            if (oldS5 != null)
            {
                clientVisit.OldPolicy.UnifiedPolicyNumber = oldS5.UnifiedPolicyNumber;
                clientVisit.OldPolicy.OGRN = oldS5.OGRN;
                clientVisit.OldPolicy.OKATO = oldS5.OKATO;
                clientVisit.OldPolicy.Series = string.Empty;
                clientVisit.OldPolicy.Number = oldS5.UnifiedPolicyNumber;
                clientVisit.OldPolicy.PolicyTypeId = oldS5.PolicyTypeId;
                clientVisit.OldPolicy.StartDate = oldS5.StartDate;
                clientVisit.OldPolicy.EndDate = oldS5.ExpirationDate;
            }
            CopyNewDocToOld();
        }

        protected void ProcessRT()
        {
            if (oldS5 != null)
            {
                clientVisit.OldPolicy.UnifiedPolicyNumber = oldS5.UnifiedPolicyNumber;
                clientVisit.OldPolicy.OGRN = oldS5.OGRN;
                clientVisit.OldPolicy.OKATO = oldS5.OKATO;
                clientVisit.OldPolicy.Series = string.Empty;
                clientVisit.OldPolicy.Number = oldS5.UnifiedPolicyNumber;
                clientVisit.OldPolicy.PolicyTypeId = oldS5.PolicyTypeId;
                clientVisit.OldPolicy.StartDate = oldS5.StartDate;
                clientVisit.OldPolicy.EndDate = oldS5.ExpirationDate;
            }
            CopyNewDocToOld();
        }

        protected void ProcessPRI()
        {
            if (oldS5 != null)
            {
                clientVisit.OldPolicy.UnifiedPolicyNumber = oldS5.UnifiedPolicyNumber;
                clientVisit.OldPolicy.OGRN = oldS5.OGRN;
                clientVisit.OldPolicy.OKATO = oldS5.OKATO;
                clientVisit.OldPolicy.PolicyTypeId = oldS5.PolicyTypeId;
                clientVisit.OldPolicy.Series = oldS5.PolicySeries;
                clientVisit.OldPolicy.Number = oldS5.PolicyNumber;
                clientVisit.OldPolicy.StartDate = oldS5.StartDate;
                clientVisit.OldPolicy.EndDate = oldS5.ExpirationDate;
            }
            CopyNewDocToOld();
        }

        protected void ProcessPRT()
        {
            if (oldS5 != null)
            {
                clientVisit.OldPolicy.UnifiedPolicyNumber = oldS5.UnifiedPolicyNumber;
                clientVisit.OldPolicy.OGRN = oldS5.OGRN;
                clientVisit.OldPolicy.OKATO = oldS5.OKATO;
                clientVisit.OldPolicy.PolicyTypeId = oldS5.PolicyTypeId;
                clientVisit.OldPolicy.Series = oldS5.PolicySeries;
                clientVisit.OldPolicy.Number = oldS5.PolicyNumber;
                clientVisit.OldPolicy.StartDate = oldS5.StartDate;
                clientVisit.OldPolicy.EndDate = oldS5.ExpirationDate;
            }
            CopyNewDocToOld();
        }

        protected void ProcessCR()
        {
            if (oldS5 != null)
            {
                clientVisit.OldPolicy.UnifiedPolicyNumber = oldS5.UnifiedPolicyNumber;
                clientVisit.OldPolicy.OGRN = oldS5.OGRN;
                clientVisit.OldPolicy.OKATO = oldS5.OKATO;
                clientVisit.OldPolicy.PolicyTypeId = oldS5.PolicyTypeId;
                clientVisit.OldPolicy.Series = string.Empty;
                clientVisit.OldPolicy.Number = oldS5.UnifiedPolicyNumber;
                clientVisit.OldPolicy.StartDate = oldS5.StartDate;
                clientVisit.OldPolicy.EndDate = oldS5.ExpirationDate;
            }
            CopyNewDocToOld();
        }

        protected void ProcessCP()
        {
            if (newS5 != null)
            {
                clientVisit.OldPolicy.UnifiedPolicyNumber = newS5.UnifiedPolicyNumber;
                clientVisit.OldPolicy.OGRN = newS5.OGRN;
                clientVisit.OldPolicy.OKATO = newS5.OKATO;
                clientVisit.OldPolicy.PolicyTypeId = newS5.PolicyTypeId;
                clientVisit.OldPolicy.Series = newS5.PolicySeries;
                clientVisit.OldPolicy.Number = newS5.PolicyNumber;
                clientVisit.OldPolicy.StartDate = newS5.StartDate;
                clientVisit.OldPolicy.EndDate = newS5.ExpirationDate ?? new DateTime(2099, 12, 31);
            }
            CopyNewDocToOld();
        }

        protected void ProcessPR()
        {
            if (oldS5 != null)
            {
                clientVisit.OldPolicy.UnifiedPolicyNumber = oldS5.UnifiedPolicyNumber;
                clientVisit.OldPolicy.OGRN = oldS5.OGRN;
                clientVisit.OldPolicy.OKATO = oldS5.OKATO;
                clientVisit.OldPolicy.PolicyTypeId = oldS5.PolicyTypeId;
                clientVisit.OldPolicy.Series = oldS5.PolicySeries;
                clientVisit.OldPolicy.Number = oldS5.PolicyNumber;
                clientVisit.OldPolicy.StartDate = oldS5.StartDate;
                clientVisit.OldPolicy.EndDate = oldS5.ExpirationDate;
            }
            CopyNewDocToOld();
        }

        protected void ProcessMP()
        {
            clientVisit.CarrierId = null;
            clientVisit.ApplicationMethodId = null;
            clientVisit.GOZNAKTypeId = null;
            clientVisit.ClientContacts = null;
            clientVisit.Representative.RepresentativeTypeId = null;
            CopyNewDocToOld();
        }

        protected void ProcessRD()
        {
            clientVisit.CarrierId = null;
            clientVisit.ApplicationMethodId = null;
            clientVisit.GOZNAKTypeId = null;
            clientVisit.ClientContacts = null;
            clientVisit.Representative.RepresentativeTypeId = null;
            CopyNewDocToOld();
        }

        protected void ProcessCD()
        {
            clientVisit.OldPolicy = new PolicyInfo.SaveData() { Id = clientVisit.OldPolicy.Id };
            if (oldS5 != null)
            {
                clientVisit.OldPolicy.PolicyTypeId = oldS5.PolicyTypeId;
            }
            CopyNewDocToOld();
        }

        protected void ProcessCLR()
        {
            clientVisit.OldPolicy = new PolicyInfo.SaveData() { Id = clientVisit.OldPolicy.Id };
            ClearOldDocument();
        }

        protected void ProcessPOK()
        {
            clientVisit.OldPolicy = new PolicyInfo.SaveData() { Id = clientVisit.OldPolicy.Id };
            ClearOldDocument();
        }

        protected void ProcessAD()
        {
            clientVisit.OldPolicy = new PolicyInfo.SaveData() { Id = clientVisit.OldPolicy.Id };

        }

        private void CopyNewDocToOld()
        {
            if (!clientVisit.OldDocument.DocumentTypeId.HasValue)
            {
                clientVisit.OldDocument.Number = clientVisit.NewDocument.Number;
                clientVisit.OldDocument.DocumentTypeId = clientVisit.NewDocument.DocumentTypeId;
                clientVisit.OldDocument.ExpirationDate = clientVisit.NewDocument.ExpirationDate;
                clientVisit.OldDocument.IsIssueCase = clientVisit.NewDocument.IsIssueCase;
                clientVisit.OldDocument.IssueDate = clientVisit.NewDocument.IssueDate;
                clientVisit.OldDocument.IssueDepartment = clientVisit.NewDocument.IssueDepartment;
                clientVisit.OldDocument.Series = clientVisit.NewDocument.Series;
            }
            else if (!clientVisit.OldForeignDocument.DocumentTypeId.HasValue)
            {
                clientVisit.OldForeignDocument.Number = clientVisit.NewForeignDocument.Number;
                clientVisit.OldForeignDocument.DocumentTypeId = clientVisit.NewForeignDocument.DocumentTypeId;
                clientVisit.OldForeignDocument.ExpirationDate = clientVisit.NewForeignDocument.ExpirationDate;
                clientVisit.OldForeignDocument.IsIssueCase = clientVisit.NewForeignDocument.IsIssueCase;
                clientVisit.OldForeignDocument.IssueDate = clientVisit.NewForeignDocument.IssueDate;
                clientVisit.OldForeignDocument.IssueDepartment = clientVisit.NewForeignDocument.IssueDepartment;
                clientVisit.OldForeignDocument.Series = clientVisit.NewForeignDocument.Series;
            }
        }

        private void ClearOldDocument()
        {
            clientVisit.OldDocument = new Document.SaveData() { Id = clientVisit.OldDocument.Id };
            clientVisit.OldForeignDocument = new Document.SaveData() { Id = clientVisit.OldForeignDocument.Id };
        }
        #endregion
    }
}
