using OMInsurance.DataAccess.Core;
using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMInsurance.DataAccess.Materializers
{
    public class RoleMaterializer : IMaterializer<Role>
    {
        private static readonly RoleMaterializer _instance = new RoleMaterializer();

        public static RoleMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }
        public Role Materialize(DataReaderAdapter dataReader)
        {
            return Materialize_List(dataReader).FirstOrDefault();
        }

        public List<Role> Materialize_List(DataReaderAdapter dataReader)
        {
            List<Role> items = new List<Role>();

            while (dataReader.Read())
            {
                Role obj = ReadItemFields(dataReader);
                items.Add(obj);
            }

            return items;
        }

        public Role ReadItemFields(DataReaderAdapter dataReader, Role item = null)
        {
            if (item == null)
            {
                item = new Role();
            }
            item.Id = dataReader.GetInt64("ID");
            item.Name = dataReader.GetString("Name");
            item.Description = dataReader.GetString("Description");

            return item;
        }
    }
}
