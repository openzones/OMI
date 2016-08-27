using OMInsurance.DataAccess.Core;
using OMInsurance.DataAccess.Materializers;
using OMInsurance.Entities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace OMInsurance.DataAccess.DAO
{
    public class PolicyDao : ItemDao
    {
        private static PolicyDao _instance = new PolicyDao();

        private PolicyDao()
            : base(DatabaseAliases.OMInsurance, new DatabaseErrorHandler())
        {
        }

        public static PolicyDao Instance
        {
            get
            {
                return _instance;
            }
        }

        public PolicyInfo GetPolicy(long id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@PolicyId", SqlDbType.BigInt, id);
            PolicyInfo item = Execute_Get(PolicyMaterializer.Instance, "Policy_Get", parameters);
            return item;
        }

        public void RegionPolicyData_Save(List<PolicyFromRegion> listPolicy)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            PolicyFromRegionTableSet set = new PolicyFromRegionTableSet(listPolicy);
            parameters.AddInputParameter("@RegionPolicyData", SqlDbType.Structured, set.PolicyFromRegionTable);
            Execute_StoredProcedure("RegionPolicyData_Save", parameters);
        }
    }
}
