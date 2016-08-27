using OMInsurance.Log;
using OMInsurance.WebApps.Models;
using OMInsurance.WebApps.Security;
using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OMInsurance.WebApps
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public MvcApplication()
        {
            AuthenticateRequest += MvcApplication_AuthenticateRequest;
        }

        private void MvcApplication_AuthenticateRequest(object sender, EventArgs e)
        {
            AuthenticationManager.AuthenticateRequest(new HttpContextWrapper(Context));
        }

        protected void Application_Start()
        {
            FileLog.Initialize("OMInsurance");
            AsyncLogger.Initialize("OMInsurance");
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ModelMapper.Configure();
        }

        protected void Application_BeginRequest()
        {
            CultureInfo cultureInfo = new CultureInfo("ru-RU")
            {
                NumberFormat =
                {
                    CurrencyDecimalSeparator = ".",
                    NumberDecimalSeparator = ".",
                    PercentDecimalSeparator = "."
                }
            };
            
            Thread.CurrentThread.CurrentCulture = cultureInfo;
			Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }
    }
}
