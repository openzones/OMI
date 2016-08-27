using OMInsurance.BusinessLogic;
using OMInsurance.Entities;
using OMInsurance.Interfaces;
using OMInsurance.WebApps.Security;
using System.Web.Mvc;

namespace OMInsurance.WebApps.Controllers
{
    [AuthorizeUser(Roles = "Anonymous")]
    [AllowAnonymous]
    public class OMWebAPIController : OMInsuranceController
    {
        IClientBusinessLogic clientBusinessLogic;
        public OMWebAPIController()
        {
            clientBusinessLogic = new ClientBusinessLogic();
        }

        public JsonResult ClientVisitStatus(int id)
        {
            ClientVisit cv = clientBusinessLogic.ClientVisit_Get(id);
            return Json(
                new
                {
                    ClientVisitId = cv.Id,
                    StatusId = cv.Status.Id,
                    StatusValue = cv.Status.Name
                }, JsonRequestBehavior.AllowGet);
        }
    }
}