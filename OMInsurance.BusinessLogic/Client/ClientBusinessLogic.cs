using OMInsurance.DataAccess.DAO;
using OMInsurance.DBUtils;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using OMInsurance.Entities.Core.Exeptions;
using OMInsurance.Entities.Searching;
using OMInsurance.Entities.Sorting;
using OMInsurance.Interfaces;
using OMInsurance.PrintedForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OMInsurance.Configuration;
using System.Dynamic;

namespace OMInsurance.BusinessLogic
{
    public class ClientBusinessLogic : IClientBusinessLogic
    {
        /// <summary>
        /// Returns sorted list of ClientBaseInfo by specified criteria
        /// </summary>
        /// <param name="criteria">Specified criteria</param>
        /// <param name="sortCriteria">Sort criteria</param>
        /// <param name="pageRequest">Page size and number</param>
        /// <returns>List of ClientBaseInfo</returns>
        public DataPage<ClientBaseInfo> Client_Find(
             ClientSearchCriteria criteria,
             List<SortCriteria<ClientSortField>> sortCriteria,
             PageRequest pageRequest)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException("criteria can't be null");
            }

            if (sortCriteria == null)
            {
                throw new ArgumentNullException("sortCriteria can't be null");
            }

            if (pageRequest == null)
            {
                throw new ArgumentNullException("pageRequest can't be null");
            }

