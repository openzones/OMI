using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMInsurance.DataAccess.Materializers
{
    public class DeliveryCenterMaterializer : IMaterializer<DeliveryCenter>
    {
        private static readonly DeliveryCenterMaterializer _instance = new DeliveryCenterMaterializer();

        public static DeliveryCenterMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }
        public DeliveryCenter Materialize(DataReaderAdapter dataReader)
        {
            return Materialize_List(dataReader).FirstOrDefault();
        }

        public List<DeliveryCenter> Materialize_List(DataReaderAdapter dataReader)
        {
            List<DeliveryCenter> items = new List<DeliveryCenter>();

            while (dataReader.Read())
            {
                DeliveryCenter obj = ReadItemFields(dataReader);
                items.Add(obj);
            }

            return items;
        }

        public DeliveryCenter ReadItemFields(DataReaderAdapter reader)
        {
            DeliveryCenter dc = new DeliveryCenter();
            dc.Id = reader.GetInt64("DeliveryCenterID");
            dc.Name = reader.GetString("DeliveryCenterName");
            dc.Code = reader.GetString("DeliveryCenterCode");
            dc.Address = reader.GetString("DeliveryCenterAddress");
            dc.DisplayName = reader.GetString("DeliveryCenterDisplayName");
            dc.District = reader.GetString("DeliveryCenterDistrict");
            dc.IsDigitPolicyAbailable = reader.GetBooleanNull("DeliveryCenterIsDigitPolicyAbailable");
            dc.ParentId = reader.GetInt64Null("DeliveryCenterParentId");
            dc.Phone = reader.GetString("DeliveryCenterPhone");
            dc.SMO = reader.GetString("DeliveryCenterSMO");
            dc.WorkHours = reader.GetString("DeliveryCenterWorkHours");
            return dc;
        }
    }
}
