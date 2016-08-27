using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using OMInsurance.Entities.Check;
using OMInsurance.Entities.Searching;
using OMInsurance.WebApps.Models;
using OMInsurance.WebApps.Security;
using OMInsurance.Interfaces;
using OMInsurance.BusinessLogic;
using OMInsurance.WebApps.Validation;

namespace OMInsurance.WebApps.Controllers
{
    [AuthorizeUser]
    [AuthorizeUser(Roles = "Administrator, AdministratorBSO")]
    public class CheckController : OMInsuranceController
    {
        ICheckBusinessLogic checkBusinessLogic;
        IClientBusinessLogic clientBusinessLogic;

        public CheckController()
        {
            checkBusinessLogic = new CheckBusinessLogic();
            clientBusinessLogic = new ClientBusinessLogic();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BaseCheckClient()
        {
            BaseCheckClientModel model = new BaseCheckClientModel();
            List<CheckClient> listCheckClient = checkBusinessLogic.Check_Client(model.SearchCheckClient.GetCheckClientSearchCriteria());
            listCheckClient = listCheckClient.FindAll(a => !BaseCheckClientModel.ListHideClientId.Contains(a.Id));
            model.ListCheckClientModel = listCheckClient.GetRange(0, listCheckClient.Count < 100 ? listCheckClient.Count : 100).Select(item => new CheckClientModel(item)).ToList();
            model.CountDublicate = BaseCheckClientModel.SumDublicate(listCheckClient, model.SearchCheckClient);
            model.CountRow = listCheckClient.Count;
            model.SetHide();
            return View(model);
        }

        [HttpPost]
        public ActionResult BaseCheckClient(BaseCheckClientModel m)
        {
            BaseCheckClientModel model = new BaseCheckClientModel();
            List<CheckClient> listCheckClient = checkBusinessLogic.Check_Client(m.SearchCheckClient.GetCheckClientSearchCriteria());
            if (m.IsHideClientId)
            {
                listCheckClient = listCheckClient.FindAll(a => !BaseCheckClientModel.ListHideClientId.Contains(a.Id));
            }
            model.SearchCheckClient = m.SearchCheckClient;
            model.ListCheckClientModel = listCheckClient.Select(item => new CheckClientModel(item)).ToList();
            if (model.ListCheckClientModel.Count >= (int)m.PageSize)
            {
                model.ListCheckClientModel = model.ListCheckClientModel.GetRange(0, (int)m.PageSize);
            }
            model.CountDublicate = BaseCheckClientModel.SumDublicate(listCheckClient, model.SearchCheckClient);
            model.CountRow = listCheckClient.Count;
            model.ViewColumn = m.ViewColumn;
            model.PageSize = m.PageSize;
            model.SetHide();
            return View(model);
        }

        public ActionResult MassMergeClients(BaseCheckClientModel m)
        {
            BaseCheckClientModel model = new BaseCheckClientModel();
            model.SearchCheckClient = m.SearchCheckClient;
            List<CheckClient> listCheckClient = checkBusinessLogic.Check_Client(model.SearchCheckClient.GetCheckClientSearchCriteria());
            if (m.IsHideClientId)
            {
                listCheckClient = listCheckClient.FindAll(a => !BaseCheckClientModel.ListHideClientId.Contains(a.Id));
            }
            Dictionary<long, List<long>> dic = model.FindDuplicate(listCheckClient, m.SearchCheckClient, m.CountMerge);

            List<long> afterMergingListClient = new List<long>();
            foreach (var item in dic)
            {
                if (item.Value.Count() > 1)
                {
                    long resultClient = clientBusinessLogic.Clients_Merge(item.Key, item.Value.FirstOrDefault(), CurrentUser);
                    for (int i = 1; i < item.Value.Count(); i++)
                    {
                        resultClient = clientBusinessLogic.Clients_Merge(resultClient, item.Value[i], CurrentUser);
                    }
                    afterMergingListClient.Add(resultClient);
                }
                else if (item.Value.Count() == 1)
                {
                    long resultClient = clientBusinessLogic.Clients_Merge(item.Key, item.Value.FirstOrDefault(), CurrentUser);
                    afterMergingListClient.Add(resultClient);
                }
                else
                {   //Count=0
                    //nothing
                }
            }
            model.ListCheckClientModel = listCheckClient.Where(a => afterMergingListClient.Contains(a.Id)).ToList()
                .GetRange(0, afterMergingListClient.Count < 100 ? afterMergingListClient.Count : 100)
                .Select(item => new CheckClientModel(item))
                .ToList();
            model.CountMerge = m.CountMerge;
            model.SearchCheckClient = m.SearchCheckClient;
            model.CountRow = listCheckClient.Count;
            return View(model);
        }

        public ActionResult CheckHistoryFiles(BaseCheckFileHistoryModel model)
        {
            List<FundFileHistory> list = checkBusinessLogic.FundFileHistory_Find(model.Search.GetSearchCriteria()).OrderBy(a => a.Date).ToList();
            List<User> listUser = userBusinessLogic.Find("");
            List<ReferenceItem> listStatus = referenceBusinessLogic.GetReferencesList(Constants.FundFileHistoryStatusRef);

            var groupByDate = list.Select(item => new FundFileHistoryModel(item, listUser, listStatus)).GroupBy(a => a.Date);
            model.ListFundFileHistoryModel = groupByDate.Select(b => b.First()).ToList();

            for (int i = 0; i < groupByDate.Count(); i++)
            {
                model.ListFundFileHistoryModel.ElementAt(i).Count = groupByDate.ElementAt(i).Count();
            }

            if (model.ListFundFileHistoryModel.Count >= (int)model.PageSize)
            {
                model.ListFundFileHistoryModel = model.ListFundFileHistoryModel.GetRange(0, (int)model.PageSize);
            }
            return View(model);
        }

        public ActionResult CheckPretension(BaseCheckPretensionModel model)
        {
            model.Search.M_daktTo = model.Search.M_daktTo.HasValue ? ((DateTime)model.Search.M_daktTo).AddDays(1) : model.Search.M_daktTo;
            model.Search.CreateDateTo = model.Search.CreateDateTo.HasValue ? ((DateTime)model.Search.CreateDateTo).AddDays(1) : model.Search.CreateDateTo;
            List<ClientPretension> list = checkBusinessLogic.ClientPretension_Find(model.Search.GetSearchCriteria()).OrderBy(a => a.CreateDate).ToList();
            List<User> listUser = userBusinessLogic.Find("");
            model.ListClientPretensionModel = list.Select(item => new ClientPretensionModel(item)).ToList();
            if (model.ListClientPretensionModel.Count >= (int)model.PageSize)
            {
                model.ListClientPretensionModel = model.ListClientPretensionModel.GetRange(0, (int)model.PageSize);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult ClientId_SetIsHide(long id, bool isReady)
        {
            try
            {
                string message = string.Format("Обновлено пользователем {0}", CurrentUser.Login);
                if (isReady)
                {
                    BaseCheckClientModel.ListHideClientId.Add(id);
                    BaseCheckClientModel.ListHideClientId = BaseCheckClientModel.ListHideClientId.Distinct().ToList();
                }
                else
                {
                    BaseCheckClientModel.ListHideClientId.Remove(id);
                }
                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error", message = ex.Message });
            }
        }

        public ActionResult ResetCheckClient()
        {
            BaseCheckClientModel.ListHideClientId = new List<long>();
            return RedirectToAction("BaseCheckClient", FormMethod.Post);
        }
    }
}