using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace OMInsurance.Migration
{
    public class ClientDataDao : ItemDao 
    {
        private static ClientDataDao _instance = new ClientDataDao();
        private static object syncRoot = new Object();
        private ClientDataDao()
            : base(DatabaseAliases.OMInsurance, new DatabaseErrorHandler())
        {
        }

        public static ClientDataDao Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                            _instance = new ClientDataDao();
                    }
                }
                return _instance;
            }
        }

        public List<ClientVisit.SaveData> GetNextClientData()
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<ClientVisit.SaveData> item = Execute_GetList(ClientVisitSaveDataMaterializer.Instance, "Migration_GetRow", parameters);
            return item;
        }

        public void SetProcessed(long id, bool success)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@Id", System.Data.SqlDbType.BigInt, id);
            parameters.AddInputParameter("@isSuccess", System.Data.SqlDbType.Bit, success);

            if (success)
            {
                Execute_Query_Get(ClientVisitSaveDataMaterializer.Instance,
                    "Update MigrationSource Set IsProcessed = '1' WHERE UniqueId = @id",
                    parameters);
            }
            else
            {
                Execute_Query_Get(ClientVisitSaveDataMaterializer.Instance,
                    "Update MigrationSource Set IsProcessed = '0' WHERE UniqueId = @id",
                    parameters);
            }
        }
    }
}
