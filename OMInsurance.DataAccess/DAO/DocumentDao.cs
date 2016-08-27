using OMInsurance.DataAccess.Core;
using OMInsurance.DataAccess.Materializers;
using OMInsurance.Entities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace OMInsurance.DataAccess.DAO
{
    public class DocumentDao : ItemDao
    {
        private static DocumentDao _instance = new DocumentDao();

        private DocumentDao()
            : base(DatabaseAliases.OMInsurance, new DatabaseErrorHandler())
        {
        }

        public static DocumentDao Instance
        {
            get
            {
                return _instance;
            }
        }

        public Document GetDocument(long id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@DocumentId", SqlDbType.BigInt, id);
            Document item = Execute_Get(DocumentMaterializer.Instance, "Document_Get", parameters);
            return item;
        }
    }
}
