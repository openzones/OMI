using OMInsurance.BusinessLogic;
using OMInsurance.Configuration;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using OMInsurance.Entities.Searching;
using OMInsurance.Entities.Sorting;
using OMInsurance.Interfaces;
using OMInsurance.PrintedForms;
using OMInsurance.WebApps.Models;
using OMInsurance.WebApps.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using OMInsurance.LoadRegionData;

namespace OMInsurance.WebApps.Controllers
{
    /// <summary>
    /// Controller provides methods to work with dbf-packages
    /// </summary>
    [AuthorizeUser]
    public class DbfPackageController : OMInsuranceController
    {
        IClientBusinessLogic clientBusinessLogic;
        IBSOBusinessLogic bsoBusinessLogic;
        IFundRequestBusinessLogic fundRequestBusinessLogic;
        INomernikBusinessLogic nomernikBusinessLogic;
        IDbfProcessingBusinessLogic dbfProcessingBusinessLogic;

        public DbfPackageController()
            : base()
        {
            clientBusinessLogic = new ClientBusinessLogic();
            bsoBusinessLogic = new BSOBusinessLogic();
            fundRequestBusinessLogic = new FundRequestBusinessLogic();
            nomernikBusinessLogic = new NomernikBusinessLogic();
            dbfProcessingBusinessLogic = new DbfProcessingBusinessLogic();
        }

        public ActionResult Index()
        {
            ClientVisitListModel model = new ClientVisitListModel(CurrentUser);
            return Index(model);
        }

        [HttpPost]
        public ActionResult Index(ClientVisitListModel clientVisitModel)
        {
            ClientVisitSearchCriteria criteria = clientVisitModel.SearchCriteriaModel.GetClientSearchCriteria();
            DataPage<ClientVisitInfo> clients = clientBusinessLogic.ClientVisit_Find(
                    criteria,
                    new List<SortCriteria<ClientVisitSortField>>(),
                    new PageRequest() { PageNumber = clientVisitModel.PageNumber, PageSize = clientVisitModel.PageSize });

            ClientVisitListModel model = new ClientVisitListModel(CurrentUser)
            {
                Items = clients.Data
                    .Select(item => new ClientVisitInfoModel(item)),
                SearchCriteriaModel = new ClientVisitSearchCriteriaModel(
                    criteria),
                PageNumber = clientVisitModel.PageNumber,
                PageSize = clientVisitModel.PageSize,
                TotalCount = clients.TotalCount
            };
            return View(model);
        }

        public ActionResult InputDBF()
        {
            return View();
        }

        public ActionResult GetReport()
        {
            ClientVisitListModel m = new ClientVisitListModel(CurrentUser) { };
            return GetReport(m);
        }

        [HttpPost]
        public ActionResult GetReport(ClientVisitListModel m)
        {
            ClientVisitSearchCriteria criteria = m != null && m.SearchCriteriaModel != null
                ? m.SearchCriteriaModel.GetClientSearchCriteria()
                : new ClientVisitSearchCriteria() { };
            ReferenceItem deliveryCenter = referenceBusinessLogic.GetReferencesList(Constants.DeliveryCenterRef).FirstOrDefault(item => item.Id == criteria.DeliveryCenterIds.FirstOrDefault());
            string filename = string.Format("e{0}_{1}_{2}.zip", deliveryCenter != null ? deliveryCenter.Code : Guid.NewGuid().ToString(),
                criteria.UpdateDateFrom.HasValue ?
                criteria.UpdateDateFrom.Value.ToString("ddMMyy") :
                string.Empty,
                criteria.UpdateDateTo.HasValue ?
                criteria.UpdateDateTo.Value.ToString("ddMMyy") :
                string.Empty);
            byte[] fileBytes = clientBusinessLogic.FileReport_Get(
                    criteria,
                    new List<SortCriteria<ClientVisitSortField>>(),
                    new PageRequest() { PageNumber = 1, PageSize = int.MaxValue });

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }

        [HttpPost]
        public ActionResult GetFullRequestPackage(ClientVisitListModel m)
        {
            //fileName - это файл отдаем пользователю и отображаем на View
            //fileUrl -  сохраняем на диске
            string fileName = "Request";
            string fileUrl = Guid.NewGuid().ToString() + ".zip";
            string path = Path.Combine(ConfiguraionProvider.FileStorageFolder, Constants.SverkaRequest, fileUrl);

            ClientVisitSearchCriteria criteria = m != null && m.SearchCriteriaModel != null
                ? m.SearchCriteriaModel.GetClientSearchCriteria()
                : new ClientVisitSearchCriteria();

            DataPage<ClientVisitInfo> clientVisitInfo = clientBusinessLogic.ClientVisit_Find(criteria, new List<SortCriteria<ClientVisitSortField>>(), new PageRequest() { PageNumber = 1, PageSize = int.MaxValue });
            byte[] fileBytes = fundRequestBusinessLogic.GetFullRequestPackage(clientVisitInfo);
            
            List<FundFileHistory> listHistory = new List<FundFileHistory>();
            //fileName = fileName + "_count(" + clientVisitInfo.Count + ")_" + DateTime.Now.ToShortDateString() + ".zip";
            fileName = "Request" + ".zip"; 
            FundFileHistory template = new FundFileHistory() {
                StatusID = FundFileHistoryStatus.SverkaRequest.Id,
                Date = DateTime.Now,
                UserID = CurrentUser.Id,
                FileName = fileName,
                FileUrl = Path.Combine(Constants.SverkaRequest, fileUrl)
            };
            listHistory = clientVisitInfo.Data.Where(a=>(a.ClientId != 0 && a.VisitGroupId != 0 && a.Id != 0))
                                              .Select(item => new FundFileHistory(item, template)).ToList();
            if(listHistory.Count > 0)
            {
                fundRequestBusinessLogic.FundFileHistory_Save(listHistory);
                System.IO.File.WriteAllBytes(path, fileBytes);
            }

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpPost]
        public ActionResult FundSubmitRequest_Get(ClientVisitListModel m)
        {
            string fileName = string.Empty;
            ClientVisitSearchCriteria criteria = m != null && m.SearchCriteriaModel != null
                ? m.SearchCriteriaModel.GetClientSearchCriteria()
                : new ClientVisitSearchCriteria();
            if (!m.SearchCriteriaModel.UpdateDateFrom.HasValue)
            {
                throw new InvalidDataException("'Дата периода с' не может быть пустой");
            }
            DataPage<ClientVisitInfo> clientVisitInfo = clientBusinessLogic.ClientVisit_Find(criteria, new List<SortCriteria<ClientVisitSortField>>(), new PageRequest() { PageNumber = 1, PageSize = int.MaxValue });
            fileName = fundRequestBusinessLogic.FundSubmitRequest_GetName(m.SearchCriteriaModel.UpdateDateFrom.Value);
            byte[] fileBytes = fundRequestBusinessLogic.FundSubmitRequest_Get(clientVisitInfo, fileName);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, string.Format("g{0}.zip", fileName));
        }

