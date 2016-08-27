using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OMInsurance.Entities;
using System.ComponentModel;

namespace OMInsurance.WebApps.Models
{
    public class BSOHistoryModel
    {
        [DisplayName("ID")]
        public long ID { get; set; }

        [DisplayName("Статус")]
        public BSOStatusRef Status { get; set; }

        [DisplayName("Дата статуса")]
        public DateTime? StatusDate { get; set; }

        [DisplayName("ID Обращения")]
        public long? VisitGroupId { get; set; }

        [DisplayName("Комментарий")]
        public string Comment { get; set; }

        [DisplayName("Пункт выдачи")]
        public long? DeliveryCenterId { get; set; }

        [DisplayName("Пункт выдачи")]
        public string DeliveryCenter { get; set; }

        [DisplayName("Точка выдачи")]
        public long? DeliveryPointId { get; set; }

        [DisplayName("Точка выдачи")]
        public string DeliveryPoint { get; set; }

        [DisplayName("Пользователь")]
        public long? UserId { get; set; }

        [DisplayName("Пользователь")]
        public string UserName { get; set; }

        [DisplayName("Ответственный")]
        public long? ResponsibleID { get; set; }

        [DisplayName("Ответственный")]
        public string ResponsibleName { get; set; }

        [DisplayName("Дата изменения")]
        public DateTime? ChangeDate { get; set; }

        public BSOHistoryModel(BSOHistoryItem a)
        {
            this.ID = a.Id;
            this.Status = a.Status;
            this.StatusDate = a.StatusDate;
            this.VisitGroupId = a.VisitGroupId;
            this.Comment = a.Comment;
            this.DeliveryCenterId = a.DeliveryCenterId;
            this.DeliveryCenter = a.DeliveryCenter;
            this.DeliveryPointId = a.DeliveryPointId;
            this.DeliveryPoint = a.DeliveryPoint;
            this.UserId = a.UserId;
            this.ResponsibleID = a.ResponsibleID;
            this.ChangeDate = a.ChangeDate;
        }
    }
}