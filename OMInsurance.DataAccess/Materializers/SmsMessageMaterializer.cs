using OMInsurance.DataAccess.Core;
using OMInsurance.Entities.SMS;
using System.Collections.Generic;
using System.Linq;

namespace OMInsurance.DataAccess.Materializers
{
    public class SmsMessageMaterializer : IMaterializer<SMSMessage>
    {
        private static readonly SmsMessageMaterializer _instance = new SmsMessageMaterializer();

        public static SmsMessageMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }

        public SMSMessage Materialize(DataReaderAdapter dataReader)
        {
            return Materialize_List(dataReader).FirstOrDefault();
        }

        public List<SMSMessage> Materialize_List(DataReaderAdapter dataReader)
        {
            List<SMSMessage> items = new List<SMSMessage>();

            while (dataReader.Read())
            {
                SMSMessage obj = ReadItemFields(dataReader);
                items.Add(obj);
            }
            return items;
        }

        public SMSMessage ReadItemFields(DataReaderAdapter dataReader, SMSMessage item = null)
        {
            if (item == null)
            {
                item = new SMSMessage();
            }
            item.Id = dataReader.GetInt64("ID");
            item.SenderId = dataReader.GetString("SenderId");
            item.Phone = dataReader.GetString("Phone");
            item.Message = dataReader.GetString("Message");
            return item;
        }
    }
}