        public ActionResult UpdateFundDbf()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateFundDbf(FormCollection form)
        {
            string filename = Guid.NewGuid().ToString() + ".dbf";
            string path = Path.Combine(ConfiguraionProvider.FileStorageFolder, filename);
            if (Request.Files == null || Request.Files.Count < 1)
            {
                return PartialView("_ErrorMessage", "Выберите dbf-файл для загрузки");
            }
            HttpPostedFileBase upload = Request.Files[0];
            upload.SaveAs(path);
            List<ClientVisit.UpdateResultData> result = clientBusinessLogic.ClientVisit_UpdateFundDbf(CurrentUser, path);
            IEnumerable<ClientVisitUpdateResultModel> report = result.Select(item => new ClientVisitUpdateResultModel(item));

            List<FundFileHistory> listHistory = new List<FundFileHistory>();
            FundFileHistory template = new FundFileHistory()
            {
                StatusID = FundFileHistoryStatus.FundResponse.Id,
                Date = DateTime.Now,
                UserID = CurrentUser.Id,
                FileName = Request.Files[0].FileName, //оригинальное название файла
                FileUrl = Path.Combine(Constants.FundResponse, filename) //имя, как мы его сохраняем
            };
            listHistory = result.Where(a=>(a.ClientId != 0 && a.ClientVisitGroupId != 0 && a.Id != 0))
                                .Select(item => new FundFileHistory(item, template)).ToList();
            if(listHistory.Count > 0)
            {
                fundRequestBusinessLogic.FundFileHistory_Save(listHistory);
                upload.SaveAs(Path.Combine(ConfiguraionProvider.FileStorageFolder, Constants.FundResponse, filename));
            }

            return PartialView("_UpdateFundDbfGrid", report);
        }

        public ActionResult UploadRegionExcel()
        {
            UploadRegionExcelModel m = new UploadRegionExcelModel();
            return View(m);
        }

        [HttpPost]
        public ActionResult UploadRegionExcel(FormCollection form, UploadRegionExcelModel model)
        {
            model.Messages.Clear();
            if (Request.Files == null || Request.Files.Count < 1)
            {
                return PartialView("_ErrorMessage", "Выберите файл для загрузки");
            }

            HttpPostedFileBase upload = Request.Files[0];
            model.OriginalFileName = upload.FileName;
            string extention = Path.GetExtension(upload.FileName);
            if (extention != ".xls" && extention != ".xlsx" && extention != ".xlsm" && extention != ".xlsb" && extention != ".xlsx")
            {
                return PartialView("_ErrorMessage", "Выберите Excel файл для загрузки");
            }
            string filename = Guid.NewGuid().ToString() + extention;
            string path = Path.Combine(ConfiguraionProvider.FileStorageFolder, filename);
            upload.SaveAs(path);
            model.FileSizeToString = GetFileSizeToString((float)upload.ContentLength);
            model.FilePath = path;
            LoadRegion.DataPreLoad data = LoadRegion.PreLoadExcel(model.FilePath);
            model.CountRow = data.CountRow;
            model.CountField = data.CountField;
            if (data.AutoRegionId != null)
            {
                model.AutoRegion = model.Regions.Where(a => a.Value == data.AutoRegionId.ToString()).Select(b => b.Text).FirstOrDefault();
                model.RegionId = (long)data.AutoRegionId;
            }
            model.StatusLoad = true;
            if (model.CountRow == null || model.CountRow < 1) model.StatusLoad = false;
            if (!model.StatusLoad) System.IO.File.Delete(path);

            return View("UploadRegionExcelGoWork", model);
        }

        public ActionResult GoWorkRegionExcel(UploadRegionExcelModel model)
        {
            List<PolicyFromRegion> listPolicy = new List<PolicyFromRegion>();
            LoadRegion.DataPreLoad data = LoadRegion.PreLoadExcel(model.FilePath);
            if (data.CountField == Constants.DefaultCountFieldInRegionExcel)
            {
                LoadRegion loadExcel = new DefaultRegion(model.FilePath, model.RegionId);
                listPolicy = loadExcel.GetPolicy(data);
            }
            else
            {
                if (model.RegionId == (long)ListRegionId.MoscowObl)
                {
                    LoadRegion loadExcel = new RegionMoscowObl(model.FilePath);
                    listPolicy = loadExcel.GetPolicy(data);
                }
                if (model.RegionId == (long)ListRegionId.Samara)
                {
                    model.Messages.Add(string.Format("Формат таблицы Excel не поддерживается этим регионом!"));
                }
                if (model.RegionId == (long)ListRegionId.Ufa)
                {
                    LoadRegion loadExcel = new RegionUfa(model.FilePath);
                    listPolicy = loadExcel.GetPolicy(data);
                }
            }
            System.IO.File.Delete(model.FilePath);
            clientBusinessLogic.RegionPolicyData_Save(listPolicy);
            if (listPolicy.Count > 0)
            {
                model.StatusLoad = true;
                model.Messages.Add(string.Format("Файл {0} успешно обработан!", model.OriginalFileName));
            }
            return View("UploadRegionExcel", model);
        }

        /// <summary>
        /// Загрузка номерника (dbf)
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadNOMP()
        {
            NomernikDisplayModel m = new NomernikDisplayModel();
            Nomernik.History history = nomernikBusinessLogic.NOMPHistory_Get();
            if (history != null)
            {
                m.CountAllRow = history.CountAll;
                m.CountOurRow = history.CountOur;
                m.FileDate = history.FileDate;
                m.LoadDate = history.LoadDate;
                m.CountChangeRow = history.CountChange;
                m.FIO = history.Lastname + " " + history.Firstname + " " + history.Secondname;
            }
            return View(m);
        }

        [HttpPost]
        public ActionResult UploadNOMP(FormCollection form, NomernikDisplayModel model)
        {
            if (string.IsNullOrEmpty(model.FilePath))
            {
                string filename = Guid.NewGuid().ToString() + ".dbf";
                string path = Path.Combine(ConfiguraionProvider.FileStorageFolder, filename);
                if (Request.Files == null || Request.Files.Count < 1)
                {
                    return PartialView("_ErrorMessage", "Выберите dbf-файл для загрузки");
                }
                HttpPostedFileBase upload = Request.Files[0];
                upload.SaveAs(path);
                model.FileSizeToString = GetFileSizeToString((float)upload.ContentLength);
                model.FilePath = path;
                model.StatusLoad = true;
                model.CountAllRow = nomernikBusinessLogic.GetAllRowCount(path);
                if (model.CountAllRow == null || model.CountAllRow < 1) model.StatusLoad = false;
                //наша компания имеет код P2
                string code = "'P2'";
                model.CountOurRow = nomernikBusinessLogic.GetOurRowCount(path, code);
                if (model.CountOurRow == null || model.CountOurRow < 1) model.StatusLoad = false;
                if (!model.StatusLoad) System.IO.File.Delete(path);
            }
            return PartialView("UploadNOMPGoWork", model);
        }

