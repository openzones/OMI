using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMInsurance.DataAccess.Materializers
{
    public class FundErrorResponseMaterializer : IMaterializer<FundErrorResponse>
    {
        private static readonly FundErrorResponseMaterializer _instance = new FundErrorResponseMaterializer();

        public static FundErrorResponseMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }
        public FundErrorResponse Materialize(DataReaderAdapter dataReader)
        {
            return Materialize_List(dataReader).FirstOrDefault();
        }

        public List<FundErrorResponse> Materialize_List(DataReaderAdapter dataReader)
        {
            List<FundErrorResponse> items = new List<FundErrorResponse>();

            while (dataReader.Read())
            {
                FundErrorResponse obj = ReadItemFields(dataReader);
                items.Add(obj);
            }

            return items;
        }

        public FundErrorResponse ReadItemFields(DataReaderAdapter dataReader, FundErrorResponse item = null)
        {
            if (item == null)
            {
                item = new FundErrorResponse();
            }

            item.Id = dataReader.GetInt64("ID");
            item.LID = dataReader.GetInt64("LID");
            item.ClientVisitId = dataReader.GetInt64("ClientVisitID");
            item.ClientVisitGroupId = dataReader.GetInt64("VisitGroupID");
            item.PolicyType = ReferencesMaterializer.Instance.ReadItemFields(dataReader, "PolicyTypeId", "PolicyTypeCode", "PolicyTypeName");
            item.FundResponseStage = ReferencesMaterializer.Instance.ReadItemFields(dataReader, "FundResponseStageId", "FundResponseStageCode", "FundResponseStageName");
            item.DeliveryCenter = ReferencesMaterializer.Instance.ReadItemFields(dataReader, "DeliveryCenterId", "DeliveryCenterCode", "DeliveryCenterName");
            item.UnifiedPolicyNumber = dataReader.GetString("UnifiedPolicyNumber");
            item.PolicySeries = dataReader.GetString("PolicySeries");
            item.PolicyNumber = dataReader.GetString("PolicyNumber");
            item.OrderNumber = dataReader.GetInt32("OrderNumber");
            item.SMO_ID = dataReader.GetInt32("SMO_ID");
            item.OGRN = dataReader.GetString("OGRN");
            item.Firstname = dataReader.GetString("Firstname");
            item.Secondname = dataReader.GetString("Secondname");
            item.Lastname = dataReader.GetString("Lastname");
            item.TemporaryPolicyDate = dataReader.GetDateTimeNull("TemporaryPolicyDate");
            item.ExpirationDate = dataReader.GetDateTimeNull("ExpirationDate");
            item.UnifiedPolicyNumber = dataReader.GetString("UnifiedPolicyNumber");
            item.SNILS = dataReader.GetString("SNILS");
            item.Sex = dataReader.GetString("Sex");
            item.Birthday = dataReader.GetDateTimeNull("Birthday");
            item.ErrorText = dataReader.GetString("ErrorText");
            item.TerritoryCode = dataReader.GetString("TerritoryCode");
            item.CreateDate = dataReader.GetDateTime("CreateDate");
            return item;
        }
    }

}
