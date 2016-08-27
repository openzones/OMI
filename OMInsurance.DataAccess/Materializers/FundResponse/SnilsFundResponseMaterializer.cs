using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System.Collections.Generic;
using System.Linq;

namespace OMInsurance.DataAccess.Materializers
{
    public class SnilsFundResponseMaterializer : IMaterializer<SnilsFundResponse>
    {
        private static readonly SnilsFundResponseMaterializer _instance = new SnilsFundResponseMaterializer();

        public static SnilsFundResponseMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }
        public SnilsFundResponse Materialize(DataReaderAdapter dataReader)
        {
            return Materialize_List(dataReader).FirstOrDefault();
        }

        public List<SnilsFundResponse> Materialize_List(DataReaderAdapter dataReader)
        {
            List<SnilsFundResponse> items = new List<SnilsFundResponse>();

            while (dataReader.Read())
            {
                SnilsFundResponse obj = ReadItemFields(dataReader);
                items.Add(obj);
            }

            return items;
        }

        public SnilsFundResponse ReadItemFields(DataReaderAdapter dataReader, SnilsFundResponse item = null)
        {
            if (item == null)
            {
                item = new SnilsFundResponse();
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