        public ActionResult GoWorkNOMP(NomernikDisplayModel model)
        {
            Nomernik.History history = new Nomernik.History();
            history.LoadDate = DateTime.Now;
            history.FileDate = new DateTime(model.Year, model.MonthID, 1);
            history.UserID = CurrentUser.Id;
            history.CountAll = model.CountAllRow;
            history.CountOur = model.CountOurRow;
            try
            {
                List<NOMP> report = nomernikBusinessLogic.GetDataFromNOMPdbf(model.FilePath, history);
                if (report.Count > 0) model.Messages.Add("Данные были успешно обработаны и записаны в БД!");

                try
                {
                    //На всякий случай пишем дополнительный отчет в *.csv
                    string fileName = string.Format("C:\\Temp\\NOMP_{0}.csv", history.LoadDate.ToShortDateString());
                    WriteReportNOMPcsv(report, history, fileName);
                    model.Messages.Add("Количество записей в отчете: " + history.CountOur);
                    model.Messages.Add("Упрощенный отчет по пути: " + fileName);
                }
                catch
                {
                    model.Messages.Add("Упрощенный отчет создан не был.");
                }

                //Отчет в Excel
                NOMPReport printedForm = new NOMPReport(report, history);
                byte[] bytesExcel = printedForm.GetExcel();
                return File(bytesExcel, System.Net.Mime.MediaTypeNames.Application.Octet, string.Format("Отчет по номернику {0}.xlsx", history.LoadDate));
            }
            catch
            {
                model.StatusLoad = false;
                return View("UploadNOMP", model);
            }
        }

        /// <summary>
        /// Загрузка СТОП-листа
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadSTOP()
        {
            NomernikDisplayModel m = new NomernikDisplayModel();
            Nomernik.History history = nomernikBusinessLogic.STOPHistory_Get();
            if (history != null)
            {
                m.CountAllRow = history.CountAll;
                m.CountOurRow = history.CountOur;
                m.FileDate = history.FileDate;
                m.LoadDate = history.LoadDate;
                m.CountChangeRow = history.CountChange;
                m.FIO = history.Lastname + " " + history.Firstname + " " + history.Secondname;
                m.HistoryID = history.Id;
            }
            return View(m);
        }

        [HttpPost]
        public ActionResult UploadSTOP(FormCollection form, NomernikDisplayModel model)
        {
            if (string.IsNullOrEmpty(model.FilePath))
            {
                string filename = Guid.NewGuid().ToString() + ".dbf";
                string path = Path.Combine(ConfiguraionProvider.FileStorageFolder, filename);
                if (Request.Files == null || Request.Files.Count < 1)
                {
                    return PartialView("_ErrorMessage", "Выберите dbf-файл для загрузки");
                }
                HttpPostedFileBase upload = Request.Files[0];
                upload.SaveAs(path);
                model.FileSizeToString = GetFileSizeToString((float)upload.ContentLength);
                model.FilePath = path;
                model.StatusLoad = true;
                model.CountAllRow = nomernikBusinessLogic.GetAllRowCount(path);
                if (model.CountAllRow == null || model.CountAllRow < 1) model.StatusLoad = false;
                //наша компания имеет код 3386
                string code = "3386";
                model.CountOurRow = nomernikBusinessLogic.GetOurRowCount(path, code);
                if (model.CountOurRow == null || model.CountOurRow < 1) model.StatusLoad = false;
                if (!model.StatusLoad) System.IO.File.Delete(path);
            }
            return PartialView("UploadSTOPGoWork", model);
        }

        public ActionResult GoWorkSTOP(NomernikDisplayModel model)
        {
            //Nomernik.History history = clientBusinessLogic.STOPHistory_Get();
            Nomernik.History history = new Nomernik.History();
            history.LoadDate = DateTime.Now;
            history.FileDate = new DateTime(model.Year, model.MonthID, 1);
            history.UserID = CurrentUser.Id;
            history.CountAll = model.CountAllRow;
            history.CountOur = model.CountOurRow;

            try
            {
                List<STOP> report = nomernikBusinessLogic.GetDataFromSTOPdbf(model.FilePath, history);
                if (report.Count > 0) model.Messages.Add("Данные были успешно обработаны и записаны в БД!");

                try
                {
                    //На всякий случай пишем дополнительный отчет в *.csv
                    string fileName = string.Format("C:\\Temp\\STOP_{0}.csv", history.LoadDate.ToShortDateString());
                    WriteReportSTOPcsv(report, history, fileName);
                    model.Messages.Add("Количество записей в отчете: " + history.CountOur);
                    model.Messages.Add("Упрощенный отчет по пути: " + fileName);
                }
                catch
                {
                    model.Messages.Add("Упрощенный отчет создан не был.");
                }

                //Отчет в Excel
                STOPReport printedForm = new STOPReport(report, history);
                byte[] bytesExcel = printedForm.GetExcel();
                return File(bytesExcel, System.Net.Mime.MediaTypeNames.Application.Octet, string.Format("Отчет по стоп-листу {0}.xlsx", history.LoadDate));
            }
            catch
            {
                model.StatusLoad = false;
                return View("UploadSTOP", model);
            }
        }

        public ActionResult SetStatusesFromDbf()
        {
            return View(DateTime.Now);
        }

        [HttpPost]
        public ActionResult SetStatusesFromDbf(FormCollection form)
        {
            string filename = Guid.NewGuid().ToString() + ".zip";
            string path = Path.Combine(ConfiguraionProvider.FileStorageFolder, filename);
            if (Request.Files == null || Request.Files.Count < 1)
            {
                return PartialView("_ErrorMessage", "Выберите dbf-файл для загрузки");
            }
            HttpPostedFileBase upload = Request.Files[0];
            upload.SaveAs(path);
            
            DateTime statusDate = DateTime.Now;
            DateTime.TryParse(form["statusdate"], out statusDate);
            List<ClientVisit.UpdateResultData> result = clientBusinessLogic.ClientVisit_SetStatusesFromDbf(CurrentUser, statusDate, path);
            IEnumerable<ClientVisitUpdateResultModel> report = result.Select(item => new ClientVisitUpdateResultModel(item));

            List<FundFileHistory> listHistory = new List<FundFileHistory>();
            FundFileHistory template = new FundFileHistory()
            {
                StatusID = FundFileHistoryStatus.FundRequest.Id,
                Date = DateTime.Now,
                UserID = CurrentUser.Id,
                FileName = Request.Files[0].FileName, //оригинальное название файла
                FileUrl = Path.Combine(Constants.FundRequest, filename) //имя, как мы его сохраняем
            };
            listHistory = result.Where(a => (a.ClientId != 0 && a.ClientVisitGroupId != 0 && a.Id != 0))
                                .Select(item => new FundFileHistory(item, template)).ToList();
            if (listHistory.Count > 0)
            {
                fundRequestBusinessLogic.FundFileHistory_Save(listHistory);
                upload.SaveAs(Path.Combine(ConfiguraionProvider.FileStorageFolder, Constants.FundRequest, filename));
            }

            return PartialView("_UpdateFundDbfGrid", report);
        }

        public ActionResult UploadClientVisitsFromDbf()
        {
            ViewBag.DeliveryPoints = ReferencesProvider.GetReferences(Constants.DeliveryPointRef, CurrentUser.Roles, null, null, false);
            ViewBag.ListClientAcquisitionEmployee = ReferencesProvider.GetListClientAcquisitionEmployee(null, true);
            return View();
        }

