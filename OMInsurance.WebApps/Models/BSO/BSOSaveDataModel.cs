using OMInsurance.WebApps.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace OMInsurance.WebApps.Models
{
    public class BSOSaveDataModel : ValidatableModel<BSOSaveDataModel>
    {
        [DisplayName("Id БСО")]
        public long? Id { get; set; }

        [DisplayName("№ БСО")]
        [StringLength(9, ErrorMessage = "Максимальная длина - 9 символов")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string TemporaryPolicyNumber { get; set; }

        [DisplayName("№ Партии")]
        [StringLength(9, ErrorMessage = "Максимальная длина - 3 символов")]
        public string PolicyPartyNumber { get; set; }

        [DisplayName("Новый статус")]
        public long? StatusId { get; set; }

        [DisplayName("Статус")]
        public string StatusName { get; set; }

        [DisplayName("Дата статуса")]
        public DateTime? StatusDate { get; set; }

        [DisplayName("Пункт выдачи")]
        public long? DeliveryCenterId { get; set; }

        [DisplayName("Идентификатор обращения")]
        public long? VisitGroupId { get; set; }

        [DisplayName("Комментарий")]
        public string Comment { get; set; }

        [DisplayName("Точка выдачи")]
        public long? DeliveryPointId { get; set; }

        [DisplayName("Идентификатор пользователя")]
        public long? UserId { get; set; }

        [DisplayName("Ответственный")]
        public long? ResponsibleID { get; set; }

        [DisplayName("Дата изменения")]
        public DateTime? ChangeDate { get; set; }

        public bool? IsSuccessfullySaved { get; set; }
        public bool FlagPrintReport { get; set; }

        public List<SelectListItem> DeliveryCenters { get; set; }
        public List<SelectListItem> DeliveryPoints { get; set; }
        public List<SelectListItem> listBSOStatuses { get; set; }
        public List<SelectListItem> listBSOStatusesAvailable { get; set; }
        public List<SelectListItem> listBSOResponsibles { get; set; }

        public BSOSaveDataModel()
        {
            this.Id = Id;
            this.TemporaryPolicyNumber = TemporaryPolicyNumber;
            this.PolicyPartyNumber = PolicyPartyNumber;
            this.StatusId = StatusId;
            this.StatusName = StatusName;
            this.StatusDate = StatusDate;
            this.DeliveryCenterId = DeliveryCenterId;
            this.DeliveryPointId = DeliveryPointId;
            this.VisitGroupId = VisitGroupId;
            this.Comment = Comment;
            this.UserId = UserId;
            this.ResponsibleID = ResponsibleID;
            this.ChangeDate = ChangeDate;
            this.DeliveryCenters = ReferencesProvider.GetReferences(Constants.DeliveryCenterForOperatorRef, null, true);
            this.DeliveryPoints = ReferencesProvider.GetReferences(Constants.DeliveryPointRef, null, true);
            this.listBSOStatuses = StatusBSOProvider.GetBSOListStatus(false);
            this.listBSOStatusesAvailable = StatusBSOProvider.GetAvailableBSOStatus(StatusId, true);
            this.listBSOResponsibles = StatusBSOProvider.GetListBSOResponsibles(true);
            validator = new BSOSaveDataModelValidator();

        }

        public BSOSaveDataModel(BSO bso)
        {
            this.Id = bso.Id;
            this.TemporaryPolicyNumber = bso.TemporaryPolicyNumber;
            this.PolicyPartyNumber = bso.PolicyPartyNumber;
            this.StatusId = bso.Status.Id;
            this.StatusName = bso.Status.Name;
            this.StatusDate = bso.StatusDate;
            this.DeliveryCenterId = bso.DeliveryCenterId;
            this.DeliveryPointId = bso.DeliveryPointId;
            this.VisitGroupId = bso.VisitGroupId;
            this.Comment = bso.Comment;
            this.UserId = bso.UserId;
            this.ResponsibleID = bso.ResponsibleID;
            this.ChangeDate = bso.ChangeDate;
            this.DeliveryCenters = ReferencesProvider.GetReferences(Constants.DeliveryCenterForOperatorRef, null, true);
            this.DeliveryPoints = ReferencesProvider.GetReferences(Constants.DeliveryPointRef, null, true);
            this.listBSOStatuses = StatusBSOProvider.GetBSOListStatus(false);
            this.listBSOStatusesAvailable = StatusBSOProvider.GetAvailableBSOStatus(bso.Status.Id, true);
            this.listBSOResponsibles = StatusBSOProvider.GetListBSOResponsibles(true);
            validator = new BSOSaveDataModelValidator();
        }

        public BSO.SaveData GetBSOSaveData()
        {
            BSO.SaveData data = new BSO.SaveData();
            data.Id = Id;
            data.TemporaryPolicyNumber = TemporaryPolicyNumber;
            data.PolicyPartyNumber = PolicyPartyNumber;
            data.StatusId = StatusId;
            data.StatusDate = StatusDate;
            data.DeliveryCenterId = DeliveryCenterId;
            data.DeliveryPointId = DeliveryPointId;
            data.VisitGroupId = VisitGroupId;
            data.Comment = Comment;
            data.UserId = UserId;
            data.ResponsibleID = ResponsibleID;
            data.ChangeDate = ChangeDate;
            return data;
        }

        public BSOSaveDataModel UpdateBSOIssuedClient(ClientVisitSaveDataModel model, User user)
        {
            this.StatusId = (long)ListBSOStatusID.OnClient;
            this.DeliveryPointId = model.DeliveryPointId;
            this.StatusDate = model.TemporaryPolicyDate == null ? (model.StatusDate == null ? System.DateTime.Now : model.StatusDate) : model.TemporaryPolicyDate;
            this.UserId = user.Id;
            this.DeliveryCenterId = model.DeliveryCenterId;
            this.VisitGroupId = model.VisitGroupId;
            this.Comment = "Выдан клиенту при обращении";
            this.ChangeDate = DateTime.Now;
            return this;
        }

        public BSOSaveDataModel GetAvailableBSOStatus(long? statusId, bool withDefaultEmpty = false)
        {
            this.listBSOStatusesAvailable = StatusBSOProvider.GetAvailableBSOStatus(statusId, withDefaultEmpty);
            return this;
        }
    }
}