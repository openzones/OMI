using OMInsurance.DataAccess.Core;
using OMInsurance.DataAccess.Materializers;
using OMInsurance.Entities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace OMInsurance.DataAccess.DAO
{
    public class RepresentativeDao : ItemDao
    {
        private static RepresentativeDao _instance = new RepresentativeDao();

        private RepresentativeDao()
            : base(DatabaseAliases.OMInsurance, new DatabaseErrorHandler())
        {
        }

        public static RepresentativeDao Instance
        {
            get
            {
                return _instance;
            }
        }

        public Representative GetRepresentative(long id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@RepresentativeId", SqlDbType.BigInt, id);
            Representative item = Execute_Get(RepresentativeMaterializer.Instance, "Representative_Get", parameters);
            return item;
        }
    }
}
