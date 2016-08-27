using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using System.Collections.Generic;
using System.Linq;


namespace OMInsurance.DataAccess.Materializers
{
    public class BSOInfoMaterializer : IMaterializer<BSOInfo>
    {
        private static readonly BSOInfoMaterializer _instance = new BSOInfoMaterializer();

        public static BSOInfoMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }

        public BSOInfo Materialize(DataReaderAdapter dataReader)
        {
            return Materialize_List(dataReader).FirstOrDefault();
        }

        public List<BSOInfo> Materialize_List(DataReaderAdapter dataReader)
        {
            List<BSOInfo> items = new List<BSOInfo>();

            while (dataReader.Read())
            {
                BSOInfo obj = ReadItemFields(dataReader);
                items.Add(obj);
            }
            return items;
        }

        public BSOInfo ReadItemFields(DataReaderAdapter dataReader, BSOInfo item = null)
        {
            if (item == null)
            {
                item = new BSOInfo();
            }
            item.Id = dataReader.GetInt64("BSO_ID");
            item.PolicyPartyNumber = dataReader.GetString("PolicyPartyNumber");
            item.TemporaryPolicyNumber = dataReader.GetString("TemporaryPolicyNumber");
            item.Status = BSOStatusRefMaterializer.Instance.ReadItemFields(dataReader, "StatusID", "StatusName");
            item.StatusDate = dataReader.GetDateTimeNull("StatusDate");
            item.Comment = dataReader.GetString("Comment");
            item.DeliveryCenterId = dataReader.GetInt64Null("DeliveryCenterID");
            item.DeliveryCenter = dataReader.GetString("DeliveryCenter");
            item.DeliveryPointId = dataReader.GetInt64Null("DeliveryPointID");
            item.DeliveryPoint = dataReader.GetString("DeliveryPoint");
            item.ResponsibleID = dataReader.GetInt64Null("ResponsibleID");
            item.ResponsibleName = dataReader.GetString("ResponsibleName");
            item.ChangeDate = dataReader.GetDateTimeNull("ChangeDate");
            return item;
        }
    }
}
