using OMInsurance.BusinessLogic;
using OMInsurance.Entities;
using OMInsurance.Entities.Check;
using OMInsurance.Entities.Core;
using OMInsurance.Entities.SMS;
using OMInsurance.Entities.Searching;
using OMInsurance.Entities.Sorting;
using OMInsurance.Interfaces;
using OMInsurance.PrintedForms;
using OMInsurance.WebApps.Models;
using OMInsurance.WebApps.Models.PrintedForms;
using OMInsurance.WebApps.Security;
using System.Collections.Generic;
using System.Web.Mvc;
using System;
using System.Linq;

namespace OMInsurance.WebApps.Controllers
{
    [AuthorizeUser]
    public class PrintedFormsController : OMInsuranceController
    {
        IClientBusinessLogic clientBusinessLogic;
        IBSOBusinessLogic bsoBusinessLogic;
        ISmsBusinessLogic smsBusinessLogic;
        IReferenceBusinessLogic referenceBusinessLogic;
        ICheckBusinessLogic checkBusinessLogic;
        public PrintedFormsController()
        {
            clientBusinessLogic = new ClientBusinessLogic();
            bsoBusinessLogic = new BSOBusinessLogic();
            smsBusinessLogic = new SmsBusinessLogic();
            referenceBusinessLogic = new ReferenceBusinessLogic();
            checkBusinessLogic = new CheckBusinessLogic();
        }

        public ActionResult Index(string m)
        {
            PrintedFormsModel model = new PrintedFormsModel();
            model.Message = m;
            var realRole = Role.GetRealRole(CurrentUser);
            if (!(realRole == Role.Administrator || realRole == Role.AdministratorBSO))
            {
                if (realRole == Role.ResponsibleBSO)
                {
                    model.BSOReportForm10Full.DeliveryPoints = ReferencesProvider.GetDeliveryPointsForResponsibleBSO(CurrentUser.Id, false);
                    model.BSOReportForm10.DeliveryPoints = ReferencesProvider.GetDeliveryPointsForResponsibleBSO(CurrentUser.Id, false);
                    model.SNILSReport.DeliveryPoints = ReferencesProvider.GetDeliveryPointsForResponsibleBSO(CurrentUser.Id, false);
                }
                else
                {
                    model.BSOReportForm10Full.DeliveryPointIds = new List<long>() { CurrentUser.DeliveryPoint.Id };
                    model.BSOReportForm10.DeliveryPointIds = new List<long>() { CurrentUser.DeliveryPoint.Id };
                    model.SNILSReport.DeliveryPointIds = new List<long>() { CurrentUser.DeliveryPoint.Id };
                }
            }

            return View(model);
        }

        public ActionResult ChangeInsuranceCompanyApplication(int id)
        {
            ClientVisit visit = clientBusinessLogic.ClientVisit_Get(id);
            ChangeInsuranceCompanyApplication printedForm = new ChangeInsuranceCompanyApplication(visit);
            return File(printedForm.GetExcel(),
                System.Net.Mime.MediaTypeNames.Application.Octet, "Заявление о выборе/замене страховой компании.xls");
        }
        public ActionResult DuplicateApplication(int id)
        {
            ClientVisit visit = clientBusinessLogic.ClientVisit_Get(id);
            DuplicateApplication printedForm = new DuplicateApplication(visit);
            return File(printedForm.GetExcel(),
                System.Net.Mime.MediaTypeNames.Application.Octet, "Заявление на дубликат.xls");
        }

        public ActionResult ClientVisitReport(ClientVisitReportModel model)
        {
            ClientVisitSearchCriteria criteria = new ClientVisitSearchCriteria();
            criteria.TemporaryPolicyDateFrom = model.CVDateFrom;
            criteria.TemporaryPolicyDateTo = (model.CVDateTo);
            //criteria.IsActualInVisitGroup = true;
            List<ClientVisitInfo> clientVisits = clientBusinessLogic.ClientVisit_Find(
                criteria,
                new List<SortCriteria<ClientVisitSortField>>(),
                new PageRequest() { PageNumber = 1, PageSize = int.MaxValue }).Data;
            List<Entities.User> listUser = userBusinessLogic.Find("");
            ClientVisitReport printedForm = new ClientVisitReport(clientVisits, model.CVDateFrom, model.CVDateTo, listUser);
            return File(printedForm.GetExcel(),
                System.Net.Mime.MediaTypeNames.Application.Octet, "Сводный отчет по обращениям.xls");
        }

