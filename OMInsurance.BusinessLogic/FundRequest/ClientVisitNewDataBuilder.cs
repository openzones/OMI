using OMInsurance.Entities;
using OMInsurance.Utils;
using OMInsurance.Entities.Core;
using System;
using System.Linq;
using System.Collections.Generic;

namespace OMInsurance.BusinessLogic.FundRequest
{
    public class ClientVisitNewDataBuilder
    {
        const string OKATO = "45000";
        protected ClientVisit firstClientVisit;
        protected ClientVisit.SaveData clientVisit;
        protected IReconciliationFundResponse newS5;
        protected IReconciliationFundResponse oldS5;
        protected IReconciliationFundResponse s6;
        protected IReconciliationFundResponse s9;
        protected IReconciliationFundResponse snils;
        protected Dictionary<long, Action> dataProcessors;
        public ClientVisitNewDataBuilder(ClientVisit.SaveData clientVisit, ClientVisit firstClientVisit, IEnumerable<IReconciliationFundResponse> responses)
        {
            this.clientVisit = clientVisit;
            this.firstClientVisit = firstClientVisit;
            dataProcessors = new Dictionary<long, Action>();
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
            this.clientVisit = clientVisit;
            dataProcessors.Add(ClientVisitScenaries.FirstRequestENP.Id, ProcessNB);
            dataProcessors.Add(ClientVisitScenaries.ReregistrationMoscowENPWithoutFIO.Id, ProcessCI);
            dataProcessors.Add(ClientVisitScenaries.ReregistrationRegionalENPWithoutFIO.Id, ProcessCT);
            dataProcessors.Add(ClientVisitScenaries.NewUnifiedPolicyNumberByKMSOtherSMO.Id, ProcessPI);
            dataProcessors.Add(ClientVisitScenaries.ReregistrationRegionalOldPolicyWithoutFIO.Id, ProcessPT);
            dataProcessors.Add(ClientVisitScenaries.ReregistrationMoscowENPWithFIO.Id, ProcessRI);
            dataProcessors.Add(ClientVisitScenaries.ReregistrationRegionalENPWithFIO.Id, ProcessRT);
            dataProcessors.Add(ClientVisitScenaries.NewUnifiedPolicyNumberByKMSOtherSMOWithFIO.Id, ProcessPRI);
            dataProcessors.Add(ClientVisitScenaries.ReregistrationRegionalOldPolicyWithFIO.Id, ProcessPRT);
            dataProcessors.Add(ClientVisitScenaries.RequestENPSameSMOChangeFIO.Id, ProcessCR);
            dataProcessors.Add(ClientVisitScenaries.NewUnifiedPolicyNumberByKMS.Id, ProcessCP);
            dataProcessors.Add(ClientVisitScenaries.LostENPWithoutFIO.Id, ProcessDP);
            dataProcessors.Add(ClientVisitScenaries.NewUnifiedPolicyNumberByOldPolicy.Id, ProcessPR);
            dataProcessors.Add(ClientVisitScenaries.PolicyMerge.Id, ProcessMP);
            dataProcessors.Add(ClientVisitScenaries.PolicySeparation.Id, ProcessRD);
            dataProcessors.Add(ClientVisitScenaries.ChangeDocument.Id, ProcessCD);
            dataProcessors.Add(ClientVisitScenaries.RemoveFromRegister.Id, ProcessCLR);
            dataProcessors.Add(ClientVisitScenaries.PolicyExtradition.Id, ProcessPOK);
            dataProcessors.Add(ClientVisitScenaries.PolicyRecovery.Id, ProcessAD);
        }

        private bool HasTemporaryNumber()
        {
            return !string.IsNullOrEmpty(firstClientVisit.TemporaryPolicyNumber);
        }

