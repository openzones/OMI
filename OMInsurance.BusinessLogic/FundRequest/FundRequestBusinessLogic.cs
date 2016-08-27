using OmInsurance.FundPackages;
using OmInsurance.FundPackages.Request;
using OMInsurance.BusinessLogic.FundRequest;
using OMInsurance.Configuration;
using OMInsurance.DataAccess.DAO;
using OMInsurance.DBUtils;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using OMInsurance.Entities.Searching;
using OMInsurance.Entities.Sorting;
using OMInsurance.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace OMInsurance.BusinessLogic
{
    public class FundRequestBusinessLogic : IFundRequestBusinessLogic
    {
        private ClientBusinessLogic clientBusinessLogic = new ClientBusinessLogic();
        public List<FundResponse.UploadReportData> UploadFundResponse(User user, string zipPath, bool runScenario)
        {
            List<FundResponse.UploadReportData> report = new List<FundResponse.UploadReportData>();
            string zipDirectoryName = Path.Combine(ConfiguraionProvider.FileStorageFolder, Path.GetFileNameWithoutExtension(zipPath));
            ZipHelper.UnZipFiles(zipPath, zipDirectoryName);
            FundResponseCreateDataBuilder builder = new FundResponseCreateDataBuilder();
            var responsesToCreate = GetResponsesFromArchive(builder, zipDirectoryName);
            var responsesByclientVisitId = GroupByClientVisitId(report, responsesToCreate);
            DateTime date = DateTime.Now;
            foreach (var pack in responsesByclientVisitId)
            {
                long clientVisitId = pack.Key;
                ClientVisit clientVisit = ClientDao.Instance.ClientVisit_Get(clientVisitId);
                if (clientVisit == null)
                {
                    foreach (var response in pack.Value)
                    {
                        report.Add(new FundResponse.UploadReportData()
                        {
                            Recid = response.Recid,
                            ClientVisitId = clientVisitId,
                            ResponseTypeName = response.GetResponseTypeName(),
                            UploadResult = "Не найдена соответствующая заявка"
                        });
                    }
                    continue;
                }
                ClientVisit lastClientVisitInGroup = ClientDao.Instance.ClientVisit_GetLastClientVisitInGroup(clientVisit.VisitGroupId);
                ReferenceItem lastCurrentStatus = clientVisit.Status;
                DateTime lastClientStatusDate = clientVisit.StatusDate;
                long? lastClientVisitId = null;
                if (lastClientVisitInGroup.Status.Id == ClientVisitStatuses.FundError.Id
                    || lastClientVisitInGroup.Status.Id == ClientVisitStatuses.AnswerPending.Id
                    || lastClientVisitInGroup.Status.Id == ClientVisitStatuses.Processed.Id)
                {
                    clientBusinessLogic.ClientVisit_SetStatus(user, lastClientVisitInGroup.Id, ClientVisitStatuses.Reconciliation.Id, true);
                    lastCurrentStatus = ClientVisitStatuses.Reconciliation;
                    lastClientStatusDate = date;
                }
                if (lastClientVisitInGroup.Status.Id == ClientVisitStatuses.SubmitPending.Id)
                {
                    ClientVisit.SaveData newClientVisitData = new ClientVisit.SaveData(lastClientVisitInGroup);
                    newClientVisitData.Status = ClientVisitStatuses.Reconciliation.Id;
                    lastCurrentStatus = ClientVisitStatuses.Reconciliation;
                    newClientVisitData.StatusDate = date;
                    lastClientStatusDate = date;
                    newClientVisitData.IsActual = true;
                    var saveResult = clientBusinessLogic.ClientVisit_Save(user, newClientVisitData, date);
                    lastClientVisitId = saveResult.ClientVisitID;
                }
                foreach (var response in pack.Value)
                {
                    FundProcessingDao.Instance.FundResponse_Create(response, date);
                    report.Add(AddReportItem(clientVisit, response, lastClientVisitId, lastCurrentStatus, lastClientStatusDate));
                }
                if (runScenario)
                {
                    clientVisit = ClientDao.Instance.ClientVisit_GetLastClientVisitInGroup(clientVisit.VisitGroupId);
                    ScenarioResolver resolver = new ScenarioResolver(clientVisit, pack.Value.OfType<ReconciliationFundResponse.CreateData>().ToList());
                    ReferenceItem resolvedScenario = resolver.GetResolvedScenario();
                    ClientVisit.SaveData data = ClientVisit.SaveData.BuildSaveData(clientVisit);
                    if (resolvedScenario != null)
                    {
                        data.ScenarioId = resolvedScenario.Id;
                        data.FundResponseApplyingMessage = string.Format("Сценарий изменён с {0} на {1}",
                            clientVisit.Scenario != null ? clientVisit.Scenario.Code : string.Empty, resolvedScenario.Code);
                        data.IsReadyToFundSubmitRequest = true;
                        ClientVisitOldDataBuilder oldDataBuilder = new ClientVisitOldDataBuilder(data,
                            pack.Value.OfType<IReconciliationFundResponse>().ToList());
                        ClientVisit firstClientVisit = clientBusinessLogic.ClientVisit_GetFirstClientVisitInGroup(clientVisit.VisitGroupId);
                        ClientVisitNewDataBuilder newDataBuilder = new ClientVisitNewDataBuilder(data, firstClientVisit,
                            pack.Value.OfType<IReconciliationFundResponse>().ToList());
                        data = oldDataBuilder.Process();
                        data = newDataBuilder.Process();
                    }
                    clientBusinessLogic.ClientVisit_Save(user, data);
                }
            }
            return report;
        }

        public List<FundResponse.UploadReportData> UploadFundResponseUnion(User user, string zipPath, bool runScenario)
        {
            List<FundResponse.UploadReportData> report = new List<FundResponse.UploadReportData>();
            string zipDirectoryName = Path.Combine(ConfiguraionProvider.FileStorageFolder, Path.GetFileNameWithoutExtension(zipPath));
            ZipHelper.UnZipFiles(zipPath, zipDirectoryName);
            FundResponseCreateDataBuilder builder = new FundResponseCreateDataBuilder();
            var responsesToCreate = GetResponsesFromArchive(builder, zipDirectoryName);
            var responsesByclientVisitId = GroupByClientVisitId(report, responsesToCreate);
            DateTime date = DateTime.Now;
            foreach (var pack in responsesByclientVisitId)
            {
                long clientVisitId = pack.Key;
                ClientVisit clientVisit = ClientDao.Instance.ClientVisit_Get(clientVisitId);
                if (clientVisit == null)
                {
                    foreach (var response in pack.Value)
                    {
                        report.Add(new FundResponse.UploadReportData()
                        {
                            Recid = response.Recid,
                            ClientVisitId = clientVisitId,
                            ResponseTypeName = response.GetResponseTypeName(),
                            UploadResult = "Не найдена соответствующая заявка"
                        });
                    }
                    continue;
                }

                ClientVisit lastClientVisitInGroup = ClientDao.Instance.ClientVisit_GetLastClientVisitInGroup(clientVisit.VisitGroupId);
                ReferenceItem lastCurrentStatus = clientVisit.Status;
                DateTime lastClientStatusDate = clientVisit.StatusDate;
                long? lastClientVisitId = null;
                if (lastClientVisitInGroup.Status.Id == ClientVisitStatuses.FundError.Id
                    || lastClientVisitInGroup.Status.Id == ClientVisitStatuses.AnswerPending.Id
                    || lastClientVisitInGroup.Status.Id == ClientVisitStatuses.Processed.Id)
                {
                    //
                    clientBusinessLogic.ClientVisit_SetStatus(user, lastClientVisitInGroup.Id, ClientVisitStatuses.Reconciliation.Id, true);
                    lastCurrentStatus = ClientVisitStatuses.Reconciliation;
                    lastClientStatusDate = date;
                }
                if (lastClientVisitInGroup.Status.Id == ClientVisitStatuses.SubmitPending.Id)
                {
                    ClientVisit.SaveData newClientVisitData = new ClientVisit.SaveData(lastClientVisitInGroup);
                    newClientVisitData.Status = ClientVisitStatuses.Reconciliation.Id;
                    lastCurrentStatus = ClientVisitStatuses.Reconciliation;
                    newClientVisitData.StatusDate = date;
                    lastClientStatusDate = date;
                    newClientVisitData.IsActual = true;
                    //
                    var saveResult = clientBusinessLogic.ClientVisit_Save(user, newClientVisitData, date);
                    lastClientVisitId = saveResult.ClientVisitID;
                }

                //подтверждающие сверки
                {
                    ClientVisit.SaveData dataSecondStepReconciliationProcessor = ClientVisit.SaveData.BuildSaveData(lastClientVisitInGroup);
                    SecondStepReconciliationProcessor processor = new SecondStepReconciliationProcessor(dataSecondStepReconciliationProcessor, pack.Value.OfType<ReconciliationFundResponse.CreateData>().ToList());
                    processor.Process();
                    if (processor.IsChanged)
                    {
                        //
                        clientBusinessLogic.ClientVisit_Save(user, dataSecondStepReconciliationProcessor);
                    }
                }

                //проверки по сценарию POK
                {
                    ClientVisit.SaveData dataPOKProcessor = ClientVisit.SaveData.BuildSaveData(lastClientVisitInGroup);
                    POKProcessor processor = new POKProcessor(dataPOKProcessor, pack.Value.OfType<ReconciliationFundResponse.CreateData>().ToList());
                    processor.Process();
                    if (processor.IsChanged)
                    {
                        //
                        clientBusinessLogic.ClientVisit_Save(user, dataPOKProcessor);
                    }
                }

                foreach (var response in pack.Value)
                {
                    //
                    FundProcessingDao.Instance.FundResponse_Create(response, date);
                    report.Add(AddReportItem(clientVisit, response, lastClientVisitId, lastCurrentStatus, lastClientStatusDate));
                }

                if (runScenario)
                {
                    clientVisit = ClientDao.Instance.ClientVisit_GetLastClientVisitInGroup(clientVisit.VisitGroupId);
                    ScenarioResolver resolver = new ScenarioResolver(clientVisit, pack.Value.OfType<ReconciliationFundResponse.CreateData>().ToList());
                    ReferenceItem resolvedScenario = resolver.GetResolvedScenario();
                    ClientVisit.SaveData data = ClientVisit.SaveData.BuildSaveData(clientVisit);
                    if (resolvedScenario != null)
                    {
                        data.ScenarioId = resolvedScenario.Id;
                        data.FundResponseApplyingMessage = string.Format("Сценарий изменён с {0} на {1}",
                            clientVisit.Scenario != null ? clientVisit.Scenario.Code : string.Empty, resolvedScenario.Code);
                        data.IsReadyToFundSubmitRequest = true;
                        ClientVisitOldDataBuilder oldDataBuilder = new ClientVisitOldDataBuilder(data,
                            pack.Value.OfType<IReconciliationFundResponse>().ToList());
                        ClientVisit firstClientVisit = clientBusinessLogic.ClientVisit_GetFirstClientVisitInGroup(clientVisit.VisitGroupId);
                        ClientVisitNewDataBuilder newDataBuilder = new ClientVisitNewDataBuilder(data, firstClientVisit,
                            pack.Value.OfType<IReconciliationFundResponse>().ToList());
                        data = oldDataBuilder.Process();
                        data = newDataBuilder.Process();
                    }
                    //
                    clientBusinessLogic.ClientVisit_Save(user, data);
                }
            }
            return report;
        }

        private static Dictionary<long, List<FundResponse.CreateData>> GroupByClientVisitId(List<FundResponse.UploadReportData> report, IEnumerable<FundResponse.CreateData> responsesToCreate)
        {
            var responsesByclientVisitId = new Dictionary<long, List<FundResponse.CreateData>>();
            List<FundRequestRecid> recids =
                FundProcessingDao.Instance.ClientVisitId_GetByFundRequesetRecid(responsesToCreate.Select(item => item.Recid.Value));
            Dictionary<long, FundRequestRecid> recidsByRecid = recids.ToDictionary(item => item.Recid);
            foreach (FundResponse.CreateData response in responsesToCreate)
            {
                FundRequestRecid recid;
                if (recidsByRecid.TryGetValue(response.Recid.Value, out recid))
                {
                    response.ClientVisitId = recid.ClientVisitId;
                    response.DataTypeId = recid.DataTypeId;
                }
                if (response.ClientVisitId != 0)
                {
                    if (!responsesByclientVisitId.ContainsKey(response.ClientVisitId))
                    {
                        responsesByclientVisitId.Add(response.ClientVisitId, new List<FundResponse.CreateData>());
                    }
                    responsesByclientVisitId[response.ClientVisitId].Add(response);
                }
                else if (response.Recid.HasValue)
                {
                    report.Add(new FundResponse.UploadReportData()
                    {
                        Recid = response.Recid,
                        ResponseTypeName = response.GetResponseTypeName(),
                        UploadResult = "Не найдена соответствующая заявка"
                    });
                }
            }
            return responsesByclientVisitId;
        }

        private static List<FundResponse.CreateData> GetResponsesFromArchive(FundResponseCreateDataBuilder builder, string zipDirectoryName)
        {
            List<FundResponse.CreateData> responsesToCreate = new List<FundResponse.CreateData>();
            foreach (var directory in Directory.GetDirectories(zipDirectoryName))
            {
                DirectoryInfo dInfo = new DirectoryInfo(directory);
                string directoryName = dInfo.Name;
                IEnumerable<string> dataFiles = Directory.GetFiles(directory).Where(f => Path.GetFileName(f).StartsWith("d"));
                foreach (var dataFile in dataFiles)
                {
                    string fname = dataFile;
                    if (!fname.EndsWith("dbf"))
                    {
                        File.Copy(fname, fname = dataFile.Split('.')[0] + ".dbf");
                    }
                    if (!string.IsNullOrEmpty(fname))
                    {
                        DataTable dataTable = DBFProcessor.GetDataTable(fname, string.Format("select * from \"{0}\";", fname));
                        IEnumerable<FundResponse.CreateData> list = builder.BuildList(directoryName, dataTable);
                        responsesToCreate.AddRange(list);
                    }
                }
            }
            return responsesToCreate;
        }

        private static FundResponse.UploadReportData AddReportItem(
            ClientVisit clientVisit,
            FundResponse.CreateData response,
            long? newClientVisitId,
            ReferenceItem status = null,
            DateTime? statusDate = null,
            string message = "Загружена сверка")
        {
            return new FundResponse.UploadReportData()
            {
                Recid = response.Recid,
                ClientVisitId = newClientVisitId ?? clientVisit.Id,
                Birthday = clientVisit.NewClientInfo.Birthday,
                ClientId = clientVisit.ClientId,
                VisitGroupId = clientVisit.VisitGroupId,
                DeliveryCenter = clientVisit.DeliveryCenter,
                Fullname = clientVisit.NewClientInfo.Fullname,
                PolicyNumber = clientVisit.NewPolicy.Number,
                PolicySeries = clientVisit.NewPolicy.Series,
                PolicyParty = clientVisit.PolicyPartyNumber,
                Sex = clientVisit.NewClientInfo.Sex,
                Status = status ?? clientVisit.Status,
                StatusDate = statusDate ?? clientVisit.StatusDate,
                TemporaryPolicyDate = clientVisit.TemporaryPolicyDate,
                TemporaryPolicyNumber = clientVisit.TemporaryPolicyNumber,
                UnifiedPolicyNumber = clientVisit.NewPolicy.UnifiedPolicyNumber,
                ResponseTypeName = response.GetResponseTypeName(),
                UploadResult = message
            };
        }

        public List<FundResponse.UploadReportData> UploadSubmitFundResponse(User user, string zipPath)
        {
            List<FundResponse.UploadReportData> report = new List<FundResponse.UploadReportData>();
            string zipDirectoryName = Path.Combine(ConfiguraionProvider.FileStorageFolder, Path.GetFileNameWithoutExtension(zipPath));
            ZipHelper.UnZipFiles(zipPath, zipDirectoryName);
            var responsesToCreate = new List<FundResponse.CreateData>();
            FundResponseCreateDataBuilder builder = new FundResponseCreateDataBuilder();
            foreach (var file in Directory.GetFiles(zipDirectoryName))
            {
                System.IO.FileInfo fInfo = new System.IO.FileInfo(file);
                DataTable dataTable = DBFProcessor.GetDataTable(fInfo.FullName, string.Format("select * from \"{0}\";", fInfo.FullName));
                if (fInfo.Name.StartsWith("errp2", StringComparison.InvariantCultureIgnoreCase))
                {
                    dataTable.TableName = Path.GetFileNameWithoutExtension(fInfo.Name);
                    IEnumerable<FundResponse.CreateData> list = builder.BuildList(FundErrorResponse.Name, dataTable);
                    responsesToCreate.AddRange(list);
                }
                if (fInfo.Name.StartsWith("zP2", StringComparison.InvariantCultureIgnoreCase))
                {
                    IEnumerable<FundResponse.CreateData> list = builder.BuildList(GoznakResponse.Name, dataTable);
                    responsesToCreate.AddRange(list);
                }
            }
            DateTime date = DateTime.Now;
            var responsesByclientVisitId = new Dictionary<long, List<FundResponse.CreateData>>();
            List<FundRequestRecid> recids =
                FundProcessingDao.Instance.ClientVisitId_GetByFundRequesetRecid(responsesToCreate.Where(item => item.Recid.HasValue).Select(item => item.Recid.Value));
            Dictionary<long, FundRequestRecid> recidsByRecid = recids.ToDictionary(item => item.Recid);
            foreach (FundResponse.CreateData response in responsesToCreate)
            {
                FundRequestRecid recid;
                if (response.Recid.HasValue && recidsByRecid.TryGetValue(response.Recid.Value, out recid))
                {
                    response.ClientVisitId = recid.ClientVisitId;
                }
                if (response.ClientVisitId != 0)
                {
                    if (!responsesByclientVisitId.ContainsKey(response.ClientVisitId))
                    {
                        responsesByclientVisitId.Add(response.ClientVisitId, new List<FundResponse.CreateData>());
                    }
                    responsesByclientVisitId[response.ClientVisitId].Add(response);
                }
                else
                {
                    report.Add(new FundResponse.UploadReportData()
                    {
                        Recid = response.Recid,
                        Fullname = response.GetFullname(),
                        ResponseTypeName = response.GetResponseTypeName(),
                        UploadResult = "Не найдена соответствующая заявка"
                    });
                }
            }
            foreach (var pack in responsesByclientVisitId)
            {
                long clientVisitId = pack.Key;
                ClientVisit clientVisit = clientBusinessLogic.ClientVisit_Get(clientVisitId);
                if (clientVisit == null)
                {
                    foreach (var response in pack.Value)
                    {
                        report.Add(new FundResponse.UploadReportData()
                        {
                            Recid = response.Recid,
                            ClientVisitId = clientVisitId,
                            ResponseTypeName = response.GetResponseTypeName(),
                            UploadResult = "Не найдена соответствующая заявка"
                        });
                    }
                    continue;
                }
                ClientVisit lastClientVisit = clientBusinessLogic.ClientVisit_GetLastClientVisitInGroup(clientVisit.VisitGroupId);
                foreach (var response in pack.Value)
                {
                    FundProcessingDao.Instance.FundResponse_Create(response, date);
                }

                foreach (FundErrorResponse.CreateData response in pack.Value.OfType<FundErrorResponse.CreateData>().Cast<FundErrorResponse.CreateData>())
                {
                    if (string.IsNullOrEmpty(response.ErrorCode) && string.IsNullOrEmpty(response.ErrorText))
                    {
                        if (lastClientVisit.Status.Id == ClientVisitStatuses.AnswerPending.Id)
                        {
                            if (lastClientVisit.Scenario.Id == ClientVisitScenaries.PolicyExtradition.Id)
                            {
                                ClientDao.Instance.ClientVisit_SetStatus(user.Id, lastClientVisit.Id, ClientVisitStatuses.PolicyIssuedAndSentToTheFond.Id, true, date);
                                report.Add(AddReportItem(clientVisit, response, lastClientVisit.Id, ClientVisitStatuses.PolicyIssuedAndSentToTheFond, date, "Загружен ответ фонда"));
                            }
							else
                            {
                                ClientDao.Instance.ClientVisit_SetStatus(user.Id, lastClientVisit.Id, ClientVisitStatuses.Processed.Id, true, date);
                                report.Add(AddReportItem(clientVisit, response, lastClientVisit.Id, ClientVisitStatuses.Processed, date, "Загружен ответ фонда"));
                            }
                        }
                        else if (lastClientVisit.Status.Id == ClientVisitStatuses.FundError.Id)
                        {
                            ClientVisit.SaveData newClientVisitData = new ClientVisit.SaveData(clientVisit);
                            newClientVisitData.Status = ClientVisitStatuses.Processed.Id;
                            newClientVisitData.StatusDate = date;
                            newClientVisitData.IsActual = true;
                            var saveResult = clientBusinessLogic.ClientVisit_Save(user, newClientVisitData, date);
                            report.Add(AddReportItem(clientVisit, response, saveResult.ClientVisitID, ClientVisitStatuses.Processed, date, "Загружен ответ фонда"));
                        }
                    }
                    else
                    {
                        if (lastClientVisit.Status.Id == ClientVisitStatuses.Reconciliation.Id ||
                            lastClientVisit.Status.Id == ClientVisitStatuses.AnswerPending.Id)
                        {
                            ClientDao.Instance.ClientVisit_SetStatus(user.Id, lastClientVisit.Id, ClientVisitStatuses.FundError.Id, true, date);
                            ClientVisit.SaveData newClientVisitData = new ClientVisit.SaveData(clientVisit);
                            newClientVisitData.Status = ClientVisitStatuses.FundError.Id;
                            newClientVisitData.StatusDate = date;
                            newClientVisitData.IsActual = true;
                            var saveResult = clientBusinessLogic.ClientVisit_Save(user, newClientVisitData, date);
                            report.Add(AddReportItem(clientVisit, response, saveResult.ClientVisitID, ClientVisitStatuses.FundError, date, "Загружен ответ фонда"));
                        }
                        else
                        {
                            report.Add(AddReportItem(clientVisit, response, lastClientVisit.Id, lastClientVisit.Status, date, "Статус не изменен"));
                        }
                    }
                }

                foreach (GoznakResponse.CreateData response in pack.Value.OfType<GoznakResponse.CreateData>().Cast<GoznakResponse.CreateData>())
                {
                    if (lastClientVisit.Status.Id == ClientVisitStatuses.AnswerPending.Id
                        || lastClientVisit.Status.Id == ClientVisitStatuses.Processed.Id
                        || lastClientVisit.Status.Id == ClientVisitStatuses.FundError.Id)
                    {
                        ClientDao.Instance.ClientVisit_SetStatus(user.Id, lastClientVisit.Id, ClientVisitStatuses.SentToGoznak.Id, true, date);
                        report.Add(AddReportItem(clientVisit, response, lastClientVisit.Id, ClientVisitStatuses.SentToGoznak, date));
                    }
                    else
                    {
                        report.Add(AddReportItem(clientVisit, response, lastClientVisit.Id, lastClientVisit.Status, date, "Статус не изменен"));
                    }
                }
            }
            return report;
        }

        public void ApplyResponse(User user, FundResponseCopyFields fundResponseCopyFields)
        {
            ClientVisit lastClientVisit = clientBusinessLogic.ClientVisit_GetLastClientVisitInGroup(fundResponseCopyFields.ClientVisitGroupId);
            if (lastClientVisit.Status.Id == ClientVisitStatuses.FundError.Id
                || lastClientVisit.Status.Id == ClientVisitStatuses.Processed.Id
                || lastClientVisit.Status.Id == ClientVisitStatuses.AnswerPending.Id)
            {
                clientBusinessLogic.ClientVisit_SetStatus(user, lastClientVisit.Id, ClientVisitStatuses.Reconciliation.Id, true);
                lastClientVisit = clientBusinessLogic.ClientVisit_GetLastClientVisitInGroup(fundResponseCopyFields.ClientVisitGroupId);
            }
            else if (lastClientVisit.Status.Id != ClientVisitStatuses.Reconciliation.Id)
            {
                ClientVisit.SaveData newClientVisitData = new ClientVisit.SaveData(lastClientVisit);
                newClientVisitData.Status = ClientVisitStatuses.Reconciliation.Id;
                newClientVisitData.IsActual = true;
                var saveResult = clientBusinessLogic.ClientVisit_Save(user, newClientVisitData, DateTime.Now);
                lastClientVisit = clientBusinessLogic.ClientVisit_GetLastClientVisitInGroup(fundResponseCopyFields.ClientVisitGroupId);
            }

            if (lastClientVisit.Status.Id == ClientVisitStatuses.Reconciliation.Id)
            {
                FundResponse response = FundProcessingDao.Instance.FundResponse_Get(fundResponseCopyFields.ResponseId);
                ClientVisit.SaveData data = ClientVisit.SaveData.BuildSaveData(lastClientVisit);
                response.Apply(data, fundResponseCopyFields.NewFields, fundResponseCopyFields.OldFields);
                clientBusinessLogic.ClientVisit_Save(user, data);
            }
        }

        public List<FundResponsePackage> FundResponsePackage_GetListByClientVisitId(long clientVisitId)
        {
            List<FundResponse> responses = FundProcessingDao.Instance.FundResponse_GetList(clientVisitId);
            List<FundResponsePackage> packages = new List<FundResponsePackage>();
            Dictionary<DateTime, FundResponsePackage> packagesByDate = new Dictionary<DateTime, FundResponsePackage>();
            foreach (var response in responses)
            {
                if (!packagesByDate.ContainsKey(response.CreateDate))
                {
                    packagesByDate.Add(response.CreateDate, new FundResponsePackage() { ImportDate = response.CreateDate });
                }
                packagesByDate[response.CreateDate].Responses.Add(response);
            }
            return packagesByDate.Values.OrderByDescending(item => item.ImportDate).ToList();
        }

        public byte[] GetFullRequestPackage(DataPage<ClientVisitInfo> clientVisitInfo)
        {
            IEnumerable<ClientVisit> clientVisits = clientVisitInfo.Select(item => ClientDao.Instance.ClientVisit_Get(item.Id));
            FullRequestPackage package = new FullRequestPackage(clientVisits.ToList());
            return package.GetResult();
        }

        public byte[] FundSubmitRequest_Get(DataPage<ClientVisitInfo> clientVisitInfo, string suffix)
        {
            IEnumerable<ClientVisit> clientVisits = clientVisitInfo.Select(item => ClientDao.Instance.ClientVisit_Get(item.Id));
            string mainFolderPath = Path.Combine(ConfiguraionProvider.FileStorageFolder, Guid.NewGuid().ToString());
            FundSubmitRequest package = new FundSubmitRequest(
                clientVisits.ToList(),
                mainFolderPath, suffix);
            package.Process();
            return package.ZipFiles();
        }

        public string FundSubmitRequest_GetName(DateTime date)
        {
            int monthPackageNumber = FundProcessingDao.Instance.MonthPackageNumber_Get(date);
            string filename = string.Format("P2{0}{1}{2}",
                date.Year.ToString()[3],
                string.Format("{0:00}", date.Month),
                string.Format("{0:00}", monthPackageNumber));
            return filename;
        }

        public List<FundResponse.UploadReportData> Upload_SecondStepReconciliationPack(User user, string zipPath)
        {
            List<FundResponse.UploadReportData> report = new List<FundResponse.UploadReportData>();
            string zipDirectoryName = Path.Combine(ConfiguraionProvider.FileStorageFolder, Path.GetFileNameWithoutExtension(zipPath));
            ZipHelper.UnZipFiles(zipPath, zipDirectoryName);
            FundResponseCreateDataBuilder builder = new FundResponseCreateDataBuilder();
            var responsesToCreate = GetResponsesFromArchive(builder, zipDirectoryName);
            var responsesByclientVisitId = GroupByClientVisitId(report, responsesToCreate);
            DateTime date = DateTime.Now;
            foreach (var pack in responsesByclientVisitId)
            {
                long clientVisitId = pack.Key;
                ClientVisit clientVisit = ClientDao.Instance.ClientVisit_Get(clientVisitId);
                if (clientVisit == null)
                {
                    foreach (var response in pack.Value)
                    {
                        report.Add(new FundResponse.UploadReportData()
                        {
                            Recid = response.Recid,
                            ClientVisitId = clientVisitId,
                            ResponseTypeName = response.GetResponseTypeName(),
                            UploadResult = "Не найдена соответствующая заявка"
                        });
                    }
                    continue;
                }
                ClientVisit lastClientVisitInGroup = ClientDao.Instance.ClientVisit_GetLastClientVisitInGroup(clientVisit.VisitGroupId);
                foreach (var response in pack.Value)
                {
                    FundProcessingDao.Instance.FundResponse_Create(response, date);
                    report.Add(AddReportItem(lastClientVisitInGroup, response, lastClientVisitInGroup.Id));
                }
                ClientVisit.SaveData data = ClientVisit.SaveData.BuildSaveData(lastClientVisitInGroup);
                SecondStepReconciliationProcessor processor = new SecondStepReconciliationProcessor(data, pack.Value.OfType<ReconciliationFundResponse.CreateData>().ToList());
                processor.Process();
                if (processor.IsChanged)
                {
                    clientBusinessLogic.ClientVisit_Save(user, data);
                }
            }
            return report;
        }
        /// <summary>
        /// При установке галочки "Готов к выгрузке"
        /// </summary>
        /// <param name="user"></param>
        /// <param name="id"></param>
        /// <param name="isReady"></param>
        /// <param name="message"></param>
        public void ClientVisit_SetReadyToFundSubmitRequest(User user, long id, bool isReady, string message)
        {
            if (isReady)
            {
                ClientVisit clientVisit = clientBusinessLogic.ClientVisit_Get(id);
                if (clientVisit.IsReadyToFundSubmitRequest)
                {
                    return;
                }
                ClientVisit.SaveData data = ClientVisit.SaveData.BuildSaveData(clientVisit);
                IEnumerable<ReconciliationFundResponse> responses = FundProcessingDao.Instance.FundResponse_GetList(id).OrderByDescending(item => item.CreateDate).OfType<ReconciliationFundResponse>();
                ClientVisit firstClientVisit = clientBusinessLogic.ClientVisit_GetFirstClientVisitInGroup(clientVisit.VisitGroupId);

                //ClientVisitOldDataBuilder oldDataBuilder = new ClientVisitOldDataBuilder(data, responses.OfType<IReconciliationFundResponse>().ToList());
                ClientVisitNewDataBuilder newDataBuilder = new ClientVisitNewDataBuilder(data, firstClientVisit, responses);
                //data = oldDataBuilder.Process();
                data = newDataBuilder.Process();

                //если установлена дата начала полиса и НЕ установлена дата окончания
                if(data.OldPolicy.StartDate.HasValue && !data.OldPolicy.EndDate.HasValue)
                {
                    data.OldPolicy.EndDate = new DateTime(2099, 12, 31);
                }
                if (data.NewPolicy.StartDate.HasValue && !data.NewPolicy.EndDate.HasValue)
                {
                    data.NewPolicy.EndDate = new DateTime(2099, 12, 31);
                }

                clientBusinessLogic.ClientVisit_Save(user, data);
            }
            FundProcessingDao.Instance.ClientVisit_SetReadyToFundSubmitRequest(id, isReady, message, DateTime.Now);
        }


        public void ClientVisits_SetDifficultCase(long id, bool isDifficult)
        {
            FundProcessingDao.Instance.ClientVisits_SetDifficultCase(id, isDifficult, DateTime.Now);
        }

        public void FundFileHistory_Save(List<FundFileHistory> list)
        {
            FundProcessingDao.Instance.FundFileHistory_Save(list);
        }

        public List<FundFileHistory> FundFileHistory_Get(long VisitGroupID)
        {
            return FundProcessingDao.Instance.FundFileHistory_Get(VisitGroupID);
        }
    }
}
