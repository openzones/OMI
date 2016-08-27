using OMInsurance.DataAccess.Materializers;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace OMInsurance.DataAccess.Core
{
    public class FiasDao : ItemDao
    {
        private static FiasDao _instance = new FiasDao();

        private FiasDao()
            : base(DatabaseAliases.OMInsurance, new DatabaseErrorHandler())
        {
        }

        public static FiasDao Instance
        {
            get
            {
                return _instance;
            }
        }

        public List<FiasEntry> Find(string parentId, string name, FiasType type)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@ParentId", SqlDbType.NVarChar, parentId);
            parameters.AddInputParameter("@Name", SqlDbType.NVarChar, name + "%");
            parameters.AddInputParameter("@Type", SqlDbType.Int, (int)type);
            List<FiasEntry> items = Execute_GetList<FiasEntry>(FiasEntryMaterializer.Instance, "Fias_Find", parameters);
            return items;
        }

        public List<Street> FindStreet(string name)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@Name", SqlDbType.NVarChar, "%" + name + "%");
            List<Street> items = Execute_GetList<Street>(StreetMaterializer.Instance, "Street_Find", parameters);
            return items;
        }

        public List<SMO> FindSmo(string name)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@SMONameSubsting", SqlDbType.NVarChar, "%" + name + "%");
            List<SMO> items = Execute_GetList<SMO>(SMOMaterializer.Instance, "SMO_GetList", parameters);
            return items;
        }
    }
}
