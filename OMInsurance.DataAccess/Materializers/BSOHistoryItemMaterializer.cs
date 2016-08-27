using OMInsurance.DataAccess.Core;
using OMInsurance.Entities.Core;
using OMInsurance.Entities;
using System.Collections.Generic;
using System.Linq;

namespace OMInsurance.DataAccess.Materializers
{
    public class BSOHistoryItemMaterializer : IMaterializer<BSOHistoryItem>
    {
        private static readonly BSOHistoryItemMaterializer _instance = new BSOHistoryItemMaterializer();

        public static BSOHistoryItemMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }

        public BSOHistoryItem Materialize(DataReaderAdapter reader)
        {
            return Materialize_List(reader).FirstOrDefault();
        }

        public List<BSOHistoryItem> Materialize_List(DataReaderAdapter reader)
        {
            List<BSOHistoryItem> items = new List<BSOHistoryItem>();

            while (reader.Read())
            {
                BSOHistoryItem obj = ReadItemFields(reader);
                items.Add(obj);
            }
            return items;
        }

        public BSOHistoryItem ReadItemFields(DataReaderAdapter reader, BSOHistoryItem item = null)
        {
            if (item == null)
            {
                item = new BSOHistoryItem();
            }

            item.Id = reader.GetInt64("BSO_ID");
            item.Status = BSOStatusRefMaterializer.Instance.ReadItemFields(reader, "StatusID", "StatusName");
            item.StatusDate = reader.GetDateTimeNull("StatusDate");
            item.VisitGroupId = reader.GetInt64Null("VisitGroupId");
            item.Comment = reader.GetString("Comment");
            item.DeliveryCenterId = reader.GetInt64Null("DeliveryCenterID");
            item.DeliveryCenter = reader.GetString("DeliveryCenter");
            item.DeliveryPointId = reader.GetInt64Null("DeliveryPointId");
            item.DeliveryPoint = reader.GetString("DeliveryPoint");
            item.UserId = reader.GetInt64("UserID");
            item.ResponsibleID = reader.GetInt64Null("ResponsibleID");
            item.ChangeDate = reader.GetDateTimeNull("ChangeDate");

            return item;
        }
    }
}
