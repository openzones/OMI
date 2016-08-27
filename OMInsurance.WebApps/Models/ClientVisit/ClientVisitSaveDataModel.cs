using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using OMInsurance.WebApps.Models.Core;
using OMInsurance.WebApps.Models.Heplers;
using OMInsurance.WebApps.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace OMInsurance.WebApps.Models
{
    public class ClientVisitSaveDataModel : ValidatableModel<ClientVisitSaveDataModel>
    {
        #region Properties

        public long? VisitId { get; set; }
        public long? ClientId { get; set; }
        public long? VisitGroupId { get; set; }

        [DisplayName("Статус")]
        public long StatusId { get; set; }
        public bool StatusEnabled { get; set; }
        public List<SelectListItem> Statuses { get; set; }

        [DisplayName("Дата статуса")]
        public DateTime StatusDate { get; set; }

        [DisplayName("Пункт выдачи")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public long? DeliveryCenterId { get; set; }
        public List<SelectListItem> DeliveryCenters { get; set; }

        [DisplayName("Сценарий")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public long? ScenarioId { get; set; }
        public List<SelectListItem> Scenaries { get; set; }

        [StringLength(9, ErrorMessage = "Максимальная длина - 9 символов")]
        [DisplayName("Временное свидетельство")]
        public string TemporaryPolicyNumber { get; set; }

        [DisplayName("Дата обращения (дата выдачи временного свидетельства)")]
        public DateTime? TemporaryPolicyDate { get; set; }

        [DisplayName("Дата окончания действия ВС")]
        public DateTime? TemporaryPolicyExpirationDate { get; set; }

        [DisplayName("Старая информация о застрахованном")]
        public ClientVersionEditModel OldClientInfo { get; set; }

        [DisplayName("Новая информация о застрахованном")]
        public ClientVersionEditModel NewClientInfo { get; set; }

        [DisplayName("Представитель")]
        public RepresentativeEditModel Representative { get; set; }

        [DisplayName("Старая информация о документах")]
        public DocumentModel OldDocument { get; set; }

        [DisplayName("Новая информация о документах")]
        public DocumentModel NewDocument { get; set; }

        [DisplayName("Дополнительный документ иностранного гражданина")]
        public DocumentModel NewForeignDocument { get; set; }

        [DisplayName("Прежний дополнительный документ иностранного гражданина")]
        public DocumentModel OldForeignDocument { get; set; }

        [DisplayName("Адрес проживания")]
        public AddressModel LivingAddress { get; set; }

        [DisplayName("Адрес регистрации")]
        public AddressModel RegistrationAddress { get; set; }

        [DisplayName("Дата регистрации")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public DateTime? RegistrationAddressDate { get; set; }

        [DisplayName("Старый полис")]
        public PolicyInfoClientVisitSaveModel OldPolicy { get; set; }

        [DisplayName("Новый полис")]
        public PolicyInfoClientVisitSaveModel NewPolicy { get; set; }

        [DisplayName("Сотрудник")]
        public UserModel Registrator { get; set; }

        [DisplayName("Гознак")]
        public long? GOZNAKTypeId { get; set; }
        public bool IsGoznakBlockVisible { get; set; }
        public List<SelectListItem> GoznakTypes { get; set; }

        [DisplayName("Дата выдачи полиса")]
        public DateTime? IssueDate { get; set; }
        public bool IssueDateEnabled { get; set; }

        [DisplayName("Актуальность полиса")]
        public bool IsActual { get; set; }
        public bool IsActualEnabled { get; set; }

        [DisplayName("Дата прикрепления")] //к Медицинской Организации
        public DateTime? AttachmentDate { get; set; }

        [DisplayName("Способ прикрепления")] //к Медицинской Организации
        public long? AttachmentTypeId { get; set; }
        public List<SelectListItem> AttachmentTypes { get; set; }

        [DisplayName("МО прикрепления")]
        public long? MedicalCentreId { get; set; }
        public List<SelectListItem> MedicalCenters { get; set; }

        [DisplayName("Форма полиса")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public long? CarrierId { get; set; }
        public List<SelectListItem> Carriers { get; set; }

        [DisplayName("Способ подачи заявления")]
        public long? ApplicationMethodId { get; set; }
        public List<SelectListItem> ApplicationMethods { get; set; }

        [DisplayName("Комментарий")]
        public string Comment { get; set; }

        [DisplayName("Дата направления полиса в печать на ГОЗНАК")]
        public DateTime? GOZNAKDate { get; set; }

        [DisplayName("Категория клиента")]
        public long? ClientCategoryId { get; set; }
        public List<SelectListItem> UralsibClientCategories { get; set; }

        [DisplayName("Место выдачи")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public long? DeliveryPointId { get; set; }
        public List<SelectListItem> DeliveryPoints { get; set; }

        [DisplayName("ФИО специалиста привлекшего клиента")]
        public string ClientAcquisitionEmployee { get; set; }
        public List<SelectListItem> ListClientAcquisitionEmployee { get; set; }

        [DisplayName("Контактные данные клиента")]
        public string ClientContacts { get; set; }

        [DisplayName("E-mail")]
        [RegularExpression(Constants.EmailRegex, ErrorMessage = "Неверное значение")]
        public string Email { get; set; }

        [DisplayName("Мобильный телефон")]
        [RegularExpression(Constants.PhoneRegex, ErrorMessage = "Неверное значение")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string Phone { get; set; }

        [DisplayName("Получение пластиковой карты Уралсиб")]
        public bool UralsibCard { get; set; }

        [DisplayName("Фотография")]
        public string PhotoFileName { get; set; }

        [DisplayName("Подпись")]
        public string SignatureFileName { get; set; }

        [DisplayName("Номер партии полиса")]
        public string PolicyPartyNumber { get; set; }

        [DisplayName("Бланк полиса")]
        public string PolicyBlanc { get; set; }

        [DisplayName("Отработанный сценарий сверки")]
        public string FundResponseApplyingMessage { get; set; }
        [DisplayName("Готово для отправки в ФОНД")]
        public bool IsReadyToFundSubmitRequest { get; set; }
        public bool IsReadyToFundSubmitRequestEnabled { get; set; }

        public long? OldSystemId { get; set; }

        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public bool? IsSuccessfullySaved { get; set; }
        public bool? DisableAll { get; set; }

        [DisplayName("Внимание! Сложный случай")]
        public bool IsDifficultCase { get; set; }

        /// <summary>
        /// при определенных условиях мы автоматически предлагаем напечатать отчеты
        /// </summary>
        public bool[] FlagPrintReport { get; set; }
        #endregion

        #region Constructors

        public ClientVisitSaveDataModel SetFlagPrintReport()
        {
            //вызов печатных форм после удачного сохранения в статусе "Ожидание подачи"
            if (this.StatusId == ClientVisitStatuses.SubmitPending.Id)
            {
                this.FlagPrintReport = new bool[3] { false, false, false };
                //если поле "временное свидетельство" не пустое, то вызывать печатную форму "Временное свидетельство"
                if (!string.IsNullOrEmpty(this.TemporaryPolicyNumber))
                {
                    FlagPrintReport[0] = true;
                }

                //если поле "Сценарий" имеет значение NB, CI, RI, CT, RT то вызываем «Заявление на смену СК»
                List<ReferenceItem> listScenario = ReferencesProvider.GetReferenceItems(Constants.ScenarioRef);
                foreach (var item in listScenario)
                {
                    item.Code = item.Code.Trim();
                }

                var tempId = listScenario.Where(a => a.Code == "NB" || a.Code == "CI" || a.Code == "RI" || a.Code == "CT" || a.Code == "RT"
                                                ).Select(b => b.Id).Where(c => c == this.ScenarioId).FirstOrDefault();
                if (tempId != 0)
                {
                    FlagPrintReport[1] = true;
                }

                //если поле "Сценарий" имеет значение DP, CR, CD, RI, RT то вызываем «Заявление на дубликат»
                tempId = listScenario.Where(a => a.Code == "DP" || a.Code == "CR" || a.Code == "CD" || a.Code == "RI" || a.Code == "RT"
                                                ).Select(b => b.Id).Where(c => c == this.ScenarioId).FirstOrDefault();
                if (tempId != 0)
                {
                    FlagPrintReport[2] = true;
                }
            }
            return this;
        }

        public ClientVisitSaveDataModel()
        {
            OldClientInfo = new ClientVersionEditModel(EntityType.Old);
            NewClientInfo = new ClientVersionEditModel(EntityType.New);
            OldDocument = new DocumentModel(DocumentType.Old);
            NewDocument = new DocumentModel(DocumentType.New);
            NewForeignDocument = new DocumentModel(DocumentType.NewForeign);
            OldForeignDocument = new DocumentModel(DocumentType.OldForeign);
            LivingAddress = new AddressModel(AddressType.Living);
            RegistrationAddress = new AddressModel(AddressType.Registration);
            validator = new ClientVisitSaveDataValidator();
            StatusId = 1;
            Statuses = ReferencesProvider.GetReferences(Constants.ClientVisitStatusRef, "1", false);
            DeliveryCenters = ReferencesProvider.GetReferences(Constants.DeliveryCenterForOperatorRef, null, false);
            Scenaries = ReferencesProvider.GetReferences(Constants.ScenarioRef, null, DateTime.Now, null, true);
            DeliveryPoints = ReferencesProvider.GetReferences(Constants.DeliveryPointRef, null, DateTime.Now, null, true);
            GoznakTypes = ReferencesProvider.GetReferences(Constants.GOZNAKTypeRef, null, true);
            UralsibClientCategories = ReferencesProvider.GetReferences(Constants.UralsibClientCategoryRef, null, true);
            AttachmentTypes = ReferencesProvider.GetReferences(Constants.PolicyAttachmentTypeRef, null, true);
            MedicalCenters = ReferencesProvider.GetReferences(Constants.MedicalCenterRef, null, true);
            Carriers = ReferencesProvider.GetReferences(Constants.CarriersRef, null, true);
            ApplicationMethods = ReferencesProvider.GetReferences(Constants.ApplicationMethodRef, null, true);
            Representative = new RepresentativeEditModel() { RepresentativeTypeId = 1 };
            ListClientAcquisitionEmployee = ReferencesProvider.GetListClientAcquisitionEmployee(null, true);

        }

        public ClientVisitSaveDataModel(User user) : this()
        {
            TemporaryPolicyDate = DateTime.Now;
            StatusEnabled = user.Roles.Contains(Role.Administrator) || user.Roles.Contains(Role.OperatorSG);
            TemporaryPolicyExpirationDate = DateTime.Now.AddWorkingDays(30);
            SignatureFileName = Guid.NewGuid().ToString();
            PhotoFileName = Guid.NewGuid().ToString();
            OldClientInfo = new ClientVersionEditModel(EntityType.Old);
            NewClientInfo = new ClientVersionEditModel(EntityType.New);
            OldDocument = new DocumentModel(DocumentType.Old);
            NewDocument = new DocumentModel(DocumentType.New);
            NewForeignDocument = new DocumentModel(DocumentType.NewForeign);
            OldForeignDocument = new DocumentModel(DocumentType.OldForeign);
            LivingAddress = new AddressModel(AddressType.Living);
            RegistrationAddress = new AddressModel(AddressType.Registration);
            OldPolicy = new PolicyInfoClientVisitSaveModel();
            NewPolicy = new PolicyInfoClientVisitSaveModel();
            Registrator = new UserModel();
            ClientCategoryId = 5;
            Scenaries = ReferencesProvider.GetReferences(Constants.ScenarioRef, user.Roles, DateTime.Now, null, true);
            DeliveryPoints = ReferencesProvider.GetReferences(Constants.DeliveryPointRef, user.Roles, DateTime.Now, null, true);
            DeliveryCenters = ReferencesProvider.GetReferences(Constants.DeliveryCenterRef, user.Roles, null, null, true);
            IsGoznakBlockVisible = user.Roles.Contains(Role.Administrator) || user.Roles.Contains(Role.OperatorSG);
            IsActualEnabled = (user.Roles.Contains(Role.Administrator) || user.Roles.Contains(Role.OperatorSG));
            UralsibClientCategories = ReferencesProvider.GetReferences(Constants.UralsibClientCategoryRef, user.Roles, null, "4", true);
            ListClientAcquisitionEmployee = ReferencesProvider.GetListClientAcquisitionEmployee(null, true);
        }

        public ClientVisitSaveDataModel(User user, long? clientId)
            : this(user)
        {
            ClientId = clientId;
        }

        public ClientVisitSaveDataModel(User user, ClientVisit clientVisit)
            : this(user)
        {
            VisitId = clientVisit.Id;
            ClientId = clientVisit.ClientId;
            VisitGroupId = clientVisit.VisitGroupId;
            SignatureFileName = clientVisit.SignatureFileName ?? Guid.NewGuid().ToString();
            PhotoFileName = clientVisit.PhotoFileName ?? Guid.NewGuid().ToString();
            DeliveryCenterId = clientVisit.DeliveryCenter.Id;
            ScenarioId = clientVisit.Scenario != null ? clientVisit.Scenario.Id : new long?();
            TemporaryPolicyDate = clientVisit.TemporaryPolicyDate;
            TemporaryPolicyNumber = clientVisit.TemporaryPolicyNumber;
            TemporaryPolicyExpirationDate = clientVisit.TemporaryPolicyExpirationDate;
            StatusId = clientVisit.Status.Id;
            StatusDate = clientVisit.StatusDate;
            StatusEnabled = user.Roles.Contains(Role.Administrator) || user.Roles.Contains(Role.OperatorSG);
            DisableAll = clientVisit.Status.Id == ClientVisitStatuses.AnswerPending.Id
                && !(user.Roles.Contains(Role.Administrator) || user.Roles.Contains(Role.OperatorSG));
            OldClientInfo = new ClientVersionEditModel(clientVisit.OldClientInfo, EntityType.Old);
            NewClientInfo = new ClientVersionEditModel(clientVisit.NewClientInfo, EntityType.New);
            OldDocument = new DocumentModel(clientVisit.OldDocument, DocumentType.Old);
            NewDocument = new DocumentModel(clientVisit.NewDocument, DocumentType.New);
            NewForeignDocument = new DocumentModel(clientVisit.NewForeignDocument, DocumentType.NewForeign);
            OldForeignDocument = new DocumentModel(clientVisit.OldForeignDocument, DocumentType.OldForeign);
            LivingAddress = new AddressModel(clientVisit.LivingAddress, AddressType.Living);
            RegistrationAddress = new AddressModel(clientVisit.RegistrationAddress, AddressType.Registration);
            RegistrationAddressDate = clientVisit.RegistrationAddressDate;
            OldPolicy = new PolicyInfoClientVisitSaveModel(clientVisit.OldPolicy, EntityType.Old);
            NewPolicy = new PolicyInfoClientVisitSaveModel(clientVisit.NewPolicy, EntityType.New);
            Registrator = new UserModel(clientVisit.Registrator);
            Representative = new RepresentativeEditModel(clientVisit.Representative);
            CarrierId = clientVisit.CarrierId;
            ApplicationMethodId = clientVisit.ApplicationMethodId;
            Comment = clientVisit.Comment;
            GOZNAKDate = clientVisit.GOZNAKDate;
            GOZNAKTypeId = clientVisit.GOZNAKType != null ? clientVisit.GOZNAKType.Id : default(long?);
            ClientCategoryId = clientVisit.ClientCategoryId == 0 ? new long() : clientVisit.ClientCategoryId;
            DeliveryPointId = clientVisit.DeliveryPointId == 0 ? new long() : clientVisit.DeliveryPointId;
            ClientAcquisitionEmployee = clientVisit.ClientAcquisitionEmployee;
            ClientContacts = clientVisit.ClientContacts;
            Phone = clientVisit.Phone;
            Email = clientVisit.Email;
            IssueDate = clientVisit.IssueDate;
            UralsibCard = clientVisit.UralsibCard;
            IsActual = clientVisit.IsActual;
            Scenaries = ReferencesProvider.GetReferences(Constants.ScenarioRef, user.Roles, DateTime.Now, null, true);
            DeliveryPoints = ReferencesProvider.GetReferences(Constants.DeliveryPointRef, user.Roles, DateTime.Now, null, true);
            ListClientAcquisitionEmployee = ReferencesProvider.GetListClientAcquisitionEmployee(null, true);
            PolicyBlanc = clientVisit.PolicyBlanc;

            if (ScenarioId != 0 && !Scenaries.Exists(item => item.Value == ScenarioId.ToString()))
            {
                Scenaries.Add(ReferencesProvider.GetReferences(Constants.ScenarioRef).FirstOrDefault(item => item.Value == ScenarioId.ToString()));
            }
            if (DeliveryPointId.HasValue && !DeliveryPoints.Exists(item => item.Value == DeliveryPointId.Value.ToString()))
            {
                DeliveryPoints.Add(ReferencesProvider.GetReferences(Constants.DeliveryPointRef).FirstOrDefault(item => item.Value == DeliveryPointId.Value.ToString()));
            }
            if (ClientCategoryId.HasValue && !UralsibClientCategories.Exists(item => item.Value == ClientCategoryId.Value.ToString()))
            {
                UralsibClientCategories.Add(ReferencesProvider.GetReferences(Constants.UralsibClientCategoryRef).FirstOrDefault(item => item.Value == ClientCategoryId.Value.ToString()));
            }
            IssueDateEnabled = (user.Roles.Contains(Role.Administrator) || user.Roles.Contains(Role.OperatorSG))
                || (StatusId == ClientVisitStatuses.PolicyReadyForClient.Id && user.Roles.Contains(Role.Registrator));
            IsReadyToFundSubmitRequestEnabled = user.Roles.Contains(Role.Administrator) || user.Roles.Contains(Role.OperatorSG);
            PolicyPartyNumber = clientVisit.PolicyPartyNumber;
            FundResponseApplyingMessage = clientVisit.FundResponseApplyingMessage;
            IsReadyToFundSubmitRequest = clientVisit.IsReadyToFundSubmitRequest;
            CreateDate = clientVisit.CreateDate;
            UpdateDate = clientVisit.UpdateDate;
            IsDifficultCase = clientVisit.IsDifficultCase;
            AttachmentDate = clientVisit.AttachmentDate;
            AttachmentTypeId = clientVisit.AttachmentType != null ? clientVisit.AttachmentType.Id : new long?();
            MedicalCentreId = clientVisit.MedicalCentre != null ? clientVisit.MedicalCentre.Id : new long?();
        }

        public ClientVisitSaveDataModel(User user, ClientVisit clientVisit, bool replaceNewOld)
            : this(user, clientVisit)
        {
            if (replaceNewOld)
            {
                OldClientInfo = new ClientVersionEditModel(clientVisit.NewClientInfo, EntityType.Old);
                NewClientInfo = new ClientVersionEditModel(EntityType.New);
                OldDocument = new DocumentModel(clientVisit.NewDocument, DocumentType.Old);
                NewDocument = new DocumentModel(DocumentType.New);
                NewForeignDocument = new DocumentModel(DocumentType.NewForeign);
                OldForeignDocument = new DocumentModel(clientVisit.NewForeignDocument, DocumentType.OldForeign);
                OldPolicy = new PolicyInfoClientVisitSaveModel(clientVisit.NewPolicy, EntityType.Old);
                NewPolicy = new PolicyInfoClientVisitSaveModel(EntityType.New);
                Registrator = new UserModel(user);
                StatusDate = new DateTime();
                StatusId = 1;
                Comment = user.Fullname;
            }
        }

        public ClientVisitSaveDataModel GetMessagesNotCritical(ClientVisitSaveDataModel model)
        {
            this.MessagesNotCritical = model.MessagesNotCritical;
            this.LivingAddress.MessagesNotCritical = model.LivingAddress.MessagesNotCritical;
            this.RegistrationAddress.MessagesNotCritical = model.RegistrationAddress.MessagesNotCritical;
            this.NewClientInfo.MessagesNotCritical = model.NewClientInfo.MessagesNotCritical;
            this.OldClientInfo.MessagesNotCritical = model.OldClientInfo.MessagesNotCritical;
            this.NewDocument.MessagesNotCritical = model.NewDocument.MessagesNotCritical;
            this.OldDocument.MessagesNotCritical = model.OldDocument.MessagesNotCritical;
            this.NewForeignDocument.MessagesNotCritical = model.NewForeignDocument.MessagesNotCritical;
            this.OldForeignDocument.MessagesNotCritical = model.OldForeignDocument.MessagesNotCritical;
            this.NewPolicy.MessagesNotCritical = model.NewPolicy.MessagesNotCritical;
            this.OldPolicy.MessagesNotCritical = model.OldPolicy.MessagesNotCritical;
            return this;
        }

        #endregion

        #region Methods

        public void ClearIds()
        {
            VisitId = null;
            VisitGroupId = null;
            OldClientInfo.Id = null;
            NewClientInfo.Id = null;
            OldDocument.Id = null;
            NewDocument.Id = null;
            NewForeignDocument.Id = null;
            OldForeignDocument.Id = null;
            LivingAddress.Id = null;
            RegistrationAddress.Id = null;
            OldPolicy.Id = null;
            NewPolicy.Id = null;
            if (Representative != null)
            {
                Representative.Id = null;
            }
        }

        public string StatusName
        {
            get
            {
                string statusName = string.Empty;
                if (StatusId != 0)
                {
                    statusName = Statuses.FirstOrDefault(s => s.Value == StatusId.ToString()).Text;
                }
                return statusName;
            }
        }

        public ClientVisit.SaveData GetClientVisitSaveData()
        {
            ClientVisit.SaveData data = new ClientVisit.SaveData();

            data.Id = VisitId;
            data.VisitGroupId = this.VisitGroupId;
            data.RegistratorId = this.Registrator.Id.Value;
            data.DeliveryPointId = this.DeliveryPointId;
            data.ScenarioId = this.ScenarioId;
            data.NewClientInfo = this.NewClientInfo.GetForBLL();
            data.OldClientInfo = this.OldClientInfo.GetForBLL();
            data.ClientId = this.ClientId;
            data.LivingAddress = this.LivingAddress.GetForBLL();
            data.RegistrationAddress = this.RegistrationAddress.GetForBLL();
            data.NewDocument = this.NewDocument.GetForBLL();
            data.OldDocument = this.OldDocument.GetForBLL();
            data.OldForeignDocument = this.OldForeignDocument.GetForBLL();
            data.NewForeignDocument = this.NewForeignDocument.GetForBLL();
            data.NewPolicy = this.NewPolicy.GetForBLL();
            data.OldPolicy = this.OldPolicy.GetForBLL();
            data.Representative = this.Representative.GetForBLL();
            data.RegistrationAddressDate = this.RegistrationAddressDate;
            data.TemporaryPolicyDate = this.TemporaryPolicyDate;
            data.TemporaryPolicyExpirationDate = this.TemporaryPolicyExpirationDate;
            data.TemporaryPolicyNumber = this.TemporaryPolicyNumber;
            data.GOZNAKTypeId = this.GOZNAKTypeId;
            data.IssueDate = this.IssueDate;
            data.IsActual = this.IsActual;
            data.AttachmentDate = this.AttachmentDate;
            data.AttachmentTypeId = this.AttachmentTypeId;
            data.MedicalCentreId = this.MedicalCentreId;
            data.Email = this.Email;
            data.Phone = this.Phone;
            data.UralsibCard = this.UralsibCard;
            data.PolicyPartyNumber = this.PolicyPartyNumber;
            data.CarrierId = this.CarrierId;
            data.ApplicationMethodId = this.ApplicationMethodId == 0 ? new long() : this.ApplicationMethodId;
            data.Comment = this.Comment;
            data.GOZNAKDate = this.GOZNAKDate;
            data.ClientCategoryId = this.ClientCategoryId == 0 ? new long() : this.ClientCategoryId;
            data.DeliveryCenterId = this.DeliveryCenterId;
            data.ClientAcquisitionEmployee = this.ClientAcquisitionEmployee;
            data.ClientContacts = this.ClientContacts;
            data.SignatureFileName = this.SignatureFileName;
            data.PhotoFileName = this.PhotoFileName;
            data.Status = this.StatusId == 0 ? new long?() : this.StatusId;
            data.StatusDate = this.StatusDate == default(DateTime) ? new DateTime?() : this.StatusDate;
            data.OldSystemId = OldSystemId;
            data.FundResponseApplyingMessage = this.FundResponseApplyingMessage;
            data.IsReadyToFundSubmitRequest = this.IsReadyToFundSubmitRequest;
            data.IsDifficultCase = this.IsDifficultCase;
            return data;
        }

        public string GetSummaryMessages()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Join(". ", validator.Messages));
            sb.AppendLine(string.Join(". ", this.NewDocument.Messages));
            sb.AppendLine(string.Join(". ", this.OldDocument.Messages));
            sb.AppendLine(string.Join(". ", this.NewClientInfo.Messages));
            sb.AppendLine(string.Join(". ", this.OldClientInfo.Messages));
            sb.AppendLine(string.Join(". ", this.NewPolicy.Messages));
            sb.AppendLine(string.Join(". ", this.OldClientInfo.Messages));
            sb.AppendLine(string.Join(". ", this.LivingAddress.Messages));
            sb.AppendLine(string.Join(". ", this.RegistrationAddress.Messages));
            sb.AppendLine(string.Join(". ", this.Representative.Messages));
            return sb.ToString();
        }

        #endregion
    }
}