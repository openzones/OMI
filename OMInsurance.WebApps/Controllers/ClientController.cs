using OMInsurance.BusinessLogic;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using OMInsurance.Interfaces;
using OMInsurance.WebApps.Models;
using OMInsurance.WebApps.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OMInsurance.PrintedForms;
using System.IO;
using OMInsurance.Configuration;
using System.Web;

namespace OMInsurance.WebApps.Controllers
{
    /// <summary>
    /// Controller provides methods to manage clients
    /// </summary>
    [AuthorizeUser]
    public class ClientController : OMInsuranceController
    {
        IClientBusinessLogic clientBusinessLogic;
        INomernikBusinessLogic nomernikBusinessLogic;
        public ClientController()
        {
            clientBusinessLogic = new ClientBusinessLogic();
            nomernikBusinessLogic = new NomernikBusinessLogic();
        }

        /// <summary>
        /// View a list of clients
        /// </summary>
        /// <returns>Returns default list of clients</returns>
        public ActionResult Index()
        {
            ClientListModel m = new ClientListModel();
            if (TempData["ClientVisitSaveDataModel"] != null)
            {
                m.SearchCriteriaModel = (ClientSearchCriteriaModel)TempData["ClientVisitSaveDataModel"];
                DataPage<ClientBaseInfo> clients = clientBusinessLogic.Client_Find(
                    m.SearchCriteriaModel.GetClientSearchCriteria(),
                    new List<SortCriteria<ClientSortField>>(),
                    new PageRequest() { PageNumber = 1, PageSize = 100 });
                m.Items = clients.Data
                    .Select(item => new ClientBaseInfoModel(item));
            }
            return Index(m);
        }

        /// <summary>
        /// View a list of specified clients
        /// </summary>
        /// <param name="clientListModel">List of clients</param>
        /// <returns>Returns specified list of clients</returns>
        [HttpPost]
        public ActionResult Index(ClientListModel clientListModel)
        {
            ClientSearchCriteria criteria = clientListModel.SearchCriteriaModel.GetClientSearchCriteria();
            DataPage<ClientBaseInfo> clients = clientBusinessLogic.Client_Find(
                    criteria,
                    new List<SortCriteria<ClientSortField>>(),
                    new PageRequest() { PageNumber = clientListModel.PageNumber, PageSize = clientListModel.PageSize });
            ClientListModel model = new ClientListModel()
            {
                Items = clients.Data
                    .Select(item => new ClientBaseInfoModel(item)),
                SearchCriteriaModel = new ClientSearchCriteriaModel(
                    criteria),
                PageNumber = clientListModel.PageNumber,
                PageSize = clientListModel.PageSize,
                TotalCount = clients.TotalCount
            };
            return View(model);
        }

        /// <summary>
        /// Get client details by identifier
        /// </summary>
        /// <param name="id">Client identifier</param>
        /// <returns>View of client details</returns>
        public ActionResult Details(int id)
        {
            FillReferences(Constants.ClientCategoryRef);
            FillReferences(Constants.CitizenshipRef);
            Client client = clientBusinessLogic.Client_Get(CurrentUser, id);

            ClientModel model = new ClientModel(client, CurrentUser);
            return View(model);
        }

        /// <summary>
        /// Get client version details by identifier
        /// </summary>
        /// <param name="id">Client version identifier</param>
        /// <returns>View of client version details</returns>
        public ActionResult ClientVersionDetails(long id)
        {
            FillReferences(Constants.ClientCategoryRef);
            FillReferences(Constants.CitizenshipRef);
            ClientVersionEditModel model = new ClientVersionEditModel(clientBusinessLogic.ClientVersion_Get(id), EntityType.General);
            return View("_ClientVersionDetails", model);
        }

        public ActionResult MergeConfirmation(long id, long DestinationClientId)
        {
            Client client = clientBusinessLogic.Client_Get(CurrentUser, DestinationClientId);
            if (client != null)
            {
                ViewBag.SourceId = id;
                ViewBag.DestinationClientId = DestinationClientId;
                return PartialView("_MergeConfirmation", new ClientModel(client, CurrentUser));
            }
            else
            {
                return PartialView("_ErrorMessage", string.Format("Клиент с идентификатором {0} не найден", DestinationClientId));
            }
        }

        public ActionResult Clients_Split(long id)
        {
            long resultClientId = clientBusinessLogic.Clients_Split(id);
            return RedirectToAction("Details", new { id = resultClientId });
        }

        [HttpPost]
        public ActionResult MergeClients(long id, long DestinationClientId)
        {
            long resultClient = clientBusinessLogic.Clients_Merge(id, DestinationClientId, CurrentUser);
            return RedirectToAction("Details", new { id = resultClient });
        }

