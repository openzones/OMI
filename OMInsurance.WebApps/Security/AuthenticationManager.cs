using OMInsurance.Configuration;
using OMInsurance.Entities;
using OMInsurance.Interfaces;
using OMInsurance.Utils;
using System;
using System.Web;

namespace OMInsurance.WebApps.Security
{
    public static class AuthenticationManager
    {
        #region Constants

        private const string AuthCookieName = "OMInsuranceAuthTicket";

        #endregion

        public static void AuthenticateRequest(HttpContextBase httpContext)
        {
            string principalName = GetPrincipalNameFromCookie(httpContext);

            if (!string.IsNullOrEmpty(principalName))
            {
                SetPrincipal(httpContext, principalName);
            }
            else
            {
                SetAnonymous(httpContext);
            }
        }

        public static bool Login(IUserBusinessLogic userBll, HttpContextBase httpContext, string login, string password)
        {
            bool validCredentials = false;
            User user = userBll.User_GetByLogin(login);
            if (user == null)
            {
                validCredentials = false;
            }
            else
            {
                validCredentials = PasswordHash.ValidatePassword(password, user.PasswordHash);
            }

            if (validCredentials)
            {
                SetAuthenticationCookie(httpContext, login);
                SetPrincipal(httpContext, login);
            }

            return validCredentials;
        }

        /// <summary>
        /// Validates authentication cookie and refreshes it if cookie is near to expiration time.
        /// </summary>
        /// <returns>User principal name if cookie is valid. Otherwise - null.</returns>
        private static string GetPrincipalNameFromCookie(HttpContextBase httpContext)
        {
            HttpCookie principalCookie = httpContext.Request.Cookies.Get(AuthCookieName);
            if (principalCookie == null || string.IsNullOrEmpty(principalCookie.Value))
            {
                return null;
            }

            AuthenticationPackage authPackage = AuthenticationPackage.FromXml(principalCookie.Value);
            if (authPackage == null)
            {
                return null;
            }

            DateTime now = DateTime.UtcNow;

            if (now > authPackage.Expires)
            {
                return null;
            }
            else if (now >= authPackage.Expires.AddSeconds(-ConfiguraionProvider.AuthenticationCookieRefreshMargin))
            {
                SetAuthenticationCookie(httpContext, authPackage.PrincipalName);
            }

            return authPackage.PrincipalName;
        }

        private static void SetAuthenticationCookie(HttpContextBase httpContext, string principalName)
        {
            AuthenticationPackage package = new AuthenticationPackage()
            {
                PrincipalName = principalName,
                Expires = DateTime.UtcNow.AddSeconds(ConfiguraionProvider.AuthenticationCookieDuration)
            };

            HttpCookie principalCookie = httpContext.Response.Cookies[AuthCookieName];
            if (principalCookie == null)
            {
                principalCookie = new HttpCookie(AuthCookieName);
                principalCookie.Value = package.ToXml();
                httpContext.Response.AppendCookie(principalCookie);
            }
            else
            {
                principalCookie.Value = package.ToXml();
                httpContext.Response.SetCookie(principalCookie);
            }
        }

        private static void SetPrincipal(HttpContextBase httpContext, string principalName)
        {
            WebIdentity identity = new WebIdentity(principalName);
            WebPrincipal principal = new WebPrincipal(identity);
            httpContext.User = principal;
        }

        private static void SetAnonymous(HttpContextBase httpContext)
        {
            WebIdentity identity = WebIdentity.GetAnonymous();
            WebPrincipal principal = new WebPrincipal(identity);
            httpContext.User = principal;
        }

        internal static void LogOut(HttpContextBase httpContext)
        {
            HttpCookie emptyCookie = new HttpCookie(AuthCookieName);
            httpContext.Response.SetCookie(emptyCookie);
        }
    }
}