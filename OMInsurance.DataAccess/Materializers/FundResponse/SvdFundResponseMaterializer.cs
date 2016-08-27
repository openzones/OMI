using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System.Collections.Generic;
using System.Linq;

namespace OMInsurance.DataAccess.Materializers
{
    public class SvdFundResponseMaterializer : IMaterializer<SvdFundResponse>
    {
        private static readonly SvdFundResponseMaterializer _instance = new SvdFundResponseMaterializer();

        public static SvdFundResponseMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }
        public SvdFundResponse Materialize(DataReaderAdapter dataReader)
        {
            return Materialize_List(dataReader).FirstOrDefault();
        }

        public List<SvdFundResponse> Materialize_List(DataReaderAdapter dataReader)
        {
            List<SvdFundResponse> items = new List<SvdFundResponse>();

            while (dataReader.Read())
            {
                SvdFundResponse obj = ReadItemFields(dataReader);
                items.Add(obj);
            }

            return items;
        }

        public SvdFundResponse ReadItemFields(DataReaderAdapter dataReader, SvdFundResponse item = null)
        {
            if (item == null)
            {
                item = new SvdFundResponse();
            }

            item.Id = dataReader.GetInt64("ID");
            item.ClientVisitId = dataReader.GetInt64("ClientVisitID");
            item.ClientVisitGroupId = dataReader.GetInt64("VisitGroupID");
            item.DocumentType = ReferencesMaterializer.Instance.ReadItemFields(dataReader, "DocumentTypeID", "DocumentTypeCode", "DocumentTypeName");
            item.Citizenship = ReferencesMaterializer.Instance.ReadItemFields(dataReader, "CitizenshipID", "CitizenshipCode", "CitizenshipName");
            item.UnifiedPolicyNumber = dataReader.GetString("UnifiedPolicyNumber");
            item.PolicySeries = dataReader.GetString("PolicySeries");
            item.PolicyNumber = dataReader.GetString("PolicyNumber");
            item.Snils = dataReader.GetString("Snils");
            item.OGRN = dataReader.GetString("OGRN");
            item.StartDate = dataReader.GetDateTimeNull("StartDate");
            item.ExpirationDate = dataReader.GetDateTimeNull("ExpirationDate");
            item.OmsCode = dataReader.GetString("OmsCode");
            item.Firstname = dataReader.GetString("Firstname");
            item.Secondname = dataReader.GetString("Secondname");
            item.Lastname = dataReader.GetString("Lastname");
            item.Birthday = dataReader.GetDateTimeNull("Birthday");
            item.CreateDate = dataReader.GetDateTime("CreateDate");
            item.Sex = dataReader.GetString("Sex");
            item.DataType = (EntityType)(dataReader.GetInt32Null("DataTypeId") ?? 0);
            item.DocumentNumber = dataReader.GetString("DocumentNumber");
            item.DocumentSeries = dataReader.GetString("DocumentSeries");
            item.DocumentIssueDate = dataReader.GetDateTimeNull("DocumentIssueDate");
            item.ERZ = dataReader.GetString("ERZ");
            return item;
        }
    }

}
