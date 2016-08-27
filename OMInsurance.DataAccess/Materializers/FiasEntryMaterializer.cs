using OMInsurance.DataAccess.Core;
using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMInsurance.DataAccess.Materializers
{
    public class FiasEntryMaterializer : IMaterializer<FiasEntry>
    {
        private static readonly FiasEntryMaterializer _instance = new FiasEntryMaterializer();

        public static FiasEntryMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }
        public FiasEntry Materialize(DataReaderAdapter dataReader)
        {
            return Materialize_List(dataReader).FirstOrDefault();
        }

        public List<FiasEntry> Materialize_List(DataReaderAdapter dataReader)
        {
            List<FiasEntry> items = new List<FiasEntry>();

            while (dataReader.Read())
            {
                FiasEntry obj = ReadItemFields(dataReader);
                items.Add(obj);
            }

            return items;
        }

        public FiasEntry ReadItemFields(DataReaderAdapter dataReader, FiasEntry item = null)
        {
            if (item == null)
            {
                item = new FiasEntry();
            }
            item.Id = dataReader.GetString("ID");

            item.Name = dataReader.GetString("Name");
            item.FiasType = (FiasType)dataReader.GetInt32("FiasType");
            item.ParentId = dataReader.GetString("ParentId");
            item.RegionCode = dataReader.GetString("RegionCode");
            item.AreaCode = dataReader.GetString("AreaCode");

            return item;
        }
    }
}
