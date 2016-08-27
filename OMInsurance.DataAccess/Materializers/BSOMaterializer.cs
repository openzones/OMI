using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using System.Collections.Generic;
using System.Linq;


namespace OMInsurance.DataAccess.Materializers
{
    public class BSOMaterializer : IMaterializer<BSO>
    {
        private static readonly BSOMaterializer _instance = new BSOMaterializer();

        public static BSOMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }

        public BSO Materialize(DataReaderAdapter dataReader)
        {
            return Materialize_List(dataReader).FirstOrDefault();
        }

        public List<BSO> Materialize_List(DataReaderAdapter dataReader)
        {
            List<BSO> items = new List<BSO>();
            Dictionary<long, BSO> bsoById = new Dictionary<long, BSO>();

            while (dataReader.Read())
            {
                BSO obj = ReadItemFields(dataReader);
                bsoById.Add(obj.Id, obj);
                items.Add(obj);
            }
            dataReader.NextResult();

            while (dataReader.Read())
            {
                long Id = dataReader.GetInt64("BSO_ID");
                bsoById[Id].History.Add(BSOHistoryItemMaterializer.Instance.ReadItemFields(dataReader));
            }
            return items;
        }

        public BSO ReadItemFields(DataReaderAdapter dataReader, BSO item = null)
        {
            if( item == null)
            {
                item = new BSO();
            }
            item.Id = dataReader.GetInt64("BSO_ID");
            item.TemporaryPolicyNumber = dataReader.GetString("TemporaryPolicyNumber"); 
            item.PolicyPartyNumber = dataReader.GetString("PolicyPartyNumber"); ;
            item.Status = BSOStatusRefMaterializer.Instance.ReadItemFields(dataReader, "StatusID", "StatusName");
            item.StatusDate = dataReader.GetDateTimeNull("StatusDate");
            item.VisitGroupId = dataReader.GetInt64Null("VisitGroupId");
            item.DeliveryCenterId = dataReader.GetInt64Null("DeliveryCenterID");
            item.DeliveryCenter = dataReader.GetString("DeliveryCenter");
            item.DeliveryPointId = dataReader.GetInt64Null("DeliveryPointId");
            item.DeliveryPoint = dataReader.GetString("DeliveryPoint");
            item.Comment = dataReader.GetString("Comment");
            item.UserId = dataReader.GetInt64Null("UserID");
            item.ResponsibleID = dataReader.GetInt64Null("ResponsibleID");
            item.ChangeDate = dataReader.GetDateTimeNull("ChangeDate");
            return item;
        }
    }
}