        [HttpPost]
        public ActionResult UploadClientVisitsFromDbf(FormCollection form)
        {
            string filename = string.Format("{0}_{1}_{2}.zip", DateTime.Now.ToString("dd_MM_yyyy_hh_mm"), CurrentUser.Login, Guid.NewGuid().ToString());
            string path = Path.Combine(ConfiguraionProvider.FileStorageFolder, "UploadFromFilialDbfSources", filename);
            long deliveryPointId;
            if (!long.TryParse(form["DeliveryPointId"], out deliveryPointId))
            {
                return PartialView("_ErrorMessage", "Выберите место выдачи");
            }
            string ClientAcquisitionEmployee = form["ClientAcquisitionEmployee"];
            if (Request.Files == null || Request.Files.Count < 1)
            {
                return PartialView("_ErrorMessage", "Выберите zip-архив c dbf-файлом для загрузки");
            }
            HttpPostedFileBase upload = Request.Files[0];
            upload.SaveAs(path);

            List<ClientVisit.UpdateResultData> report = new List<ClientVisit.UpdateResultData>();
            List<string> parseErrors = new List<string>();
            List<ClientVisit> clientVisitDataList = clientBusinessLogic.ClientVisit_GetFromDBF(path, parseErrors);
            foreach (var cv in clientVisitDataList)
            {
                ClientVisitSaveDataModel data = new ClientVisitSaveDataModel(CurrentUser, cv);
                long recid = cv.Id;
                data.ClearIds();
                data.ClientId = null;
                data.Registrator = new UserModel(CurrentUser);
                data.IsActual = true;
                data.ClientAcquisitionEmployee = ClientAcquisitionEmployee;
                ClientVisit lastClientVisit = clientBusinessLogic.FindLastClientVisit(cv);
                if (!data.DeliveryPointId.HasValue)
                {
                    data.DeliveryPointId = deliveryPointId;
                }

                //если bso будет найден - ему меняем статус на "Выдан клиенту"
                BSO bso = new BSO();
                bso = bsoBusinessLogic.BSO_GetByNumber(cv.TemporaryPolicyNumber);
                if (bso != null)
                {
                    if (bso.Status.Id == (long)ListBSOStatusID.OnDelivery || bso.Status.Id == (long)ListBSOStatusID.OnStorage)
                    {
                        bso.Status.Id = (long)ListBSOStatusID.OnClient;
                        bso.UserId = CurrentUser.Id;
                        bso.DeliveryPointId = cv.DeliveryPointId == null? bso.DeliveryPointId : cv.DeliveryPointId; //Если точка пустая - оставляем Точку, кот. была в БСО
                        bso.StatusDate = cv.TemporaryPolicyDate == null ? cv.StatusDate : cv.TemporaryPolicyDate;
                        bso.Comment = string.Format("Изменение статуса при загрузке DBF (загрузка из филиалов)");
                        if (lastClientVisit != null)
                        {
                            bso.VisitGroupId = lastClientVisit.VisitGroupId;
                        }
                    }
                    else
                    {
                        //если bso уже выдан клиенту - то дальнейшие действия с БСО не имеют значения
                        bso = null;
                    }
                }

                if (lastClientVisit != null)
                {
                    data.ClientId = lastClientVisit.ClientId;
                    if ((cv.TemporaryPolicyNumber ?? string.Empty) != (lastClientVisit.TemporaryPolicyNumber ?? string.Empty)
                        && lastClientVisit.TemporaryPolicyDate == cv.TemporaryPolicyDate)
                    {
                        report.Add(ReportErrorMessage(lastClientVisit, "Даты ВС совпали, номер ВС не совпал", recid));
                        continue;
                    }
                    if ((cv.TemporaryPolicyNumber ?? string.Empty) == (lastClientVisit.TemporaryPolicyNumber ?? string.Empty)
                        && lastClientVisit.TemporaryPolicyDate != cv.TemporaryPolicyDate)
                    {
                        report.Add(ReportErrorMessage(lastClientVisit, "Номер ВС совпал, даты ВС не совпали", recid));
                        continue;
                    }
                    if (AreIdentical(lastClientVisit, data))
                    {
                        data.VisitGroupId = lastClientVisit.VisitGroupId;
                        if (cv.Scenario.Id == ClientVisitScenaries.PolicyExtradition.Id)
                        {
                            if (lastClientVisit.Scenario.Id == ClientVisitScenaries.PolicyExtradition.Id)
                            {
                                report.Add(ReportErrorMessage(lastClientVisit, "Ошибка, повторение записи. Уже существует запись со сценарием POK", recid));
                            }
                            else
                            {
                                if (lastClientVisit.Status.Id == ClientVisitStatuses.PolicyIssuedAndSentToTheFond.Id
                                    || lastClientVisit.Status.Id == ClientVisitStatuses.PolicyIssued.Id)
                                {
                                    report.Add(ReportErrorMessage(lastClientVisit, string.Format("Ошибка, повторение записи. Существует заявка в статусе '{0}'", lastClientVisit.Status.Name), recid));
                                }
                                else if (lastClientVisit.Status.Id == ClientVisitStatuses.PolicyReadyForClient.Id)
                                {
                                    ClientVisit.SaveData lastClientVisitUpdateData = new ClientVisit.SaveData(lastClientVisit);
                                    lastClientVisitUpdateData.Status = ClientVisitStatuses.PolicyIssued.Id;
                                    lastClientVisitUpdateData.StatusDate = cv.StatusDate;
                                    lastClientVisitUpdateData.IssueDate = cv.IssueDate;
                                    lastClientVisitUpdateData.Id = lastClientVisit.Id;
                                    lastClientVisitUpdateData.VisitGroupId = lastClientVisit.VisitGroupId;
                                    lastClientVisitUpdateData.OldClientInfo.Id = lastClientVisit.OldClientInfo.Id;
                                    lastClientVisitUpdateData.NewClientInfo.Id = lastClientVisit.NewClientInfo.Id;
                                    lastClientVisitUpdateData.OldDocument.Id = lastClientVisit.OldDocument.Id;
                                    lastClientVisitUpdateData.NewDocument.Id = lastClientVisit.NewDocument.Id;
                                    lastClientVisitUpdateData.NewForeignDocument.Id = lastClientVisit.NewForeignDocument.Id;
                                    lastClientVisitUpdateData.OldForeignDocument.Id = lastClientVisit.OldForeignDocument.Id;
                                    lastClientVisitUpdateData.LivingAddress.Id = lastClientVisit.LivingAddress.Id;
                                    lastClientVisitUpdateData.RegistrationAddress.Id = lastClientVisit.RegistrationAddress.Id;
                                    lastClientVisitUpdateData.OldPolicy.Id = lastClientVisit.OldPolicy.Id;
                                    lastClientVisitUpdateData.NewPolicy.Id = lastClientVisit.NewPolicy.Id;
                                    lastClientVisitUpdateData.Representative.Id = lastClientVisit.Representative.Id;
                                    lastClientVisitUpdateData.IsActual = true;
                                    var res = clientBusinessLogic.ClientVisit_Save(CurrentUser, lastClientVisitUpdateData, lastClientVisitUpdateData.StatusDate);
                                    if (bso != null) { bsoBusinessLogic.BSO_Save(new BSO.SaveData(bso)); }
                                    var updateResult = new ClientVisit.UpdateResultData()
                                    {
                                        RECID = recid,
                                        Id = lastClientVisit.Id,
                                        ClientId = lastClientVisit.ClientId,
                                        ClientVisitGroupId = lastClientVisit.VisitGroupId,
                                        UnifiedPolicyNumber = lastClientVisitUpdateData.NewPolicy.UnifiedPolicyNumber,
                                        Birthday = lastClientVisitUpdateData.NewClientInfo.Birthday,
                                        Firstname = lastClientVisitUpdateData.NewClientInfo.Firstname,
                                        Secondname = lastClientVisitUpdateData.NewClientInfo.Secondname,
                                        Lastname = lastClientVisitUpdateData.NewClientInfo.Lastname,
                                        Status = new ReferenceItem() { Id = ClientVisitStatuses.PolicyIssued.Id },
                                        IsSuccess = true,
                                        Message = string.Format("Изменена заявка. Прежний статус {0}, прежняя дата статуса {1}", lastClientVisit.Status.Id, lastClientVisit.StatusDate.ToShortDateString())
                                    };
                                    report.Add(updateResult);
                                }
                                else
                                {
                                    report.Add(ReportErrorMessage(lastClientVisit, "Ошибка, заявка со статусом 'Полис изготовлен' отсутствует", recid));
                                }
                            }
                        }
                        else
                        {
                            report.Add(ReportErrorMessage(lastClientVisit, "Ошибка, повторение записи", recid));
                        }
                    }
                    else
                    {
                        var res = SaveClientVisit(data, "Создано новое обращение", recid);
                        if (bso != null) { bsoBusinessLogic.BSO_Save(new BSO.SaveData(bso)); }
                        report.Add(res);
                    }
                }
                else
                {
                    var res = SaveClientVisit(data, "Созданы новый клиент и обращение", recid);
                    if (bso != null)
                    {
                        if (res.ClientVisitGroupId != 0) bso.VisitGroupId = res.ClientVisitGroupId;
                        bsoBusinessLogic.BSO_Save(new BSO.SaveData(bso));
                    }

                    report.Add(res);
                }
            }
            foreach (string errorMessage in parseErrors)
            {
                report.Add(new ClientVisit.UpdateResultData
                {
                    Message = errorMessage
                });
            }

            try
            {
                XDocument doc = new XDocument();
                XElement root = new XElement("Report");
                doc.Add(root);
                foreach (ClientVisit.UpdateResultData item in report)
                {
                    XElement reportItem = new XElement("ReportItem");
                    reportItem.Add(new XElement("RECID", item.RECID));
                    reportItem.Add(new XElement("RECID", item.Id));
                    reportItem.Add(new XElement("ENP", item.UnifiedPolicyNumber));
                    reportItem.Add(new XElement("Lastname", item.Lastname));
                    reportItem.Add(new XElement("Firstname", item.Firstname));
                    reportItem.Add(new XElement("Secondname", item.Secondname));
                    reportItem.Add(new XElement("Birthday", item.Birthday));
                    reportItem.Add(new XElement("Message", item.Message));
                    reportItem.Add(new XElement("Status", item.Status != null ? item.Status.Id : 0));
                    reportItem.Add(new XElement("IsSuccess", item.IsSuccess));
                    reportItem.Add(new XElement("Id", item.Id));
                    reportItem.Add(new XElement("ClientId", item.ClientId));
                    reportItem.Add(new XElement("ClientVisitGroupId", item.ClientVisitGroupId));
                    root.Add(reportItem);
                }
                doc.Save(path.Replace(".zip", ".xml"));
            }
            catch { };

            try
            {
                UploadingReport printedForm = new UploadingReport(filename, report);
                byte[] excelReport = printedForm.GetExcel();
                System.IO.File.WriteAllBytes(path.Replace(".zip", ".xls"), excelReport);
            }
            catch { };

            //Сохраняем
            List<FundFileHistory> listHistory = new List<FundFileHistory>();
            FundFileHistory template = new FundFileHistory()
            {
                StatusID = FundFileHistoryStatus.Filial.Id,
                Date = DateTime.Now,
                UserID = CurrentUser.Id,
                FileName = Request.Files[0].FileName, //оригинальное название файла
                FileUrl = Path.Combine(Constants.Filial, filename) //имя, как мы его сохраняем
            };
            listHistory = report.Where(a => (a.ClientId != 0 && a.ClientVisitGroupId != 0 && a.Id != 0))
                                .Select(item => new FundFileHistory(item, template)).ToList();
            if (listHistory.Count > 0)
            {
                fundRequestBusinessLogic.FundFileHistory_Save(listHistory);
                upload.SaveAs(Path.Combine(ConfiguraionProvider.FileStorageFolder, Constants.Filial, filename));
            }

            return PartialView("_UpdateFundDbfGrid", report.Select(item => new ClientVisitUpdateResultModel(item)));
        }

