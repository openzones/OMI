using OMInsurance.DataAccess.Core;
using OMInsurance.DataAccess.Materializers;
using OMInsurance.Entities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace OMInsurance.DataAccess.DAO
{
    public class AddressDao : ItemDao
    {
        private static AddressDao _instance = new AddressDao();

        private AddressDao()
            : base(DatabaseAliases.OMInsurance, new DatabaseErrorHandler())
        {
        }

        public static AddressDao Instance
        {
            get
            {
                return _instance;
            }
        }

        public Address GetAddress(long id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@AddressId", SqlDbType.BigInt, id);
            Address item = Execute_Get(AddressMaterializer.Instance, "Address_Get", parameters);
            return item;
        }
    }
}
