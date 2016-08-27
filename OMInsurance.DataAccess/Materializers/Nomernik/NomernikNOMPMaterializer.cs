using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using System.Collections.Generic;
using System.Linq;


namespace OMInsurance.DataAccess.Materializers
{
    public class NomernikNOMPMaterializer : IMaterializer<NOMP>
    {
        private static readonly NomernikNOMPMaterializer _instance = new NomernikNOMPMaterializer();

        public static NomernikNOMPMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }

        public NOMP Materialize(DataReaderAdapter reader)
        {
            return Materialize_List(reader).FirstOrDefault();
        }

        public List<NOMP> Materialize_List(DataReaderAdapter reader)
        {
            List<NOMP> items = new List<NOMP>();

            /// <summary>
            /// выбираем из таблицы номерников (NOMP)
            /// </summary>
            while (reader.Read())
            {
                NOMP obj = ReadItemFields1(reader);
                items.Add(obj);
            }

            /// <summary>
            /// потом собираем все, что нашли в базе по UnifiedPolicyNumber (ENP)
            /// </summary>
            reader.NextResult();
            while (reader.Read())
            {
                NOMP obj = ReadItemFields2(reader);
                items.Add(obj);
            }

            /// <summary>
            /// потом собираем все, что нашли в базе по "старому" номеру полиса N_CARD
            /// </summary>
            reader.NextResult();
            while (reader.Read())
            {
                NOMP obj = ReadItemFields3(reader);
                items.Add(obj);
            }
            return items;
        }

        public NOMP ReadItemFields1(DataReaderAdapter reader, NOMP item = null)
        {
            if (item == null)
            {
                item = new NOMP();
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
            return item;
        }
        public NOMP ReadItemFields2(DataReaderAdapter reader, NOMP item = null)
        {
            if (item == null)
            {
                item = new NOMP();
            }
            item.Id = 0;
            //item.S_CARD = reader.GetString("S_CARD");
            //item.N_CARD = reader.GetInt64Null("N_CARD");
            item.ENP = reader.GetString("UnifiedPolicyNumber");
            item.VSN = reader.GetString("TemporaryPolicyNumber");
            item.LPU_ID = reader.GetInt64Null("LPU_ID");
            item.DATE_IN = reader.GetDateTimeNull("DATE_IN");
            item.SPOS = reader.GetInt32Null("SPOS");
            item.ClientID = reader.GetInt64Null("ClientID");
            return item;
        }

        public NOMP ReadItemFields3(DataReaderAdapter reader, NOMP item = null)
        {
            if (item == null)
            {
                item = new NOMP();
            }
            item.Id = 0;
            //item.S_CARD = reader.GetString("S_CARD");
            item.N_CARD = reader.GetString("N_CARD");
            item.ENP = reader.GetString("UnifiedPolicyNumber");
            item.VSN = reader.GetString("TemporaryPolicyNumber");
            //item.LPU_ID = reader.GetInt64Null("LPU_ID");
            //item.DATE_IN = reader.GetDateTimeNull("DATE_IN");
            //item.SPOS = reader.GetInt32Null("SPOS");
            item.ClientID = reader.GetInt64Null("ClientID");
            return item;
        }
    }
}
