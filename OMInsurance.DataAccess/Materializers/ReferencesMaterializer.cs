using OMInsurance.DataAccess.Core;
using OMInsurance.Entities.Core;
using System.Collections.Generic;
using System.Linq;

namespace OMInsurance.DataAccess.Materializers
{
    public class ReferencesMaterializer : IMaterializer<ReferenceItem>
    {
        private static readonly ReferencesMaterializer _instance = new ReferencesMaterializer();

        public static ReferencesMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }

        public ReferenceItem Materialize(DataReaderAdapter reader)
        {
            return Materialize_List(reader).FirstOrDefault();
        }

        public List<ReferenceItem> Materialize_List(DataReaderAdapter reader)
        {
            List<ReferenceItem> items = new List<ReferenceItem>();

            while (reader.Read())
            {
                ReferenceItem obj = ReadItemFields(reader);
                items.Add(obj);
            }
            return items;
        }

        public ReferenceItem ReadItemFields(DataReaderAdapter reader, ReferenceItem item = null)
        {
            if (item == null)
            {
                item = new ReferenceItem();
            }

            item.Id = reader.GetInt64("ID");
            item.Name = reader.GetString("Name");
            item.Code = reader.GetString("Code");
            item.StartDate = reader.GetDateTimeNull("StartDate");
            item.StartDate = reader.GetDateTimeNull("EndDate");
            item.IsEnabledForOperator = reader.GetBooleanNull("IsEnabledForOperator");
            item.IsEnabledForRegistrator = reader.GetBooleanNull("IsEnabledForRegistrator");
            return item;
        }

        public ReferenceItem ReadItemFields(DataReaderAdapter reader, string id, string code, string name)
        {
            ReferenceItem item = new ReferenceItem();

            if (reader.IsNotNull(id))
            {
                item.Id = reader.GetInt64(id);
                item.Name = reader.GetString(name);
                item.Code = reader.GetString(code);
            }
            return item;
        }
    }
}
