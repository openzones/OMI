using OMInsurance.BusinessLogic;
using OMInsurance.Entities;
using System;
using System.Linq;
using System.Web.Security;

namespace OMInsurance.WebApps.Security
{
    public class OMInsuranceRoleProvider : RoleProvider
    {
        private UserBusinessLogic userBll;
        public OMInsuranceRoleProvider()
        {
            ApplicationName = "OMInsurance";
            userBll = new UserBusinessLogic();
        }
        public override string ApplicationName{ get; set; }

        public override void CreateRole(string roleName)
        {
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            User user = userBll.User_GetByLogin(username);
            return user.Roles.Select(role => role.Name).ToArray();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            User user = userBll.User_GetByLogin(username);
            return user.Roles.Exists(u => u.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}