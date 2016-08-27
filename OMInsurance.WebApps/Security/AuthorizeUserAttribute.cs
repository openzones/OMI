using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace OMInsurance.WebApps.Security
{
	/// <summary>
	/// Handles cases when not authenticated user accesses controller actions.
	/// </summary>
	/// <remarks>When JSON request is handled response will be HTTP 401 error, but if it is general request
	/// user will be redirected to login page.</remarks>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class AuthorizeUserAttribute : AuthorizeAttribute
	{
		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
		{
			if (!filterContext.HttpContext.Request.IsAuthenticated)
			{
				if (filterContext.HttpContext.Request.IsAjaxRequest())
				{
					filterContext.Result = new HttpUnauthorizedResult();
				}
				else
				{
					filterContext.Result = new RedirectToRouteResult("Login",
						new RouteValueDictionary(new { returnUrl = filterContext.HttpContext.Request.Url.PathAndQuery }));
				}
			}
			else
			{
				base.HandleUnauthorizedRequest(filterContext);
			}
		}
	}
}