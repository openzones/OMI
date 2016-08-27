using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using OMInsurance.Entities.SMS;
using OMInsurance.Entities.Searching;
using OMInsurance.WebApps.Models;
using OMInsurance.WebApps.Security;
using OMInsurance.Interfaces;
using OMInsurance.BusinessLogic;
using OMInsurance.WebApps.Validation;

namespace OMInsurance.WebApps.Controllers
{
    [AuthorizeUser]
    [AuthorizeUser(Roles = "Administrator, AdministratorBSO, Registrator, ResponsibleBSO")]
    public class BSOController : OMInsuranceController
    {
        IBSOBusinessLogic bsoBusinessLogic;
        IUserBusinessLogic userBusinessLogic;
        ClientBusinessLogic clientBusinessLogic = new ClientBusinessLogic();

        public BSOController()
        {
            bsoBusinessLogic = new BSOBusinessLogic();
            userBusinessLogic = new UserBusinessLogic();
        }

        public ActionResult Index(BSOListModel bsoListModel, string sortField)
        {
            bsoListModel.SortField = sortField;
            bsoListModel.SortOrder = bsoListModel.SortOrder == "ASC" ? "DESC" : "ASC";

            if (Role.Registrator.Id == Role.GetRealRole(CurrentUser).Id)
            {
                bsoListModel.SearchCriteriaModel.DeliveryCenterIds = new List<long>() { CurrentUser.Department.Id };
                bsoListModel.SearchCriteriaModel.DeliveryPointIds =  new List<long>() { CurrentUser.DeliveryPoint.Id };
            }

            if (Role.ResponsibleBSO.Id == Role.GetRealRole(CurrentUser).Id)
            {
                bsoListModel.SearchCriteriaModel.ResponsibleID = CurrentUser.Id;
            }

            BSOSearchCriteria criteria = bsoListModel.SearchCriteriaModel.GetBSOSearchCriteria();
            List<SortCriteria<BSOSortField>> sortList = new List<SortCriteria<BSOSortField>>();
            if (!string.IsNullOrEmpty(bsoListModel.SortField))
            {
                SortCriteria<BSOSortField> temp = new SortCriteria<BSOSortField>();
                temp.SortField = (BSOSortField)long.Parse(bsoListModel.SortField);
                temp.SortOrder = (SortOrder)((bsoListModel.SortOrder == "ASC") ? 1 : 0);
                sortList.Add(temp);
            }
            DataPage<BSOInfo> bsos = bsoBusinessLogic.BSO_Find(
                    criteria,
                    sortList,
                    new PageRequest() { PageNumber = bsoListModel.PageNumber, PageSize = bsoListModel.PageSize });
            BSOListModel model = new BSOListModel()
            {
                Items = bsos.Data
                    .Select(item => new BSOBaseInfoModel(item)),
                SearchCriteriaModel = new BSOSearchCriteriaModel(
                    criteria),
                PageNumber = bsoListModel.PageNumber,
                PageSize = bsoListModel.PageSize,
                TotalCount = bsos.TotalCount,
                SortField = bsoListModel.SortField,
                SortOrder = bsoListModel.SortOrder,
            };
            model.GetAvailableBSOStatus(bsoListModel.SearchCriteriaModel.StatusId, true);
            model.SearchCriteriaModel.NewStatusDate = DateTime.Now;
            return View(model);
        }

        public ActionResult GoToBSOHistory(string temporaryPolicyNumber)
        {
            BSO bso = bsoBusinessLogic.BSO_GetByNumber(temporaryPolicyNumber);
            if(bso != null)
            {
                return RedirectToAction("BSOHistory", "BSO", new { id = bso.Id });
            }
            else
            {
                return RedirectToAction("Index");
            }
            
        }