        private void ProcessNB()
        {
            clientVisit.NewPolicy.UnifiedPolicyNumber = string.Empty;
            clientVisit.NewPolicy.OGRN = string.Empty;
            clientVisit.NewPolicy.OKATO = string.Empty;
            clientVisit.NewPolicy.Series = string.Empty;
            clientVisit.NewPolicy.PolicyTypeId = PolicyTypeRef.TemporaryPolicy.Id;
            clientVisit.NewPolicy.Number = firstClientVisit.TemporaryPolicyNumber;
            clientVisit.NewPolicy.StartDate = DateTime.Now.AddDays(-1);
            clientVisit.CarrierId = firstClientVisit.CarrierId;
            clientVisit.ApplicationMethodId = firstClientVisit.ApplicationMethodId;
            clientVisit.Representative.RepresentativeTypeId = firstClientVisit.Representative.RepresentativeTypeId;
            if (firstClientVisit.CarrierId == Carriers.PaperPolicy.Id)
            {
                clientVisit.GOZNAKTypeId = GoznakTypes.PrintPolicyFirstTime.Id;
            }
            else if (firstClientVisit.CarrierId == Carriers.DigitalPolicy.Id)
            {
                clientVisit.GOZNAKTypeId = GoznakTypes.DigitalPolicyFirstTime.Id;
            }
            else
            {
                clientVisit.GOZNAKTypeId = GoznakTypes.DontSent.Id;
            }
            ClearOldDocument();
            ClearOldPolicy(); 
            ClearOldClientVersionData();
        }

        private void ProcessCI()
        {
            clientVisit.NewPolicy.UnifiedPolicyNumber = clientVisit.OldPolicy.UnifiedPolicyNumber;
            clientVisit.NewPolicy.OGRN = clientVisit.OldPolicy.OGRN;
            clientVisit.NewPolicy.OKATO = clientVisit.OldPolicy.OKATO;
            clientVisit.NewPolicy.Series = string.Empty;
            clientVisit.NewPolicy.StartDate = DateTime.Now.AddWorkingDays(-1);
            clientVisit.CarrierId = firstClientVisit.CarrierId;
            clientVisit.ApplicationMethodId = firstClientVisit.ApplicationMethodId;
            clientVisit.Representative.RepresentativeTypeId = firstClientVisit.Representative.RepresentativeTypeId;
            if (HasTemporaryNumber())
            {
                clientVisit.NewPolicy.Number = firstClientVisit.TemporaryPolicyNumber;
                clientVisit.NewPolicy.PolicyTypeId = PolicyTypeRef.TemporaryPolicy.Id;
                if (firstClientVisit.CarrierId == Carriers.PaperPolicy.Id)
                {
                    clientVisit.GOZNAKTypeId = GoznakTypes.PrintPolicyRepeatedly.Id;
                }
                else if (firstClientVisit.CarrierId == Carriers.DigitalPolicy.Id)
                {
                    clientVisit.GOZNAKTypeId = GoznakTypes.DigitalPolicyRepeatedly.Id;
                }
                else
                {
                    clientVisit.GOZNAKTypeId = GoznakTypes.DontSent.Id;
                }
            }
            else
            {
                clientVisit.NewPolicy.PolicyTypeId = clientVisit.OldPolicy.PolicyTypeId;
                clientVisit.NewPolicy.Number = clientVisit.OldPolicy.UnifiedPolicyNumber;
                clientVisit.GOZNAKTypeId = GoznakTypes.DontSent.Id;
            }
            CopyNewDocToOld();
        }

