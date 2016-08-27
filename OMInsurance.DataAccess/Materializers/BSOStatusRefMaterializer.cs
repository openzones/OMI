using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using System.Collections.Generic;
using System.Linq;

namespace OMInsurance.DataAccess.Materializers
{
    public class BSOStatusRefMaterializer : IMaterializer<BSOStatusRef>
    {
        private static readonly BSOStatusRefMaterializer _instance = new BSOStatusRefMaterializer();

        public static BSOStatusRefMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }

        public BSOStatusRef Materialize(DataReaderAdapter reader)
        {
            return Materialize_List(reader).FirstOrDefault();
        }

        public List<BSOStatusRef> Materialize_List(DataReaderAdapter reader)
        {
            List<BSOStatusRef> items = new List<BSOStatusRef>();

            while (reader.Read())
            {
                BSOStatusRef obj = ReadItemFields(reader);
                items.Add(obj);
            }
            return items;
        }

        public BSOStatusRef ReadItemFields(DataReaderAdapter reader, BSOStatusRef item = null)
        {
            if (item == null)
            {
                item = new BSOStatusRef();
            }

            item.Id = reader.GetInt64("StatusID");
            item.Name = reader.GetString("Name");
            return item;
        }

        public BSOStatusRef ReadItemFields(DataReaderAdapter reader, string id, string name)
        {
            BSOStatusRef item = new BSOStatusRef();

            if (reader.IsNotNull(id))
            {
                item.Id = reader.GetInt64(id);
                item.Name = reader.GetString(name);
            }
            return item;
        }
    }
}