        public ActionResult BSOHistory(long id)
        {
            BSO bso = bsoBusinessLogic.BSO_GetByID(id);
            BSOBaseModel model = new BSOBaseModel(bso);
            if (bso.VisitGroupId != null)
            {
                ClientVisit clientVisit = clientBusinessLogic.ClientVisit_GetLastClientVisitInGroup((long)bso.VisitGroupId);
                model.ClientVisitId = clientVisit.Id;
                model.ClientId = clientVisit.ClientId;
            }

            foreach (var a in model.HistoryModel)
            {
                if (model.UserId != null)
                {
                    User user = userBusinessLogic.User_Get((long)a.UserId);
                    if (user != null) a.UserName = user.Lastname + " " + user.Firstname.Remove(1) + "." + " " + user.Secondname.Remove(1) + ".";
                }

                if (a.ResponsibleID != null)
                {
                    User user = userBusinessLogic.User_Get((long)a.ResponsibleID);
                    if (user != null) a.ResponsibleName = user.Lastname + " " + user.Firstname.Remove(1) + "." + " " + user.Secondname.Remove(1) + ".";
                }
            }
            return View(model);
        }

        public ActionResult Edit(long? id)
        {
            if (id != null)
            {
                BSO bso = bsoBusinessLogic.BSO_GetByID((long)id);
                BSOSaveDataModel model = new BSOSaveDataModel(bso);
                model.Comment = null;
                model.StatusDate = DateTime.Now;
                return PartialView(model);
            }
            else
            {
                BSOSaveDataModel model = new BSOSaveDataModel();
                return PartialView(model);
            }
        }

        [HttpPost]
        public ActionResult Edit(BSOSaveDataModel model)
        {
            long? BSO_Id = model.Id;
            model.Validate(new ModelValidationContext() { currenUser = CurrentUser });
            model.IsSuccessfullySaved = model.IsValid();
            model.GetAvailableBSOStatus(model.StatusId, true);
            if (Request.IsAjaxRequest())
            {
                if (model.IsValid())
                {
                    model.UserId = CurrentUser.Id;
                    if (model.StatusId == (long)ListBSOStatusID.OnDelivery) model.FlagPrintReport = true;
                    bsoBusinessLogic.BSO_Save(model.GetBSOSaveData());
                    return PartialView("Edit", model);
                }
                else
                {
                    return PartialView("Edit", model);
                }
            }
            else
            {
                if (model.IsValid())
                {
                    model.UserId = CurrentUser.Id;
                    bsoBusinessLogic.BSO_Save(model.GetBSOSaveData());
                    return RedirectToAction("BSOHistory", "BSO", new { id = BSO_Id });
                }
                else
                {
                    return View("BSO");
                    //return RedirectToAction("BSOHistory", "BSO", new { id = BSO_Id });
                }
            }
        }

        [HttpGet]
        public ActionResult MassChanges(long? id, BSOListModel bsoListModel)
        {
            bsoListModel.SearchCriteriaModel.NewStatusDate = DateTime.Now;
            if (bsoListModel.SearchCriteriaModel.StatusId != null)
            {
                bsoListModel.GetAvailableBSOStatus(bsoListModel.SearchCriteriaModel.StatusId, true);
            }
            return PartialView(bsoListModel);
        }