        private void ProcessCT()
        {
            clientVisit.NewPolicy.UnifiedPolicyNumber = clientVisit.OldPolicy.UnifiedPolicyNumber;
            clientVisit.NewPolicy.OGRN = string.Empty;
            clientVisit.NewPolicy.OKATO = string.Empty;
            clientVisit.NewPolicy.Series = string.Empty;
            clientVisit.NewPolicy.StartDate = DateTime.Now.AddWorkingDays(-1);
            clientVisit.CarrierId = firstClientVisit.CarrierId;
            clientVisit.ApplicationMethodId = firstClientVisit.ApplicationMethodId;
            clientVisit.Representative.RepresentativeTypeId = firstClientVisit.Representative.RepresentativeTypeId;
            if (HasTemporaryNumber())
            {
                clientVisit.NewPolicy.Number = clientVisit.TemporaryPolicyNumber;
                clientVisit.NewPolicy.PolicyTypeId = PolicyTypeRef.TemporaryPolicy.Id;
                if (firstClientVisit.CarrierId == Carriers.PaperPolicy.Id)
                {
                    clientVisit.GOZNAKTypeId = GoznakTypes.PrintPolicyRepeatedly.Id;
                }
                else if (firstClientVisit.CarrierId == Carriers.DigitalPolicy.Id)
                {
                    clientVisit.GOZNAKTypeId = GoznakTypes.DigitalPolicyRepeatedly.Id;
                }
                else
                {
                    clientVisit.GOZNAKTypeId = GoznakTypes.DontSent.Id;
                }
            }
            else
            {
                clientVisit.NewPolicy.PolicyTypeId = clientVisit.OldPolicy.PolicyTypeId;
                clientVisit.NewPolicy.Number = clientVisit.OldPolicy.UnifiedPolicyNumber;
                clientVisit.GOZNAKTypeId = GoznakTypes.DontSent.Id;
            }
            CopyNewDocToOld();
        }

        private void ProcessPI()
        {
            clientVisit.NewPolicy.UnifiedPolicyNumber = clientVisit.OldPolicy.UnifiedPolicyNumber;
            clientVisit.NewPolicy.OGRN = string.Empty;
            clientVisit.NewPolicy.OKATO = string.Empty;
            clientVisit.NewPolicy.PolicyTypeId = PolicyTypeRef.TemporaryPolicy.Id;
            clientVisit.NewPolicy.Series = string.Empty;
            clientVisit.NewPolicy.Number = firstClientVisit.TemporaryPolicyNumber;
            clientVisit.CarrierId = firstClientVisit.CarrierId;
            clientVisit.ApplicationMethodId = firstClientVisit.ApplicationMethodId;
            clientVisit.Representative.RepresentativeTypeId = firstClientVisit.Representative.RepresentativeTypeId;
            clientVisit.NewPolicy.StartDate = DateTime.Now.AddWorkingDays(-1);
            if (firstClientVisit.CarrierId == Carriers.PaperPolicy.Id)
            {
                clientVisit.GOZNAKTypeId = GoznakTypes.PrintPolicyFirstTime.Id;
            }
            else if (firstClientVisit.CarrierId == Carriers.DigitalPolicy.Id)
            {
                clientVisit.GOZNAKTypeId = GoznakTypes.DigitalPolicyFirstTime.Id;
            }
            else
            {
                clientVisit.GOZNAKTypeId = GoznakTypes.DontSent.Id;
            }
            CopyNewDocToOld();
        }

        private void ProcessPT()
        {
            clientVisit.NewPolicy.UnifiedPolicyNumber = clientVisit.OldPolicy.UnifiedPolicyNumber;
            clientVisit.NewPolicy.OGRN = string.Empty;
            clientVisit.NewPolicy.OKATO = string.Empty;
            clientVisit.NewPolicy.PolicyTypeId = PolicyTypeRef.TemporaryPolicy.Id;
            clientVisit.NewPolicy.Series = string.Empty;
            clientVisit.NewPolicy.Number = firstClientVisit.TemporaryPolicyNumber;
            clientVisit.CarrierId = firstClientVisit.CarrierId;
            clientVisit.ApplicationMethodId = firstClientVisit.ApplicationMethodId;
            clientVisit.Representative.RepresentativeTypeId = firstClientVisit.Representative.RepresentativeTypeId;
            clientVisit.NewPolicy.StartDate = DateTime.Now.AddWorkingDays(-1);
            if (firstClientVisit.CarrierId == Carriers.PaperPolicy.Id)
            {
                clientVisit.GOZNAKTypeId = GoznakTypes.PrintPolicyFirstTime.Id;
            }
            else if (firstClientVisit.CarrierId == Carriers.DigitalPolicy.Id)
            {
                clientVisit.GOZNAKTypeId = GoznakTypes.DigitalPolicyFirstTime.Id;
            }
            else
            {
                clientVisit.GOZNAKTypeId = GoznakTypes.DontSent.Id;
            }
            CopyNewDocToOld();
        }

