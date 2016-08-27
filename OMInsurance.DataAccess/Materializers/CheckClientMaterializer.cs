using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using OMInsurance.Entities.Check;
using System.Collections.Generic;
using System.Linq;


namespace OMInsurance.DataAccess.Materializers
{
    public class CheckClientMaterializer : IMaterializer<CheckClient>
    {
        private static readonly CheckClientMaterializer _instance = new CheckClientMaterializer();

        public static CheckClientMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }

        public CheckClient Materialize(DataReaderAdapter reader)
        {
            return Materialize_List(reader).FirstOrDefault();
        }

        public List<CheckClient> Materialize_List(DataReaderAdapter reader)
        {
            List<CheckClient> items = new List<CheckClient>();

            while (reader.Read())
            {
                CheckClient obj = ReadItemFields(reader);
                items.Add(obj);
            }
            return items;
        }

        public CheckClient ReadItemFields(DataReaderAdapter reader, CheckClient item = null)
        {
            if (item == null)
            {
                item = new CheckClient();
            }

            item.Id = reader.GetInt64("ClientID");
            item.Lastname = reader.GetString("Lastname");
            item.Firstname = reader.GetString("Firstname");
            item.Secondname = reader.GetString("Secondname");
            item.Sex = reader.GetString("Sex");
            item.Birthday = reader.GetDateTimeNull("Birthday");
            item.PolicySeries = reader.GetString("PolicySeries");
            item.PolicyNumber = reader.GetString("PolicyNumber");
            item.UnifiedPolicyNumber = reader.GetString("UnifiedPolicyNumber");
            item.DocumentSeries = reader.GetString("DocumentSeries");
            item.DocumentNumber = reader.GetString("DocumentNumber");
            item.LivingFullAddressString = reader.GetString("LivingFullAddressString");
            item.OfficialFullAddressString = reader.GetString("OfficialFullAddressString");
            item.TemporaryPolicyNumber = reader.GetString("TemporaryPolicyNumber");
            item.TemporaryPolicyDate = reader.GetDateTimeNull("TemporaryPolicyDate");
            item.SNILS = reader.GetString("SNILS");
            item.Phone = reader.GetString("Phone");
            return item;
        }
    }
}