        [HttpPost]
        public ActionResult MassChanges(BSOListModel bsoListModel)
        {
            long newStatus = (long)bsoListModel.SearchCriteriaModel.CurrentStatusId;
            long? newDelivery = bsoListModel.SearchCriteriaModel.NewDeliveryPointId;
            long? newResponsible = bsoListModel.SearchCriteriaModel.NewResponsibleID;
            DateTime? newStatusDate = bsoListModel.SearchCriteriaModel.NewStatusDate;

            long countPrintBso = 0;

            //Регистратор не может изменить 'Точку выдачи' и 'Ответственного'
            if (Role.Registrator.Id == Role.GetRealRole(CurrentUser).Id)
            {
                bsoListModel.SearchCriteriaModel.NewDeliveryPointId = CurrentUser.DeliveryPoint.Id;
                newDelivery = CurrentUser.DeliveryPoint.Id;
                bsoListModel.SearchCriteriaModel.NewResponsibleID = bsoListModel.SearchCriteriaModel.ResponsibleID;
                newResponsible = bsoListModel.SearchCriteriaModel.ResponsibleID;
                bsoListModel.SearchCriteriaModel.Messages.Add("В роли Регистратор, вы не можете изменить Ответственного или Точку Выдачи");
            }

            //Ответственный не может изменить 'Ответственного'
            if (Role.ResponsibleBSO.Id == Role.GetRealRole(CurrentUser).Id
                && (bsoListModel.SearchCriteriaModel.NewResponsibleID != bsoListModel.SearchCriteriaModel.ResponsibleID))
            {
                bsoListModel.SearchCriteriaModel.NewResponsibleID = bsoListModel.SearchCriteriaModel.ResponsibleID;
                newResponsible = bsoListModel.SearchCriteriaModel.ResponsibleID;
                bsoListModel.SearchCriteriaModel.Messages.Add("В роли Ответственного, вы не можете изменить 'Ответственного'");
            }

            BSOSearchCriteria oldCriteria = bsoListModel.SearchCriteriaModel.GetBSOSearchCriteria();
            bsoListModel.SearchCriteriaModel.Validate(new ModelValidationContext() { currenUser = CurrentUser });
            bsoListModel.SearchCriteriaModel.IsSuccessfullySaved = bsoListModel.SearchCriteriaModel.IsValid();
            if (Request.IsAjaxRequest())
            {
                if (bsoListModel.SearchCriteriaModel.IsValid())
                {
                    BSOSearchCriteria criteria = bsoListModel.SearchCriteriaModel.GetBSOSearchCriteria();
                    DataPage<BSOInfo> listBso = bsoBusinessLogic.BSO_Find(
                            criteria,
                            new List<SortCriteria<BSOSortField>>(),
                            new PageRequest() { PageNumber = bsoListModel.PageNumber, PageSize = int.MaxValue });

                    BSOSaveDataModel bsoSaveDataModel = new BSOSaveDataModel()
                    {
                        StatusId = newStatus,
                        ResponsibleID = newResponsible,
                        PolicyPartyNumber = bsoListModel.SearchCriteriaModel.PolicyPartyNumber,
                        DeliveryPointId = newDelivery,
                        UserId = CurrentUser.Id,
                        Comment = string.Format("Массовая смена статуса")
                    };

                    if (listBso.Count == 0) bsoListModel.SearchCriteriaModel.IsSuccessfullySaved = false;
                    foreach (var b in listBso)
                    {
                        //если не указана новая точка выдачи, то сохраняем прежную
                        //если указана - записываем новую
                        if (newDelivery == null)
                        {
                            bsoSaveDataModel.DeliveryPointId = b.DeliveryPointId;
                        }
                        else
                        {
                            bsoSaveDataModel.DeliveryPointId = newDelivery;
                        }

                        //если не указан новый ответственный, то сохраняем прежнего
                        //если указан - записываем нового
                        if (newResponsible == null)
                        {
                            bsoSaveDataModel.ResponsibleID = b.ResponsibleID;
                        }
                        else
                        {
                            bsoSaveDataModel.ResponsibleID = newResponsible;
                        }

                        //если не указана новая дата статуса, то сохраняем прежную
                        //если указан - записываем новую
                        if (newStatusDate == null)
                        {
                            bsoSaveDataModel.StatusDate = b.StatusDate;
                        }
                        else
                        {
                            bsoSaveDataModel.StatusDate = newStatusDate;
                        }

                        bsoSaveDataModel.Id = b.Id;
                        BSO.SaveData save = bsoSaveDataModel.GetBSOSaveData();
                        bsoBusinessLogic.BSO_Set(save);
                    }
                    countPrintBso = listBso.Count;
                }
                else
                {
                    //nothing
                }
            }
            else
            {
                return RedirectToAction("Index", "BSO");
            }

            DataPage<BSOInfo> bsos = bsoBusinessLogic.BSO_Find(
                    oldCriteria,
                    new List<SortCriteria<BSOSortField>>(),
                    new PageRequest() { PageNumber = bsoListModel.PageNumber, PageSize = bsoListModel.PageSize });
            BSOListModel model = new BSOListModel()
            {
                Items = bsos.Data.Select(item => new BSOBaseInfoModel(item)),
                SearchCriteriaModel = new BSOSearchCriteriaModel(oldCriteria),
                PageNumber = bsoListModel.PageNumber,
                PageSize = bsoListModel.PageSize,
                TotalCount = bsos.TotalCount
            };
            model.SearchCriteriaModel.Messages.AddRange(bsoListModel.SearchCriteriaModel.Messages);
            model.SearchCriteriaModel.CurrentStatusId = newStatus;
            model.SearchCriteriaModel.NewDeliveryPointId = newDelivery;
            model.SearchCriteriaModel.NewResponsibleID = newResponsible;
            model.SearchCriteriaModel.IsSuccessfullySaved = bsoListModel.SearchCriteriaModel.IsSuccessfullySaved;
            model.GetAvailableBSOStatus(bsoListModel.SearchCriteriaModel.StatusId, true);
            //печатаем отчет, если статус изменился: На точке
            if (model.SearchCriteriaModel.CurrentStatusId == (long)ListBSOStatusID.OnDelivery && model.SearchCriteriaModel.IsSuccessfullySaved.Value)
            {
                model.FlagPrintReport = countPrintBso;
            }
            else
            {
                model.FlagPrintReport = 0;
            }
            return PartialView("MassChanges", model);
        }

