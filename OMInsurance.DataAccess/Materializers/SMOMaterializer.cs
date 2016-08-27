using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using System.Collections.Generic;
using System.Linq;

namespace OMInsurance.DataAccess.Materializers
{
    public class SMOMaterializer : IMaterializer<SMO>
    {
        private static readonly SMOMaterializer _instance = new SMOMaterializer();

        public static SMOMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }
        public SMO Materialize(DataReaderAdapter dataReader)
        {
            return Materialize_List(dataReader).FirstOrDefault();
        }

        public List<SMO> Materialize_List(DataReaderAdapter dataReader)
        {
            List<SMO> items = new List<SMO>();

            while (dataReader.Read())
            {
                SMO obj = ReadItemFields(dataReader);
                items.Add(obj);
            }

            return items;
        }

        public SMO ReadItemFields(DataReaderAdapter dataReader, SMO item = null)
        {
            if (item == null)
            {
                item = new SMO();
            }
            item.Id = dataReader.GetInt64("ID");
            item.RegionCode = dataReader.GetDouble("RegionCode").ToString();
            item.TerritoryName = dataReader.GetString("TerritoryName");
            item.Shortname = dataReader.GetString("Shortname");
            item.Fullname = dataReader.GetString("Fullname");
            item.OGRN = dataReader.GetString("OGRN");
            item.SMOCode = dataReader.GetString("SMOCode");
            item.OKATO = dataReader.GetString("OKATO");

            return item;
        }
    }
}
