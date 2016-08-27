using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMInsurance.DataAccess.Materializers
{
    public class DocumentMaterializer : IMaterializer<Document>
    {
        private static readonly DocumentMaterializer _instance = new DocumentMaterializer();

        public static DocumentMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }

        public Document Materialize(DataReaderAdapter dataReader)
        {
            return Materialize_List(dataReader).FirstOrDefault();
        }

        public List<Document> Materialize_List(DataReaderAdapter dataReader)
        {
            List<Document> items = new List<Document>();

            while (dataReader.Read())
            {
                Document obj = ReadItemFields(dataReader);
                items.Add(obj);
            }

            return items;
        }

        public Document ReadItemFields(DataReaderAdapter dataReader, Document item = null)
        {
            if (item == null)
            {
                item = new Document();
            }
            item.Id = dataReader.GetInt64("ID");
            item.Series = dataReader.GetString("Series");
            item.Number = dataReader.GetString("Number");
            item.IssueDate = dataReader.GetDateTimeNull("IssueDate");
            item.ExpirationDate = dataReader.GetDateTimeNull("ExpirationDate");
            item.IsIssueCase = dataReader.GetBooleanNull("IsIssueCase") ?? false;
            item.IssueDepartment = dataReader.GetString("IssueDepartment");
            item.CreateDate = dataReader.GetDateTime("CreateDate");
            item.UpdateDate = dataReader.GetDateTime("UpdateDate");
            item.DocumentType = ReferencesMaterializer.Instance.ReadItemFields(
                dataReader,
                "DocumentTypeID",
                "DocumentTypeCode",
                "DocumentTypeName");
            return item;
        }
    }
}
