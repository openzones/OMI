using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using System.Collections.Generic;
using System.Linq;

namespace OMInsurance.DataAccess.Materializers
{
    public class NomernikClientNOMPMaterializer : IMaterializer<NomernikForClient>
    {
        private static readonly NomernikClientNOMPMaterializer _instance = new NomernikClientNOMPMaterializer();

        public static NomernikClientNOMPMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }

        public NomernikForClient Materialize(DataReaderAdapter reader)
        {
            return Materialize_List(reader).FirstOrDefault();
        }

        public List<NomernikForClient> Materialize_List(DataReaderAdapter reader)
        {
            List<NomernikForClient> items = new List<NomernikForClient>();

            while (reader.Read())
            {
                NomernikForClient obj = ReadItemFields(reader);
                items.Add(obj);
            }
            return items;
        }

        public NomernikForClient ReadItemFields(DataReaderAdapter reader, NomernikForClient item = null)
        {
            if (item == null)
            {
                item = new NomernikForClient();
            }
            item.Id = reader.GetInt64("ID");
            item.S_CARD = reader.GetString("S_CARD");
            item.N_CARD = reader.GetString("N_CARD");
            item.ENP = reader.GetString("UnifiedPolicyNumber");
            item.VSN = reader.GetString("TemporaryPolicyNumber");
            item.LPU_ID = reader.GetInt64Null("LPU_ID");
            item.DATE_IN = reader.GetDateTimeNull("DATE_IN");
            item.SPOS = reader.GetInt32Null("SPOS");
            item.ClientID = reader.GetInt64Null("ClientID");
            item.LoadDate = reader.GetDateTime("LoadDate");
            item.FileDate = reader.GetDateTime("FileDate");
            item.Firstname = reader.GetString("Firstname");
            item.Secondname = reader.GetString("Secondname");
            item.Lastname = reader.GetString("Lastname");
            return item;
        }
    }
}