        private void ProcessRI()
        {
            clientVisit.NewPolicy.UnifiedPolicyNumber = clientVisit.OldPolicy.UnifiedPolicyNumber;
            clientVisit.NewPolicy.OGRN = string.Empty;
            clientVisit.NewPolicy.OKATO = string.Empty;
            clientVisit.NewPolicy.PolicyTypeId = PolicyTypeRef.TemporaryPolicy.Id;
            clientVisit.NewPolicy.Series = string.Empty;
            clientVisit.NewPolicy.Number = firstClientVisit.TemporaryPolicyNumber;
            clientVisit.CarrierId = firstClientVisit.CarrierId;
            clientVisit.ApplicationMethodId = firstClientVisit.ApplicationMethodId;
            clientVisit.Representative.RepresentativeTypeId = firstClientVisit.Representative.RepresentativeTypeId;
            clientVisit.NewPolicy.StartDate = DateTime.Now.AddWorkingDays(-1);
            if (firstClientVisit.CarrierId == Carriers.PaperPolicy.Id)
            {
                clientVisit.GOZNAKTypeId = GoznakTypes.PrintPolicyRepeatedly.Id;
            }
            else if (firstClientVisit.CarrierId == Carriers.DigitalPolicy.Id)
            {
                clientVisit.GOZNAKTypeId = GoznakTypes.DigitalPolicyRepeatedly.Id;
            }
            else
            {
                clientVisit.GOZNAKTypeId = GoznakTypes.DontSent.Id;
            }
            CopyNewDocToOld();
        }

        private void ProcessRT()
        {
            clientVisit.NewPolicy.UnifiedPolicyNumber = clientVisit.OldPolicy.UnifiedPolicyNumber;
            clientVisit.NewPolicy.OGRN = string.Empty;
            clientVisit.NewPolicy.OKATO = string.Empty;
            clientVisit.NewPolicy.PolicyTypeId = PolicyTypeRef.TemporaryPolicy.Id;
            clientVisit.NewPolicy.Series = string.Empty;
            clientVisit.NewPolicy.Number = firstClientVisit.TemporaryPolicyNumber;
            clientVisit.CarrierId = firstClientVisit.CarrierId;
            clientVisit.ApplicationMethodId = firstClientVisit.ApplicationMethodId;
            clientVisit.Representative.RepresentativeTypeId = firstClientVisit.Representative.RepresentativeTypeId;
            clientVisit.NewPolicy.StartDate = DateTime.Now.AddWorkingDays(-1);
            if (firstClientVisit.CarrierId == Carriers.PaperPolicy.Id)
            {
                clientVisit.GOZNAKTypeId = GoznakTypes.PrintPolicyRepeatedly.Id;
            }
            else if (firstClientVisit.CarrierId == Carriers.DigitalPolicy.Id)
            {
                clientVisit.GOZNAKTypeId = GoznakTypes.DigitalPolicyRepeatedly.Id;
            }
            else
            {
                clientVisit.GOZNAKTypeId = GoznakTypes.DontSent.Id;
            }
            CopyNewDocToOld();
        }

        private void ProcessPRI()
        {
            clientVisit.NewPolicy.UnifiedPolicyNumber = clientVisit.OldPolicy.UnifiedPolicyNumber;
            clientVisit.NewPolicy.OGRN = string.Empty;
            clientVisit.NewPolicy.OKATO = string.Empty;
            clientVisit.NewPolicy.PolicyTypeId = PolicyTypeRef.TemporaryPolicy.Id;
            clientVisit.NewPolicy.Series = string.Empty;
            clientVisit.NewPolicy.Number = firstClientVisit.TemporaryPolicyNumber;
            clientVisit.CarrierId = firstClientVisit.CarrierId;
            clientVisit.ApplicationMethodId = firstClientVisit.ApplicationMethodId;
            clientVisit.Representative.RepresentativeTypeId = firstClientVisit.Representative.RepresentativeTypeId;
            clientVisit.NewPolicy.StartDate = DateTime.Now.AddWorkingDays(-1);
            if (firstClientVisit.CarrierId == Carriers.PaperPolicy.Id)
            {
                clientVisit.GOZNAKTypeId = GoznakTypes.PrintPolicyFirstTime.Id;
            }
            else if (firstClientVisit.CarrierId == Carriers.DigitalPolicy.Id)
            {
                clientVisit.GOZNAKTypeId = GoznakTypes.DigitalPolicyFirstTime.Id;
            }
            else
            {
                clientVisit.GOZNAKTypeId = GoznakTypes.DontSent.Id;
            }
            CopyNewDocToOld();
        }