        public ActionResult ClientNomernikNOMP(long id)
        {
            List<NomernikForClient> items = nomernikBusinessLogic.NomernikClientNOMP_Get(id);
            return PartialView("_ClientNomernikNOMP", items.Select(item => new ClientNomernikModel(item)));
        }

        public ActionResult ClientNomernikSTOP(long id)
        {
            List<NomernikForClient> items = nomernikBusinessLogic.NomernikClientSTOP_Get(id);
            return PartialView("_ClientNomernikSTOP", items.Select(item => new ClientNomernikModel(item)));
        }


        public ActionResult Pretension_RequestLPU(ClientPretensionModel model)
        {
            if (model.ClientId > 0 && model.Generator > 0)
            {
                List<ClientPretension> items = clientBusinessLogic.ClientPretension_Get(model.ClientId);
                ClientPretension pretension = items.Where(a => a.Generator == model.Generator).FirstOrDefault();
                if (pretension != null)
                {
                    Client client = clientBusinessLogic.Client_Get(CurrentUser, model.ClientId);
                    PretensionRequestLPU printedForm = new PretensionRequestLPU(pretension, client);
                    byte[] bytesExcel = printedForm.GetExcel();
                    return File(bytesExcel, System.Net.Mime.MediaTypeNames.Application.Octet, string.Format("Запрос в ЛПУ {0,4:D4}.xlsx", model.Generator));
                }
                else
                {
                    throw new Exception("Убедитесь, что претензия была верно сгенерирована и сохранена!");
                }

            }
            else
            {
                throw new FileNotFoundException("ClientId неизвестен или Generator не правильный. Файл невозможно сгенерировать.");
            }
        }

        public ActionResult Load_RequestLPU(FormCollection form, ClientPretensionModel model)
        {
            List<ClientPretension> items = clientBusinessLogic.ClientPretension_Get(model.ClientId);
            ClientPretension pretension = items.Where(a => a.Generator == model.Generator).FirstOrDefault();

            if (pretension == null)
            {
                return PartialView("_ErrorMessage", "Претензия не существует/не загружена.");
            }
            if (Request.Files == null || Request.Files.Count < 1)
            {
                return PartialView("_ErrorMessage", "Выберите файл для загрузки.");
            }

            HttpPostedFileBase upload = Request.Files[0];
            string extention = Path.GetExtension(upload.FileName);
            if (extention != ".pdf")
            {
                return PartialView("_ErrorMessage", "Выберите PDF-файл для загрузки");
            }

            string filename = string.Format("{0,4:D4}", pretension.Generator) + "_LPU_" + Guid.NewGuid().ToString() + ".pdf";
            string path = Path.Combine(ConfiguraionProvider.FileStorageFolder, Constants.Pretension, filename);
            upload.SaveAs(path);

            pretension.FileNameLPU = upload.FileName;
            pretension.FileUrlLPU = Path.Combine(Constants.Pretension, filename);

            clientBusinessLogic.ClientPretension_Save(pretension);
            return PartialView("_Message", "Файл успешно загружен. Желательно обновить страницу.");
        }

        public ActionResult Pretension_PackageLPU(ClientPretensionModel model)
        {
            List<ClientPretension> items = clientBusinessLogic.ClientPretension_Get(model.ClientId);
            ClientPretension pretension = items.Where(a => a.Generator == model.Generator).FirstOrDefault();
            if (pretension == null)
            {
                throw new Exception("Претензия не существует/не загружена.");
                //return PartialView("_ErrorMessage", "Претензия не существует/не загружена.");
            }
            if (string.IsNullOrEmpty(pretension.FileNameLPU) ||
                !System.IO.File.Exists(Path.Combine(ConfiguraionProvider.FileStorageFolder, pretension.FileUrlLPU)))
            {
                throw new Exception("Документ отсутствует. Загрузите файл pdf.");
                //return PartialView("_ErrorMessage", "Документ отсутствует. Загрузите файл pdf.");
            }
            pretension.sevenSimbolGeneration = "121" + string.Format("{0,4:D4}", pretension.Generator);
            byte[] fileBytes = clientBusinessLogic.PretensionGetLPU(pretension);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "d_mek_" + pretension.sevenSimbolGeneration + ".zip");
        }

