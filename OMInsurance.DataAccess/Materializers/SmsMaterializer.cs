using OMInsurance.DataAccess.Core;
using OMInsurance.Entities.SMS;
using System.Collections.Generic;
using System.Linq;

namespace OMInsurance.DataAccess.Materializers
{
    public class SmsMaterializer : IMaterializer<SmsBase>
    {
        private static readonly SmsMaterializer _instance = new SmsMaterializer();

        public static SmsMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }

        public SmsBase Materialize(DataReaderAdapter reader)
        {
            return Materialize_List(reader).FirstOrDefault();
        }

        public List<SmsBase> Materialize_List(DataReaderAdapter reader)
        {
            List<SmsBase> items = new List<SmsBase>();

            while (reader.Read())
            {
                SmsBase obj = ReadItemFields(reader);
                items.Add(obj);
            }
            return items;
        }

        public SmsBase ReadItemFields(DataReaderAdapter reader, SmsBase item = null)
        {
            if (item == null)
            {
                item = new SmsBase();
            }
            item.SenderId = reader.GetString("SenderId");
            item.ClientId = reader.GetInt64("ClientId");
            item.VisitGroupId = reader.GetInt64("VisitGroupId");
            item.VisitId = reader.GetInt64("VisitId");
            item.Phone = reader.GetString("Phone");
            item.Message = reader.GetString("Message");
            item.CreateDate = reader.GetDateTime("CreateDate");
            item.Comment = reader.GetString("Comment");
            item.StatusIdInside = reader.GetInt64Null("StatusIdInside");
            item.StatusFromService = reader.GetString("StatusFromService");
            item.MessageFromService = reader.GetString("MessageFromService");
            item.SendDate = reader.GetDateTimeNull("SendDate");
            item.StatusIdRepeat = reader.GetInt64Null("StatusIdRepeat");
            item.StatuRepeatDate = reader.GetDateTimeNull("StatuRepeatDate");
             return item;
        }
    }
}
