using OMInsurance.DataAccess.Core;
using OMInsurance.DataAccess.DAO;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System.Collections.Generic;
using System.Linq;

namespace OMInsurance.DataAccess.Materializers
{
    public class ClientVisitMaterializer : IMaterializer<ClientVisit>
    {
        private static readonly ClientVisitMaterializer _instance = new ClientVisitMaterializer();

        public static ClientVisitMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }

        public ClientVisit Materialize(DataReaderAdapter dataReader)
        {
            return Materialize_List(dataReader).FirstOrDefault();
        }

        public List<ClientVisit> Materialize_List(DataReaderAdapter dataReader)
        {
            List<ClientVisit> items = new List<ClientVisit>();

            while (dataReader.Read())
            {
                ClientVisit obj = ReadItemFields(dataReader);
                obj.OldClientInfo = ClientDao.Instance.ClientVersion_Get(obj.OldClientInfo.Id);
                obj.NewClientInfo = ClientDao.Instance.ClientVersion_Get(obj.NewClientInfo.Id);

                obj.OldDocument = DocumentDao.Instance.GetDocument(obj.OldDocument.Id);
                obj.NewDocument = DocumentDao.Instance.GetDocument(obj.NewDocument.Id);
                obj.NewForeignDocument = DocumentDao.Instance.GetDocument(obj.NewForeignDocument.Id);
                obj.OldForeignDocument = DocumentDao.Instance.GetDocument(obj.OldForeignDocument.Id);

                obj.OldPolicy = PolicyDao.Instance.GetPolicy(obj.OldPolicy.Id);
                obj.NewPolicy = PolicyDao.Instance.GetPolicy(obj.NewPolicy.Id);
                obj.RegistrationAddress = AddressDao.Instance.GetAddress(obj.RegistrationAddress.Id);
                obj.LivingAddress = AddressDao.Instance.GetAddress(obj.LivingAddress.Id);
                obj.Registrator = UserDao.Instance.User_Get(obj.Registrator.Id);
                obj.Representative = RepresentativeDao.Instance.GetRepresentative(obj.Representative.Id);

                items.Add(obj);
            }
            return items;
        }

        public ClientVisit ReadItemFields(DataReaderAdapter reader, ClientVisit item = null)
        {
            if (item == null)
            {
                item = new ClientVisit();
            }

            item.Id = reader.GetInt64("ID");
            item.ClientId = reader.GetInt64("ClientID");
            item.Scenario = ReferencesMaterializer.Instance.ReadItemFields(
                reader,
                "ScenarioId",
                "ScenarioCode",
                "ScenarioName");
            item.StatusDate = reader.GetDateTime("StatusDate");
            item.Status = ReferencesMaterializer.Instance.ReadItemFields(
                reader,
                "StatusId",
                "StatusCode",
                "StatusName");
            item.VisitGroupId = reader.GetInt64("VisitGroupId");
            item.DeliveryCenter = DeliveryCenterMaterializer.Instance.ReadItemFields(reader);
            item.DeliveryPointId = reader.GetInt64Null("DeliveryPointId");
            item.TemporaryPolicyNumber = reader.GetString("TemporaryPolicyNumber");
            item.TemporaryPolicyExpirationDate = reader.GetDateTimeNull("TemporaryPolicyExpirationDate");
            item.TemporaryPolicyDate = reader.GetDateTimeNull("TemporaryPolicyDate");
            item.OldClientInfo.Id = reader.GetInt64("OldClientVersionID");
            item.NewClientInfo.Id = reader.GetInt64("NewClientVersionID");
            item.OldDocument.Id = reader.GetInt64("OldClientDocumentID");
            item.NewDocument.Id = reader.GetInt64("NewClientDocumentID");
            item.NewForeignDocument.Id = reader.GetInt64("NewForeignDocumentID");
            item.OldForeignDocument.Id = reader.GetInt64("OldForeignDocumentID");
            item.LivingAddress.Id = reader.GetInt64("ClientLivingAddressID");
            item.RegistrationAddress.Id = reader.GetInt64("ClientOfficialAddressID");
            item.OldPolicy.Id = reader.GetInt64("OldClientPolicyID");
            item.NewPolicy.Id = reader.GetInt64("NewClientPolicyID");
            item.RegistrationAddressDate = reader.GetDateTimeNull("RegistryAddressDate");
            item.Registrator.Id = reader.GetInt64("RegistratorId");
            item.IsActual = reader.GetBoolean("IsActual");
            item.InfoSource = reader.GetString("InfoSource");
            item.AttachmentDate = reader.GetDateTimeNull("AttachmentDate");
            item.AttachmentType = new ReferenceItem() { Id = reader.GetInt64Null("AttachmentTypeID") ?? 0 };
            item.MedicalCentre = new ReferenceItem() { Id = reader.GetInt64Null("MedicalCentreID") ?? 0 };
            item.DeregistrationDate = reader.GetDateTimeNull("DeregistrationDate");
            item.ArchivationDate = reader.GetDateTimeNull("ArchivationDate");
            item.IssueDate = reader.GetDateTimeNull("IssueDate");
            item.CreateDate = reader.GetDateTime("CreateDate");
            item.UpdateDate = reader.GetDateTime("UpdateDate");
            item.PhotoFileName = reader.GetString("PhotoFileName");
            item.SignatureFileName = reader.GetString("SignatureFileName");
            item.Representative.Id = reader.GetInt64("RepresentativeID");
            item.CarrierId = reader.GetInt64Null("CarrierID");
            item.ApplicationMethodId = reader.GetInt64Null("ApplicationMethodID");
            item.Comment = reader.GetString("Comment");
            item.GOZNAKDate = reader.GetDateTimeNull("GOZNAKDate");
            item.ClientCategoryId = reader.GetInt64Null("ClientCategoryId");
            item.ClientAcquisitionEmployee = reader.GetString("ClientAcquisitionEmployee");
            item.ClientContacts = reader.GetString("ClientContacts");
            item.Phone = reader.GetString("Phone");
            item.Email = reader.GetString("Email");
            item.UralsibCard = reader.GetBoolean("UralsibCard");
            item.PolicyPartyNumber = reader.GetString("PolicyPartyNumber");
            item.GOZNAKType = ReferencesMaterializer.Instance.ReadItemFields(
                reader,
                "GOZNAKTypeID",
                "GOZNAKTypeCode",
                "GOZNAKTypeName");
            item.PolicyBlanc = reader.GetString("PolicyBlanc");
            item.FundResponseApplyingMessage = reader.GetString("FundResponseApplyingMessage");
            item.IsReadyToFundSubmitRequest = reader.GetBoolean("IsReadyToFundSubmitRequest");
            item.IsDifficultCase = reader.GetBoolean("IsDifficultCase");
            return item;
        }
    }
}
