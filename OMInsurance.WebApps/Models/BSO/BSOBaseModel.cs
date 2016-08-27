using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OMInsurance.Entities;
using System.ComponentModel;

namespace OMInsurance.WebApps.Models
{
    public class BSOBaseModel
    {
        [DisplayName("ID")]
        public long BSO_ID { get; set; }

        [DisplayName("Номер БСО")]
        public string TemporaryPolicyNumber { get; set; }

        [DisplayName("Номер партии")]
        public string PolicyPartyNumber { get; set; }

        [DisplayName("Статус")]
        public BSOStatusRef Status { get; set; }

        [DisplayName("Дата статуса")]
        public DateTime? StatusDate { get; set; }

        public long? DeliveryCenterId { get; set; }

        [DisplayName("Пункт выдачи")]
        public string DeliveryCenter { get; set; }

        [DisplayName("Точка выдачи")]
        public long? DeliveryPointId { get; set; }

        [DisplayName("Точка выдачи")]
        public string DeliveryPoint { get; set; }

        [DisplayName("Комментарий")]
        public string Comment { get; set; }

        [DisplayName("История БСО")]
        public List<BSOHistoryItem> History { get; set; }

        [DisplayName("История БСО")]
        public IEnumerable<BSOHistoryModel> HistoryModel { get; set; }

        [DisplayName("Пользователь")]
        public long? UserId { get; set; }

        [DisplayName("Ответственный")]
        public long? ResponsibleID { get; set; }

        [DisplayName("Дата изменения")]
        public DateTime? ChangeDate { get; set; }

        [DisplayName("Идентификатор обращения")]
        public long? VisitGroupId { get; set; }

        public long? ClientVisitId{ get; set; }

        public long? ClientId { get; set; }

        public BSOBaseModel(BSO bso) : base()
        {
            if(bso != null)
            {
                this.BSO_ID = bso.Id;
                this.TemporaryPolicyNumber = bso.TemporaryPolicyNumber;
                this.PolicyPartyNumber = bso.PolicyPartyNumber;
                this.Status = bso.Status;
                this.StatusDate = bso.StatusDate;
                this.DeliveryCenterId = bso.DeliveryCenterId;
                this.DeliveryCenter = bso.DeliveryCenter;
                this.DeliveryPointId = bso.DeliveryPointId;
                this.DeliveryPoint = bso.DeliveryPoint;
                this.Comment = bso.Comment;
                this.UserId = bso.UserId;
                this.History = bso.History;
                this.VisitGroupId = bso.VisitGroupId;
                this.ResponsibleID = bso.ResponsibleID;
                this.ChangeDate = bso.ChangeDate;

                List<BSOHistoryModel> listBsoHistoryModel = new List<BSOHistoryModel>();
                foreach(BSOHistoryItem a in bso.History)
                {
                    BSOHistoryModel bsoHistoryModel = new BSOHistoryModel(a);
                    listBsoHistoryModel.Add(bsoHistoryModel);
                }
                this.HistoryModel = listBsoHistoryModel;
            }
        }
    }
}