using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMInsurance.DataAccess.Materializers
{
    public class StreetMaterializer : IMaterializer<Street>
    {
        private static readonly StreetMaterializer _instance = new StreetMaterializer();

        public static StreetMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }
        public Street Materialize(DataReaderAdapter dataReader)
        {
            return Materialize_List(dataReader).FirstOrDefault();
        }

        public List<Street> Materialize_List(DataReaderAdapter dataReader)
        {
            List<Street> items = new List<Street>();

            while (dataReader.Read())
            {
                Street obj = ReadItemFields(dataReader);
                items.Add(obj);
            }

            return items;
        }

        public Street ReadItemFields(DataReaderAdapter dataReader, Street item = null)
        {
            if (item == null)
            {
                item = new Street();
            }

            item.Code = dataReader.GetString("Code");
            item.Name = dataReader.GetString("Name");
            item.Property = dataReader.GetString("Property");

            return item;
        }
    }
}