        public ActionResult MigrateClientVisitsFromDbf()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MigrateClientVisitsFromDbf(FormCollection form)
        {
            string filename = string.Format("{0}_{1}_{2}.zip", DateTime.Now.ToString("dd_MM_yyyy_hh_mm"), CurrentUser.Login, Guid.NewGuid().ToString());
            string path = Path.Combine(ConfiguraionProvider.FileStorageFolder, "MigrationDbfSources", filename);
            if (Request.Files == null || Request.Files.Count < 1)
            {
                return PartialView("_ErrorMessage", "Выберите zip-архив c dbf-файлом для загрузки");
            }
            HttpPostedFileBase upload = Request.Files[0];
            upload.SaveAs(path);

            //[TODO] should be move to business logic
            List<ClientVisit.UpdateResultData> report = new List<ClientVisit.UpdateResultData>();
            List<string> parseErrors = new List<string>();
            List<ClientVisit> clientVisitDataList = clientBusinessLogic.ClientVisit_GetFromDBF(path, parseErrors);
            foreach (var cv in clientVisitDataList)
            {

                ClientVisitSaveDataModel data = new ClientVisitSaveDataModel(CurrentUser, cv);
                long recid = cv.Id;
                data.ClearIds();
                data.ClientId = null;
                data.Registrator = new UserModel(CurrentUser);
                data.IsActual = true;
                if (data.StatusId == ClientVisitStatuses.PolicyIssued.Id)
                {
                    if (data.ScenarioId == ClientVisitScenaries.PolicyExtradition.Id)
                    {
                        data.StatusId = ClientVisitStatuses.PolicyIssuedAndSentToTheFond.Id;
                        data.StatusDate = cv.StatusDate;
                    }
                    else
                    {
                        data.StatusId = ClientVisitStatuses.PolicyReadyForClient.Id;
                        data.StatusDate = cv.StatusDate;
                    }
                }

                ClientVisit lastClientVisit = clientBusinessLogic.FindLastClientVisit(cv);
                //если bso будет найден - ему меняем статус на "Выдан клиенту"
                BSO bso = new BSO();
                bso = bsoBusinessLogic.BSO_GetByNumber(cv.TemporaryPolicyNumber);
                if (bso != null)
                {
                    if (bso.Status.Id == (long)ListBSOStatusID.OnDelivery || bso.Status.Id == (long)ListBSOStatusID.OnStorage)
                    {
                        bso.Status.Id = (long)ListBSOStatusID.OnClient;
                        bso.UserId = CurrentUser.Id;
                        bso.DeliveryPointId = cv.DeliveryPointId == null ? bso.DeliveryPointId : cv.DeliveryPointId; //Если точка пустая - оставляем Точку, кот. была в БСО
                        bso.StatusDate = cv.TemporaryPolicyDate == null ? cv.StatusDate : cv.TemporaryPolicyDate;
                        bso.Comment = string.Format("Изменение статуса при загрузке DBF (Миграция DBF из других ИС)");
                        if (lastClientVisit != null)
                        {
                            bso.VisitGroupId = lastClientVisit.VisitGroupId;
                        }
                    }
                    else
                    {
                        //если bso уже выдан клиенту - то дальнейшие действия с БСО не имеют значения
                        bso = null;
                    }
                }
                if (lastClientVisit != null)
                {
                    data.ClientId = lastClientVisit.ClientId;
                    if (AreIdentical(lastClientVisit, data))
                    {
                        data.VisitGroupId = lastClientVisit.VisitGroupId;
                        if (data.ScenarioId == ClientVisitScenaries.PolicyExtradition.Id)
                        {
                            data.StatusId = ClientVisitStatuses.PolicyIssuedAndSentToTheFond.Id;
                            data.StatusDate = cv.StatusDate;
                            if (lastClientVisit.Status.Id != ClientVisitStatuses.PolicyIssuedAndSentToTheFond.Id)
                            {
                                var res = SaveClientVisit(data, "Создана заявка в статусе 'Полис выдан и отправлен в фонд'", recid);
                                if (bso != null) { bsoBusinessLogic.BSO_Save(new BSO.SaveData(bso)); }
                                report.Add(res);
                            }
                            else
                            {
                                report.Add(new ClientVisit.UpdateResultData
                                {
                                    RECID = recid,
                                    Id = lastClientVisit.Id,
                                    ClientId = lastClientVisit.ClientId,
                                    ClientVisitGroupId = lastClientVisit.VisitGroupId,
                                    UnifiedPolicyNumber = cv.NewPolicy.UnifiedPolicyNumber,
                                    Birthday = cv.NewClientInfo.Birthday,
                                    Firstname = cv.NewClientInfo.Firstname,
                                    Secondname = cv.NewClientInfo.Secondname,
                                    Lastname = cv.NewClientInfo.Lastname,
                                    Status = new ReferenceItem() { Id = lastClientVisit.Status.Id },
                                    IsSuccess = false,
                                    Message = string.Format("Уже существует заявка по выдаче и отправке в фонд с номером {0}", lastClientVisit.Id)
                                });
                            }
                        }
                        else
                        {
                            if (lastClientVisit.StatusDate <= data.StatusDate)
                            {
                                if (data.StatusDate == lastClientVisit.StatusDate && lastClientVisit.Status.Id == data.StatusId)
                                {
                                    report.Add(new ClientVisit.UpdateResultData
                                    {
                                        RECID = recid,
                                        Id = lastClientVisit.Id,
                                        ClientId = lastClientVisit.ClientId,
                                        ClientVisitGroupId = lastClientVisit.VisitGroupId,
                                        UnifiedPolicyNumber = cv.NewPolicy.UnifiedPolicyNumber,
                                        Birthday = cv.NewClientInfo.Birthday,
                                        Firstname = cv.NewClientInfo.Firstname,
                                        Secondname = cv.NewClientInfo.Secondname,
                                        Lastname = cv.NewClientInfo.Lastname,
                                        Status = new ReferenceItem() { Id = lastClientVisit.Status.Id },
                                        IsSuccess = false,
                                        Message = string.Format("Дата статуса и статус равны в предыдущей заявке - предыдущая заявка номер {0}", lastClientVisit.Id)
                                    });
                                }
                                else
                                {
                                    var res = SaveClientVisit(data, string.Format("Создана заявка в статусе {0}", data.StatusId), recid);
                                    if (bso != null) { bsoBusinessLogic.BSO_Save(new BSO.SaveData(bso)); }
                                    report.Add(res);
                                }
                            }
                            else
                            {
                                if (ClientVisitStatuses.MigrationNeedUpdate(data.StatusId, lastClientVisit.Status.Id))
                                {
                                    data.VisitId = lastClientVisit.Id;
                                    data.VisitGroupId = lastClientVisit.VisitGroupId;
                                    data.OldClientInfo.Id = lastClientVisit.OldClientInfo.Id;
                                    data.NewClientInfo.Id = lastClientVisit.NewClientInfo.Id;
                                    data.OldDocument.Id = lastClientVisit.OldDocument.Id;
                                    data.NewDocument.Id = lastClientVisit.NewDocument.Id;
                                    data.NewForeignDocument.Id = lastClientVisit.NewForeignDocument.Id;
                                    data.OldForeignDocument.Id = lastClientVisit.OldForeignDocument.Id;
                                    data.LivingAddress.Id = lastClientVisit.LivingAddress.Id;
                                    data.RegistrationAddress.Id = lastClientVisit.RegistrationAddress.Id;
                                    data.OldPolicy.Id = lastClientVisit.OldPolicy.Id;
                                    data.NewPolicy.Id = lastClientVisit.NewPolicy.Id;
                                    data.Representative.Id = lastClientVisit.Representative.Id;
                                    report.Add(SaveClientVisit(data, string.Format("Новая дата статуса меньше старой, но статус больше: старый статус {0}, новый статус {1}", lastClientVisit.Status.Id, data.StatusId), recid));
                                    if (bso != null) { bsoBusinessLogic.BSO_Save(new BSO.SaveData(bso)); }
                                }
                                else
                                {
                                    report.Add(new ClientVisit.UpdateResultData
                                    {
                                        RECID = recid,
                                        Id = lastClientVisit.Id,
                                        ClientId = lastClientVisit.ClientId,
                                        ClientVisitGroupId = lastClientVisit.VisitGroupId,
                                        UnifiedPolicyNumber = cv.NewPolicy.UnifiedPolicyNumber,
                                        Birthday = cv.NewClientInfo.Birthday,
                                        Firstname = cv.NewClientInfo.Firstname,
                                        Secondname = cv.NewClientInfo.Secondname,
                                        Lastname = cv.NewClientInfo.Lastname,
                                        Status = new ReferenceItem() { Id = lastClientVisit.Status.Id },
                                        IsSuccess = false,
                                        Message = string.Format("Новая дата и статус меньше (или отношение между статусами не определено): старый статус {0}, новый статус {1}", lastClientVisit.Status.Id, data.StatusId)
                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        var res = SaveClientVisit(data, "Создано новое обращние", recid);
                        if (bso != null) { bsoBusinessLogic.BSO_Save(new BSO.SaveData(bso)); }
                        report.Add(res);
                    }
                }
                else
                {
                    var res = SaveClientVisit(data, "Созданы новый клиент и обращние", recid);
                    if (bso != null)
                    {
                        if (res.ClientVisitGroupId != 0) bso.VisitGroupId = res.ClientVisitGroupId;
                        bsoBusinessLogic.BSO_Save(new BSO.SaveData(bso));
                    }
                    report.Add(res);
                }
            }

            //Сохраняем историю файлов
            List<FundFileHistory> listHistory = new List<FundFileHistory>();
            FundFileHistory template = new FundFileHistory()
            {
                StatusID = FundFileHistoryStatus.Migrate.Id,
                Date = DateTime.Now,
                UserID = CurrentUser.Id,
                FileName = Request.Files[0].FileName, //оригинальное название файла
                FileUrl = Path.Combine(Constants.Migrate, filename) //имя, как мы его сохраняем
            };
            listHistory = report.Where(a => (a.ClientId != 0 && a.ClientVisitGroupId != 0 && a.Id != 0))
                                .Select(item => new FundFileHistory(item, template)).ToList();
            if (listHistory.Count > 0)
            {
                fundRequestBusinessLogic.FundFileHistory_Save(listHistory);
                upload.SaveAs(Path.Combine(ConfiguraionProvider.FileStorageFolder, Constants.Migrate, filename));
            }

            foreach (string errorMessage in parseErrors)
            {
                report.Add(new ClientVisit.UpdateResultData
                {
                    Message = errorMessage
                });
            }

            try
            {
                XDocument doc = new XDocument();
                XElement root = new XElement("Report");
                doc.Add(root);
                foreach (ClientVisit.UpdateResultData item in report)
                {
                    XElement reportItem = new XElement("ReportItem");
                    reportItem.Add(new XElement("RECID", item.RECID));
                    reportItem.Add(new XElement("RECID", item.Id));
                    reportItem.Add(new XElement("ENP", item.UnifiedPolicyNumber));
                    reportItem.Add(new XElement("Lastname", item.Lastname));
                    reportItem.Add(new XElement("Firstname", item.Firstname));
                    reportItem.Add(new XElement("Secondname", item.Secondname));
                    reportItem.Add(new XElement("Birthday", item.Birthday));
                    reportItem.Add(new XElement("Message", item.Message));
                    reportItem.Add(new XElement("Status", item.Status != null ? item.Status.Id : 0));
                    reportItem.Add(new XElement("IsSuccess", item.IsSuccess));
                    reportItem.Add(new XElement("Id", item.Id));
                    reportItem.Add(new XElement("ClientId", item.ClientId));
                    reportItem.Add(new XElement("ClientVisitGroupId", item.ClientVisitGroupId));
                    root.Add(reportItem);
                }
                doc.Save(path.Replace(".zip", ".xml"));
            }
            catch { };

            try
            {
                UploadingReport printedForm = new UploadingReport(filename, report);
                byte[] excelReport = printedForm.GetExcel();
                System.IO.File.WriteAllBytes(path.Replace(".zip", ".xls"), excelReport);
            }
            catch { };
            return PartialView("_UpdateFundDbfGrid", report.Select(item => new ClientVisitUpdateResultModel(item)));
        }

        public ActionResult UploadMFCClientVisits()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadMFCClientVisits(FormCollection form)
        {
            string filename = Guid.NewGuid().ToString() + ".zip";
            string path = Path.Combine(ConfiguraionProvider.FileStorageFolder, filename);
            List<ClientVisit.UpdateResultData> report = new List<ClientVisit.UpdateResultData>();
            if (Request.Files == null || Request.Files.Count < 1)
            {
                return PartialView("_ErrorMessage", "Выберите dbf-файл для загрузки");
            }
            try
            {
                HttpPostedFileBase upload = Request.Files[0];
                upload.SaveAs(path);
                report = dbfProcessingBusinessLogic.UploadMFCClientVisits(CurrentUser, path);

                //Сохраняем историю файлов
                List<FundFileHistory> listHistory = new List<FundFileHistory>();
                FundFileHistory template = new FundFileHistory()
                {
                    StatusID = FundFileHistoryStatus.MFC.Id,
                    Date = DateTime.Now,
                    UserID = CurrentUser.Id,
                    FileName = Request.Files[0].FileName, //оригинальное название файла
                    FileUrl = Path.Combine(Constants.MFC, filename) //имя, как мы его сохраняем
                };
                listHistory = report.Where(a => (a.ClientId != 0 && a.ClientVisitGroupId != 0 && a.Id != 0))
                                    .Select(item => new FundFileHistory(item, template)).ToList();
                if (listHistory.Count > 0)
                {
                    fundRequestBusinessLogic.FundFileHistory_Save(listHistory);
                    upload.SaveAs(Path.Combine(ConfiguraionProvider.FileStorageFolder, Constants.MFC, filename));
                }
            }
            finally
            {
                if (System.IO.File.Exists(filename))
                {
                    System.IO.File.Delete(filename);
                }
            }
            return PartialView("_UpdateFundDbfGrid", report.Select(item => new ClientVisitUpdateResultModel(item)));
        }

        [HttpPost]
        public ActionResult UploadMFCClientVisitsExtradition(FormCollection form)
        {
            string filename = Guid.NewGuid().ToString() + ".zip";
            string path = Path.Combine(ConfiguraionProvider.FileStorageFolder, filename);
            List<ClientVisit.UpdateResultData> report = new List<ClientVisit.UpdateResultData>();
            if (Request.Files == null || Request.Files.Count < 1)
            {
                return PartialView("_ErrorMessage", "Выберите dbf-файл для загрузки");
            }
            try
            {
                HttpPostedFileBase upload = Request.Files[0];
                upload.SaveAs(path);
                report = dbfProcessingBusinessLogic.UploadMFCClientVisitsExtradition(CurrentUser, path);

                //Сохраняем историю файлов
                List<FundFileHistory> listHistory = new List<FundFileHistory>();
                FundFileHistory template = new FundFileHistory()
                {
                    StatusID = FundFileHistoryStatus.MFC.Id,
                    Date = DateTime.Now,
                    UserID = CurrentUser.Id,
                    FileName = Request.Files[0].FileName, //оригинальное название файла
                    FileUrl = Path.Combine(Constants.MFC, filename) //имя, как мы его сохраняем
                };
                listHistory = report.Where(a => (a.ClientId != 0 && a.ClientVisitGroupId != 0 && a.Id != 0))
                                    .Select(item => new FundFileHistory(item, template)).ToList();
                if (listHistory.Count > 0)
                {
                    fundRequestBusinessLogic.FundFileHistory_Save(listHistory);
                    upload.SaveAs(Path.Combine(ConfiguraionProvider.FileStorageFolder, Constants.MFC, filename));
                }
            }
            finally
            {
                if (System.IO.File.Exists(filename))
                {
                    System.IO.File.Delete(filename);
                }
            }
            return PartialView("_UpdateFundDbfGrid", report.Select(item => new ClientVisitUpdateResultModel(item)));
        }


        [HttpPost]
        public ActionResult UploadMFCClientVisitsOldData(FormCollection form)
        {
            string filename = Guid.NewGuid().ToString() + ".zip";
            string path = Path.Combine(ConfiguraionProvider.FileStorageFolder, filename);
            List<ClientVisit.UpdateResultData> report = new List<ClientVisit.UpdateResultData>();
            if (Request.Files == null || Request.Files.Count < 1)
            {
                return PartialView("_ErrorMessage", "Выберите dbf-файл для загрузки");
            }
            try
            {
                HttpPostedFileBase upload = Request.Files[0];
                upload.SaveAs(path);
                report = dbfProcessingBusinessLogic.UploadMFCClientVisitsOldData(CurrentUser, path);

                //Сохраняем историю файлов
                List<FundFileHistory> listHistory = new List<FundFileHistory>();
                FundFileHistory template = new FundFileHistory()
                {
                    StatusID = FundFileHistoryStatus.MFC.Id,
                    Date = DateTime.Now,
                    UserID = CurrentUser.Id,
                    FileName = Request.Files[0].FileName, //оригинальное название файла
                    FileUrl = Path.Combine(Constants.MFC, filename) //имя, как мы его сохраняем
                };
                listHistory = report.Where(a => (a.ClientId != 0 && a.ClientVisitGroupId != 0 && a.Id != 0))
                                    .Select(item => new FundFileHistory(item, template)).ToList();
                if (listHistory.Count > 0)
                {
                    fundRequestBusinessLogic.FundFileHistory_Save(listHistory);
                    upload.SaveAs(Path.Combine(ConfiguraionProvider.FileStorageFolder, Constants.MFC, filename));
                }
            }
            finally
            {
                if (System.IO.File.Exists(filename))
                {
                    System.IO.File.Delete(filename);
                }
            }
            return PartialView("_UpdateFundDbfGrid", report.Select(item => new ClientVisitUpdateResultModel(item)));
        }


        #region Private methods

        private ClientVisit.UpdateResultData SaveClientVisit(ClientVisitSaveDataModel data, string successMessage = null, long? recid = null)
        {
            try
            {
                data.IsActual = true;
                var result = clientBusinessLogic.ClientVisit_Save(CurrentUser, data.GetClientVisitSaveData(), data.CreateDate);
                return new ClientVisit.UpdateResultData()
                {
                    RECID = recid ?? 0,
                    Id = result.ClientVisitID,
                    ClientId = result.ClientID,
                    ClientVisitGroupId = result.VisitGroupId,
                    UnifiedPolicyNumber = data.NewPolicy.UnifiedPolicyNumber,
                    Birthday = data.NewClientInfo.Birthday,
                    Firstname = data.NewClientInfo.Firstname,
                    Secondname = data.NewClientInfo.Secondname,
                    Lastname = data.NewClientInfo.Lastname,
                    Status = new ReferenceItem() { Id = data.StatusId },
                    IsSuccess = true,
                    Message = successMessage ?? "Успешно сохранено"
                };
            }
            catch (Exception ex)
            {
                return new ClientVisit.UpdateResultData
                {
                    UnifiedPolicyNumber = data.NewPolicy.UnifiedPolicyNumber,
                    Birthday = data.NewClientInfo.Birthday,
                    Firstname = data.NewClientInfo.Firstname,
                    Secondname = data.NewClientInfo.Secondname,
                    Lastname = data.NewClientInfo.Lastname,
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        private bool AreIdentical(ClientVisit cv, ClientVisitSaveDataModel data)
        {
            return (cv.TemporaryPolicyNumber ?? string.Empty) == (data.TemporaryPolicyNumber ?? string.Empty) && data.TemporaryPolicyDate == cv.TemporaryPolicyDate;
        }

        private void WriteReportSTOPcsv(List<STOP> report, Nomernik.History history, string fileName)
        {
            StreamWriter file;
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                using (file = new StreamWriter(fs))
                {
                    file.WriteLine("Отчет по обработанным записям. Дата обработки (загрузки): " + history.LoadDate.ToString());
                    string line;
                    foreach (var element in report)
                    {
                        line = element.SCENARIO + " " +
                            element.S_CARD + " " +
                            element.N_CARD + " " +
                            element.ENP + " " +
                            element.VSN + " " +
                            element.QZ + " " +
                            element.DATE_ARC + " " +
                            element.DATE_END + " " +
                            element.IST + " " +
                            element.Status + " " +
                            element.Comment;
                        file.WriteLine(line);
                    }
                    file.Flush();
                    file.Close();
                }
            }
        }

        private void WriteReportNOMPcsv(List<NOMP> report, Nomernik.History history, string fileName)
        {
            StreamWriter file;
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                using (file = new StreamWriter(fs))
                {
                    file.WriteLine("Отчет по обработанным записям. Дата обработки (загрузки): " + history.LoadDate.ToString());
                    string line;
                    foreach (var element in report)
                    {
                        line = element.S_CARD + " " +
                            element.N_CARD + " " +
                            element.ENP + " " +
                            element.VSN + " " +
                            element.LPU_ID + " " +
                            element.DATE_IN + " " +
                            element.SPOS + " " +
                            element.Status + " " +
                            element.Comment;
                        file.WriteLine(line);
                    }
                    file.Flush();
                    file.Close();
                }
            }
        }

        private static ClientVisit.UpdateResultData ReportErrorMessage(ClientVisit lastClientVisit, string message, long recid)
        {
            return new ClientVisit.UpdateResultData
            {
                RECID = recid,
                Id = lastClientVisit.Id,
                ClientId = lastClientVisit.ClientId,
                ClientVisitGroupId = lastClientVisit.VisitGroupId,
                UnifiedPolicyNumber = lastClientVisit.NewPolicy.UnifiedPolicyNumber,
                Birthday = lastClientVisit.NewClientInfo.Birthday,
                Firstname = lastClientVisit.NewClientInfo.Firstname,
                Secondname = lastClientVisit.NewClientInfo.Secondname,
                Lastname = lastClientVisit.NewClientInfo.Lastname,
                Status = new ReferenceItem() { Id = lastClientVisit.Status.Id },
                IsSuccess = false,
                Message = string.Format(message ?? "Ошибка добавления")
            };
        }

        private string GetFileSizeToString(float size)
        {
            if ((size / 1024 / 1024 / 1024) > 1)
            {
                return string.Format("{0:F2} Гб", size / 1024 / 1024 / 1024);
            }
            else
            {
                if ((size / 1024 / 1024) > 1)
                {
                    return string.Format("{0:F2} Мб", size / 1024 / 1024);
                }
                else
                {
                    if ((size / 1024) > 1)
                    {
                        return string.Format("{0:F2} Кб", size / 1024);
                    }
                    else
                    {
                        return string.Format("{0:F2} байт", size);
                    }
                }
            }
        }

        #endregion
    }
}