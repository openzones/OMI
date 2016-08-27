using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using System.Collections.Generic;
using System.Linq;

namespace OMInsurance.DataAccess.Materializers
{
    public class NomernikSTOPMaterializer : IMaterializer <STOP>
    {
        private static readonly NomernikSTOPMaterializer _instance = new NomernikSTOPMaterializer();

        public static NomernikSTOPMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }

        public STOP Materialize(DataReaderAdapter reader)
        {
            return Materialize_List(reader).FirstOrDefault();
        }

        public List<STOP> Materialize_List(DataReaderAdapter reader)
        {
            List<STOP> items = new List<STOP>();

            /// <summary>
            /// выбираем из таблицы стоп-листа (STOP)
            /// </summary>
            while (reader.Read())
            {
                STOP obj = ReadItemFields1(reader);
                items.Add(obj);
            }

            /// <summary>
            /// потом собираем все, что нашли в базе по UnifiedPolicyNumber (ENP)
            /// </summary>
            reader.NextResult();
            while (reader.Read())
            {
                STOP obj = ReadItemFields2(reader);
                items.Add(obj);
            }

            /// <summary>
            /// потом собираем все, что нашли в базе по "старому" номеру полиса N_CARD
            /// </summary>
            reader.NextResult();
            while (reader.Read())
            {
                STOP obj = ReadItemFields3(reader);
                items.Add(obj);
            }
            return items;
        }

        public STOP ReadItemFields1(DataReaderAdapter reader, STOP item = null)
        {
            if (item == null)
            {
                item = new STOP();
            }
            item.Id = reader.GetInt64("ID");
            item.SCENARIO = reader.GetString("SCENARIO");
            item.S_CARD = reader.GetString("S_CARD");
            item.N_CARD = reader.GetString("N_CARD");
            item.ENP = reader.GetString("UnifiedPolicyNumber");
            item.VSN = reader.GetString("TemporaryPolicyNumber");
            item.QZ = reader.GetInt64Null("QZ");
            item.DATE_END = reader.GetDateTimeNull("DATE_END");
            item.DATE_ARC = reader.GetDateTimeNull("DATE_ARC");
            item.IST = reader.GetString("IST");
            item.ClientID = reader.GetInt64Null("ClientID");
            return item;
        }

        public STOP ReadItemFields2(DataReaderAdapter reader, STOP item = null)
        {
            if (item == null)
            {
                item = new STOP();
            }
            item.Id = 0;
            //item.SCENARIO = reader.GetString("SCENARIO");
            //item.S_CARD = reader.GetString("S_CARD");
            //item.N_CARD = reader.GetInt64Null("N_CARD");
            item.ENP = reader.GetString("UnifiedPolicyNumber");
            item.VSN = reader.GetString("TemporaryPolicyNumber");
            //item.QZ = reader.GetInt64Null("QZ");
            //item.DATE_END = reader.GetDateTimeNull("DATE_END");
            //item.DATE_ARC = reader.GetDateTimeNull("DATE_ARC");
            //item.IST = reader.GetString("IST");
            item.ClientID = reader.GetInt64Null("ClientID");
            return item;
        }

        public STOP ReadItemFields3(DataReaderAdapter reader, STOP item = null)
        {
            if (item == null)
            {
                item = new STOP();
            }
            item.Id = 0;
            //item.SCENARIO = reader.GetString("SCENARIO");
            //item.S_CARD = reader.GetString("S_CARD");
            item.N_CARD = reader.GetString("N_CARD");
            item.ENP = reader.GetString("UnifiedPolicyNumber");
            item.VSN = reader.GetString("TemporaryPolicyNumber");
            //item.QZ = reader.GetInt64Null("QZ");
            //item.DATE_END = reader.GetDateTimeNull("DATE_END");
            //item.DATE_ARC = reader.GetDateTimeNull("DATE_ARC");
            //item.IST = reader.GetString("IST");
            item.ClientID = reader.GetInt64Null("ClientID");
            return item;
        }
    }
}
