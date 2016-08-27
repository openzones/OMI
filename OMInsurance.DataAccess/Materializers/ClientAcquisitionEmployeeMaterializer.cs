using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using System.Collections.Generic;
using System.Linq;


namespace OMInsurance.DataAccess.Materializers
{
    class ClientAcquisitionEmployeeMaterializer : IMaterializer<ClientAcquisitionEmployee>
    {
        private static readonly ClientAcquisitionEmployeeMaterializer _instance = new ClientAcquisitionEmployeeMaterializer();

        public static ClientAcquisitionEmployeeMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }

        public ClientAcquisitionEmployee Materialize(DataReaderAdapter reader)
        {
            return Materialize_List(reader).FirstOrDefault();
        }

        public List<ClientAcquisitionEmployee> Materialize_List(DataReaderAdapter reader)
        {
            List<ClientAcquisitionEmployee> items = new List<ClientAcquisitionEmployee>();

            while (reader.Read())
            {
                ClientAcquisitionEmployee obj = ReadItemFields(reader);
                items.Add(obj);
            }
            return items;
        }

        public ClientAcquisitionEmployee ReadItemFields(DataReaderAdapter reader, ClientAcquisitionEmployee item = null)
        {
            if (item == null)
            {
                item = new ClientAcquisitionEmployee();
            }
            item.Id = reader.GetInt64("ID");
            item.UserId = reader.GetInt64Null("UserId");
            item.Firstname = reader.GetString("Firstname");
            if (string.IsNullOrEmpty(item.Firstname)) item.Firstname = reader.GetString("First_n");
            item.Lastname = reader.GetString("Lastname");
            if (string.IsNullOrEmpty(item.Lastname)) item.Lastname = reader.GetString("Last_n");
            item.Secondname = reader.GetString("Secondname");
            if (string.IsNullOrEmpty(item.Secondname)) item.Secondname = reader.GetString("Second_n");
            return item;
        }

    }
}