        [AuthorizeUser(Roles = "Administrator, AdministratorBSO")]
        public ActionResult CreateBSO()
        {
            BSOCreateModel model = new BSOCreateModel();
            return PartialView(model);
        }

        [HttpPost]
        [AuthorizeUser(Roles = "Administrator, AdministratorBSO")]
        public ActionResult CreateBSO(BSOCreateModel model)
        {
            long? BSO_Id = model.Id;
            if (model.TemporaryPolicyNumber == null)
            {
                model.FailMessages.Add(string.Format("Заполните поле: № БСО."));
                return View("CreateBSO", model);
            }

            if (model.TemporaryPolicyNumberTo == null)
            { //если вводим 1 БСО
                try
                {
                    long start = Int64.Parse(model.TemporaryPolicyNumber);
                    model.TemporaryPolicyNumber = string.Format("{0,9:D9}", start);
                }
                catch
                {
                    model.FailMessages.Add(string.Format("Невалидный номер БСО '{0}'. Введите правильное числовое значение.", model.TemporaryPolicyNumber));
                    return View(model);
                }

                BSO bso = bsoBusinessLogic.BSO_GetByNumber(model.TemporaryPolicyNumber);
                if (bso != null)
                {
                    model.FailMessages.Add(string.Format("Введенный номер БСО {0} - уже существует!", model.TemporaryPolicyNumber));
                    return View(model);
                }
                else
                {
                    model.UserId = CurrentUser.Id;
                    bsoBusinessLogic.BSO_Save(model.GetBSOSaveData());
                    bso = bsoBusinessLogic.BSO_GetByNumber(model.TemporaryPolicyNumber);
                    return RedirectToAction("BSOHistory", "BSO", new { id = bso.Id });
                }
            }
            else
            {//если вводим диапазон БСО.
                //тут быдлокод надо переписать, чтоб не шло столько лишних запросов к базе
                try
                {
                    long start = Int64.Parse(model.TemporaryPolicyNumber);
                    long end = Int64.Parse(model.TemporaryPolicyNumberTo);
                    long countTrue = 0;
                    long countFalse = 0;

                    for (long i = start; i <= end; ++i)
                    {
                        BSO bso = bsoBusinessLogic.BSO_GetByNumber(string.Format("{0,9:D9}", i));
                        if (bso != null)
                        {
                            countFalse++;
                        }
                        else
                        {
                            model.TemporaryPolicyNumber = string.Format("{0,9:D9}", i);
                            model.UserId = CurrentUser.Id;
                            bsoBusinessLogic.BSO_Save(model.GetBSOSaveData());
                            countTrue++;
                        }
                    }
                    model.GoodMessages.Add(string.Format("Из диапазона номеров: {0} - {1}", string.Format("{0,9:D9}", start), string.Format("{0,9:D9}", end)));
                    model.GoodMessages.Add(string.Format("добавлено новых: {0}", countTrue));
                    if (countFalse > 0) model.GoodMessages.Add(string.Format("обнаружено уже существующих: {0}", countFalse));
                    return View("CreateBSO", model);
                }
                catch
                {
                    model.FailMessages.Add(string.Format("Невалидный номер БСО: '{0}' или '{1}'. Введите правильные числовые значения.",
                                                            model.TemporaryPolicyNumber, model.TemporaryPolicyNumberTo));
                    return View("CreateBSO", model);
                }
            }
        }
    }
}