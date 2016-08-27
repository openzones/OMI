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
    public class S9FundResponseMaterializer : IMaterializer<S9FundResponse>
    {
        private static readonly S9FundResponseMaterializer _instance = new S9FundResponseMaterializer();

        public static S9FundResponseMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }

        public S9FundResponse Materialize(DataReaderAdapter dataReader)
        {
            return Materialize_List(dataReader).FirstOrDefault();
        }

        public List<S9FundResponse> Materialize_List(DataReaderAdapter dataReader)
        {
            List<S9FundResponse> items = new List<S9FundResponse>();

            while (dataReader.Read())
            {
                S9FundResponse obj = ReadItemFields(dataReader);
                items.Add(obj);
            }

            return items;
        }

        public S9FundResponse ReadItemFields(DataReaderAdapter dataReader, S9FundResponse item = null)
        {
            if (item == null)
            {
                item = new S9FundResponse();
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
