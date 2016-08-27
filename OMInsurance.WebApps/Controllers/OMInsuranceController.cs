using OMInsurance.BusinessLogic;
using OMInsurance.Interfaces;
using OMInsurance.Log;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace OMInsurance.WebApps.Controllers
{
    public abstract class OMInsuranceController : Controller
    {
        protected IReferenceBusinessLogic referenceBusinessLogic;
        protected IUserBusinessLogic userBusinessLogic;
        private Entities.User currentUser;

        public Entities.User CurrentUser 
        { 
            get 
            {
                if (currentUser == null)
                {
                    currentUser = userBusinessLogic.User_GetByLogin(User.Identity.Name);
                    currentUser = Entities.Core.Role.FillWeightRoles(currentUser);
                }
                return currentUser;
            } 
        }

        public OMInsuranceController()
        {
            referenceBusinessLogic = new ReferenceBusinessLogic();
            userBusinessLogic = new UserBusinessLogic();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;
            filterContext.ExceptionHandled = true;
            if (!ex.GetType().IsSubclassOf(typeof(ApplicationException)))
            {
                AsyncLogger.Error(ex);
            }

            HandleErrorInfo model = new HandleErrorInfo(filterContext.Exception, "Controller", "Action");
            filterContext.Result = new ViewResult()
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(model)
            };
        }

        protected virtual void FillReferences(
            string referenceName,
            string selectedValue = null,
            bool withDefaultEmpty = false)
        {
            List<SelectListItem> items = ReferencesProvider.GetReferences(referenceName, selectedValue, withDefaultEmpty);
            ViewData[referenceName] = items;
        }
    }
}