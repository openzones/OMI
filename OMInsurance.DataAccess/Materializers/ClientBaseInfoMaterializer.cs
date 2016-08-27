using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using System.Collections.Generic;
using System.Linq;

namespace OMInsurance.DataAccess.Materializers
{
    public class ClientBaseInfoMaterializer : IMaterializer<ClientBaseInfo>
    {
        private static readonly ClientBaseInfoMaterializer _instance = new ClientBaseInfoMaterializer();

        public static ClientBaseInfoMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }

        public ClientBaseInfo Materialize(DataReaderAdapter reader)
        {
            return Materialize_List(reader).FirstOrDefault();
        }

        public List<ClientBaseInfo> Materialize_List(DataReaderAdapter reader)
        {
            List<ClientBaseInfo> items = new List<ClientBaseInfo>();

            while (reader.Read())
            {
                ClientBaseInfo obj = ReadItemFields(reader);
                items.Add(obj);
            }
            return items;
        }

        public ClientBaseInfo ReadItemFields(DataReaderAdapter reader, ClientBaseInfo item = null)
        {
            if (item == null)
            {
                item = new ClientBaseInfo();
            }

            item.Id = reader.GetInt64("ID");
            item.Firstname = reader.GetString("FirstName");
            item.Secondname = reader.GetString("Secondname");
            item.Lastname = reader.GetString("Lastname");
            item.TemporaryPolicyNumber = reader.GetString("TemporaryPolicyNumber");
            item.UnifiedPolicyNumber = reader.GetString("UnifiedPolicyNumber");
            item.PolicyNumber = reader.GetString("PolicyNumber");
            item.PolicySeries = reader.GetString("PolicySeries");
            item.Birthday = reader.GetDateTimeNull("Birthday");

            item.TemporaryPolicyDate = reader.GetDateTimeNull("TemporaryPolicyDate");
            item.PolicyDate = reader.GetDateTimeNull("PolicyDate");

            return item;
        }
    }
}