        private void ProcessPRT()
        {
            clientVisit.NewPolicy.UnifiedPolicyNumber = clientVisit.OldPolicy.UnifiedPolicyNumber;
            clientVisit.NewPolicy.OGRN = string.Empty;
            clientVisit.NewPolicy.OKATO = string.Empty;
            clientVisit.NewPolicy.PolicyTypeId = PolicyTypeRef.TemporaryPolicy.Id;
            clientVisit.NewPolicy.Series = string.Empty;
            clientVisit.NewPolicy.Number = firstClientVisit.TemporaryPolicyNumber;
            clientVisit.CarrierId = firstClientVisit.CarrierId;
            clientVisit.ApplicationMethodId = firstClientVisit.ApplicationMethodId;
            clientVisit.Representative.RepresentativeTypeId = firstClientVisit.Representative.RepresentativeTypeId;
            clientVisit.NewPolicy.StartDate = DateTime.Now.AddWorkingDays(-1);
            if (firstClientVisit.CarrierId == Carriers.PaperPolicy.Id)
            {
                clientVisit.GOZNAKTypeId = GoznakTypes.PrintPolicyFirstTime.Id;
            }
            else if (firstClientVisit.CarrierId == Carriers.DigitalPolicy.Id)
            {
                clientVisit.GOZNAKTypeId = GoznakTypes.DigitalPolicyFirstTime.Id;
            }
            else
            {
                clientVisit.GOZNAKTypeId = GoznakTypes.DontSent.Id;
            }
            CopyNewDocToOld();
        }

        private void ProcessDP()
        {
            if (newS5 != null)
            {
                clientVisit.NewPolicy.UnifiedPolicyNumber = newS5.UnifiedPolicyNumber;
            }
            clientVisit.NewPolicy.OGRN = string.Empty;
            clientVisit.NewPolicy.OKATO = string.Empty;
            clientVisit.NewPolicy.PolicyTypeId = PolicyTypeRef.TemporaryPolicy.Id;
            clientVisit.NewPolicy.Series = string.Empty;
            clientVisit.NewPolicy.Number = firstClientVisit.TemporaryPolicyNumber;
            clientVisit.CarrierId = firstClientVisit.CarrierId;
            clientVisit.ApplicationMethodId = firstClientVisit.ApplicationMethodId;
            clientVisit.Representative.RepresentativeTypeId = firstClientVisit.Representative.RepresentativeTypeId;
            clientVisit.NewPolicy.StartDate = DateTime.Now.AddWorkingDays(-1);
            if (firstClientVisit.CarrierId == Carriers.PaperPolicy.Id)
            {
                clientVisit.GOZNAKTypeId = GoznakTypes.PrintPolicyRepeatedly.Id;
            }
            else if (firstClientVisit.CarrierId == Carriers.DigitalPolicy.Id)
            {
                clientVisit.GOZNAKTypeId = GoznakTypes.DigitalPolicyRepeatedly.Id;
            }
            else
            {
                clientVisit.GOZNAKTypeId = GoznakTypes.DontSent.Id;
            }
            ClearOldDocument();
            ClearOldPolicy(); 
            ClearOldClientVersionData();
        }

