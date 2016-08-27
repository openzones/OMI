using AutoMapper;
using OMInsurance.BusinessLogic;
using OMInsurance.Configuration;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using OMInsurance.Entities.Searching;
using OMInsurance.Entities.Sorting;
using OMInsurance.Interfaces;
using OMInsurance.WebApps.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OMInsurance.WebApps.Controllers
{
    public class FundRequestProcessingController : OMInsuranceController
    {
        IClientBusinessLogic clientBusinessLogic;
        IFundRequestBusinessLogic fundRequestBusinessLogic;
        public FundRequestProcessingController()
        {
            clientBusinessLogic = new ClientBusinessLogic();
            userBusinessLogic = new UserBusinessLogic();
            referenceBusinessLogic = new ReferenceBusinessLogic();
            fundRequestBusinessLogic = new FundRequestBusinessLogic();
        }

        public ActionResult Index()
        {
            FundRequestIndexModel model = new FundRequestIndexModel();
            FundRequestIndexModel m = new FundRequestIndexModel()
            {
                ClientVisits = new List<ClientVisitFundInfoModel>(),
                SearchCriteriaModel = new ClientVisitSearchCriteriaModel() { IsActualInVisitGroup = true }
            };
            return View(m);
        }

        [HttpPost]
        public ActionResult Index(FundRequestIndexModel model)
        {
            ClientVisitSearchCriteria criteria = model.SearchCriteriaModel.GetClientSearchCriteria();
            DataPage<ClientVisitInfo> visits = clientBusinessLogic.ClientVisit_Find(
                    criteria,
                    new List<SortCriteria<ClientVisitSortField>>(),
                    new PageRequest() { PageNumber = 1, PageSize = 1000 });
            FundRequestIndexModel m = new FundRequestIndexModel()
            {
                ClientVisits = visits.Data
                    .Select(item => new ClientVisitFundInfoModel(item)).OrderBy(item => item.Lastname).ToList(),
                SearchCriteriaModel = new ClientVisitSearchCriteriaModel(
                    criteria)
            };
            return View(m);
        }

        public ActionResult UploadFundResponse()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFundResponse(FormCollection form)
        {
            try
            {
                string filename = string.Format("{0}_{1}_{2}.zip", DateTime.Now.ToString("dd_MM_yyyy_hh_mm"), CurrentUser.Login, Guid.NewGuid().ToString());
                string path = Path.Combine(ConfiguraionProvider.FileStorageFolder, filename);
                if (Request.Files == null || Request.Files.Count < 1)
                {
                    return PartialView("_ErrorMessage", "Выберите zip-архив c dbf-файлом для загрузки");
                }
                HttpPostedFileBase upload = Request.Files[0];
                upload.SaveAs(path);
                bool runScenario = false;
                var temp = bool.TryParse(form["runScenario"], out runScenario);
                List<FundResponse.UploadReportData> report = fundRequestBusinessLogic.UploadFundResponse(CurrentUser, path, runScenario);

                //сохранение истории файлов
                List<FundFileHistory> listHistory = new List<FundFileHistory>();
                FundFileHistory template = new FundFileHistory()
                {
                    StatusID = FundFileHistoryStatus.SverkaResponse.Id,
                    Date = DateTime.Now,
                    UserID = CurrentUser.Id,
                    FileName = Request.Files[0].FileName, //оригинальное название файла
                    FileUrl = Path.Combine(Constants.SverkaResponse, filename) //имя, как мы его сохраняем
                };
                listHistory = report.GroupBy(a => a.ClientVisitId)
                                    .Select(b => b.First())
                                    .Where(a => (a.ClientVisitId != 0 && a.VisitGroupId != 0 && a.ClientId != 0))
                                    .Select(item => new FundFileHistory(item, template))
                                    .ToList();

                if (listHistory.Count > 0)
                {
                    fundRequestBusinessLogic.FundFileHistory_Save(listHistory);
                    upload.SaveAs(Path.Combine(ConfiguraionProvider.FileStorageFolder, Constants.SverkaResponse, filename));
                }

                return PartialView("_FundResponseUploadReport", report.Select(item => Mapper.Map<FundResponseUploadReportModel>(item)));
            }
            catch (Exception ex)
            {
                return PartialView("_ErrorMessage", ex.Message);
            }
        }

        [HttpPost]
        public ActionResult UploadFundResponseUnion(FormCollection form)
        {
            try
            {
                string filename = string.Format("{0}_{1}_{2}.zip", DateTime.Now.ToString("dd_MM_yyyy_hh_mm"), CurrentUser.Login, Guid.NewGuid().ToString());
                string path = Path.Combine(ConfiguraionProvider.FileStorageFolder, filename);
                if (Request.Files == null || Request.Files.Count < 1)
                {
                    return PartialView("_ErrorMessage", "Выберите zip-архив c dbf-файлом для загрузки");
                }
                HttpPostedFileBase upload = Request.Files[0];
                upload.SaveAs(path);
                bool runScenario = false;
                bool.TryParse(form["runScenario"], out runScenario);
                List<FundResponse.UploadReportData> report = fundRequestBusinessLogic.UploadFundResponseUnion(CurrentUser, path, runScenario);

                //сохранение истории файлов
                List<FundFileHistory> listHistory = new List<FundFileHistory>();
                FundFileHistory template = new FundFileHistory()
                {
                    StatusID = FundFileHistoryStatus.SverkaResponse.Id,
                    Date = DateTime.Now,
                    UserID = CurrentUser.Id,
                    FileName = Request.Files[0].FileName, //оригинальное название файла
                    FileUrl = Path.Combine(Constants.SverkaResponse, filename) //имя, как мы его сохраняем
                };
                //делаем distinct
                var groupByClientVisitId = report.GroupBy(a => a.ClientVisitId).Select(b => b.First()).ToList();
                listHistory = groupByClientVisitId.Where(a => (a.ClientVisitId != 0 && a.VisitGroupId != 0 && a.ClientId != 0))
                                    .Select(item => new FundFileHistory(item, template)).ToList();
                if (listHistory.Count > 0)
                {
                    fundRequestBusinessLogic.FundFileHistory_Save(listHistory);
                    upload.SaveAs(Path.Combine(ConfiguraionProvider.FileStorageFolder, Constants.SverkaResponse, filename));
                }

                return PartialView("_FundResponseUploadReport", report.Select(item => Mapper.Map<FundResponseUploadReportModel>(item)));
            }
            catch (Exception ex)
            {
                return PartialView("_ErrorMessage", ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Upload_SecondStepReconciliationPack(FormCollection form)
        {
            string filename = string.Empty;
            string path = string.Empty;
            try
            {
                filename = string.Format("{0}_{1}_{2}.zip", DateTime.Now.ToString("dd_MM_yyyy_hh_mm"), CurrentUser.Login, Guid.NewGuid().ToString());
                path = Path.Combine(ConfiguraionProvider.FileStorageFolder, filename);
                if (Request.Files == null || Request.Files.Count < 1)
                {
                    return PartialView("_ErrorMessage", "Выберите zip-архив c dbf-файлом для загрузки");
                }
                HttpPostedFileBase upload = Request.Files[0];
                upload.SaveAs(path);
                List<FundResponse.UploadReportData> report = fundRequestBusinessLogic.Upload_SecondStepReconciliationPack(CurrentUser, path);

                //сохранение истории файлов
                List<FundFileHistory> listHistory = new List<FundFileHistory>();
                FundFileHistory template = new FundFileHistory()
                {
                    StatusID = FundFileHistoryStatus.SverkaResponse.Id,
                    Date = DateTime.Now,
                    UserID = CurrentUser.Id,
                    FileName = Request.Files[0].FileName, //оригинальное название файла
                    FileUrl = Path.Combine(Constants.SverkaResponse, filename) //имя, как мы его сохраняем
                };
                var groupByClientVisitId = report.GroupBy(a => a.ClientVisitId).Select(b => b.First()).ToList();
                listHistory = groupByClientVisitId.Where(a => (a.ClientVisitId != 0 && a.VisitGroupId != 0 && a.ClientId != 0))
                                    .Select(item => new FundFileHistory(item, template)).ToList();
                if (listHistory.Count > 0)
                {
                    fundRequestBusinessLogic.FundFileHistory_Save(listHistory);
                    upload.SaveAs(Path.Combine(ConfiguraionProvider.FileStorageFolder, Constants.SverkaResponse, filename));
                }

                return PartialView("_FundResponseUploadReport", report.Select(item => Mapper.Map<FundResponseUploadReportModel>(item)));
            }
            catch (Exception ex)
            {
                return PartialView("_ErrorMessage", ex.Message);
            }
            finally
            {
                if (!string.IsNullOrEmpty(path) && System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
        }

        [HttpPost]
        public ActionResult UploadSubmitFundResponse(FormCollection form)
        {
            try
            {
                string filename = string.Format("{0}_{1}_{2}.zip", DateTime.Now.ToString("dd_MM_yyyy_hh_mm"), CurrentUser.Login, Guid.NewGuid().ToString());
                string path = Path.Combine(ConfiguraionProvider.FileStorageFolder, filename);
                if (Request.Files == null || Request.Files.Count < 1)
                {
                    return PartialView("_ErrorMessage", "Выберите zip-архив c dbf-файлом для загрузки");
                }
                HttpPostedFileBase upload = Request.Files[0];
                upload.SaveAs(path);
                List<FundResponse.UploadReportData> report = fundRequestBusinessLogic.UploadSubmitFundResponse(CurrentUser, path);

                List<FundFileHistory> listHistory = new List<FundFileHistory>();
                FundFileHistory template = new FundFileHistory()
                {
                    StatusID = FundFileHistoryStatus.FundResponse.Id,
                    Date = DateTime.Now,
                    UserID = CurrentUser.Id,
                    FileName = Request.Files[0].FileName, //оригинальное название файла
                    FileUrl = Path.Combine(Constants.FundResponse, filename) //имя, как мы его сохраняем
                };
                var groupByClientVisitId = report.GroupBy(a => a.ClientVisitId).Select(b => b.First()).ToList();
                listHistory = groupByClientVisitId.Where(a => (a.ClientVisitId != 0 && a.VisitGroupId != 0 && a.ClientId != 0))
                                    .Select(item => new FundFileHistory(item, template)).ToList();
                if (listHistory.Count > 0)
                {
                    fundRequestBusinessLogic.FundFileHistory_Save(listHistory);
                    upload.SaveAs(Path.Combine(ConfiguraionProvider.FileStorageFolder, Constants.FundResponse, filename));
                }

                return PartialView("_FundResponseUploadReport", report.Select(item => Mapper.Map<FundResponseUploadReportModel>(item)));
            }
            catch (Exception ex)
            {
                return PartialView("_ErrorMessage", ex.Message);
            }
        }

        public ActionResult FundResponse(long id)
        {
            List<FundResponsePackage> responses = fundRequestBusinessLogic.FundResponsePackage_GetListByClientVisitId(id);
            IEnumerable<FundResponsePackageModel> packs = responses.Select(item => new FundResponsePackageModel()
            {
                ImportDate = item.ImportDate,
                Responses = item.Responses.Select(res => Mapper.Map<FundResponseModel>(res)).OrderBy(res => res.Id).ToList()
            });
            return PartialView("_FundResponsePackageModelGrid", packs.ToList());
        }

        [HttpPost]
        public ActionResult ApplyResponse(FundResponseCopyFieldsModel model)
        {
            fundRequestBusinessLogic.ApplyResponse(CurrentUser, model.GetForBll());
            return View("_Message", "Заявка обновлена");
        }

        [HttpPost]
        public ActionResult ClientVisit_SetReadyToFundSubmitRequest(long? id, bool? isReady)
        {
            try
            {
                string message = string.Format("Обновлено пользователем {0}", CurrentUser.Login);
                fundRequestBusinessLogic.ClientVisit_SetReadyToFundSubmitRequest(CurrentUser, id.Value, isReady.Value, message);
                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult ClientVisits_SetReadyToFundSubmitRequest(List<long> ids, bool isReady)
        {
            List<long> successIds = new List<long>();
            foreach (var id in ids)
            {
                try
                {
                    string message = string.Format("Обновлено пользователем {0}", CurrentUser.Login);
                    fundRequestBusinessLogic.ClientVisit_SetReadyToFundSubmitRequest(CurrentUser, id, isReady, message);
                    successIds.Add(id);
                }
                catch (Exception ex)
                {
                }
            }
            return Json(new { result = "OK", successIds = successIds });
        }

        [HttpPost]
        public ActionResult ClientVisit_SetDifficultCase(long? id, bool? isDifficult)
        {
            try
            {
                fundRequestBusinessLogic.ClientVisits_SetDifficultCase(id.Value, isDifficult.Value);
                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        public ActionResult FundFileHistoryGet(long id)
        {
            List<FundFileHistory> items = fundRequestBusinessLogic.FundFileHistory_Get(id).OrderBy(a => a.Date).ToList();
            List<User> listUser = userBusinessLogic.Find("");
            List<ReferenceItem> listStatus = referenceBusinessLogic.GetReferencesList(Constants.FundFileHistoryStatusRef);
            return PartialView("_FundFileHistoryGrid", items.Select(item => new FundFileHistoryModel(item, listUser, listStatus)));
        }
    }
}