        public ActionResult Load_Annulirovanie(FormCollection form, ClientPretensionModel model)
        {
            List<ClientPretension> items = clientBusinessLogic.ClientPretension_Get(model.ClientId);
            ClientPretension pretension = items.Where(a => a.Generator == model.Generator).FirstOrDefault();

            if (pretension == null)
            {
                return PartialView("_ErrorMessage", "Претензия не существует/не загружена.");
            }
            if (Request.Files == null || Request.Files.Count < 1)
            {
                return PartialView("_ErrorMessage", "Выберите файл для загрузки.");
            }

            HttpPostedFileBase upload = Request.Files[0];
            string extention = Path.GetExtension(upload.FileName);
            if (extention != ".pdf")
            {
                return PartialView("_ErrorMessage", "Выберите PDF-файл для загрузки");
            }

            string filename = string.Format("{0,4:D4}", pretension.Generator) + "_Annulirovanie_" + Guid.NewGuid().ToString() + ".pdf";
            string path = Path.Combine(ConfiguraionProvider.FileStorageFolder, Constants.Pretension, filename);
            upload.SaveAs(path);
            pretension.FileName2 = upload.FileName;
            pretension.FileUrl2 = Path.Combine(Constants.Pretension, filename);
            clientBusinessLogic.ClientPretension_Save(pretension);

            return PartialView("_Message", "Файл успешно загружен. Желательно обновить страницу.");
        }

        public ActionResult Pretension_PackageLast(ClientPretensionModel model)
        {
            List<ClientPretension> items = clientBusinessLogic.ClientPretension_Get(model.ClientId);
            ClientPretension pretension = items.Where(a => a.Generator == model.Generator).FirstOrDefault();
            Client client = clientBusinessLogic.Client_Get(CurrentUser, model.ClientId);
            if (client == null)
            {
                throw new Exception("Клиент не найден/не загужен.");
                //return PartialView("_ErrorMessage", "Претензия не существует/не загружена.");
            }
            if (pretension == null)
            {
                throw new Exception("Претензия не существует/не загружена.");
                //return PartialView("_ErrorMessage", "Претензия не существует/не загружена.");
            }
            if (string.IsNullOrEmpty(pretension.FileName2) ||
                !System.IO.File.Exists(Path.Combine(ConfiguraionProvider.FileStorageFolder, pretension.FileUrl2)))
            {
                throw new Exception("Документ отсутствует. Загрузите файл pdf.");
                //return PartialView("_ErrorMessage", "Документ отсутствует. Загрузите файл pdf.");
            }
            pretension.sevenSimbolGeneration = "121" + string.Format("{0,4:D4}", pretension.Generator);
            byte[] fileBytes = clientBusinessLogic.PretensionGetLast(pretension, client);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "d" + pretension.sevenSimbolGeneration + ".zip");
        }

        [HttpGet]
        public ActionResult Pretension(long clientId, long? gen)
        {
            if (gen.HasValue && gen > 0)
            {//загружаем претензию
                List<ClientPretension> items = clientBusinessLogic.ClientPretension_Get(clientId);
                ClientPretension pretension = items.Where(a => a.Generator == gen).FirstOrDefault();
                if (pretension != null)
                {
                    return View(new ClientPretensionModel(pretension));
                }
                else
                {
                    return View(new ClientPretensionModel() { ClientId = clientId });
                }
            }
            else
            {//генерируем новую
                ClientPretension pretension = clientBusinessLogic.ClientPretension_Generation(clientId);
                Client client = clientBusinessLogic.Client_Get(CurrentUser, clientId);
                if (pretension != null)
                {
                    ClientPretensionModel model = new ClientPretensionModel();
                    model.ClientPretensionGeneration(pretension, client, CurrentUser);
                    return View(model);
                }
                else
                {
                    return View(new ClientPretensionModel() { ClientId = clientId });
                }
            }
        }

        [HttpPost]
        public ActionResult Pretension(ClientPretensionModel model)
        {
            model.UpdateUserId = CurrentUser.Id;
            model.UpdateDate = DateTime.Now;
            model.UpdateUserFIO = CurrentUser.Fullname;
            clientBusinessLogic.ClientPretension_Save(model.GetClientPretension());
            return View(model);
        }

        public ActionResult PretensionGetFile(ClientPretensionModel model)
        {
            List<ClientPretension> items = clientBusinessLogic.ClientPretension_Get(model.ClientId);
            ClientPretension pretension = items.Where(a => a.Generator == model.Generator).FirstOrDefault();
            if (pretension == null)
            {
                throw new Exception("Претензия не существует/не загружена.");
            }
            model = new ClientPretensionModel(pretension);
            model.sevenSimbolGeneraton = "121" + string.Format("{0,4:D4}", model.Generator);
            if (model.sevenSimbolGeneraton.Length != 7) model.sevenSimbolGeneraton = "121ERROR";
            byte[] fileBytes = clientBusinessLogic.PretensionGetFile(model.GetClientPretension());

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, model.sevenSimbolGeneraton + ".zip");
        }



        public ActionResult ClientPretensions(long id)
        {
            List<ClientPretension> items = clientBusinessLogic.ClientPretension_Get(id);
            return PartialView("_ClientPretensions", items.Select(item => new ClientPretensionModel(item)));
        }
    }
}
