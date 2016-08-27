using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using System.Collections.Generic;
using System.Linq;

namespace OMInsurance.DataAccess.Materializers
{
    internal class ClientVersionMaterializer : IMaterializer<ClientVersion>
    {
        private static readonly ClientVersionMaterializer _instance = new ClientVersionMaterializer();

        public static ClientVersionMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }

        public ClientVersion Materialize(DataReaderAdapter reader)
        {
            return Materialize_List(reader).FirstOrDefault();
        }

        public List<ClientVersion> Materialize_List(DataReaderAdapter reader)
        {
            List<ClientVersion> items = new List<ClientVersion>();

            while (reader.Read())
            {
                ClientVersion obj = ReadItemFields(reader);
                items.Add(obj);
            }
            return items;
        }

        public ClientVersion ReadItemFields(DataReaderAdapter reader, ClientVersion item = null)
        {
            if (item == null)
            {
                item = new ClientVersion();
            }

            item.Id = reader.GetInt64("ID");
            item.Firstname = reader.GetString("FirstName");
            item.Secondname = reader.GetString("Secondname");
            item.Lastname = reader.GetString("Lastname");
            item.Birthday = reader.GetDateTimeNull("Birthday");
            item.Birthplace = reader.GetString("Birthplace");
            item.Sex = reader.GetString("Sex");
            item.SNILS = reader.GetString("SNILS");
            item.Citizenship = ReferencesMaterializer.Instance.ReadItemFields(reader, "CitizenshipID", "CitizenshipCode", "CitizenshipName");
            item.FirstnameType = ReferencesMaterializer.Instance.ReadItemFields(reader, "FirstnameTypeID", "FirstnameTypeCode", "FirstnameTypeName");
            item.SecondnameType = ReferencesMaterializer.Instance.ReadItemFields(reader, "SecondnameTypeID", "SecondnameTypeCode", "SecondnameTypeName");
            item.LastnameType = ReferencesMaterializer.Instance.ReadItemFields(reader, "LastnameTypeID", "LastnameTypeCode", "LastnameTypeName");
            item.Category = ReferencesMaterializer.Instance.ReadItemFields(reader, "CategoryID", "CategoryCode", "CategoryName");

            return item;
        }
    }
}
