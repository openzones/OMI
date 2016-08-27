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
    public class DbfProcessingBusinessLogic : IDbfProcessingBusinessLogic
    {
        ClientBusinessLogic clientBusinessLogic = new ClientBusinessLogic();
        BSOBusinessLogic bsoBusinessLogic = new BSOBusinessLogic();
        public List<ClientVisit.UpdateResultData> UploadMFCClientVisits(User user, string zipFilepath)
        {
            List<ClientVisit.UpdateResultData> report = new List<ClientVisit.UpdateResultData>();
            string zipDirectoryName = Path.Combine(ConfiguraionProvider.FileStorageFolder, Path.GetFileNameWithoutExtension(zipFilepath));
            try
            {
                ZipHelper.UnZipFiles(zipFilepath, zipDirectoryName);
                string[] filenames = Directory.GetFiles(zipDirectoryName);
                string persFile = filenames.FirstOrDefault(item => Path.GetFileName(item).StartsWith("pers"));
                string addrFile = filenames.FirstOrDefault(item => Path.GetFileName(item).StartsWith("addr"));
                string docuFile = filenames.FirstOrDefault(item => Path.GetFileName(item).StartsWith("docu"));

                Dictionary<int, ClientVisit.SaveData> clientVisitByRecid = new Dictionary<int, ClientVisit.SaveData>();
                List<string> errors = new List<string>();
                ClientVisitReportDao.Instance.BuildClientVisitDataFromMFC(clientVisitByRecid, errors, persFile);
                ClientVisitReportDao.Instance.BuildDocumentsFromMFC(clientVisitByRecid, errors, docuFile);
                ClientVisitReportDao.Instance.BuildAddressedFromMFC(clientVisitByRecid, errors, addrFile);

                foreach (var pair in clientVisitByRecid)
                {
                    var data = pair.Value;
                    string bsoMessage = null;
                    BSO bso = new BSO();
                    bso = bsoBusinessLogic.BSO_GetByNumber(data.TemporaryPolicyNumber);
                    if (!CheckBSO(data, bso, out bsoMessage))
                    {
                        report.Add(new ClientVisit.UpdateResultData()
                        {
                            Birthday = data.NewClientInfo.Birthday,
                            Firstname = data.NewClientInfo.Firstname,
                            Secondname = data.NewClientInfo.Secondname,
                            Lastname = data.NewClientInfo.Lastname,
                            UnifiedPolicyNumber = data.NewPolicy.UnifiedPolicyNumber,
                            Message = bsoMessage,
                            RECID = pair.Key,
                            IsSuccess = false
                        });
                        continue;
                    }
                    var clients = clientBusinessLogic.Client_Find(new ClientSearchCriteria()
                    {
                        Birthday = data.NewClientInfo.Birthday,
                        Firstname = data.NewClientInfo.Firstname,
                        Secondname = data.NewClientInfo.Secondname,
                        Lastname = data.NewClientInfo.Lastname
                    },
                    new List<SortCriteria<ClientSortField>>(),
                    new PageRequest()).Data;

                    if (clients.Count != 0)
                    {
                        Client client = clientBusinessLogic.Client_Get(user, clients.FirstOrDefault().Id);
                        var existedClientVisit = client.Visits.FirstOrDefault(item => item.TemporaryPolicyDate == data.TemporaryPolicyDate
                            && item.TemporaryPolicyNumber == data.TemporaryPolicyNumber);
                        if (existedClientVisit != null)
                        {
                            report.Add(new ClientVisit.UpdateResultData()
                            {
                                ClientId = client.Id,
                                Birthday = existedClientVisit.Birthday,
                                IsSuccess = false,
                                ClientVisitGroupId = existedClientVisit.VisitGroupId,
                                Firstname = existedClientVisit.Firstname,
                                Id = existedClientVisit.Id,
                                Lastname = existedClientVisit.Lastname,
                                Secondname = existedClientVisit.Secondname,
                                Status = existedClientVisit.Status,
                                UnifiedPolicyNumber = existedClientVisit.UnifiedPolicyNumber,
                                Message = "Запись уже существует"
                            });
                            continue;
                        }
                        data.ClientId = client.Id;
                    }

                    data.RegistratorId = user.Id;
                    data.Status = ClientVisitStatuses.AnswerPending.Id;
                    var saveResult = ClientDao.Instance.ClientVisit_Save(data, user.Id, data.StatusDate);
                    report.Add(new ClientVisit.UpdateResultData()
                    {
                        ClientId = saveResult.ClientID,
                        Birthday = data.NewClientInfo.Birthday,
                        IsSuccess = true,
                        ClientVisitGroupId = saveResult.VisitGroupId,
                        Firstname = data.NewClientInfo.Firstname,
                        Id = saveResult.VisitGroupId,
                        Lastname = data.NewClientInfo.Lastname,
                        Secondname = data.NewClientInfo.Secondname,
                        Status = ClientVisitStatuses.AnswerPending,
                        UnifiedPolicyNumber = data.NewPolicy.UnifiedPolicyNumber,
                        Message = "Успешно"
                    });
                    bso.Status.Id = (long)ListBSOStatusID.OnClient;
                    bso.UserId = user.Id;
                    bso.DeliveryPointId = data.DeliveryPointId == null ? bso.DeliveryPointId : data.DeliveryPointId; //Если точка пустая - оставляем Точку, кот. была в БСО
                    bso.StatusDate = data.TemporaryPolicyDate == null ? data.StatusDate : data.TemporaryPolicyDate;
                    bso.Comment = string.Format("Изменение статуса при загрузке DBF (загрузка из МФЦ)");
                    bso.VisitGroupId = saveResult.VisitGroupId;
                    bsoBusinessLogic.BSO_Save(new BSO.SaveData(bso));
                }
                foreach (var error in errors)
                {
                    report.Add(new ClientVisit.UpdateResultData() { Message = error });
                }
                return report;
            }
            finally
            {
                if (Directory.Exists(zipDirectoryName))
                {
                    Directory.Delete(zipDirectoryName, true);
                }
            }
        }

        public List<ClientVisit.UpdateResultData> UploadMFCClientVisitsOldData(User user, string zipFilepath)
        {
            List<ClientVisit.UpdateResultData> report = new List<ClientVisit.UpdateResultData>();
            string zipDirectoryName = Path.Combine(ConfiguraionProvider.FileStorageFolder, Path.GetFileNameWithoutExtension(zipFilepath));
            try
            {
                ZipHelper.UnZipFiles(zipFilepath, zipDirectoryName);
                string[] filenames = Directory.GetFiles(zipDirectoryName);

                List<string> fioFiles = filenames.Where(item => Path.GetFileName(item).StartsWith("fio")).ToList();
                List<string> adrFiles = filenames.Where(item => Path.GetFileName(item).StartsWith("adr")).ToList();

                //для документов: dul используется в 2015, doc в 2014
                List<string> dulFiles = filenames.Where(item => Path.GetFileName(item).StartsWith("dul")).ToList();
                if(dulFiles == null || dulFiles.Count == 0)
                {
                    dulFiles = filenames.Where(item => Path.GetFileName(item).StartsWith("doc")).ToList();
                }

                Dictionary<long, ClientVisit.SaveData> clientVisitByRecid = new Dictionary<long, ClientVisit.SaveData>();
                List<string> errors = new List<string>();
                
                foreach(var file in fioFiles)
                {
                    ClientVisitReportDao.Instance.BuildClientVisitFromMFC_OldData(clientVisitByRecid, errors, file);
                }

                foreach (var file in adrFiles)
                {
                    ClientVisitReportDao.Instance.BuildAddressedFromMFC_OldData(clientVisitByRecid, errors, file);
                }

                foreach (var file in dulFiles)
                {
                    ClientVisitReportDao.Instance.BuildDocumentsFromMFC_OldData(clientVisitByRecid, errors, file);
                }
                
                foreach (var pair in clientVisitByRecid)
                {
                    var data = pair.Value;
                    BSO bso = bsoBusinessLogic.BSO_GetByNumber(data.TemporaryPolicyNumber);
                    if (bso == null)
                    {
                        bso = new BSO();
                        bso.TemporaryPolicyNumber = data.TemporaryPolicyNumber;
                    }

                    var clients = clientBusinessLogic.Client_Find(new ClientSearchCriteria()
                    {
                        Birthday = data.NewClientInfo.Birthday,
                        Firstname = data.NewClientInfo.Firstname,
                        Secondname = data.NewClientInfo.Secondname,
                        Lastname = data.NewClientInfo.Lastname
                    },
                    new List<SortCriteria<ClientSortField>>(),
                    new PageRequest()).Data;

                    if (clients.Count != 0)
                    {
                        Client client = clientBusinessLogic.Client_Get(user, clients.FirstOrDefault().Id);
                        var existedClientVisit = client.Visits.FirstOrDefault(item => item.TemporaryPolicyDate == data.TemporaryPolicyDate
                            && item.TemporaryPolicyNumber == data.TemporaryPolicyNumber);
                        if (existedClientVisit != null)
                        {
                            report.Add(new ClientVisit.UpdateResultData()
                            {
                                ClientId = client.Id,
                                Birthday = existedClientVisit.Birthday,
                                IsSuccess = false,
                                ClientVisitGroupId = existedClientVisit.VisitGroupId,
                                Firstname = existedClientVisit.Firstname,
                                Id = existedClientVisit.Id,
                                Lastname = existedClientVisit.Lastname,
                                Secondname = existedClientVisit.Secondname,
                                Status = existedClientVisit.Status,
                                UnifiedPolicyNumber = existedClientVisit.UnifiedPolicyNumber,
                                Message = "Запись уже существует"
                            });
                            continue;
                        }
                        data.ClientId = client.Id;
                    }

                    data.RegistratorId = user.Id;
                    data.Status = ClientVisitStatuses.AnswerPending.Id;
                    var saveResult = ClientDao.Instance.ClientVisit_Save(data, user.Id, data.StatusDate);
                    report.Add(new ClientVisit.UpdateResultData()
                    {
                        ClientId = saveResult.ClientID,
                        Birthday = data.NewClientInfo.Birthday,
                        IsSuccess = true,
                        ClientVisitGroupId = saveResult.VisitGroupId,
                        Firstname = data.NewClientInfo.Firstname,
                        Id = saveResult.VisitGroupId,
                        Lastname = data.NewClientInfo.Lastname,
                        Secondname = data.NewClientInfo.Secondname,
                        Status = ClientVisitStatuses.AnswerPending,
                        UnifiedPolicyNumber = data.NewPolicy.UnifiedPolicyNumber,
                        Message = "Успешно"
                    });
                    
                    bso.Status.Id = (long)ListBSOStatusID.OnClient;
                    bso.UserId = user.Id;
                    bso.DeliveryPointId = data.DeliveryPointId == null ? bso.DeliveryPointId : data.DeliveryPointId; //Если точка пустая - оставляем Точку, кот. была в БСО
                    bso.StatusDate = data.TemporaryPolicyDate == null ? data.StatusDate : data.TemporaryPolicyDate;
                    bso.Comment = string.Format("Изменение статуса при загрузке DBF (загрузка из МФЦ)");
                    bso.VisitGroupId = saveResult.VisitGroupId;
                    if (!string.IsNullOrEmpty(bso.TemporaryPolicyNumber)) //Если нет номера БСО, то не сохраняем его
                    {
                        var id = bsoBusinessLogic.BSO_Save(new BSO.SaveData(bso));
                    }
                }
                foreach (var error in errors)
                {
                    report.Add(new ClientVisit.UpdateResultData() { Message = error });
                }
                return report;
            }
            finally
            {
                if (Directory.Exists(zipDirectoryName))
                {
                    Directory.Delete(zipDirectoryName, true);
                }
            }
        }

        public List<ClientVisit.UpdateResultData> UploadMFCClientVisitsExtradition(User user, string zipFilepath)
        {
            List<ClientVisit.UpdateResultData> report = new List<ClientVisit.UpdateResultData>();
            string zipDirectoryName = Path.Combine(ConfiguraionProvider.FileStorageFolder, Path.GetFileNameWithoutExtension(zipFilepath));
            ZipHelper.UnZipFiles(zipFilepath, zipDirectoryName);
            string[] filenames = Directory.GetFiles(zipDirectoryName);
            string file = filenames.FirstOrDefault();
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(file);
            DataTable table = DBFProcessor.GetDataTable(file, string.Format("select * from \"{0}\";", fileInfo.FullName));
            foreach (DataRow row in table.Rows)
            {
                string unifiedPolicyNumber = (string)row["ENP"];
                string temporaryPolicyNumber = (string)row["VSN"];
                DateTime issueDate = (DateTime)row["DATE_V"];
                ClientVisitInfo clientVisitInfo = clientBusinessLogic.ClientVisit_Find(
                    new ClientVisitSearchCriteria()
                    {
                        TemporaryPolicyNumber = temporaryPolicyNumber
                    }, new List<SortCriteria<ClientVisitSortField>>(), new PageRequest()).Data.FirstOrDefault();

                if (clientVisitInfo != null)
                {
                    ClientVisit visit = clientBusinessLogic.ClientVisit_GetLastClientVisitInGroup(clientVisitInfo.VisitGroupId);
                    ClientVisit.SaveData data = ClientVisit.SaveData.BuildSaveData(visit);
                    data.ScenarioId = ClientVisitScenaries.PolicyExtradition.Id;
                    data.Status = ClientVisitStatuses.PolicyIssuedAndSentToTheFond.Id;
                    data.StatusDate = issueDate;
                    data.IssueDate = issueDate;
                    data.NewPolicy.UnifiedPolicyNumber = unifiedPolicyNumber;
                    clientBusinessLogic.ClientVisit_Save(user, data, data.IssueDate);
                    report.Add(new ClientVisit.UpdateResultData()
                    {
                        Birthday = visit.NewClientInfo.Birthday,
                        ClientId = visit.ClientId,
                        ClientVisitGroupId = visit.VisitGroupId,
                        Firstname = visit.NewClientInfo.Firstname,
                        Id = visit.Id,
                        IsSuccess = true,
                        Lastname = visit.NewClientInfo.Lastname,
                        Secondname = visit.NewClientInfo.Secondname,
                        Sex = int.Parse(visit.NewClientInfo.Sex),
                        Status = ClientVisitStatuses.PolicyIssuedAndSentToTheFond,
                        UnifiedPolicyNumber = unifiedPolicyNumber,
                        Message = "Успешно"
                    });
                }
                else
                {
                    report.Add(new ClientVisit.UpdateResultData()
                    {
                        IsSuccess = false,
                        UnifiedPolicyNumber = unifiedPolicyNumber,
                        Message = "Не найден"
                    });
                }
            }
            return report;
        }

        private bool CheckBSO(ClientVisit.SaveData data, BSO bso, out string errorMessage)
        {
            if (bso != null)
            {
                if (bso.Status.Id != (long)ListBSOStatusID.OnDelivery)
                {
                    errorMessage = string.Format("БСО {0} не находится в статусе 'На точке'", bso.TemporaryPolicyNumber);
                    return false;
                }
                if (bso.DeliveryCenterId != data.DeliveryCenterId)
                {
                    errorMessage = string.Format("БСО {0} не находится на другой точке {1}", bso.TemporaryPolicyNumber, bso.DeliveryCenterId);
                    return false;
                }
                errorMessage = null;
                return true;
            }
            else
            {
                errorMessage = string.Format("БСО с номером {0} не найден", data.TemporaryPolicyNumber);
                return false;
            }
        }

    }
}
