using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Web.Mvc;

namespace OMInsurance.WebApps.Models
{
    public class BSOBaseInfoModel
    {
        #region Properties
        [DisplayName("Id БСО")]
        public long BSO_ID { get; set; }

        [DisplayName("№ БСО")]
        public string TemporaryPolicyNumber { get; set; }

        [DisplayName("№ Партии")]
        public string PolicyPartyNumber { get; set; }

        [DisplayName("Статус")]
        public BSOStatusRef Status { get; set; }

        [DisplayName("Дата статуса")]
        public DateTime? StatusDate { get; set; }

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

        [DisplayName("Ответственный")]
        public long? ResponsibleID { get; set; }

        [DisplayName("Ответственный")]
        public string ResponsibleName { get; set; }

        [DisplayName("Дата изменения")]
        public DateTime? ChangeDate { get; set; }

        #endregion

        #region Constructors
        public BSOBaseInfoModel()
        {
        }

        public BSOBaseInfoModel(BSOInfo bsoInfo)
        {
            this.BSO_ID = bsoInfo.Id;
            this.TemporaryPolicyNumber = bsoInfo.TemporaryPolicyNumber;
            this.PolicyPartyNumber = bsoInfo.PolicyPartyNumber;
            this.Status = bsoInfo.Status;
            this.StatusDate = bsoInfo.StatusDate;
            this.Comment = bsoInfo.Comment;
            this.DeliveryCenterId = bsoInfo.DeliveryCenterId;
            this.DeliveryCenter = bsoInfo.DeliveryCenter;
            this.DeliveryPointId = bsoInfo.DeliveryPointId;
            this.DeliveryPoint = bsoInfo.DeliveryPoint;
            this.ResponsibleID = bsoInfo.ResponsibleID;
            this.ResponsibleName = bsoInfo.ResponsibleName;
            this.ChangeDate = bsoInfo.ChangeDate;
        }

        #endregion

        public BSOInfo GetBSOBaseInfo()
        {
            BSOInfo bsoInfo = new BSOInfo();
            bsoInfo.Id = this.BSO_ID;
            bsoInfo.TemporaryPolicyNumber = this.TemporaryPolicyNumber;
            bsoInfo.PolicyPartyNumber = this.PolicyPartyNumber;
            bsoInfo.Status = this.Status;
            bsoInfo.StatusDate = this.StatusDate;
            bsoInfo.Comment = this.Comment;
            bsoInfo.DeliveryCenterId = this.DeliveryCenterId;
            bsoInfo.DeliveryCenter = this.DeliveryCenter;
            bsoInfo.DeliveryPointId = this.DeliveryPointId;
            bsoInfo.DeliveryPoint = this.DeliveryPoint;
            bsoInfo.ResponsibleID = this.ResponsibleID;
            bsoInfo.ResponsibleName = this.ResponsibleName;
            bsoInfo.ChangeDate = this.ChangeDate;
            return bsoInfo;
        }
    }
}