using OMInsurance.WebApps.Security;
using System.Web.Mvc;

namespace OMInsurance.WebApps
{
    public static class FilterConfig
    {
        internal static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AuthorizeUserAttribute());
        }
    }
}