        public ActionResult ScenarioForm2(ScenarioForm2Model model)
        {
            ClientVisitSearchCriteria criteria = new ClientVisitSearchCriteria();
            criteria.StatusDateFrom = new DateTime(model.Year, model.MonthID, 1);
            criteria.StetusDateTo = new DateTime(model.Year, model.MonthID + 1, 1).AddDays(-1);
            List<ClientVisitInfo> clientVisits = clientBusinessLogic.ClientVisit_Find(
                        criteria,
                        new List<SortCriteria<ClientVisitSortField>>(),
                        new PageRequest() { PageNumber = 1, PageSize = int.MaxValue }).Data;
            
            List<int> listInt = new List<int>(6);

            //Trim Code!
            foreach (var item in clientVisits)
            {
                if (!string.IsNullOrEmpty(item.Scenario.Code))
                {
                    item.Scenario.Code = item.Scenario.Code.Trim();
                }
            }

            var VisitGroup = clientVisits.GroupBy(a => a.VisitGroupId);
            //Количество обращений считается, как количество обращений по сценарию из самой последней заявки предшествующей POK.
            Dictionary<long, string> clearVisitGroup = new Dictionary<long, string>();
            string scenario = string.Empty;
            foreach (var item in VisitGroup)
            {
                if (item.Count() <= 1)
                {
                    clearVisitGroup.Add(item.Key, item.FirstOrDefault().Scenario.Code);
                }
                else //>1
                {
                    var orderItem = item.OrderBy(a => a.Id);
                    scenario = orderItem.FirstOrDefault().Scenario.Code;
                    foreach (ClientVisitInfo element in orderItem)
                    {
                        var s = element.Scenario.Code;

                        if (s == "POK")
                        {
                            break;
                        }
                        else
                        {
                            scenario = s;
                        }
                    }
                    clearVisitGroup.Add(item.Key, scenario);
                }
            }
            //В clearVisitGroup мы получили обращение и код для подсчета количества обращений
            int Visit1 = 0, //кол-во сценариев NB, PI, PRI, PT, PRT, CP
                Visit2 = 0, //CI, CT, RI, RT
                Visit3 = 0, //DP, CR
                otherVisit = 0;
            foreach (var item in clearVisitGroup)
            {
                if (item.Value == "NB" || item.Value == "PI" || item.Value == "PRI" || item.Value == "PT" || item.Value == "PRT" || item.Value == "CP")
                {
                    Visit1++;
                }
                else if (item.Value == "CI" || item.Value == "CT" || item.Value == "RI" || item.Value == "RT")
                {
                    Visit2++;
                }
                else if (item.Value == "DP" || item.Value == "CR")
                {
                    Visit3++;
                }
                else
                {
                    otherVisit++;
                }
            }
            listInt.Add(Visit1); listInt.Add(Visit2); listInt.Add(Visit3);


            int issued1 = 0, issued2 = 0, issued3 = 0, otherIssued = 0;
            foreach (var item in VisitGroup)
            {
                var temp = item.Where(a => a.Scenario.Code == "POK" ||
                                           a.Scenario.Code == "CI" ||
                                           (a.Scenario.Code == "CT" && a.Status.Id == 7 && string.IsNullOrEmpty(a.TemporaryPolicyNumber))).Count();
                if (temp > 0)
                {
                    Client client = clientBusinessLogic.Client_Get(CurrentUser, item.FirstOrDefault().ClientId);
                    List<string> listScn = new List<string>();
                    foreach (var visit in client.Visits.OrderBy(a => a.StatusDate))
                    {
                        listScn.Add(string.IsNullOrEmpty(visit.Scenario.Code) ? string.Empty : visit.Scenario.Code.Trim());
                    }

                    listScn.Remove("POK"); listScn.Remove("CI"); listScn.Remove("CT");
                    var code = listScn.LastOrDefault();
                    if (code == "NB" || code == "PI" || code == "PRI" || code == "PT" || code == "PRT" || code == "CP")
                    {
                        issued1++;
                    }
                    else if (code == "CI" || code == "CT" || code == "RI" || code == "RT")
                    {
                        issued2++;
                    }
                    else if (code == "DP" || code == "CR")
                    {
                        issued3++;
                    }
                    else
                    {
                        otherIssued++;
                    }
                }
            }
            listInt.Add(issued1); listInt.Add(issued2); listInt.Add(issued3);

            listInt.Add(otherVisit);
            listInt.Add(otherIssued);

            ScenarioForm2 printedForm = new ScenarioForm2(listInt);
            string month = model.Months.Where(a => a.Value == model.MonthID.ToString()).Select(b => b.Text).FirstOrDefault();
            return File(printedForm.GetExcel(),
                System.Net.Mime.MediaTypeNames.Application.Octet, string.Format("Отчет по сценариям форма_2 {0} {1}.xls", month, model.Year)); ;
        }

