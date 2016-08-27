using OMInsurance.DataAccess.Core;
using OMInsurance.Entities.SMS;
using System.Collections.Generic;
using System.Linq;

namespace OMInsurance.DataAccess.Materializers
{
    public class SmsTemplateMaterializer : IMaterializer<SmsTemplate>
    {
        private static readonly SmsTemplateMaterializer _instance = new SmsTemplateMaterializer();

        public static SmsTemplateMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }

        public SmsTemplate Materialize(DataReaderAdapter dataReader)
        {
            return Materialize_List(dataReader).FirstOrDefault();
        }

        public List<SmsTemplate> Materialize_List(DataReaderAdapter dataReader)
        {
            List<SmsTemplate> items = new List<SmsTemplate>();

            while (dataReader.Read())
            {
                SmsTemplate obj = ReadItemFields(dataReader);
                items.Add(obj);
            }
            return items;
        }

        public SmsTemplate ReadItemFields(DataReaderAdapter dataReader, SmsTemplate item = null)
        {
            if (item == null)
            {
                item = new SmsTemplate();
            }
            item.SenderId = string.IsNullOrEmpty(dataReader.GetString("SenderId")) ? dataReader.GetString("SenderId") : dataReader.GetString("SenderId").Trim();
            item.Phone = dataReader.GetString("Phone");
            item.Message = dataReader.GetString("Message");
            item.CreateDate = dataReader.GetDateTimeNull("CreateDate");
            item.StatusId = dataReader.GetInt64Null("StatusId");
            return item;
        }

    }
}
