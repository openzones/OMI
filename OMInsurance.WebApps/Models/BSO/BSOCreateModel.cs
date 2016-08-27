using OMInsurance.WebApps.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using OMInsurance.Entities;
using System.Web.Mvc;
using OMInsurance.Entities.Core;
using System.ComponentModel.DataAnnotations;

namespace OMInsurance.WebApps.Models
{
    public class BSOCreateModel
    {
        public BSOCreateModel()
        {
            this.StatusDate = DateTime.Now;
            this.ChangeDate = DateTime.Now;
            this.DeliveryCenters = ReferencesProvider.GetReferences(Constants.DeliveryCenterForOperatorRef, null, true);
            this.DeliveryPoints = ReferencesProvider.GetReferences(Constants.DeliveryPointRef, null, true);
            this.listBSOStatuses = StatusBSOProvider.GetBSOListStatus(false);
            this.GoodMessages = new List<string>();
            this.FailMessages = new List<string>();
        }

        [DisplayName("Id БСО")]
        public long? Id { get; set; }

        [DisplayName("№ БСО")]
        [StringLength(9, ErrorMessage = "Максимальная длина - 9 символов")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string TemporaryPolicyNumber { get; set; }

        [DisplayName("до № БСО")]
        [StringLength(9, ErrorMessage = "Максимальная длина - 9 символов")]
        public string TemporaryPolicyNumberTo { get; set; }

        [DisplayName("№ Партии")]
        [StringLength(9, ErrorMessage = "Максимальная длина - 3 символов")]
        public string PolicyPartyNumber { get; set; }

        [DisplayName("Статус")]
        public long? StatusId { get; set; }

        [DisplayName("Статус")]
        public string StatusName { get; set; }

        [DisplayName("Дата статуса")]
        public DateTime StatusDate { get; set; }

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

        [DisplayName("Дата изменения")]
        public DateTime? ChangeDate { get; set; }


        public List<string> GoodMessages { get; set; }
        public List<string> FailMessages { get; set; }
        public List<SelectListItem> DeliveryCenters { get; set; }
        public List<SelectListItem> DeliveryPoints { get; set; }
        public List<SelectListItem> listBSOStatuses { get; set; }


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
            data.ChangeDate = ChangeDate;
            return data;
        }
    }
}