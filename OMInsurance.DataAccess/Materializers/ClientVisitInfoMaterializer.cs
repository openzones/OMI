using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using System.Collections.Generic;
using System.Linq;

namespace OMInsurance.DataAccess.Materializers
{
    public class ClientVisitInfoMaterializer : IMaterializer<ClientVisitInfo>
    {
        private static readonly ClientVisitInfoMaterializer _instance = new ClientVisitInfoMaterializer();

        public static ClientVisitInfoMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }

        public ClientVisitInfo Materialize(DataReaderAdapter reader)
        {
            return Materialize_List(reader).FirstOrDefault();
        }

        public List<ClientVisitInfo> Materialize_List(DataReaderAdapter reader)
        {
            List<ClientVisitInfo> items = new List<ClientVisitInfo>();

            while (reader.Read())
            {
                ClientVisitInfo obj = ReadItemFields(reader);
                items.Add(obj);
            }
            return items;
        }

        public ClientVisitInfo ReadItemFields(DataReaderAdapter reader, ClientVisitInfo item = null)
        {
            if (item == null)
            {
                item = new ClientVisitInfo();
            }

            item.Id = reader.GetInt64("ID");
            item.ClientId = reader.GetInt64("ClientId");
            item.VisitGroupId = reader.GetInt64("VisitGroupId");
            item.StatusDate = reader.GetDateTime("StatusDate");
            item.Firstname = reader.GetString("FirstName");
            item.Secondname = reader.GetString("Secondname");
            item.Lastname = reader.GetString("Lastname");
            item.Sex = reader.GetString("Sex");
            item.Birthday = reader.GetDateTimeNull("Birthday");
            item.TemporaryPolicyNumber = reader.GetString("TemporaryPolicyNumber");
            item.TemporaryPolicyDate = reader.GetDateTimeNull("TemporaryPolicyDate");
            item.UnifiedPolicyNumber = reader.GetString("UnifiedPolicyNumber");
            item.PolicySeries = reader.GetString("PolicySeries");
            item.PolicyNumber = reader.GetString("PolicyNumber");
            item.FundResponseApplyingMessage = reader.GetString("FundResponseApplyingMessage");
            item.IsReadyToFundSubmitRequest = reader.GetBoolean("IsReadyToFundSubmitRequest");
            item.IsDifficultCase = reader.GetBoolean("IsDifficultCase");
            item.PolicyIssueDate = reader.GetDateTimeNull("PolicyIssueDate");
            item.SNILS = reader.GetString("SNILS");
            item.PolicyParty = reader.GetString("PolicyParty");
            item.Status = ReferencesMaterializer.Instance.ReadItemFields(reader, "StatusID", "StatusCode", "StatusName");
            item.Scenario = ReferencesMaterializer.Instance.ReadItemFields(reader, "ScenarioID", "ScenarioCode", "ScenarioName");
            item.Comment = reader.GetString("Comment");
            item.DeliveryCenter = ReferencesMaterializer.Instance.ReadItemFields(reader, "DeliveryCenterId", "DeliveryCenterCode", "DeliveryCenter");
            item.DeliveryPoint = reader.GetString("DeliveryPoint");
            item.Phone = reader.GetString("Phone");
            item.UserId = reader.GetInt64Null("UserId");
            item.ClientAcquisitionEmployee = reader.GetString("ClientAcquisitionEmployee");
            item.Citizenship = reader.GetString("Citizenship");

            return item;
        }
    }
}
