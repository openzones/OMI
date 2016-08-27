using OMInsurance.DataAccess.Core;
using OMInsurance.Entities.Core;
using OMInsurance.Entities;
using System.Collections.Generic;
using System.Linq;

namespace OMInsurance.DataAccess.Materializers
{
    public class ReferenceUniversalItemMaterializer : IMaterializer<ReferenceUniversalItem>
    {
        private static readonly ReferenceUniversalItemMaterializer _instance = new ReferenceUniversalItemMaterializer();

        public static ReferenceUniversalItemMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }

        public ReferenceUniversalItem Materialize(DataReaderAdapter reader)
        {
            return Materialize_List(reader).FirstOrDefault();
        }
        public List<ReferenceUniversalItem> Materialize_List(DataReaderAdapter reader)
        {
            List<ReferenceUniversalItem> items = new List<ReferenceUniversalItem>();

            while (reader.Read())
            {
                ReferenceUniversalItem obj = ReadItemFields(reader);
                items.Add(obj);
            }
            return items;
        }
        public ReferenceUniversalItem ReadItemFields(DataReaderAdapter reader, ReferenceUniversalItem item = null)
        {
            if (item == null)
            {
                item = new ReferenceUniversalItem();
            }
            item.ReferenceName = reader.GetString("ReferenceName");
            item.Id = reader.GetInt64("ID");
            item.Name = reader.GetString("Name");
            item.Code = reader.GetString("Code");
            item.StartDate = reader.GetDateTimeNull("StartDate");
            item.EndDate = reader.GetDateTimeNull("EndDate");
            item.IsEnabledForOperator = reader.GetBooleanNull("IsEnabledForOperator");
            item.IsEnabledForRegistrator = reader.GetBooleanNull("IsEnabledForRegistrator");

            item.SMO = reader.GetString("SMO");
            item.DisplayName = reader.GetString("DisplayName");
            item.District = reader.GetString("District");
            item.Address = reader.GetString("Address");
            item.Phone = reader.GetString("Phone");
            item.WorkHours = reader.GetString("WorkHours");
            item.IsDigitPolicyAbailable = reader.GetBooleanNull("IsEnabledForRegistrator");
            item.ParentId = reader.GetInt64Null("ParentId");
            item.IsMFC = reader.GetBooleanNull("IsMFC");

            item.DeliveryCenterId = reader.GetInt64Null("DeliveryCenterId");
            item.DeliveryPointHeadId = reader.GetInt64Null("DeliveryPointHeadId");
            item.SendSms = reader.GetBooleanNull("SendSms");

            item.LPU_ID_AIS = reader.GetInt64Null("LPU_ID_AIS");
            item.FIL_ID = reader.GetInt64Null("FIL_ID");
            item.MCOD = reader.GetString("MCOD");
            item.FULLNAME = reader.GetString("FULLNAME");
            item.OGRN = reader.GetString("OGRN");
            item.FCOD = reader.GetString("FCOD");

            item.Lastname = reader.GetString("Lastname");
            item.Firstname = reader.GetString("Firstname");
            item.Secondname = reader.GetString("Secondname");

            item.ErrCode = reader.GetString("ErrCode");

            return item;
        }

    }
}
