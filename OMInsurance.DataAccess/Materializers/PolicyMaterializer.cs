using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMInsurance.DataAccess.Materializers
{
    public class PolicyMaterializer : IMaterializer<PolicyInfo>
    {
        private static readonly PolicyMaterializer _instance = new PolicyMaterializer();

        public static PolicyMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }
        public PolicyInfo Materialize(DataReaderAdapter dataReader)
        {
            return Materialize_List(dataReader).FirstOrDefault();
        }

        public List<PolicyInfo> Materialize_List(DataReaderAdapter dataReader)
        {
            List<PolicyInfo> items = new List<PolicyInfo>();

            while (dataReader.Read())
            {
                PolicyInfo obj = ReadItemFields(dataReader);
                items.Add(obj);
            }

            return items;
        }

        public PolicyInfo ReadItemFields(DataReaderAdapter dataReader, PolicyInfo item = null)
        {
            if (item == null)
            {
                item = new PolicyInfo();
            }

            item.Id = dataReader.GetInt64("ID");
            item.Series = dataReader.GetString("Series");
            item.Number = dataReader.GetString("Number");
            item.UnifiedPolicyNumber = dataReader.GetString("UnifiedPolicyNumber");
            item.OGRN = dataReader.GetString("OGRN");
            item.OKATO = dataReader.GetString("OKATO");
            item.StartDate = dataReader.GetDateTimeNull("StartDate");
            item.EndDate = dataReader.GetDateTimeNull("EndDate");
            item.CreateDate = dataReader.GetDateTime("CreateDate");
            item.UpdateDate = dataReader.GetDateTime("UpdateDate");
            item.PolicyType = ReferencesMaterializer.Instance.ReadItemFields(
                dataReader,
                "PolicyTypeID",
                "PolicyTypeCode",
                "PolicyTypeName");
            item.SmoId = dataReader.GetInt64Null("SmoId");
            item.SmoName = dataReader.GetString("SmoName");
            return item;
        }
    }
}