        public ActionResult TemporaryPolicyBSO(int id)
        {
            ClientVisit visit = clientBusinessLogic.ClientVisit_Get(id);
            TemporaryPolicyBSO printedForm = new TemporaryPolicyBSO(visit);
            return File(printedForm.GetExcel(),
                System.Net.Mime.MediaTypeNames.Application.Octet, "БСО.xls");
        }

        public ActionResult BSOMoveReportYear(BSOMoveReportYearModel model)
        {
            List<ReferenceItem> listDeliveryCenter = referenceBusinessLogic.GetReferencesList(Constants.DeliveryCenterRef);
            List<DeliveryCenter> list = referenceBusinessLogic.GetDeliveryCenterList();
            List<BSOHistoryItem> listBsoHistory = bsoBusinessLogic.BSO_GetListHistory(new DateTime(2010, 1, 1), new DateTime(model.Year + 1, 1, 1), null);
            BSOMoveReportYear printedForm = new BSOMoveReportYear(listBsoHistory, listDeliveryCenter, model.Year);
            return File(printedForm.GetExcel(),
                System.Net.Mime.MediaTypeNames.Application.Octet, string.Format("Отчет по движению бланков за {0} год.xls", model.Year));
        }

        public ActionResult AllocationBSO(AllocationBSOModel model)
        {
            ClientVisitSearchCriteria criteria = new ClientVisitSearchCriteria();
            criteria.PartyNumber = model.PartyNumber;
            criteria.DeliveryCenterIds = model.DeliveryCenterIds;// new List<long>();
            criteria.DeliveryPointIds = model.DeliveryPointIds;//new List<long>();
            criteria.IsActualInVisitGroup = true;
            criteria.TemporaryPolicyDateFrom = model.AllocationBSODateFrom;
            criteria.TemporaryPolicyDateTo = (model.AllocationBSODateTo);
            List<ClientVisitInfo> clientVisits = clientBusinessLogic.ClientVisit_Find(
                criteria,
                new List<SortCriteria<ClientVisitSortField>>(),
                new PageRequest() { PageNumber = 1, PageSize = int.MaxValue }).Data;
            
            if (clientVisits.Count > 65500)
            {
                return RedirectToAction("Index", "PrintedForms",
                    new { m = string.Format("Получено слишком много записей ({0}) в отчете по распределению БСО. Сузьте диапазон выбора!", clientVisits.Count) });
            }
            AllocationBSO printedForm = new AllocationBSO(clientVisits);
            return File(printedForm.GetExcel(),
                System.Net.Mime.MediaTypeNames.Application.Octet, string.Format("Отчет по распределению БСО_{0}.xlsx", DateTime.Now.ToShortDateString()));
        }

        public ActionResult StatusReport(StatusReportModel model)
        {
            ClientVisitSearchCriteria criteria = new ClientVisitSearchCriteria();
            criteria.StatusIds = model.StatusIds;
            criteria.IsActualInVisitGroup = true;
            criteria.StatusDateFrom = model.StatusDateFrom;
            criteria.StetusDateTo = (model.StatusDateTo);
            //criteria.Lastname = "Иванов1";
            //criteria.Firstname = "Иван";
            //criteria.Secondname = "Иванович";
            List<ClientVisitInfo> clientVisits = clientBusinessLogic.ClientVisit_Find(
                criteria,
                new List<SortCriteria<ClientVisitSortField>>(),
                new PageRequest() { PageNumber = 1, PageSize = int.MaxValue }).Data;
            var Statuses = ReferencesProvider.GetReferenceItems(Constants.ClientVisitStatusRef);
            List<string> listStatus = new List<string>();
            listStatus = Statuses.Where(item => (model.StatusIds).Contains(item.Id)).Select(a => a.Name).ToList();
            StatusReport printedForm = new StatusReport(clientVisits, model.StatusDateFrom, model.StatusDateTo, listStatus);
            return File(printedForm.GetExcel(),
                System.Net.Mime.MediaTypeNames.Application.Octet, string.Format("Отчет по статусам {0}.xlsx", DateTime.Now.ToShortDateString()));
        }