            return ClientDao.Instance.Client_Find(criteria, sortCriteria, pageRequest);
        }


        /// <summary>
        /// Set new status for specifies client visit
        /// </summary>
        /// <param name="id">Client visit identifier</param>
        /// <param name="status">New status identifier</param>
        public void ClientVisit_SetStatus(User user, long id, long status, bool isActualClientVisit)
        {
            ClientVisit_SetStatus(user, id, status, isActualClientVisit, null);
        }

        /// <summary>
        /// Set new status for specifies client visit
        /// </summary>
        /// <param name="id">Client visit identifier</param>
        /// <param name="status">New status identifier</param>
        public void ClientVisit_SetStatus(User user, long id, long status, bool isActualClientVisit, DateTime? statusDate)
        {
            ClientDao.Instance.ClientVisit_SetStatus(user.Id, id, status, isActualClientVisit, statusDate);
        }

        /// <summary>
        /// Get status history for clientvisitgroup
        /// </summary>
        /// <param name="clientVisitGroupId">Client visit group identifier</param>
        public List<ClientVisitHistoryItem> ClientVisitHistory_Get(long clientVisitGroupId)
        {
            return ClientDao.Instance.ClientVisitHistory_Get(clientVisitGroupId);
        }

        /// <summary>
        /// Create or update client visit
        /// </summary>
        /// <param name="saveData">Data to save client visit</param>
        /// <returns>Identifiers of saved client visit</returns>
        public ClientVisitSaveResult ClientVisit_Save(User user, ClientVisit.SaveData saveData, DateTime? saveDate = null)
        {
            if (saveData == null)
            {
                throw new ArgumentNullException("createData can't be null");
            }

            if (saveData.ClientId.HasValue)
            {
                Client client = Client_Get(user, saveData.ClientId.Value);
                if (client == null)
                {
                    throw new DataObjectNotFoundException(string.Format("Клиент с идентификатором {0} не найден", saveData.ClientId.Value));
                }
            }

            SetPolicyEndDate(saveData);
            SetGoznakType(saveData);
            SetPolicy(saveData);
            SetStatus(saveData);
            ClientVisitSaveResult result = ClientDao.Instance.ClientVisit_Save(saveData, user.Id, saveDate);
            SetStatusForReceivedPolicies(user, saveData);

            return result;
        }

        /// <summary>
        /// Returnes client version by identifier
        /// </summary>
        /// <param name="id">Identifier of specified client version</param>
        /// <returns>Instance of client version</returns>
        public ClientVersion ClientVersion_Get(long id)
        {
            ClientVersion clientVersion = ClientDao.Instance.ClientVersion_Get(id);
            if (clientVersion == null)
            {
                throw new DataObjectNotFoundException(string.Format("Версия клиента с идентификатором {0} не найдена", id));
            }
            return clientVersion;
        }

        /// <summary>
        /// Returnes client visit by identifier
        /// </summary>
        /// <param name="id">Identifier of specified client visit</param>
        /// <returns>Instance of client visit</returns>
        public ClientVisit ClientVisit_Get(long id)
        {
            ClientVisit clientVisit = ClientDao.Instance.ClientVisit_Get(id);
            if (clientVisit == null)
            {
                throw new DataObjectNotFoundException(string.Format("Обращение клиента с идентификатором {0} не найдено", id));
            }
            return clientVisit;
        }


        /// <summary>
        /// Returnes last client visit by visit group identifier Id
        /// </summary>
        /// <param name="id">Identifier of specified visit group</param>
        /// <returns>Instance of client visit</returns>
        public ClientVisit ClientVisit_GetLastClientVisitInGroup(long clientVisitId)
        {
            ClientVisit clientVisit = ClientDao.Instance.ClientVisit_GetLastClientVisitInGroup(clientVisitId);
            if (clientVisit == null)
            {
                throw new DataObjectNotFoundException(string.Format("Группа обращений с идентификатором {0} не найдена", clientVisitId));
            }
            return clientVisit;
        }


        /// <summary>
        /// Returnes first client visit by visit group identifier Id
        /// </summary>
        /// <param name="id">Identifier of specified visit group</param>
        /// <returns>Instance of client visit</returns>
        public ClientVisit ClientVisit_GetFirstClientVisitInGroup(long clientVisitGroupId)
        {
            ClientVisit clientVisit = ClientDao.Instance.ClientVisit_GetFirstClientVisitInGroup(clientVisitGroupId);
            if (clientVisit == null)
            {
                throw new DataObjectNotFoundException(string.Format("Группа обращений с идентификатором {0} не найдена", clientVisitGroupId));
            }
            return clientVisit;
        }

        /// <summary>
        /// Returnes client by identifier for specified user depends on roles
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="id">Identifier of specified client</param>
        /// <returns>Instance of client</returns>
        public Client Client_Get(User user, long id)
        {
            Client client = ClientDao.Instance.Client_Get(id,
                !user.Roles.Exists(role => role.Id == Role.Administrator.Id || role.Id == Role.OperatorSG.Id));
            if (client == null)
            {
                throw new DataObjectNotFoundException(string.Format("Клиент с идентификатором {0} не найден", id));
            }
            return client;
        }

        /// <summary>
        /// Returns sorted list of ClientVisitInfo by specified criteria
        /// </summary>
        /// <param name="criteria">Specified criteria</param>
        /// <param name="sortCriteria">Sort criteria</param>
        /// <param name="pageRequest">Page size and number</param>
        /// <returns>List of ClientVisitInfo</returns>
        public DataPage<ClientVisitInfo> ClientVisit_Find(
            ClientVisitSearchCriteria criteria,
            List<SortCriteria<ClientVisitSortField>> sortCriteria,
            PageRequest pageRequest)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException("criteria can't be null");
            }

            if (sortCriteria == null)
            {
                throw new ArgumentNullException("sortCriteria can't be null");
            }

            if (pageRequest == null)
            {
                throw new ArgumentNullException("pageRequest can't be null");
            }

            return ClientDao.Instance.ClientVisit_Find(criteria, sortCriteria, pageRequest);
        }

        public byte[] FileReport_Get(
            ClientVisitSearchCriteria criteria,
            List<SortCriteria<ClientVisitSortField>> sortCriteria,
            PageRequest pageRequest)
        {
            return ClientVisitReportDao.Instance.GetClientVisitsReport(criteria, sortCriteria, pageRequest);
        }

        /// <summary>
        /// Method to update visit group and create a new client visit for fund
        /// </summary>
        /// <param name="dbfFilePath">DBF file that contains information to update</param>
        /// <returns>Result of updating</returns>
        public List<ClientVisit.UpdateResultData> ClientVisit_UpdateFundDbf(User user, string dbfFilePath)
        {
            BSOBusinessLogic bsoBusinessLogic = new BSOBusinessLogic();
            List<ClientVisit.UpdateData> dataToUpdate = ClientVisitReportDao.Instance.GetUpdateDataFromDbf(dbfFilePath);
            List<ClientVisit.UpdateResultData> results = new List<ClientVisit.UpdateResultData>();
            foreach (var updateClientVisitItem in dataToUpdate)
            {
                var clientVisits = FindClientVisitsToUpdate(updateClientVisitItem);

                ClientVisitInfo lastClientVisitInfo = clientVisits.OrderBy(v => v.Id).LastOrDefault();

                if (lastClientVisitInfo != null && (lastClientVisitInfo.Status.Id == ClientVisitStatuses.AnswerPending.Id
                    || lastClientVisitInfo.Status.Id == ClientVisitStatuses.AnswerPending.Id
                    || lastClientVisitInfo.Status.Id == ClientVisitStatuses.SubmitPending.Id
                    || lastClientVisitInfo.Status.Id == ClientVisitStatuses.SentToGoznak.Id
                    || lastClientVisitInfo.Status.Id == ClientVisitStatuses.Processed.Id
                    || lastClientVisitInfo.Status.Id == ClientVisitStatuses.Comment.Id))
                {
                    ClientVisit visit = ClientVisit_Get(lastClientVisitInfo.Id);

                    // create new clientVisit to update policy information in the fund
                    if (visit.Status.Id != ClientVisitStatuses.PolicyReadyForClient.Id)
                    {
                        ClientVisit.SaveData data = BuildUpdatedClientVisitSaveData(updateClientVisitItem, visit);
                        ClientVisitSaveResult updatingResult = ClientVisit_Save(user, data);

                        //если bso будет найден - ему меняем статус на "Выдан клиенту"
                        BSO bso = new BSO();
                        bso = bsoBusinessLogic.BSO_GetByNumber(updateClientVisitItem.TemporaryPolicyNumber);
                        if (bso != null)
                        {
                            if (bso.Status.Id == (long)ListBSOStatusID.OnDelivery || bso.Status.Id == (long)ListBSOStatusID.OnStorage)
                            {
                                bso.Status.Id = (long)ListBSOStatusID.OnClient;
                                bso.UserId = user.Id;
                                bso.DeliveryPointId = data.DeliveryPointId == null ? bso.DeliveryPointId : data.DeliveryPointId; //Если точка пустая - оставляем Точку, кот. была в БСО
                                bso.StatusDate = data.TemporaryPolicyDate == null ? data.StatusDate : data.TemporaryPolicyDate;
                                bso.Comment = string.Format("Изменение статуса при загрузке DBF (Загрузить DBF из фонда)");
                                bso.VisitGroupId = updatingResult.VisitGroupId;
                            }
                            else
                            {
                                //если bso уже выдан клиенту - то дальнейшие действия с БСО не имеют значения
                                bso = null;
                            }
                        }

                        // set status for created client visit
                        ClientVisit_SetStatus(user, updatingResult.ClientVisitID, ClientVisitStatuses.PolicyReadyForClient.Id, true);
                        if (bso != null) { bsoBusinessLogic.BSO_Save(new BSO.SaveData(bso)); }
                        results.Add(new ClientVisit.UpdateResultData(updateClientVisitItem, true, "Успешно", visit) { Status = ClientVisitStatuses.PolicyReadyForClient });
                    }
                    else
                    {
                        results.Add(new ClientVisit.UpdateResultData(updateClientVisitItem, true, "Заявка на выдачу была создана ранее", visit));
                    }
                }
                else
                {
                    results.Add(new ClientVisit.UpdateResultData(updateClientVisitItem, true, "Заявка в подходящем статусе не найдена"));
                }
            }
            return results;
        }

        public List<ClientVisit.UpdateResultData> ClientVisit_SetStatusesFromDbf(User CurrentUser, DateTime statusDate, string dbfFilePath)
        {
            try
            {
                List<ClientVisit.UpdateResultData> results = new List<ClientVisit.UpdateResultData>();
                List<long> ids = ClientVisitReportDao.Instance.GetClientVisitIdsFromDbf(dbfFilePath, "RECID_PV");
                if (ids.Count == 0)
                {
                    List<long> recids = ClientVisitReportDao.Instance.GetClientVisitIdsFromDbf(dbfFilePath, "RECID");
                    ids = FundProcessingDao.Instance.ClientVisitId_GetByFundRequesetRecid(recids).Select(item => item.ClientVisitId).ToList();
                }
                foreach (var id in ids)
                {
                    var clientVisit = ClientDao.Instance.ClientVisit_Get(id);
                    ClientVisit.UpdateResultData data = new ClientVisit.UpdateResultData();
                    data.Id = clientVisit.Id;
                    data.ClientId = clientVisit.ClientId;
                    data.ClientVisitGroupId = clientVisit.VisitGroupId;
                    data.Birthday = clientVisit.NewClientInfo.Birthday;
                    data.Firstname = clientVisit.NewClientInfo.Firstname;
                    data.Secondname = clientVisit.NewClientInfo.Secondname;
                    data.Lastname = clientVisit.NewClientInfo.Lastname;
                    data.UnifiedPolicyNumber = clientVisit.NewPolicy.UnifiedPolicyNumber;
                    if (clientVisit.Status.Id == ClientVisitStatuses.SubmitPending.Id
                        || clientVisit.Status.Id == ClientVisitStatuses.Reconciliation.Id
                        || clientVisit.Status.Id == ClientVisitStatuses.FundError.Id
                        || clientVisit.Status.Id == ClientVisitStatuses.PolicyIssued.Id)
                    {
                        ClientDao.Instance.ClientVisit_SetStatus(
                            CurrentUser.Id,
                            id,
                            ClientVisitStatuses.AnswerPending.Id,
                            clientVisit.IsActual,
                            statusDate);
                        data.IsSuccess = true;
                        data.Status = ClientVisitStatuses.AnswerPending;
                        data.Message = string.Format("Статус изменен c {0} на {1}", clientVisit.Status.Name, data.Status.Name);
                    }
                    else
                    {
                        data.Status = clientVisit.Status;
                        data.Message = "Статус не изменен";
                    }
                    results.Add(data);
                }
                return results;
            }
            finally
            {
                File.Delete(dbfFilePath);
            }
        }

        public List<ClientVisit> ClientVisit_GetFromDBF(string path, List<string> errors)
        {
            List<ClientVisit> list = ClientVisitReportDao.Instance.GetDataToCreateClientVisitfromDBF(path, errors);
            return list;
        }

        public ClientVisit FindLastClientVisit(ClientVisit cv)
        {
            DataPage<ClientVisitInfo> foundClientVisits = new DataPage<ClientVisitInfo>();
            ClientVisit lastClientVisit = null;
            ClientVisitSearchCriteria clientVisitSC = new ClientVisitSearchCriteria();
            clientVisitSC.Firstname = cv.NewClientInfo.Firstname;
            clientVisitSC.Secondname = cv.NewClientInfo.Secondname;
            clientVisitSC.Lastname = cv.NewClientInfo.Lastname;
            clientVisitSC.Birthday = cv.NewClientInfo.Birthday;

            if (cv.TemporaryPolicyDate.HasValue)
            {
                clientVisitSC.TemporaryPolicyDateFrom = cv.TemporaryPolicyDate;
                clientVisitSC.TemporaryPolicyDateTo = cv.TemporaryPolicyDate;
                foundClientVisits = ClientVisit_Find(
                    clientVisitSC,
                    new List<SortCriteria<ClientVisitSortField>>(),
                    new PageRequest() { PageNumber = 1, PageSize = 10 });
                clientVisitSC.TemporaryPolicyDateFrom = null;
                clientVisitSC.TemporaryPolicyDateTo = null;

                if (foundClientVisits.Count != 0)
                {
                    long clientVisitId = foundClientVisits.OrderBy(item => item.StatusDate).ThenBy(item => item.Id).LastOrDefault().Id;
                    return ClientVisit_Get(clientVisitId);
                }
            }

            if (!string.IsNullOrEmpty(cv.NewClientInfo.SNILS))
            {
                clientVisitSC.SNILS = cv.NewClientInfo.SNILS;
                foundClientVisits = ClientVisit_Find(
                    clientVisitSC,
                    new List<SortCriteria<ClientVisitSortField>>(),
                    new PageRequest() { PageNumber = 1, PageSize = 10 });
                clientVisitSC.SNILS = null;
            }

            if (cv.NewDocument.DocumentType != null && !string.IsNullOrEmpty(cv.NewDocument.Number))
            {
                clientVisitSC.DocumentTypeId = cv.NewDocument.DocumentType.Id;
                clientVisitSC.DocumentSeries = cv.NewDocument.Series;
                clientVisitSC.DocumentNumber = cv.NewDocument.Number;
                foundClientVisits = ClientVisit_Find(clientVisitSC, new List<SortCriteria<ClientVisitSortField>>(), new PageRequest() { PageNumber = 1, PageSize = 10 });

                clientVisitSC.DocumentTypeId = null;
                clientVisitSC.DocumentSeries = null;
                clientVisitSC.DocumentNumber = null;
            }

            if (!string.IsNullOrEmpty(cv.NewPolicy.Series) && !string.IsNullOrEmpty(cv.NewPolicy.Number))
            {
                clientVisitSC.PolicySeries = cv.NewPolicy.Series;
                clientVisitSC.PolicyNumber = cv.NewPolicy.Number;
                foundClientVisits = ClientVisit_Find(clientVisitSC, new List<SortCriteria<ClientVisitSortField>>(), new PageRequest() { PageNumber = 1, PageSize = 10 });
                clientVisitSC.PolicySeries = null;
                clientVisitSC.PolicyNumber = null;
            }

            if (!string.IsNullOrEmpty(cv.NewPolicy.UnifiedPolicyNumber))
            {
                clientVisitSC.UnifiedPolicyNumber = cv.NewPolicy.UnifiedPolicyNumber;
                foundClientVisits = ClientVisit_Find(clientVisitSC, new List<SortCriteria<ClientVisitSortField>>(), new PageRequest() { PageNumber = 1, PageSize = 10 });
                clientVisitSC.UnifiedPolicyNumber = null;
            }

            ClientVisitInfo clientVisitInfo = foundClientVisits.OrderBy(item => item.StatusDate).LastOrDefault();
            if (clientVisitInfo == null)
            {
                return null;
            }
            lastClientVisit = ClientVisit_Get(clientVisitInfo.Id);
            return lastClientVisit;
        }

        public void RegionPolicyData_Save(List<PolicyFromRegion> listPolicy)
        {
            PolicyDao.Instance.RegionPolicyData_Save(listPolicy);
        }

        public ClientPretension ClientPretension_Generation(long ClientId)
        {
            return ClientDao.Instance.ClientPretension_Generation(ClientId);
        }

        public void ClientPretension_Save(ClientPretension pretension)
        {
            ClientDao.Instance.ClientPretension_Save(pretension);
        }

        public List<ClientPretension> ClientPretension_Get(long ClientId)
        {
            return ClientDao.Instance.ClientPretension_Get(ClientId);
        }

        public byte[] PretensionGetLPU(ClientPretension pretension)
        {
            List<FileWrapper> files = new List<FileWrapper>();
            string pathTxt = Path.Combine(ConfiguraionProvider.FileStorageFolder, "b_mek_" + pretension.sevenSimbolGeneration + ".txt");
            string pathPdf = Path.Combine(ConfiguraionProvider.FileStorageFolder, pretension.FileUrlLPU);

            using (TextWriter writer = new StreamWriter(pathTxt))
            {
                writer.WriteLine("To: oms@pfns.msk.oms");
                writer.WriteLine(string.Format("Message-Id: {0}.OMS@skpomed.msk.oms", pretension.sevenSimbolGeneration));
                writer.WriteLine("Subject: Запрос на целевую МЭЭ.");
                writer.WriteLine("Content-Type: multipart/mixed");
                writer.WriteLine(string.Format("Attachment: d_mek_{0}.pdf", pretension.sevenSimbolGeneration));
            }
            var fileTxt = System.IO.File.ReadAllBytes(pathTxt);
            System.IO.File.Delete(pathTxt);
            var filePdf = System.IO.File.ReadAllBytes(pathPdf);

            files.Add(new FileWrapper() { Filename = Path.GetFileName(pathTxt), Content = fileTxt });
            files.Add(new FileWrapper() { Filename = string.Format("d_mek_{0}.pdf", pretension.sevenSimbolGeneration), Content = filePdf });
            return ZipHelper.ZipFiles(files); ;
        }

        public byte[] PretensionGetFile(ClientPretension pretension)
        {
            Random rnd = new Random("pretension".GetHashCode() + DateTime.Now.GetHashCode());
            long messageId = rnd.Next(10000000, 99999999);
            pretension.sevenSimbolGeneration = "121" + string.Format("{0,4:D4}", pretension.Generator);
            if (pretension.sevenSimbolGeneration.Length != 7) pretension.sevenSimbolGeneration = "121ERROR";

            List<FileWrapper> files = new List<FileWrapper>();

            string pathTxt = Path.Combine(ConfiguraionProvider.FileStorageFolder, "b_mek_" + pretension.sevenSimbolGeneration + ".txt");
            string fileNameExcelZip = string.Format("Excel_{0}.zip", pretension.sevenSimbolGeneration);

            using (TextWriter writer = new StreamWriter(pathTxt))
            {
                writer.WriteLine("To: oms@pfns.msk.oms");
                writer.WriteLine(string.Format("Message-Id: {0}.OMS@skpomed.msk.oms", messageId));
                writer.WriteLine("Subject: Запрос на целевую МЭЭ.");
                writer.WriteLine("Content-Type: multipart/mixed");
                writer.WriteLine("Attachment: {0}", fileNameExcelZip);
            }
            var file = System.IO.File.ReadAllBytes(pathTxt);
            System.IO.File.Delete(pathTxt);

            PretensionAnnulirovanie printedAnnulirovanie = new PretensionAnnulirovanie(pretension);
            PretensionOrder183 printedPretensionOrder183 = new PretensionOrder183(pretension);

            files.Add(new FileWrapper() { Filename = Path.GetFileName(pathTxt), Content = file });

            List<FileWrapper> filesForInnerZip = new List<FileWrapper>();
            filesForInnerZip.Add(new FileWrapper() { Filename = "Annulirovanie.xlsx", Content = printedAnnulirovanie.GetExcel() });
            filesForInnerZip.Add(new FileWrapper() { Filename = "Предписание 183.xlsx", Content = printedPretensionOrder183.GetExcel() });
            var ContentExcelZip = ZipHelper.ZipFiles(filesForInnerZip);
            files.Add(new FileWrapper() { Filename = fileNameExcelZip, Content = ContentExcelZip });

            return ZipHelper.ZipFiles(files);
        }

        public byte[] PretensionGetLast(ClientPretension pretension, Client client)
        {
            List<FileWrapper> files = new List<FileWrapper>();
            string pathTxt = Path.Combine(ConfiguraionProvider.FileStorageFolder, "b" + pretension.sevenSimbolGeneration);
            string pathPdf = Path.Combine(ConfiguraionProvider.FileStorageFolder, pretension.FileUrl2);

            using (TextWriter writer = new StreamWriter(pathTxt))
            {
                writer.WriteLine("To: oms@pfns.msk.oms");
                writer.WriteLine(string.Format("Message-Id: {0}.OMS@skpomed.msk.oms", pretension.sevenSimbolGeneration));
                writer.WriteLine("Subject: Запрос на целевую МЭЭ.");
                writer.WriteLine("Content-Type: multipart/mixed");
                writer.WriteLine(string.Format("Attachment: d{0}", pretension.sevenSimbolGeneration));
            }
            var fileTxt = System.IO.File.ReadAllBytes(pathTxt);
            System.IO.File.Delete(pathTxt);
            var filePdf = System.IO.File.ReadAllBytes(pathPdf);

            List<FileWrapper> filesForInnerZip = new List<FileWrapper>();
            filesForInnerZip.Add(new FileWrapper() { Filename = Path.GetFileName(pathPdf), Content = filePdf });

            LastPackageForPretension pr = new LastPackageForPretension();
            string filepathDBF = string.Format("npP2{0}.dbf", pretension.sevenSimbolGeneration);

            FileWrapper fileDBF = pr.SaveToUralsibDBF(pretension, client, filepathDBF);

            filesForInnerZip.Add(fileDBF);
            //filesForInnerZip.Add(new FileWrapper() { Filename = filepathDBF, Content = new byte[1] });
            var contentInnerZip = ZipHelper.ZipFiles(filesForInnerZip);

            files.Add(new FileWrapper() { Filename = Path.GetFileName(pathTxt), Content = fileTxt });
            files.Add(new FileWrapper() { Filename = "d"+ pretension.sevenSimbolGeneration, Content = contentInnerZip });
            return ZipHelper.ZipFiles(files); ;
        }

        #region Private methods

        private ClientVisit.SaveData BuildUpdatedClientVisitSaveData(ClientVisit.UpdateData updateClientVisitItem, ClientVisit visit)
        {
            ClientVisit.SaveData data = new ClientVisit.SaveData(visit);
            data.Blanc = updateClientVisitItem.Blanc;
            data.N_KOR = updateClientVisitItem.N_KOR;
            data.DATA_FOND = updateClientVisitItem.DATA_FOND;
            data.NZ_GOZNAK = updateClientVisitItem.NZ_GOZNAK;
            data.PolicyPartyNumber = updateClientVisitItem.PolicyPartyNumber.Trim();
            data.NewPolicy.UnifiedPolicyNumber = updateClientVisitItem.UnifiedPolicyNumber.Trim();
            data.NewPolicy.OGRN = updateClientVisitItem.OGRN.Trim();
            data.NewClientInfo.Lastname = updateClientVisitItem.Lastname.Trim();
            data.NewClientInfo.Firstname = updateClientVisitItem.Firstname.Trim();
            data.NewClientInfo.Secondname = updateClientVisitItem.Secondname.Trim();
            data.NewClientInfo.Birthday = updateClientVisitItem.Birthday;
            data.Dat_U = updateClientVisitItem.Dat_U;
            data.Dat_S = updateClientVisitItem.Dat_S;
            return data;
        }

        private void SetPolicy(ClientVisit.SaveData saveData)
        {
            if (string.IsNullOrEmpty(saveData.NewPolicy.UnifiedPolicyNumber)
                && !string.IsNullOrEmpty(saveData.OldPolicy.UnifiedPolicyNumber))
            {
                saveData.NewPolicy.UnifiedPolicyNumber = saveData.OldPolicy.UnifiedPolicyNumber;
            }

            if (saveData.Status == ClientVisitStatuses.SubmitPending.Id
                && !string.IsNullOrEmpty(saveData.OldDocument.Number))
            {
                CopyNewClientToOldClientData(saveData);
            }
        }

        private void SetStatus(ClientVisit.SaveData saveData)
        {
            //если статус "полис изготовлен" + сценарий POK + дата отсутствует, то проставлять текущую дату в поле "дата выдачи полиса"
            if (saveData.ClientId.HasValue && saveData.Id.HasValue)
            {
                if (saveData.Status == ClientVisitStatuses.PolicyReadyForClient.Id
                    && saveData.ScenarioId == ClientVisitScenaries.PolicyExtradition.Id)
                {
                    if (!saveData.IssueDate.HasValue)
                    {
                        saveData.IssueDate = DateTime.Now;
                    }
                }
            }
        }

        private static void CopyNewClientToOldClientData(ClientVisit.SaveData saveData)
        {
            if (!string.IsNullOrEmpty(saveData.OldDocument.Number))
            {
                if (string.IsNullOrEmpty(saveData.OldClientInfo.Firstname)
                    && !string.IsNullOrEmpty(saveData.NewClientInfo.Firstname))
                {
                    saveData.OldClientInfo.Firstname = saveData.NewClientInfo.Firstname;
                    saveData.OldClientInfo.FirstnameTypeId = saveData.NewClientInfo.FirstnameTypeId;
                }
                if (string.IsNullOrEmpty(saveData.OldClientInfo.Secondname)
                    && !string.IsNullOrEmpty(saveData.NewClientInfo.Secondname))
                {
                    saveData.OldClientInfo.Secondname = saveData.NewClientInfo.Secondname;
                    saveData.OldClientInfo.SecondnameTypeId = saveData.NewClientInfo.SecondnameTypeId;
                }
                if (string.IsNullOrEmpty(saveData.OldClientInfo.Lastname)
                    && !string.IsNullOrEmpty(saveData.NewClientInfo.Lastname))
                {
                    saveData.OldClientInfo.Lastname = saveData.NewClientInfo.Lastname;
                    saveData.OldClientInfo.LastnameTypeId = saveData.NewClientInfo.LastnameTypeId;
                }
                if (!saveData.OldClientInfo.Birthday.HasValue
                    && saveData.NewClientInfo.Birthday.HasValue)
                {
                    saveData.OldClientInfo.Birthday = saveData.NewClientInfo.Birthday;
                }
                if (!saveData.OldClientInfo.Category.HasValue
                    && saveData.NewClientInfo.Category.HasValue)
                {
                    saveData.OldClientInfo.Category = saveData.NewClientInfo.Category;
                }
                if (!saveData.OldClientInfo.Citizenship.HasValue
                    && saveData.NewClientInfo.Citizenship.HasValue)
                {
                    saveData.OldClientInfo.Citizenship = saveData.NewClientInfo.Citizenship;
                }
                if (string.IsNullOrEmpty(saveData.OldClientInfo.SNILS)
                    && !string.IsNullOrEmpty(saveData.NewClientInfo.SNILS))
                {
                    saveData.OldClientInfo.SNILS = saveData.NewClientInfo.SNILS;
                }
                if (string.IsNullOrEmpty(saveData.OldClientInfo.Birthplace)
                    && !string.IsNullOrEmpty(saveData.NewClientInfo.Birthplace))
                {
                    saveData.OldClientInfo.Birthplace = saveData.NewClientInfo.Birthplace;
                }
                if (!saveData.OldClientInfo.Sex.HasValue
                    && saveData.NewClientInfo.Sex.HasValue)
                {
                    saveData.OldClientInfo.Sex = saveData.NewClientInfo.Sex;
                }
            }
        }

        private DataPage<ClientVisitInfo> FindClientVisitsToUpdate(ClientVisit.UpdateData item)
        {
            ClientVisitSearchCriteria criteria = new ClientVisitSearchCriteria();
            criteria.Firstname = item.Firstname.Trim();
            criteria.Secondname = item.Secondname.Trim();
            criteria.Lastname = item.Lastname.Trim();
            criteria.Birthday = item.Birthday;
            criteria.TemporaryPolicyNumber = item.TemporaryPolicyNumber.Trim();
            var clientVisits = ClientVisit_Find(
                criteria,
                new List<SortCriteria<ClientVisitSortField>>(),
                new PageRequest() { PageNumber = 1, PageSize = 100 });
            return clientVisits;
        }

        /// <summary>
        /// Set status PolicyExtradition for clientVisit with IssueDate
        /// </summary>
        /// <param name="saveData">Data to dave policy</param>
        private void SetStatusForReceivedPolicies(User user, ClientVisit.SaveData saveData)
        {
            if (saveData.ClientId.HasValue && saveData.Id.HasValue)
            {
                ClientVisit clientVisit = ClientVisit_Get(saveData.Id.Value);
                if (clientVisit.Status.Id == ClientVisitStatuses.PolicyReadyForClient.Id
                    //&& saveData.IssueDate.HasValue
                    && saveData.ScenarioId == ClientVisitScenaries.PolicyExtradition.Id)
                {
                    if (!saveData.IssueDate.HasValue)
                    {
                        saveData.IssueDate = DateTime.Now;
                    }
                    ClientVisit_SetStatus(user, clientVisit.Id, ClientVisitStatuses.PolicyIssued.Id, true);
                }
            }
        }


        /// <summary>
        /// Merge source client to destination
        /// </summary>
        public long Clients_Merge(long sourceClientId, long destinationClientId, User currenUser)
        {
            Client source = Client_Get(currenUser, sourceClientId);
            Client destination = Client_Get(currenUser, destinationClientId);
            return ClientDao.Instance.Clients_Merge(sourceClientId, destinationClientId);
        }

        /// <summary>
        /// Split specified visit group to new client
        /// </summary>
        public long Clients_Split(long visitGroupId)
        {
            return ClientDao.Instance.Clients_Split(visitGroupId);
        }


        /// <summary>
        /// Set default policy end date by client category
        /// </summary>
        private void SetGoznakType(ClientVisit.SaveData saveData)
        {
            if (!saveData.GOZNAKTypeId.HasValue)
            {
                if (string.IsNullOrEmpty(saveData.TemporaryPolicyNumber))
                {
                    if (saveData.ScenarioId == ClientVisitScenaries.ChangeDocument.Id
                        || saveData.ScenarioId == ClientVisitScenaries.ReregistrationMoscowENPWithoutFIO.Id
                        || saveData.ScenarioId == ClientVisitScenaries.ReregistrationRegionalENPWithoutFIO.Id)
                    {
                        saveData.GOZNAKTypeId = GoznakTypes.DontSent.Id;
                    }
                }
                else
                {
                    if (saveData.CarrierId == Carriers.UEK.Id)
                    {
                        if (saveData.ScenarioId == ClientVisitScenaries.ReregistrationMoscowENPWithoutFIO.Id
                        || saveData.ScenarioId == ClientVisitScenaries.RequestENPSameSMOChangeFIO.Id
                        || saveData.ScenarioId == ClientVisitScenaries.ReregistrationRegionalENPWithoutFIO.Id
                        || saveData.ScenarioId == ClientVisitScenaries.LostENPWithoutFIO.Id
                        || saveData.ScenarioId == ClientVisitScenaries.FirstRequestENP.Id
                        || saveData.ScenarioId == ClientVisitScenaries.ReregistrationMoscowENPWithFIO.Id
                        || saveData.ScenarioId == ClientVisitScenaries.ReregistrationRegionalENPWithFIO.Id)
                        {
                            saveData.GOZNAKTypeId = GoznakTypes.DontSent.Id;
                        }
                    }
                    if (saveData.CarrierId == Carriers.PaperPolicy.Id)
                    {
                        if (saveData.ScenarioId == ClientVisitScenaries.FirstRequestENP.Id)
                        {
                            saveData.GOZNAKTypeId = GoznakTypes.PrintPolicyFirstTime.Id;
                        }
                        if (saveData.ScenarioId == ClientVisitScenaries.ReregistrationMoscowENPWithoutFIO.Id
                        || saveData.ScenarioId == ClientVisitScenaries.RequestENPSameSMOChangeFIO.Id
                        || saveData.ScenarioId == ClientVisitScenaries.ReregistrationRegionalENPWithoutFIO.Id
                        || saveData.ScenarioId == ClientVisitScenaries.LostENPWithoutFIO.Id
                        || saveData.ScenarioId == ClientVisitScenaries.ReregistrationMoscowENPWithFIO.Id
                        || saveData.ScenarioId == ClientVisitScenaries.ReregistrationRegionalENPWithFIO.Id)
                        {
                            saveData.GOZNAKTypeId = GoznakTypes.PrintPolicyRepeatedly.Id;
                        }
                    }
                    if (saveData.CarrierId == Carriers.DigitalPolicy.Id)
                    {
                        if (saveData.ScenarioId == ClientVisitScenaries.FirstRequestENP.Id)
                        {
                            saveData.GOZNAKTypeId = GoznakTypes.DigitalPolicyFirstTime.Id;
                        }
                        if (saveData.ScenarioId == ClientVisitScenaries.ReregistrationMoscowENPWithoutFIO.Id
                        || saveData.ScenarioId == ClientVisitScenaries.RequestENPSameSMOChangeFIO.Id
                        || saveData.ScenarioId == ClientVisitScenaries.ReregistrationRegionalENPWithoutFIO.Id
                        || saveData.ScenarioId == ClientVisitScenaries.LostENPWithoutFIO.Id
                        || saveData.ScenarioId == ClientVisitScenaries.ReregistrationMoscowENPWithFIO.Id
                        || saveData.ScenarioId == ClientVisitScenaries.ReregistrationRegionalENPWithFIO.Id)
                        {
                            saveData.GOZNAKTypeId = GoznakTypes.DigitalPolicyRepeatedly.Id;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Set default policy end date by client category
        /// </summary>
        private void SetPolicyEndDate(ClientVisit.SaveData saveData)
        {
            long? clientCategoryId = saveData.NewClientInfo.Category;
            long registredForeign = 3;
            long notRegistredForeign = 5;
            long refugee = 4;
            if (!saveData.NewPolicy.EndDate.HasValue)
            {
                if (clientCategoryId == registredForeign || clientCategoryId == notRegistredForeign)
                {
                    saveData.NewPolicy.EndDate = saveData.NewForeignDocument.ExpirationDate;
                }
                else if (clientCategoryId == refugee)
                {
                    saveData.NewPolicy.EndDate = saveData.NewDocument.ExpirationDate;
                }
                else
                {
                    saveData.NewPolicy.EndDate = new DateTime(2099, 12, 31);
                }
            }
        }

        #endregion

    }
}
