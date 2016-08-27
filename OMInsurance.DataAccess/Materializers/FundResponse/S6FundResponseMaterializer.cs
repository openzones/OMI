using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System.Collections.Generic;
using System.Linq;

namespace OMInsurance.DataAccess.Materializers
{
    public class S6FundResponseMaterializer : IMaterializer<S6FundResponse>
    {
        private static readonly S6FundResponseMaterializer _instance = new S6FundResponseMaterializer();

        public static S6FundResponseMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }
        public S6FundResponse Materialize(DataReaderAdapter dataReader)
        {
            return Materialize_List(dataReader).FirstOrDefault();
        }

        public List<S6FundResponse> Materialize_List(DataReaderAdapter dataReader)
        {
            List<S6FundResponse> items = new List<S6FundResponse>();

            while (dataReader.Read())
            {
                S6FundResponse obj = ReadItemFields(dataReader);
                items.Add(obj);
            }

            return items;
        }

        public S6FundResponse ReadItemFields(DataReaderAdapter dataReader, S6FundResponse item = null)
        {
            if (item == null)
            {
                item = new S6FundResponse();
            }
            item.Id = dataReader.GetInt64("ID");
            item.ClientVisitId = dataReader.GetInt64("ClientVisitID");
            item.ClientVisitGroupId = dataReader.GetInt64("VisitGroupID");
            item.PolicyType = ReferencesMaterializer.Instance.ReadItemFields(dataReader, "PolicyTypeId", "PolicyTypeCode", "PolicyTypeName");
            item.UnifiedPolicyNumber = dataReader.GetString("UnifiedPolicyNumber");
            item.PolicySeries = dataReader.GetString("PolicySeries");
            item.PolicyNumber = dataReader.GetString("PolicyNumber");
            item.OKATO = dataReader.GetString("OKATO");
            item.OGRN = dataReader.GetString("OGRN");
            item.StartDate = dataReader.GetDateTimeNull("StartDate");
            item.ExpirationDate = dataReader.GetDateTimeNull("ExpirationDate");
            item.FundAnswer = dataReader.GetString("FundAnswer");
            item.ErrorMessage = dataReader.GetString("ErrorMessage");
            item.FundAnswer = dataReader.GetString("FundAnswer");
            item.DataType = (EntityType)(dataReader.GetInt32Null("DataTypeId") ?? 0);
            item.Order = dataReader.GetInt32("OrderNumber");
            item.CreateDate = dataReader.GetDateTime("CreateDate");
            return item;
        }
    }

}