        private void ProcessCR()
        {
            clientVisit.NewPolicy.UnifiedPolicyNumber = clientVisit.OldPolicy.UnifiedPolicyNumber;
            clientVisit.NewPolicy.OGRN = string.Empty;
            clientVisit.NewPolicy.OKATO = string.Empty;
            clientVisit.NewPolicy.PolicyTypeId = PolicyTypeRef.TemporaryPolicy.Id;
            clientVisit.NewPolicy.Series = string.Empty;
            clientVisit.NewPolicy.Number = firstClientVisit.TemporaryPolicyNumber;
            clientVisit.CarrierId = firstClientVisit.CarrierId;
            clientVisit.ApplicationMethodId = firstClientVisit.ApplicationMethodId;
            clientVisit.Representative.RepresentativeTypeId = firstClientVisit.Representative.RepresentativeTypeId;
            clientVisit.NewPolicy.StartDate = DateTime.Now.AddWorkingDays(-1);
            if (firstClientVisit.CarrierId == Carriers.PaperPolicy.Id)
            {
                clientVisit.GOZNAKTypeId = GoznakTypes.PrintPolicyRepeatedly.Id;
            }
            else if (firstClientVisit.CarrierId == Carriers.DigitalPolicy.Id)
            {
                clientVisit.GOZNAKTypeId = GoznakTypes.DigitalPolicyRepeatedly.Id;
            }
            else
            {
                clientVisit.GOZNAKTypeId = GoznakTypes.DontSent.Id;
            }
            CopyNewDocToOld();
        }

        private void ProcessCP()
        {
            clientVisit.NewPolicy.UnifiedPolicyNumber = clientVisit.OldPolicy.UnifiedPolicyNumber;
            clientVisit.NewPolicy.OGRN = string.Empty;
            clientVisit.NewPolicy.OKATO = string.Empty;
            clientVisit.NewPolicy.PolicyTypeId = PolicyTypeRef.TemporaryPolicy.Id;
            clientVisit.NewPolicy.Series = string.Empty;
            clientVisit.NewPolicy.Number = firstClientVisit.TemporaryPolicyNumber;
            clientVisit.CarrierId = firstClientVisit.CarrierId;
            clientVisit.ApplicationMethodId = firstClientVisit.ApplicationMethodId;
            clientVisit.Representative.RepresentativeTypeId = firstClientVisit.Representative.RepresentativeTypeId;
            clientVisit.NewPolicy.StartDate = DateTime.Now.AddWorkingDays(-1);
            if (firstClientVisit.CarrierId == Carriers.PaperPolicy.Id)
            {
                clientVisit.GOZNAKTypeId = GoznakTypes.PrintPolicyFirstTime.Id;
            }
            else if (firstClientVisit.CarrierId == Carriers.DigitalPolicy.Id)
            {
                clientVisit.GOZNAKTypeId = GoznakTypes.DigitalPolicyFirstTime.Id;
            }
            else
            {
                clientVisit.GOZNAKTypeId = GoznakTypes.DontSent.Id;
            }
            CopyNewDocToOld();
        }

        private void ProcessPR()
        {
            clientVisit.NewPolicy.UnifiedPolicyNumber = clientVisit.OldPolicy.UnifiedPolicyNumber;
            clientVisit.NewPolicy.OGRN = string.Empty;
            clientVisit.NewPolicy.OKATO = string.Empty;
            clientVisit.NewPolicy.PolicyTypeId = PolicyTypeRef.TemporaryPolicy.Id;
            clientVisit.NewPolicy.Series = string.Empty;
            clientVisit.NewPolicy.Number = firstClientVisit.TemporaryPolicyNumber;
            clientVisit.CarrierId = firstClientVisit.CarrierId;
            clientVisit.ApplicationMethodId = firstClientVisit.ApplicationMethodId;
            clientVisit.Representative.RepresentativeTypeId = firstClientVisit.Representative.RepresentativeTypeId;
            clientVisit.NewPolicy.StartDate = DateTime.Now.AddWorkingDays(-1);
            if (firstClientVisit.CarrierId == Carriers.PaperPolicy.Id)
            {
                clientVisit.GOZNAKTypeId = GoznakTypes.PrintPolicyFirstTime.Id;
            }
            else if (firstClientVisit.CarrierId == Carriers.DigitalPolicy.Id)
            {
                clientVisit.GOZNAKTypeId = GoznakTypes.DigitalPolicyFirstTime.Id;
            }
            else
            {
                clientVisit.GOZNAKTypeId = GoznakTypes.DontSent.Id;
            }
            CopyNewDocToOld();
        }

