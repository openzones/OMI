using OMInsurance.BusinessLogic;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using OMInsurance.Entities.Searching;
using OMInsurance.Entities.Sorting;
using OMInsurance.Interfaces;
using OMInsurance.WebApps.Models;
using OMInsurance.WebApps.Security;
using OMInsurance.WebApps.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OMInsurance.WebApps.Controllers
{
    /// <summary>
    /// Controller provides methods to manage clients visits
    /// </summary>
    [AuthorizeUser]
    public class ClientVisitController : OMInsuranceController
    {
        IClientBusinessLogic clientBusinessLogic;
        IBSOBusinessLogic bsoBusinessLogic;

        public ClientVisitController()
        {
            clientBusinessLogic = new ClientBusinessLogic();
            userBusinessLogic = new UserBusinessLogic();
            referenceBusinessLogic = new ReferenceBusinessLogic();
            bsoBusinessLogic = new BSOBusinessLogic();
        }

        public ActionResult Index()
        {
            ClientVisitListModel model = new ClientVisitListModel(CurrentUser);

            if (TempData["ClientVisitSaveDataModel"] != null)
            {
                model.SearchCriteriaModel = (ClientVisitSearchCriteriaModel)TempData["ClientVisitSaveDataModel"];
                DataPage<ClientVisitInfo> clients = clientBusinessLogic.ClientVisit_Find(
                    model.SearchCriteriaModel.GetClientSearchCriteria(),
                    new List<SortCriteria<ClientVisitSortField>>(),
                    new PageRequest() { PageNumber = 1, PageSize = 100 });

                model.Items = clients.Data
                    .Select(item => new ClientVisitInfoModel(item));
            }
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

            ClientVisitListModel model = new ClientVisitListModel()
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

        public ActionResult Actuals()
        {
            ClientVisitListModel m = new ClientVisitListModel();
            m.SearchCriteriaModel.IsActualInVisitGroup = true;
            return Actuals(m);
        }

        [HttpPost]
        public ActionResult Actuals(ClientVisitListModel clientVisitModel)
        {
            ClientVisitSearchCriteria criteria = clientVisitModel.SearchCriteriaModel.GetClientSearchCriteria();
            criteria.IsActualInVisitGroup = true;
            DataPage<ClientVisitInfo> clients = clientBusinessLogic.ClientVisit_Find(
                    criteria,
                    new List<SortCriteria<ClientVisitSortField>>(),
                    new PageRequest() { PageNumber = clientVisitModel.PageNumber, PageSize = clientVisitModel.PageSize });

            ClientVisitListModel model = new ClientVisitListModel()
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

        public ActionResult Create()
        {
            ClientVisitSaveDataModel m = new ClientVisitSaveDataModel(CurrentUser);
            m.Registrator = new UserModel(CurrentUser);
            m.DeliveryCenterId = CurrentUser.Department.Id;
            m.DeliveryPointId = CurrentUser.DeliveryPoint.Id;
            m.Comment = "Сотрудник: " + CurrentUser.Fullname + Environment.NewLine;
            return View(m);
        }

        [HttpPost]
        public ActionResult Create(ClientVisitSaveDataModel model)
        {
            model.Comment = model.Comment + Environment.NewLine +
                " Место выдачи: " + model.DeliveryCenters.Where(a => a.Value == model.DeliveryCenterId.ToString()).Select(b => b.Text).FirstOrDefault() + Environment.NewLine +
                " Категория клиента: " + model.UralsibClientCategories.Where(a => a.Value == model.ClientCategoryId.ToString()).Select(b => b.Text).FirstOrDefault() + Environment.NewLine;
            model.Registrator = new UserModel(userBusinessLogic.User_GetByLogin(this.HttpContext.User.Identity.Name));
            model.Validate(new ModelValidationContext() { currenUser = CurrentUser });
            model.IsSuccessfullySaved = model.IsValid();
            if (model.IsValid())
            {
                ClientVisitSaveResult result = clientBusinessLogic.ClientVisit_Save(CurrentUser, model.GetClientVisitSaveData());
                model.VisitGroupId = result.VisitGroupId;
                model.VisitId = result.ClientVisitID;
                model.ClientId = result.ClientID;
                if (!string.IsNullOrEmpty(model.TemporaryPolicyNumber))
                {
                    BSOSaveDataModel bsoSaveDataModel = new BSOSaveDataModel(bsoBusinessLogic.BSO_GetByNumber(model.TemporaryPolicyNumber));
                    bsoBusinessLogic.BSO_Save((bsoSaveDataModel.UpdateBSOIssuedClient(model, CurrentUser)).GetBSOSaveData());
                }

                //return RedirectToAction("Details", "Client", new { id = result.ClientID });
                model.SetFlagPrintReport();
                return View(model);
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult Edit(long id)
        {
            ClientVisitSaveDataModel m = new ClientVisitSaveDataModel(CurrentUser, clientBusinessLogic.ClientVisit_Get(id));
            return PartialView("Edit", m);
        }

        [HttpPost]
        public ActionResult Edit(long? id, ClientVisitSaveDataModel model)
        {
            model.Validate(new ModelValidationContext() { currenUser = CurrentUser });
            model.IsSuccessfullySaved = model.IsValid();
            model.Registrator = new UserModel(userBusinessLogic.User_Get(model.Registrator.Id.Value));
            if (Request.IsAjaxRequest())
            {
                if (model.IsValid())
                {
                    ClientVisitSaveResult result = clientBusinessLogic.ClientVisit_Save(CurrentUser, model.GetClientVisitSaveData());
                    model.VisitGroupId = result.VisitGroupId;
                    model.ClientId = result.ClientID;
                    model.VisitId = result.ClientVisitID;
                    if (!string.IsNullOrEmpty(model.TemporaryPolicyNumber))
                    {
                        BSO bso = bsoBusinessLogic.BSO_GetByNumber(model.TemporaryPolicyNumber);
                        BSOSaveDataModel bsoSaveDataModel = new BSOSaveDataModel(bso);
                        bsoSaveDataModel.UpdateBSOIssuedClient(model, CurrentUser);
                        bsoSaveDataModel.Validate(new ModelValidationContext());
                        bsoSaveDataModel.IsSuccessfullySaved = model.IsValid();
                        if (bsoSaveDataModel.IsValid())
                        {
                            bsoBusinessLogic.BSO_Save(bsoSaveDataModel.GetBSOSaveData());
                        }
                    }
                    ClientVisitSaveDataModel m = new ClientVisitSaveDataModel(CurrentUser, clientBusinessLogic.ClientVisit_Get(model.VisitId.Value));
                    m.GetMessagesNotCritical(model);
                    m.IsSuccessfullySaved = true;
                    m.SetFlagPrintReport();
                    return PartialView("Edit", m);
                }
                else
                {
                    return PartialView("Edit", model);
                }
            }
            else
            {
                var t = model.IsValid();
                var tt = model.IsValidNotCritical();
                if (model.IsValid())
                {
                    ClientVisitSaveResult result = clientBusinessLogic.ClientVisit_Save(CurrentUser, model.GetClientVisitSaveData());
                    model.VisitGroupId = result.VisitGroupId;
                    model.ClientId = result.ClientID;
                    model.VisitId = result.ClientVisitID;
                    //TempData["ClientVisitSaveDataModel"] = new ClientVisitSearchCriteriaModel()
                    TempData["ClientVisitSaveDataModel"] = new ClientSearchCriteriaModel()
                    {
                        Firstname = model.NewClientInfo.Firstname,
                        Secondname = model.NewClientInfo.Secondname,
                        Lastname = model.NewClientInfo.Lastname,
                        Birthday = model.NewClientInfo.Birthday
                    };
                    if (!string.IsNullOrEmpty(model.TemporaryPolicyNumber))
                    {
                        BSO bso = bsoBusinessLogic.BSO_GetByNumber(model.TemporaryPolicyNumber);
                        if (bso != null)
                        {
                            BSOSaveDataModel bsoSaveDataModel = new BSOSaveDataModel(bso);
                            bsoSaveDataModel.UpdateBSOIssuedClient(model, CurrentUser);
                            bsoSaveDataModel.Validate(new ModelValidationContext());
                            bsoSaveDataModel.IsSuccessfullySaved = model.IsValid();
                            if (bsoSaveDataModel.IsValid())
                            {
                                bsoBusinessLogic.BSO_Save(bsoSaveDataModel.GetBSOSaveData());
                            }
                        }
                    }

                    //Если нет никаких ошибок вообще - перебрасываем в Клиенты с поиском этого клиента
                    if (model.IsValidNotCritical())
                    {
                        return RedirectToAction("Index", "Client");
                    }
                    else
                    {
                        return View("Details", model);
                    }
                    //return RedirectToAction("Index");
                }
                else
                {
                    return View("Details", model);
                }
            }
        }

        public ActionResult Details(long id)
        {
            ClientVisitSaveDataModel m = new ClientVisitSaveDataModel(CurrentUser, clientBusinessLogic.ClientVisit_Get(id));
            return View("Details", m);
        }

        public ActionResult AddToClient(long? id, long? CopyVisitId, bool? newGroup)
        {
            ClientVisitSaveDataModel model = new ClientVisitSaveDataModel(CurrentUser, id);
            if (CopyVisitId.HasValue && CopyVisitId.Value != 0)
            {
                ClientVisit clientVisit = clientBusinessLogic.ClientVisit_Get(CopyVisitId.Value);
                model = new ClientVisitSaveDataModel(CurrentUser, clientVisit, true);
                model.ClearIds();
                if (newGroup.HasValue && !newGroup.Value)
                {
                    model.VisitGroupId = clientVisit.VisitGroupId;
                }
                else
                {
                    model.TemporaryPolicyDate = null;
                    model.IssueDate = null;
                    model.TemporaryPolicyNumber = null;
                }
            }
            model.DeliveryCenterId = CurrentUser.Department.Id;
            model.DeliveryPointId = CurrentUser.DeliveryPoint.Id;

            model.Registrator = new UserModel(userBusinessLogic.User_GetByLogin(this.HttpContext.User.Identity.Name));
            model.Messages.Clear();
            model.MessagesNotCritical.Clear();
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult AddToClient(ClientVisitSaveDataModel model)
        {
            long clientId = model.ClientId.Value;
            model.Validate(new ModelValidationContext() { currenUser = CurrentUser });
            model.IsSuccessfullySaved = model.IsValid();
            if (!model.IsValid())
            {
                model.TemporaryPolicyNumber = string.Empty;
                return View("Details", model);
            }
            else
            {
                ClientVisitSaveResult result = clientBusinessLogic.ClientVisit_Save(CurrentUser, model.GetClientVisitSaveData());
                model.VisitGroupId = result.VisitGroupId;
                model.ClientId = result.ClientID;
                model.VisitId = result.ClientVisitID;
                if (!string.IsNullOrEmpty(model.TemporaryPolicyNumber))
                {
                    BSOSaveDataModel bsoSaveDataModel = new BSOSaveDataModel(bsoBusinessLogic.BSO_GetByNumber(model.TemporaryPolicyNumber));
                    bsoBusinessLogic.BSO_Save((bsoSaveDataModel.UpdateBSOIssuedClient(model, CurrentUser)).GetBSOSaveData());
                }
                model.SetFlagPrintReport();
                //return View("Create", model);
                return View("Details", model);

                //нет
                //return RedirectToAction("Details", "Client", new { id = clientId });
            }
        }

        /// <summary>
        /// Returns client visit group status history
        /// </summary>
        /// <param name="id">ClientVisit group id</param>
        /// <returns></returns>
        public ActionResult ClienVisitGroupHistory(long id)
        {
            List<ClientVisitHistoryItem> items = clientBusinessLogic.ClientVisitHistory_Get(id);
            return PartialView("_ClientVisitHistoryGrid", items.Select(item => new ClientVisitHistoryItemModel(item)));
        }

        /// <summary>
        /// Проверка БСО, чтоб не могли ввести 2 разных БСО в одно обращение
        /// </summary>
        /// <param name="id"></param>
        /// <param name="visitGroupId"></param>
        /// <param name="bso"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CheckBSOinVisitGroup(long id, long visitGroupId, string bso)
        {
            if (!string.IsNullOrEmpty(bso))
            {
                bso = bso.Trim();
            }
            else
            {//если поле было просто очищено - то не реагируем
                return Json(new { result = "OK", message = "bso было очищено" });
            }
            try
            {
                Client client = clientBusinessLogic.Client_Get( new Entities.User { Roles = { Entities.Core.Role.Administrator } }, id);
                if(client == null) return Json(new { result = "OK", message = "client is null" });
                
                ClientVisitInfo VG = client.Visits.Where(a => a.VisitGroupId == visitGroupId).Where(b=> !string.IsNullOrEmpty(b.TemporaryPolicyNumber)).FirstOrDefault();
                if (VG == null || string.IsNullOrEmpty(VG.TemporaryPolicyNumber)) return Json(new { result = "OK", message = " vg bso is null" });
                
                BSO BSOinVG = bsoBusinessLogic.BSO_GetByNumber(VG.TemporaryPolicyNumber);
                if (BSOinVG == null) return Json(new { result = "OK", message = "bso is null" });

                if (BSOinVG.TemporaryPolicyNumber == bso)
                {
                    return Json(new { result = "OK", message = "Все Ок" });
                }
                else
                {
                    if (BSOinVG.Status.Id == (long)ListBSOStatusID.OnClient)
                    {
                        return Json(new
                        {
                            result = "Error",
                            message = "ВНИМАНИЕ! На данное обращение уже выдан БСО №" + BSOinVG.TemporaryPolicyNumber + "." + Environment.NewLine +
                                      "При необходимости создайте новое обращение. Если бланк испорчен, отметьте это."
                        });
                    }

                    //Если, например, бланк испорчен, то позволять выписать еще один БСО.
                    if (BSOinVG.Status.Id == (long)ListBSOStatusID.FailGotoStorage ||
                        BSOinVG.Status.Id == (long)ListBSOStatusID.FailOnResponsible ||
                        BSOinVG.Status.Id == (long)ListBSOStatusID.FailOnStorage)
                    {
                        return Json(new { result = "OK", message = "Все Ок" });
                    }
                }

                return Json(new { result = "OK", message = "Все Ок" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        public ActionResult ChangeDeliveryPoint(string id)
        {
            List<ReferenceUniversalItem> listDeliveryPoint = referenceBusinessLogic.GetUniversalList(Constants.DeliveryPointRef);
            long DeliveryPointId;
            if ((!long.TryParse(id,out DeliveryPointId)) || id == "1")
            {
                return Json(new { result = "OK", message = "" });
            }

            try
            {
                long? deliveryCenter = listDeliveryPoint.Where(a => a.Id == DeliveryPointId).Select(b => b.DeliveryCenterId).FirstOrDefault();
                if (deliveryCenter.HasValue)
                {
                    return Json(new { result = "OK", message = deliveryCenter });
                }
                else
                {
                    return Json(new { result = "Error", message = "Error" });
                }
                
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }
    }
}