        public ActionResult PolicyExtraitionSignatureList(PartyJournalModel model)
        {
            if (string.IsNullOrEmpty(model.PartyNumber))
            {
                return new EmptyResult();
            }
            ClientVisitSearchCriteria criteria = new ClientVisitSearchCriteria();
            criteria.PartyNumber = model.PartyNumber;
            criteria.DeliveryCenterIds = new List<long>();
            if (model.DeliveryCenterId.HasValue)
            {
                criteria.DeliveryCenterIds.Add(model.DeliveryCenterId.Value);
            }
            criteria.DeliveryPointIds = new List<long>();
            if (model.DeliveryPointId.HasValue)
            {
                criteria.DeliveryPointIds.Add(model.DeliveryPointId.Value);
            }
            criteria.IsActualInVisitGroup = true;
            List<ClientVisitInfo> clientVisits = clientBusinessLogic.ClientVisit_Find(
                criteria,
                new List<SortCriteria<ClientVisitSortField>>(),
                new PageRequest() { PageNumber = 1, PageSize = int.MaxValue }).Data;
            PolicyExtraitionSignatureList printedForm = new PolicyExtraitionSignatureList(clientVisits);
            return File(printedForm.GetExcel(),
                System.Net.Mime.MediaTypeNames.Application.Octet, string.Format("Журнал_{0}.xls", model.PartyNumber));
        }

        public ActionResult GetBSOFailForm13(BSOFailForm13Model model)
        {
            BSOListModel bsoListModel = new BSOListModel();
            bsoListModel.SearchCriteriaModel.StatusDateFrom = model.DateFrom;
            bsoListModel.SearchCriteriaModel.StatusDateTo = model.DateTo;
            bsoListModel.SearchCriteriaModel.DeliveryCenterIds = new List<long>() { CurrentUser.Department.Id };
            bsoListModel.SearchCriteriaModel.StatusId = (long)ListBSOStatusID.FailOnResponsible;
            BSOSearchCriteria criteria = bsoListModel.SearchCriteriaModel.GetBSOSearchCriteria();
            List<BSOInfo> bsos = bsoBusinessLogic.BSO_Find(
                    criteria,
                    new List<SortCriteria<BSOSortField>>(),
                    new PageRequest() { PageNumber = 1, PageSize = int.MaxValue }).Data;

            criteria.StatusId = (long)ListBSOStatusID.FailGotoStorage;
            bsos.AddRange(bsoBusinessLogic.BSO_Find(
                    criteria,
                    new List<SortCriteria<BSOSortField>>(),
                    new PageRequest() { PageNumber = 1, PageSize = int.MaxValue }).Data);

            criteria.StatusId = (long)ListBSOStatusID.FailOnStorage;
            bsos.AddRange(bsoBusinessLogic.BSO_Find(
                    criteria,
                    new List<SortCriteria<BSOSortField>>(),
                    new PageRequest() { PageNumber = 1, PageSize = int.MaxValue }).Data);

            BSOFailForm13 printedForm = new BSOFailForm13(bsos, model.DateFrom, model.DateTo, CurrentUser.DepartmentDisplayName, CurrentUser.Fullname);
            return File(printedForm.GetExcel(),
                System.Net.Mime.MediaTypeNames.Application.Octet, string.Format("Акт передачи испорченных бланков БСО (временных свидетельств). Форма №13.xls"));
        }

        public ActionResult GetBSOInvoice(BSOSaveDataModel model)
        {
            List<ReferenceUniversalItem> DP = referenceBusinessLogic.GetUniversalList(Constants.DeliveryPointRef);
            string deliveryPointName = DP.Where(m => m.Id == model.DeliveryPointId).FirstOrDefault().Name;
            long? deliveryPointHeadId = DP.Where(m => m.Id == model.DeliveryPointId).FirstOrDefault().DeliveryPointHeadId;
            List<SelectListItem> Users = ReferencesProvider.GetUsers();
            string fio;
            if (deliveryPointHeadId != null)
            {
                fio = Users.Where(a => a.Value == deliveryPointHeadId.ToString()).Select(b => b.Text).FirstOrDefault();
            }
            else
            {
                fio = CurrentUser.Fullname;
            }

            List<string> listBsoNumber = new List<string>();
            listBsoNumber.Add(model.TemporaryPolicyNumber);
            BSOInvoice printedForm = new BSOInvoice(listBsoNumber, fio, deliveryPointName);
            return File(printedForm.GetExcel(),
                System.Net.Mime.MediaTypeNames.Application.Octet, string.Format("Накладная БСО (ВС).xls"));
        }