        private void ProcessMP()
        {
            clientVisit.CarrierId = null;
            clientVisit.ApplicationMethodId = null;
            clientVisit.GOZNAKTypeId = null;
            clientVisit.Representative.RepresentativeTypeId = null;
            clientVisit.ClientContacts = null;
            CopyNewDocToOld();
        }

        private void ProcessRD()
        {
            clientVisit.CarrierId = null;
            clientVisit.ApplicationMethodId = null;
            clientVisit.GOZNAKTypeId = null;
            clientVisit.Representative.RepresentativeTypeId = null;
            clientVisit.ClientContacts = null;
            CopyNewDocToOld();
        }

        private void ProcessCD()
        {
            if (oldS5 != null)
            {
                clientVisit.NewPolicy.PolicyTypeId = oldS5.PolicyTypeId;
                clientVisit.NewPolicy.UnifiedPolicyNumber = oldS5.UnifiedPolicyNumber;
            }
            clientVisit.NewPolicy.Series = string.Empty;
            clientVisit.NewPolicy.OGRN = string.Empty;
            clientVisit.NewPolicy.OKATO = string.Empty;
            clientVisit.GOZNAKTypeId = null;
            clientVisit.CarrierId = firstClientVisit.CarrierId;
            clientVisit.ApplicationMethodId = firstClientVisit.ApplicationMethodId;
            clientVisit.Representative.RepresentativeTypeId = firstClientVisit.Representative.RepresentativeTypeId;
            CopyNewDocToOld();
        }

        private void ProcessCLR()
        {
            clientVisit.CarrierId = null;
            clientVisit.ApplicationMethodId = null;
            clientVisit.GOZNAKTypeId = null;
            clientVisit.DeliveryPointId = null;
            clientVisit.Representative.RepresentativeTypeId = null;
            clientVisit.NewPolicy.EndDate = DateTime.Now;
            ClearOldDocument();
            ClearOldPolicy();
            ClearOldClientVersionData();
        }

        private void ProcessPOK()
        {
            clientVisit.NewPolicy.Series = string.Empty;
            clientVisit.NewPolicy.Number = clientVisit.NewPolicy.UnifiedPolicyNumber;
            clientVisit.NewPolicy.OGRN = string.Empty;
            clientVisit.NewPolicy.OKATO = string.Empty;
            clientVisit.CarrierId = null;
            clientVisit.ApplicationMethodId = null;
            clientVisit.GOZNAKTypeId = null;
            clientVisit.Representative.RepresentativeTypeId = null;
            if (firstClientVisit.CarrierId == Carriers.PaperPolicy.Id)
            {
                clientVisit.NewPolicy.PolicyTypeId = PolicyTypeRef.UnifiedPolicy.Id;
            }
            if (firstClientVisit.CarrierId == Carriers.DigitalPolicy.Id)
            {
                clientVisit.NewPolicy.PolicyTypeId = PolicyTypeRef.DigitalPolicy.Id;
            }
            ClearOldDocument();
            ClearOldPolicy();
            ClearOldClientVersionData();
        }

        private void ProcessAD()
        {
            clientVisit.GOZNAKTypeId = null;
            clientVisit.CarrierId = firstClientVisit.CarrierId;
            clientVisit.ApplicationMethodId = firstClientVisit.ApplicationMethodId;
            clientVisit.Representative.RepresentativeTypeId = firstClientVisit.Representative.RepresentativeTypeId;
            ClearOldDocument();
            ClearOldPolicy();
            ClearOldClientVersionData();
        }

        public ClientVisit.SaveData Process()
        {
            if (clientVisit.ScenarioId.HasValue && dataProcessors.ContainsKey(clientVisit.ScenarioId.Value))
            {
                dataProcessors[clientVisit.ScenarioId.Value]();
            }
            return clientVisit;
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

        private void ClearOldPolicy()
        {
            clientVisit.OldPolicy = new PolicyInfo.SaveData() { Id = clientVisit.OldPolicy.Id };
        }

        private void ClearOldClientVersionData()
        {
            clientVisit.OldClientInfo = new ClientVersion.SaveData() { Id = clientVisit.OldClientInfo.Id };
        }
    }
}
