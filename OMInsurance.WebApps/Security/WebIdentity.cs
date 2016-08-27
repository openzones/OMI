using System;
using System.Security.Principal;

namespace OMInsurance.WebApps.Security
{
    public class WebIdentity : IIdentity
    {
        #region Fields

        private const string _customAuthenticationType = "Custom";

        private readonly string _authenticationType;
        private readonly bool _isAuthenticated;
        private readonly string _userName;

        #endregion

        #region Properties

        /// <summary>
        /// Gets user principal name.
        /// </summary>
        public string Name
        {
            get
            {
                if (!IsAuthenticated)
                {
                    return "Anonymous";
                }
                if (_userName != null)
                {
                    return _userName;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Returns true is user is authenticated, otherwise returns false.
        /// </summary>
        public bool IsAuthenticated
        {
            get { return _isAuthenticated; }
        }

        /// <summary>
        /// Gets authentication type.
        /// </summary>
        public string AuthenticationType
        {
            get { return _authenticationType; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor. Creates identity of non-authenticated user.
        /// </summary>
        private WebIdentity()
        {
            _userName = null;
            _authenticationType = string.Empty;
            _isAuthenticated = false;
        }

        /// <summary>
        /// Creates new instance of type <see cref="WebIdentity"/>.
        /// </summary>
        /// <param name="userName">User to whom identity is related.</param>
        public WebIdentity(string userName)
        {
            if (userName == null)
            {
                throw new ArgumentNullException("userName");
            }

            _userName = userName;
            _authenticationType = _customAuthenticationType;
            _isAuthenticated = true;
        }

        #endregion

        /// <summary>
        /// Gets web identity for anonymous user.
        /// </summary>
        /// <returns>Anonymous web identity.</returns>
        public static WebIdentity GetAnonymous()
        {
            return new WebIdentity();
        }
    }
}