        public ActionResult GetBSOInvoiceS(BSOListModel model)
        {
            List<ReferenceUniversalItem> DP = referenceBusinessLogic.GetUniversalList(Constants.DeliveryPointRef);
            string deliveryPointName = DP.Where(m => m.Id == model.SearchCriteriaModel.NewDeliveryPointId).FirstOrDefault().Name;
            long? deliveryPointHeadId = DP.Where(m => m.Id == model.SearchCriteriaModel.NewDeliveryPointId).FirstOrDefault().DeliveryPointHeadId;
            List<SelectListItem> Users = ReferencesProvider.GetUsers();
            string fio;
            if (deliveryPointHeadId != null)
            {
                fio = Users.Where(a => a.Value == deliveryPointHeadId.ToString()).Select(b => b.Text).FirstOrDefault();
            }
            else
            {
                fio = CurrentUser.Fullname;
            }

            model.SearchCriteriaModel.StatusId = model.SearchCriteriaModel.CurrentStatusId;
            model.SearchCriteriaModel.DeliveryPointIds = new List<long>();
            model.SearchCriteriaModel.DeliveryPointIds.Add(model.SearchCriteriaModel.NewDeliveryPointId.Value);
            BSOSearchCriteria criteria = model.SearchCriteriaModel.GetBSOSearchCriteria();
            List<BSOInfo> bsos = bsoBusinessLogic.BSO_Find(
               criteria,
               new List<SortCriteria<BSOSortField>>(),
               new PageRequest() { PageNumber = 1, PageSize = int.MaxValue }).Data;
            List<string> listBsoNumber = new List<string>();
            foreach (var elem in bsos)
            {
                listBsoNumber.Add(elem.TemporaryPolicyNumber);
            }
            BSOInvoice printedForm = new BSOInvoice(listBsoNumber, fio, deliveryPointName);
            return File(printedForm.GetExcel(),
                System.Net.Mime.MediaTypeNames.Application.Octet, string.Format("Накладная БСО (ВС).xls"));
        }

        public ActionResult GetBSOOperativeInformation(BSOOperativeInformationModel model)
        {
            if (model.Date == null) model.Date = DateTime.Now;
            DateTime tempDate = model.Date.AddDays(1);
            List<BSOSumStatus> list = bsoBusinessLogic.BSO_GetSumAllStatus(tempDate);
            BSOOperativeInformation printedForm = new BSOOperativeInformation(model.Date, list);
            return File(printedForm.GetExcel(),
                System.Net.Mime.MediaTypeNames.Application.Octet, string.Format("Оперативная информация о расходовании БСО(ВС).xls"));
        }

