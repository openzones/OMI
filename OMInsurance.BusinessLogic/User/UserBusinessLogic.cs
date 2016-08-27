using OMInsurance.DataAccess.DAO;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using OMInsurance.Interfaces;
using System.Collections.Generic;

namespace OMInsurance.BusinessLogic
{
    public class UserBusinessLogic : IUserBusinessLogic
    {
        /// <summary>
        /// Creates or update a user in a database
        /// </summary>
        /// <param name="data">Data to save a user</param>
        /// <returns>Identifier of a saved user</returns>
        public long User_Save(User.SaveData data)
        {
            return UserDao.Instance.User_Save(data);
        }

        /// <summary>
        /// Returns a user by identifier
        /// </summary>
        /// <param name="id">Identifier od a user</param>
        /// <returns>Instance of user</returns>
        public User User_Get(long id)
        {
            return UserDao.Instance.User_Get(id);
        }

        /// <summary>
        /// Returns user by login
        /// </summary>
        /// <param name="login">User login</param>
        /// <returns>Instance of user</returns>
        public User User_GetByLogin(string login)
        {
            return UserDao.Instance.User_GetByLogin(login);
        }

        /// <summary>
        /// Return list of roles
        /// </summary>
        /// <returns>List of roles</returns>
        public List<Role> Role_GetList()
        {
            return UserDao.Instance.Role_GetList();
        }

        public List<User> Find(string name)
        {
            return UserDao.Instance.User_Find(name);
        }

        public List<ClientAcquisitionEmployee> ClientAcquisitionEmployee_Get(long? UserId)
        {
            return UserDao.Instance.ClientAcquisitionEmployee_Get(UserId);
        }

    }
}
