using OMInsurance.DataAccess.Core;
using OMInsurance.Entities.Core;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace OMInsurance.DataAccess.DAO
{
    public class LogDao : ItemDao
    {
        private static LogDao _instance = new LogDao();

        private LogDao()
            : base(DatabaseAliases.OMInsurance, new DatabaseErrorHandler())
        {
        }

        public static LogDao Instance
        {
            get
            {
                return _instance;
            }
        }

        public void Create(IEnumerable<LogMessage> messages)
        {
            LogMessageTableSet messageSet = new LogMessageTableSet(messages);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@Messages", SqlDbType.Structured, messageSet.LogMessageTable);

            Execute_StoredProcedure("st.Log_Create", parameters);
        }
    }
}
