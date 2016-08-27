using OMInsurance.Entities.Core;
using System;

namespace OMInsurance.Entities
{
    public class ClientVisit : DataObject
    {
        #region Constructors

        public ClientVisit()
        {
            OldClientInfo = new ClientVersion();
            NewClientInfo = new ClientVersion();
            OldDocument = new Document();
            NewDocument = new Document();
            NewForeignDocument = new Document();
            OldForeignDocument = new Document();
            LivingAddress = new Address();
            RegistrationAddress = new Address();
            OldPolicy = new PolicyInfo();
            NewPolicy = new PolicyInfo();
            Status = new ReferenceItem();
            Registrator = new User();
            DeliveryCenter = new DeliveryCenter();
            Representative = new Representative();
        }

        #endregion

        #region Properties

        public long ClientId { get; set; }
        public long VisitGroupId { get; set; }
        public ReferenceItem Scenario { get; set; }
        public DeliveryCenter DeliveryCenter { get; set; }
        public ReferenceItem Status { get; set; }
        public DateTime StatusDate { get; set; }
        public string TemporaryPolicyNumber { get; set; }
        public DateTime? TemporaryPolicyDate { get; set; }
        public DateTime? TemporaryPolicyExpirationDate { get; set; }
        public ReferenceItem GOZNAKType { get; set; }
        public bool IsActual { get; set; }
        public DateTime? IssueDate { get; set; }
        public string InfoSource { get; set; }
        public DateTime? DeregistrationDate { get; set; }
        public DateTime? ArchivationDate { get; set; }
        public DateTime? AttachmentDate { get; set; }
        public ReferenceItem AttachmentType { get; set; }
        public ReferenceItem MedicalCentre { get; set; }
        public User Registrator { get; set; }

        public bool HasOldDocument { get; set; }
        public bool HasOldPolicy { get; set; }
        public bool HasOldClientInfo { get; set; }

        public ClientVersion OldClientInfo { get; set; }
        public ClientVersion NewClientInfo { get; set; }

        public Document NewDocument { get; set; }
        public Document OldDocument { get; set; }
        public Document NewForeignDocument { get; set; }
        public Document OldForeignDocument { get; set; }

        public Address LivingAddress { get; set; }
        public Address RegistrationAddress { get; set; }
        public DateTime? RegistrationAddressDate { get; set; }

        public PolicyInfo OldPolicy { get; set; }
        public PolicyInfo NewPolicy { get; set; }

