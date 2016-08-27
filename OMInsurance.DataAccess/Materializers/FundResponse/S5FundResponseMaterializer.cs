using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMInsurance.DataAccess.Materializers
{
    public class S5FundResponseMaterializer : IMaterializer<S5FundResponse>
    {
        private static readonly S5FundResponseMaterializer _instance = new S5FundResponseMaterializer();

        public static S5FundResponseMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }
        public S5FundResponse Materialize(DataReaderAdapter dataReader)
        {
            return Materialize_List(dataReader).FirstOrDefault();
        }

        public List<S5FundResponse> Materialize_List(DataReaderAdapter dataReader)
        {
            List<S5FundResponse> items = new List<S5FundResponse>();

            while (dataReader.Read())
            {
                S5FundResponse obj = ReadItemFields(dataReader);
                items.Add(obj);
            }

            return items;
        }

        public S5FundResponse ReadItemFields(DataReaderAdapter dataReader, S5FundResponse item = null)
        {
            if (item == null)
            {
                item = new S5FundResponse();
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
            item.Order = dataReader.GetInt32("OrderNumber");
            item.DataType = (EntityType)(dataReader.GetInt32Null("DataTypeId") ?? 0);
            item.CreateDate = dataReader.GetDateTime("CreateDate");
            return item;
        }
    }

}
