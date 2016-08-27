using OMInsurance.DataAccess.Core;
using OMInsurance.DataAccess.Core.Helpers;
using OMInsurance.DataAccess.Materializers;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace OMInsurance.DataAccess.DAO
{
    public class UserDao : ItemDao
    {
        private static UserDao _instance = new UserDao();

        private UserDao()
            : base(DatabaseAliases.OMInsurance, new DatabaseErrorHandler())
        {
        }

        public static UserDao Instance
        {
            get
            {
                return _instance;
            }
        }

        public long User_Save(User.SaveData data)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@Login", SqlDbType.NVarChar, data.Login);
            parameters.AddInputParameter("@PasswordHash", SqlDbType.NVarChar, data.PasswordHash);
            parameters.AddInputParameter("@DepartmentId", SqlDbType.BigInt, data.DepartmentId);
            parameters.AddInputParameter("@DeliveryPointId", SqlDbType.BigInt, data.DeliveryPointId);
            parameters.AddInputParameter("@Firstname", SqlDbType.NVarChar, data.Firstname);
            parameters.AddInputParameter("@Secondname", SqlDbType.NVarChar, data.Secondname);
            parameters.AddInputParameter("@Lastname", SqlDbType.NVarChar, data.Lastname);
            parameters.AddInputParameter("@Roles", SqlDbType.Structured, DaoHelper.GetObjectIds(data.Roles));
            parameters.AddInputParameter("@SaveDate", SqlDbType.DateTime, DateTime.Now);
            parameters.AddInputParameter("@Position", SqlDbType.NVarChar, data.Position);
            parameters.AddInputParameter("@Email", SqlDbType.NVarChar, data.Email);
            parameters.AddInputParameter("@Phone", SqlDbType.NVarChar, data.Phone);
            SqlParameter userId = parameters.AddInputOutputParameter("@UserID", SqlDbType.BigInt, data.Id);

            Execute_StoredProcedure("User_Save", parameters);
            ClientVisitSaveResult result = new ClientVisitSaveResult();
            result.ClientID = (long)userId.Value;
            return (long)userId.Value;
        }

        public User User_Get(long userId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@UserID", SqlDbType.BigInt, userId);
            User user = Execute_Get(UserMaterializer.Instance, "User_Get", parameters);
            return user;
        }

        public User User_GetByLogin(string userLogin)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@Login", SqlDbType.NVarChar, userLogin);
            User user = Execute_Get(UserMaterializer.Instance, "User_GetByLogin", parameters);
            return user;
        }

        public List<Role> Role_GetList()
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<Role> roles = Execute_GetList(RoleMaterializer.Instance, "Role_GetList", parameters);
            return roles;
        }

        public List<User> User_Find(string name)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@Username", SqlDbType.NVarChar, string.Format("%{0}%", name));
            List<User> users = Execute_GetList(UserMaterializer.Instance, "User_Find", parameters);
            return users;
        }

        public List<ClientAcquisitionEmployee> ClientAcquisitionEmployee_Get(long? UserId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@UserId", SqlDbType.BigInt, UserId);
            List<ClientAcquisitionEmployee> listClientAcquisitionEmployee = Execute_GetList(ClientAcquisitionEmployeeMaterializer.Instance, "ClientAcquisitionEmployee_Get", parameters);
            return listClientAcquisitionEmployee;
        }
    }
}
