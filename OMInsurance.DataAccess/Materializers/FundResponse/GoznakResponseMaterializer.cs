using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMInsurance.DataAccess.Materializers
{
    public class GoznakResponseMaterializer : IMaterializer<GoznakResponse>
    {
        private static readonly GoznakResponseMaterializer _instance = new GoznakResponseMaterializer();

        public static GoznakResponseMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }
        public GoznakResponse Materialize(DataReaderAdapter dataReader)
        {
            return Materialize_List(dataReader).FirstOrDefault();
        }

        public List<GoznakResponse> Materialize_List(DataReaderAdapter dataReader)
        {
            List<GoznakResponse> items = new List<GoznakResponse>();

            while (dataReader.Read())
            {
                GoznakResponse obj = ReadItemFields(dataReader);
                items.Add(obj);
            }

            return items;
        }

        public GoznakResponse ReadItemFields(DataReaderAdapter dataReader, GoznakResponse item = null)
        {
            if (item == null)
            {
                item = new GoznakResponse();
            }

            item.Id = dataReader.GetInt64("ID");
            item.ClientVisitId = dataReader.GetInt64("ClientVisitID");
            item.ClientVisitGroupId = dataReader.GetInt64("VisitGroupID");
            item.DeliveryCenter = ReferencesMaterializer.Instance.ReadItemFields(dataReader, "DeliveryCenterId", "DeliveryCenterCode", "DeliveryCenterName");
            item.UnifiedPolicyNumber = dataReader.GetString("UnifiedPolicyNumber");
            item.Sex = dataReader.GetString("Sex");
            item.TemporaryPolicyNumber = dataReader.GetString("TemporaryPolicyNumber");
            item.Firstname = dataReader.GetString("Firstname");
            item.Secondname = dataReader.GetString("Secondname");
            item.Lastname = dataReader.GetString("Lastname");
            item.Birthday = dataReader.GetDateTime("Birthday");
            item.CreateDate = dataReader.GetDateTime("UpdateDate");
            item.UpdateDate = dataReader.GetDateTime("UpdateDate");
            return item;
        }
    }

}