        public ActionResult SNILSReport(SNILSReportModel model)
        {
            string NameDeliveryPoint = string.Empty;
            ClientVisitListModel clientVisitModel = new ClientVisitListModel();
            clientVisitModel.SearchCriteriaModel.StatusDateFrom = model.DateSnilsFrom;
            clientVisitModel.SearchCriteriaModel.StatusDateTo = model.DateSnilsTo;

            //для Администраторов ограничений по точкам - нет
            if (Role.Administrator.Id == Role.GetRealRole(CurrentUser).Id || Role.AdministratorBSO.Id == Role.GetRealRole(CurrentUser).Id)
            {
                if (model.DeliveryPointIds.Count > 0)
                {
                    clientVisitModel.SearchCriteriaModel.DeliveryPointIds = model.DeliveryPointIds;
                    foreach (var item in model.DeliveryPointIds)
                    {
                        NameDeliveryPoint = NameDeliveryPoint + ", " + model.DeliveryPoints.Where(a => a.Value == item.ToString()).Select(b => b.Text).FirstOrDefault();
                    }
                    NameDeliveryPoint = NameDeliveryPoint.Trim(',', ' ');
                }
                else
                {
                    NameDeliveryPoint = "Все точки";
                }
            }
            else
            {   //для ответственного
                if (Role.ResponsibleBSO == Role.GetRealRole(CurrentUser))
                {
                    if (model.DeliveryPointIds.Count > 0)
                    {
                        clientVisitModel.SearchCriteriaModel.DeliveryPointIds = model.DeliveryPointIds;
                    }
                    else
                    {//все точки для ответственного
                        model.DeliveryPoints = ReferencesProvider.GetDeliveryPointsForResponsibleBSO(CurrentUser.Id, false);
                        foreach (var item in model.DeliveryPoints)
                        {
                            if (item.Value != "")
                            {
                                model.DeliveryPointIds.Add(long.Parse(item.Value));
                            }
                        }
                        clientVisitModel.SearchCriteriaModel.DeliveryPointIds = model.DeliveryPointIds;
                    }

                    foreach (var item in model.DeliveryPointIds)
                    {
                        NameDeliveryPoint = NameDeliveryPoint + ", " + model.DeliveryPoints.Where(a => a.Value == item.ToString()).Select(b => b.Text).FirstOrDefault();
                    }
                    NameDeliveryPoint = NameDeliveryPoint.Trim(',', ' ');
                }
                else
                {
                    //для всех остальных, только по их собственной точке
                    clientVisitModel.SearchCriteriaModel.DeliveryPointIds = new List<long>() { CurrentUser.DeliveryPoint.Id };
                    model.DeliveryPointIds = new List<long>() { CurrentUser.DeliveryPoint.Id };
                    NameDeliveryPoint = model.DeliveryPoints.Where(a => a.Value == model.DeliveryPointIds.FirstOrDefault().ToString()).Select(b => b.Text).FirstOrDefault();
                }
            }

            ClientVisitSearchCriteria criteria = clientVisitModel.SearchCriteriaModel.GetClientSearchCriteria();
            criteria.IsActualInVisitGroup = true;
            List<ClientVisitInfo> clients = clientBusinessLogic.ClientVisit_Find(
                    criteria,
                    new List<SortCriteria<ClientVisitSortField>>(),
                    new PageRequest() { PageNumber = 1, PageSize = int.MaxValue }).Data;

            //Отсекаем только гр. РФ
            clients = clients.Where(a => a.Citizenship == "РОССИЯ").ToList();

            // Все статусы кроме: "ошибочная запись код 6", "выдан и отправлен в фонд код 11", "изготовлен другой компанией код 7", "перерегистрация завершена код 8"
            clients = clients.Where(a => !(a.Status.Code == "6" ||
                                         a.Status.Code == "11" ||
                                         a.Status.Code == "7" ||
                                         a.Status.Code == "8")
                                    ).ToList();

            //Отсекаем по СНИЛС
            //нет СНИЛС
            //заявкой без СНИЛС является любая заявка, где поле СНИЛС заполнено иначе, чем XXX-XXX-XXX YY
            if (model.IsSnilsNotEmpty == false)
            {
                clients = clients.Where(a => (string.IsNullOrEmpty(a.SNILS) || !(a.SNILS.Length == 14 && a.SNILS[3] == '-' && a.SNILS[7] == '-' && a.SNILS[11] == ' '))).ToList();
            }
            //есть СНИЛС
            if (model.IsSnilsNotEmpty == true)
            {
                clients = clients.Where(a => (!string.IsNullOrEmpty(a.SNILS)) && (a.SNILS.Length == 14 && a.SNILS[3] == '-' && a.SNILS[7] == '-' && a.SNILS[11] == ' ')).ToList();
            }

            List<Entities.User> listUser = userBusinessLogic.Find("");
            SNILSReport printedForm = new SNILSReport(clients, model.DateSnilsFrom, model.DateSnilsTo, NameDeliveryPoint, CurrentUser.Fullname,
                                                      model.IsSnilsNotEmpty, listUser);
            return File(printedForm.GetExcel(),
                System.Net.Mime.MediaTypeNames.Application.Octet, string.Format("Заявки и наличие СНИЛС.xlsx"));

        }

        public ActionResult GetBSOReportForm10(BSOReportForm10Model model)
        {
            string NameDeliveryPoint = string.Empty;
            BSOListModel bsoListModel = new BSOListModel();
            bsoListModel.SearchCriteriaModel.StatusDateFrom = model.DateForm10From;
            bsoListModel.SearchCriteriaModel.StatusDateTo = model.DateForm10To;
            //для Администраторов ограничений по точкам - нет
            if (Role.Administrator.Id == Role.GetRealRole(CurrentUser).Id || Role.AdministratorBSO.Id == Role.GetRealRole(CurrentUser).Id)
            {
                if (model.DeliveryPointIds.Count > 0)
                {
                    bsoListModel.SearchCriteriaModel.DeliveryPointIds = model.DeliveryPointIds;
                    foreach (var item in model.DeliveryPointIds)
                    {
                        NameDeliveryPoint = NameDeliveryPoint + ", " + model.DeliveryPoints.Where(a => a.Value == item.ToString()).Select(b => b.Text).FirstOrDefault();
                    }
                    NameDeliveryPoint = NameDeliveryPoint.Trim(',', ' ');
                }
                else
                {
                    NameDeliveryPoint = "Все точки";
                }
            }
            else
            {   //для ответственного
                if (Role.ResponsibleBSO == Role.GetRealRole(CurrentUser))
                {
                    if (model.DeliveryPointIds.Count > 0)
                    {
                        bsoListModel.SearchCriteriaModel.DeliveryPointIds = model.DeliveryPointIds;
                    }
                    else
                    {//все точки для ответственного
                        model.DeliveryPoints = ReferencesProvider.GetDeliveryPointsForResponsibleBSO(CurrentUser.Id, false);
                        foreach (var item in model.DeliveryPoints)
                        {
                            if (item.Value != "")
                            {
                                model.DeliveryPointIds.Add(long.Parse(item.Value));
                            }
                        }
                        bsoListModel.SearchCriteriaModel.DeliveryPointIds = model.DeliveryPointIds;
                    }

                    foreach (var item in model.DeliveryPointIds)
                    {
                        NameDeliveryPoint = NameDeliveryPoint + ", " + model.DeliveryPoints.Where(a => a.Value == item.ToString()).Select(b => b.Text).FirstOrDefault();
                    }
                    NameDeliveryPoint = NameDeliveryPoint.Trim(',', ' ');
                }
                else
                {
                    //для всех остальных, только по их собственной точке
                    bsoListModel.SearchCriteriaModel.DeliveryPointIds = new List<long>() { CurrentUser.DeliveryPoint.Id };
                    model.DeliveryPointIds = new List<long>() { CurrentUser.DeliveryPoint.Id };
                    NameDeliveryPoint = model.DeliveryPoints.Where(a => a.Value == model.DeliveryPointIds.FirstOrDefault().ToString()).Select(b => b.Text).FirstOrDefault();
                }
            }
            BSOSearchCriteria criteria = bsoListModel.SearchCriteriaModel.GetBSOSearchCriteria();
            List<BSOInfo> bsos = bsoBusinessLogic.BSO_Find(
                    criteria,
                    new List<SortCriteria<BSOSortField>>(),
                    new PageRequest() { PageNumber = 1, PageSize = int.MaxValue }).Data;
            BSOReportForm10 printedForm = new BSOReportForm10(bsos, model.DateForm10From, model.DateForm10To,
                NameDeliveryPoint, CurrentUser.Fullname);
            return File(printedForm.GetExcel(),
                System.Net.Mime.MediaTypeNames.Application.Octet, string.Format("Отчет по движению (статусам) временных свидетельств.xls"));
        }

