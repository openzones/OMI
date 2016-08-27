using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OMInsurance.WebApps
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Files",
                url: "File/Image/{filename}",
                defaults: new { controller = "File", action = "Image", filename = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "ClientVisit", action = "Actuals", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Login",
                url: "Account/Login",
                defaults: new { controller = "Account", action = "Login" }
            );
            routes.MapRoute(
                name: "PrintedForms",
                url: "PrintedForms/{action}/{id}",
                defaults: new { controller = "PrintedForms", action = "ChangeInsuranceCompanyApplication", id = UrlParameter.Optional }
            );

        }
    }
}
