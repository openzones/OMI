using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using System.Collections.Generic;
using System.Linq;

namespace OMInsurance.DataAccess.Materializers
{
    public class NomernikClientShotInfoMaterializer : IMaterializer<Nomernik.ClientShotInfo>
    {
        private static readonly NomernikClientShotInfoMaterializer _instance = new NomernikClientShotInfoMaterializer();

        public static NomernikClientShotInfoMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }

        public Nomernik.ClientShotInfo Materialize(DataReaderAdapter reader)
        {
            return Materialize_List(reader).FirstOrDefault();
        }

        public List<Nomernik.ClientShotInfo> Materialize_List(DataReaderAdapter reader)
        {
            List<Nomernik.ClientShotInfo> items = new List<Nomernik.ClientShotInfo>();

            while (reader.Read())
            {
                Nomernik.ClientShotInfo obj = ReadItemFields(reader);
                items.Add(obj);
            }

            return items;
        }

        public Nomernik.ClientShotInfo ReadItemFields(DataReaderAdapter reader, Nomernik.ClientShotInfo item = null)
        {
            if (item == null)
            {
                item = new Nomernik.ClientShotInfo();
            }
            item.Id = 0;
            item.Firstname = reader.GetString("Firstname");
            item.Secondname = reader.GetString("Secondname");
            item.Lastname = reader.GetString("Lastname");
            item.Birthday = reader.GetDateTimeNull("Birthday");
            item.ClientID = reader.GetInt64Null("ClientID");
            return item;
        }
    }
}