        public ActionResult GetBSOReportForm10Full(BSOReportForm10FullModel model)
        {
            string delivery = string.Empty;
            BSOListModel bsoListModel = new BSOListModel();
            bsoListModel.SearchCriteriaModel.StatusDateFrom = model.DateForm10FullFrom;
            bsoListModel.SearchCriteriaModel.StatusDateTo = model.DateForm10FullTo;

            List<string> ListDelivery = new List<string>();
            List<ReferenceUniversalItem> ListDeliveryPointUniversal = referenceBusinessLogic.GetUniversalList(Constants.DeliveryPointRef);
            List<DeliveryCenter> listDeliveryCenter = referenceBusinessLogic.GetDeliveryCenterList();

            //для Администраторов ограничений по точкам - нет
            if (Role.Administrator.Id == Role.GetRealRole(CurrentUser).Id || Role.AdministratorBSO.Id == Role.GetRealRole(CurrentUser).Id)
            {
                if (model.DeliveryPointIds.Count > 0)
                {
                    bsoListModel.SearchCriteriaModel.DeliveryPointIds = model.DeliveryPointIds;
                    foreach (var item in model.DeliveryPointIds)
                    {
                        delivery = listDeliveryCenter.Where(a => a.Id == ListDeliveryPointUniversal.Where(b => b.Id == item).Select(c => c.DeliveryCenterId).FirstOrDefault()).Select(d => d.DisplayName).FirstOrDefault();
                        ListDelivery.Add(delivery);
                    }
                }
                else
                {
                    delivery = "Все точки";
                }
            }
            else
            {   //для ответственного
                if (Role.ResponsibleBSO == Role.GetRealRole(CurrentUser))
                {
                    if (model.DeliveryPointIds.Count > 0)
                    {
                        bsoListModel.SearchCriteriaModel.DeliveryPointIds = model.DeliveryPointIds;
                    }
                    else
                    {//все точки для ответственного
                        model.DeliveryPoints = ReferencesProvider.GetDeliveryPointsForResponsibleBSO(CurrentUser.Id, false);
                        foreach (var item in model.DeliveryPoints)
                        {
                            if (item.Value != "")
                            {
                                model.DeliveryPointIds.Add(long.Parse(item.Value));
                            }
                        }
                        bsoListModel.SearchCriteriaModel.DeliveryPointIds = model.DeliveryPointIds;
                    }
                    foreach (var item in model.DeliveryPointIds)
                    {
                        delivery = listDeliveryCenter.Where(a => a.Id == ListDeliveryPointUniversal.Where(b => b.Id == item).Select(c => c.DeliveryCenterId).FirstOrDefault()).Select(d => d.DisplayName).FirstOrDefault();
                        ListDelivery.Add(delivery);
                    }
                }
                else
                {
                    //для всех остальных, только по их собственной точке
                    bsoListModel.SearchCriteriaModel.DeliveryPointIds = new List<long>() { CurrentUser.DeliveryPoint.Id };
                    model.DeliveryPointIds = new List<long>() { CurrentUser.DeliveryPoint.Id };
                    delivery = listDeliveryCenter.Where(a => a.Id == ListDeliveryPointUniversal.Where(b => b.Id == CurrentUser.DeliveryPoint.Id).Select(c => c.DeliveryCenterId).FirstOrDefault()).Select(d => d.DisplayName).FirstOrDefault();
                    ListDelivery.Add(delivery);
                }
            }
            BSOSearchCriteria criteria = bsoListModel.SearchCriteriaModel.GetBSOSearchCriteria();
            List<BSOInfo> listBSOinRangeDate = bsoBusinessLogic.BSO_Find(
                    criteria,
                    new List<SortCriteria<BSOSortField>>(),
                    new PageRequest() { PageNumber = 1, PageSize = int.MaxValue }).Data;

            List<BSOHistoryItem> listHistory = new List<BSOHistoryItem>();
            if (listBSOinRangeDate.Count > 0)
            {
                List<long> listBSO_IDs = listBSOinRangeDate.Select(a => a.Id).ToList();
                listHistory = bsoBusinessLogic.BSO_GetListHistory(new DateTime(2000, 1, 1), (DateTime)bsoListModel.SearchCriteriaModel.StatusDateTo, listBSO_IDs);
            }

            List<BSOInfo> listBSOBeforeDate = bsoBusinessLogic.BSO_Find(
                    new BSOSearchCriteria()
                    {
                        StatusDateTo = bsoListModel.SearchCriteriaModel.StatusDateFrom,
                        DeliveryPointIds = bsoListModel.SearchCriteriaModel.DeliveryPointIds
                    },
                    new List<SortCriteria<BSOSortField>>(),
                    new PageRequest() { PageNumber = 1, PageSize = int.MaxValue }).Data;

            List<BSOInfo> listBSOAllDate = bsoBusinessLogic.BSO_Find(
                    new BSOSearchCriteria()
                    {
                        StatusDateTo = bsoListModel.SearchCriteriaModel.StatusDateTo,
                        DeliveryPointIds = bsoListModel.SearchCriteriaModel.DeliveryPointIds
                    },
                    new List<SortCriteria<BSOSortField>>(),
                    new PageRequest() { PageNumber = 1, PageSize = int.MaxValue }).Data;


            //фильтруем по DeliveryPointId
            //if (dicDeliveryPoint.Count > 0)
            //{
            //    listHistory = listHistory.Where(a => dicDeliveryPoint.Keys.Contains(a.DeliveryPointId)).ToList();
            //}

            BSOReportForm10Full printedForm = new BSOReportForm10Full(listHistory, listBSOBeforeDate, listBSOinRangeDate, listBSOAllDate, ListDelivery.Distinct().ToList(), model.DateForm10FullFrom, model.DateForm10FullTo, CurrentUser);
            return File(printedForm.GetExcel(),
                System.Net.Mime.MediaTypeNames.Application.Octet, string.Format("Отчет по движению ВС с группировкой по пулам номеров БСО. Форма №10.xls"));
        }

