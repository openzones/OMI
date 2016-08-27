using OMInsurance.DataAccess.DAO;
using OMInsurance.Entities.Core;
using System.Collections.Generic;

namespace OMInsurance.BusinessLogic.Core
{
    public static class LogBusinessLogic
    {
        /// <summary>
        /// Creates a new log message in database
        /// </summary>
        /// <param name="messages">List of messages</param>
        public static void Create(IEnumerable<LogMessage> messages)
        {
            LogDao.Instance.Create(messages);
        }
    }
}
