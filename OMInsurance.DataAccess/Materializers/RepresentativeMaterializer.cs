using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using System.Collections.Generic;
using System.Linq;

namespace OMInsurance.DataAccess.Materializers
{
    public class RepresentativeMaterializer : IMaterializer<Representative>
    {
        private static readonly RepresentativeMaterializer _instance = new RepresentativeMaterializer();

        public static RepresentativeMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }

        public Representative Materialize(DataReaderAdapter reader)
        {
            return Materialize_List(reader).FirstOrDefault();
        }

        public List<Representative> Materialize_List(DataReaderAdapter reader)
        {
            List<Representative> items = new List<Representative>();

            while (reader.Read())
            {
                Representative obj = ReadItemFields(reader);
                items.Add(obj);
            }
            return items;
        }

        public Representative ReadItemFields(DataReaderAdapter reader, Representative item = null)
        {
            if (item == null)
            {
                item = new Representative();
            }

            item.Id = reader.GetInt64("ID");
            item.Firstname = reader.GetString("FirstName");
            item.Secondname = reader.GetString("Secondname");
            item.Lastname = reader.GetString("Lastname");
            item.Birthday = reader.GetDateTimeNull("Birthday");
            item.IssueDate = reader.GetDateTimeNull("IssueDate");
            item.IssueDepartment = reader.GetString("IssueDepartment");
            item.Number = reader.GetString("Number");
            item.Series = reader.GetString("Series");
            item.RepresentativeTypeId = reader.GetInt64Null("RepresentativeTypeId");
            if (reader.IsNotNull("DocumentTypeId"))
            {
                item.DocumentType = ReferencesMaterializer.Instance.ReadItemFields(reader, "DocumentTypeID", "DocumentTypeCode", "DocumentTypeName");
            }

            return item;
        }
    }
}