        public ActionResult GetSMSBaseReport(SMSBaseReportModel model)
        {
            SmsBase.SmsBaseGet get = new SmsBase.SmsBaseGet()
            {
                CreateDateFrom = model.SmsBaseDateFrom,
                CreateDateTo = model.SmsBaseDateTo.AddDays(1)
            };
            List<SmsBase> listSmsBase = smsBusinessLogic.SMSBase_GetAll(get);
            SMSBaseReport printedForm = new SMSBaseReport(listSmsBase, model.SmsBaseDateFrom, model.SmsBaseDateTo);
            return File(printedForm.GetExcel(),
                System.Net.Mime.MediaTypeNames.Application.Octet, string.Format("Отчет по отправленным смс сообщениям.xls"));
        }

        public ActionResult CheckClient(BaseCheckClientModel model, long? CountDublicate)
        {
            List<CheckClient> listCheckClient = checkBusinessLogic.Check_Client(model.SearchCheckClient.GetCheckClientSearchCriteria());
            if (model.IsHideClientId)
            {
                listCheckClient = listCheckClient.FindAll(a => !BaseCheckClientModel.ListHideClientId.Contains(a.Id));
            }
            CheckClientReport printedForm = new CheckClientReport(listCheckClient,
                                                                  CountDublicate,
                                                                  model.SearchCheckClient.GetCheckClientSearchCriteria(),
                                                                  model.ViewColumn.GetViewColumn());
            return File(printedForm.GetExcel(),
                 System.Net.Mime.MediaTypeNames.Application.Octet, string.Format("Отчет по дубликатам клиентов.xlsx"));
        }
    }
}