        public Representative Representative { get; set; }
        public long? CarrierId { get; set; }
        public long? ApplicationMethodId { get; set; }
        public string Comment { get; set; }
        public DateTime? GOZNAKDate { get; set; }
        public long? ClientCategoryId { get; set; }
        public long? DeliveryPointId { get; set; }
        public string ClientAcquisitionEmployee { get; set; }
        public string ClientContacts { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool UralsibCard { get; set; }
        public string SignatureFileName { get; set; }
        public string PolicyPartyNumber { get; set; }
        public string PhotoFileName { get; set; }
        public string PolicyBlanc { get; set; }
        public string FundResponseApplyingMessage { get; set; }
        public bool IsReadyToFundSubmitRequest { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsDifficultCase { get; set; }

        #endregion

        public class SaveData
        {
            public SaveData()
            {
                OldClientInfo = new ClientVersion.SaveData();
                NewClientInfo = new ClientVersion.SaveData();

                NewDocument = new Document.SaveData();
                OldDocument = new Document.SaveData();
                NewForeignDocument = new Document.SaveData();
                OldForeignDocument = new Document.SaveData();

                LivingAddress = new Address.SaveData();
                RegistrationAddress = new Address.SaveData();

                OldPolicy = new PolicyInfo.SaveData();
                NewPolicy = new PolicyInfo.SaveData();
                Representative = new Representative.SaveData();
            }

            /// <summary>
            /// Creates new SaveData without ids
            /// </summary>
            public SaveData(ClientVisit visit)
            {
                this.ClientId = visit.ClientId;
                this.VisitGroupId = visit.VisitGroupId;
                this.ScenarioId = visit.Scenario != null && visit.Scenario.Id != 0 ? visit.Scenario.Id : default(long?);
                this.ApplicationMethodId = visit.ApplicationMethodId;
                this.CarrierId = visit.CarrierId;
                this.DeliveryCenterId = visit.DeliveryCenter.Id;
                this.TemporaryPolicyNumber = visit.TemporaryPolicyNumber;
                this.TemporaryPolicyDate = visit.TemporaryPolicyDate;
                this.TemporaryPolicyExpirationDate = visit.TemporaryPolicyExpirationDate;
                this.GOZNAKTypeId = visit.GOZNAKType != null && visit.GOZNAKType.Id != 0 ? visit.GOZNAKType.Id : default(long?);
                this.IssueDate = visit.IssueDate;
                this.InfoSource = visit.InfoSource;
                this.IsActual = visit.IsActual;
                this.ArchivationDate = visit.ArchivationDate;
                this.DeregistrationDate = visit.DeregistrationDate;
                this.AttachmentDate = visit.AttachmentDate;
                this.AttachmentTypeId = visit.AttachmentType != null && visit.AttachmentType.Id != 0 ? visit.AttachmentType.Id : new long?();
                this.MedicalCentreId = visit.MedicalCentre != null && visit.MedicalCentre.Id != 0 ? visit.MedicalCentre.Id : new long?();
                this.RegistratorId = visit.Registrator.Id;
                this.Comment = visit.Comment;
                this.GOZNAKDate = visit.GOZNAKDate;
                this.ClientCategoryId = visit.ClientCategoryId;
                this.DeliveryPointId = visit.DeliveryPointId;
                this.ClientAcquisitionEmployee = visit.ClientAcquisitionEmployee;
                this.ClientContacts = visit.ClientContacts;
                this.Email = visit.Email;
                this.Phone = visit.Phone;
                this.SignatureFileName = visit.SignatureFileName;
                this.PhotoFileName = visit.PhotoFileName;
                this.UralsibCard = visit.UralsibCard;
                this.RegistrationAddressDate = visit.RegistrationAddressDate;
                this.PolicyPartyNumber = visit.PolicyPartyNumber;

                this.OldClientInfo = new ClientVersion.SaveData(visit.OldClientInfo);
                this.NewClientInfo = new ClientVersion.SaveData(visit.NewClientInfo);

                this.NewDocument = new Document.SaveData(visit.NewDocument);
                this.OldDocument = new Document.SaveData(visit.OldDocument);
                this.NewForeignDocument = new Document.SaveData(visit.NewForeignDocument);
                this.OldForeignDocument = new Document.SaveData(visit.OldForeignDocument);

                this.LivingAddress = new Address.SaveData(visit.LivingAddress);
                this.RegistrationAddress = new Address.SaveData(visit.RegistrationAddress);

                this.OldPolicy = new PolicyInfo.SaveData(visit.OldPolicy);
                this.NewPolicy = new PolicyInfo.SaveData(visit.NewPolicy);
                this.Representative = new Representative.SaveData(visit.Representative);

                this.IsDifficultCase = visit.IsDifficultCase;
            }

            /// <summary>
            /// Build data to update
            /// </summary>
            /// <param name="visit"></param>
            /// <returns></returns>
            public static ClientVisit.SaveData BuildSaveData(ClientVisit visit)
            {
                ClientVisit.SaveData data = new ClientVisit.SaveData(visit);
                data.Status = visit.Status.Id;
                data.StatusDate = visit.StatusDate;
                data.IssueDate = visit.IssueDate;
                data.Id = visit.Id;
                data.VisitGroupId = visit.VisitGroupId;
                data.OldClientInfo.Id = visit.OldClientInfo.Id;
                data.NewClientInfo.Id = visit.NewClientInfo.Id;
                data.OldDocument.Id = visit.OldDocument.Id;
                data.NewDocument.Id = visit.NewDocument.Id;
                data.NewForeignDocument.Id = visit.NewForeignDocument.Id;
                data.OldForeignDocument.Id = visit.OldForeignDocument.Id;
                data.LivingAddress.Id = visit.LivingAddress.Id;
                data.RegistrationAddress.Id = visit.RegistrationAddress.Id;
                data.OldPolicy.Id = visit.OldPolicy.Id;
                data.NewPolicy.Id = visit.NewPolicy.Id;
                data.Representative.Id = visit.Representative.Id;
                data.IsActual = visit.IsActual;
                data.IsReadyToFundSubmitRequest = visit.IsReadyToFundSubmitRequest;
                data.IsDifficultCase = visit.IsDifficultCase;
                return data;
            }

            #region Properties

            public long? Id { get; set; }
            public long? ClientId { get; set; }
            public long? VisitGroupId { get; set; }
            public long? ScenarioId { get; set; }
            public long? DeliveryCenterId { get; set; }
            public string TemporaryPolicyNumber { get; set; }
            public DateTime? TemporaryPolicyDate { get; set; }
            public DateTime? TemporaryPolicyExpirationDate { get; set; }
            public long? GOZNAKTypeId { get; set; }
            public DateTime? IssueDate { get; set; }
            public string InfoSource { get; set; }
            public bool IsActual { get; set; }
            public DateTime? ArchivationDate { get; set; }
            public DateTime? DeregistrationDate { get; set; }
            public DateTime? AttachmentDate { get; set; }
            public long? AttachmentTypeId { get; set; }
            public long? MedicalCentreId { get; set; }
            public long RegistratorId { get; set; }
            public long? CarrierId { get; set; }
            public long? ApplicationMethodId { get; set; }
            public string Comment { get; set; }
            public DateTime? GOZNAKDate { get; set; }
            public long? ClientCategoryId { get; set; }
            public long? DeliveryPointId { get; set; }
            public string ClientAcquisitionEmployee { get; set; }
            public string ClientContacts { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string SignatureFileName { get; set; }
            public string PhotoFileName { get; set; }
            public bool UralsibCard { get; set; }
            public long? Status { get; set; }
            public DateTime? StatusDate { get; set; }

            public string Blanc { get; set; }
            public string N_KOR { get; set; }
            public DateTime? DATA_FOND { get; set; }
            public string NZ_GOZNAK { get; set; }
            public DateTime? Dat_U { get; set; }
            public DateTime? Dat_S { get; set; }
            public long? OldSystemId { get; set; }
            public long? UniqueId { get; set; }
            public string PolicyPartyNumber { get; set; }
            public string FundResponseApplyingMessage { get; set; }
            public bool? IsReadyToFundSubmitRequest { get; set; }
            public bool IsDifficultCase { get; set; }

            public ClientVersion.SaveData OldClientInfo { get; set; }
            public ClientVersion.SaveData NewClientInfo { get; set; }

            public Document.SaveData OldDocument { get; set; }
            public Document.SaveData NewDocument { get; set; }
            public Document.SaveData NewForeignDocument { get; set; }
            public Document.SaveData OldForeignDocument { get; set; }

            public Address.SaveData LivingAddress { get; set; }
            public Address.SaveData RegistrationAddress { get; set; }
            public DateTime? RegistrationAddressDate { get; set; }

            public PolicyInfo.SaveData OldPolicy { get; set; }
            public PolicyInfo.SaveData NewPolicy { get; set; }
            public Representative.SaveData Representative { get; set; }

            #endregion

            public void SetFundResponseApplyingMessage(string message)
            {
                this.IsReadyToFundSubmitRequest = true;
                this.FundResponseApplyingMessage = message;
            }
        }

        /// <summary>
        /// Data to update client visits
        /// </summary>
        public class UpdateData
        {
            public string Blanc { get; set; }
            public string N_KOR { get; set; }
            public DateTime? DATA_FOND { get; set; }
            public string NZ_GOZNAK { get; set; }
            public string PolicyPartyNumber { get; set; }
            public string UnifiedPolicyNumber { get; set; }
            public string TemporaryPolicyNumber { get; set; }
            public string Lastname { get; set; }
            public string Firstname { get; set; }
            public string Secondname { get; set; }
            public int Sex { get; set; }
            public DateTime? Birthday { get; set; }
            public DateTime? Dat_U { get; set; }
            public DateTime? Dat_S { get; set; }
            public string OGRN { get; set; }
        }

        /// <summary>
        /// Data to update client visits
        /// </summary>
        public class UpdateResultData
        {
            public UpdateResultData()
            {
            }

            public UpdateResultData(UpdateData data, bool isSuccess, string message, ClientVisit visit = null)
            {
                UnifiedPolicyNumber = data.UnifiedPolicyNumber;
                Lastname = data.Lastname;
                Firstname = data.Firstname;
                Secondname = data.Secondname;
                Sex = data.Sex;
                Birthday = data.Birthday;
                IsSuccess = isSuccess;
                Message = message;
                if (visit != null)
                {
                    Id = visit.Id;
                    ClientId = visit.ClientId;
                    ClientVisitGroupId = visit.VisitGroupId;
                }

            }
            public long Id { get; set; }
            public long ClientId { get; set; }
            public long ClientVisitGroupId { get; set; }
            public long RECID { get; set; }
            public string UnifiedPolicyNumber { get; set; }
            public string Lastname { get; set; }
            public string Firstname { get; set; }
            public string Secondname { get; set; }
            public int Sex { get; set; }
            public DateTime? Birthday { get; set; }
            public ReferenceItem Status { get; set; }
            public bool IsSuccess { get; set; }
            public string Message { get; set; }
        }
    }
}
