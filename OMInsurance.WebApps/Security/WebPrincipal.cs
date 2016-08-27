using OMInsurance.Entities;
using System;
using System.Security.Principal;
using System.Web.Security;

namespace OMInsurance.WebApps.Security
{
    public sealed class WebPrincipal : IPrincipal
    {
        private readonly WebIdentity _identity;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebPrincipal"/> class.
        /// </summary>
        /// <param name="identity">The identity that should be used as primary for principal.</param>
        public WebPrincipal(WebIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }
            _identity = identity;
        }

        public IIdentity Identity
        {
            get { return _identity; }
        }

        public bool IsInRole(string role)
        {
            return Roles.Provider.IsUserInRole(_identity.Name, role);
        }
    }
}
