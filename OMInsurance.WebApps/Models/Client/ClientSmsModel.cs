using System;
using OMInsurance.Entities.SMS;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace OMInsurance.WebApps.Models
{
    public class ClientSmsModel
    {
        [DisplayName("SenderId")]
        public string SenderId { get; set; }

        [DisplayName("ID клиента")]
        public long ClientId { get; set; }

        [DisplayName("Обращение")]
        public long VisitGroupId { get; set; }

        [DisplayName("Заявка")]
        public long VisitId { get; set; }

        [DisplayName("Телефон")]
        public string Phone { get; set; }

        [DisplayName("Отправленное сообщение")]
        public string Message { get; set; }

        [DisplayName("Дата создания")]
        public DateTime CreateDate { get; set; }

        [DisplayName("Причина оповещения")]
        public string Comment { get; set; }

        [DisplayName("Внутр. статус")]
        public long? StatusIdInside { get; set; }

        [DisplayName("Статус отправки")]
        public string StatusFromService { get; set; }

        [DisplayName("Сообщение из сервиса")]
        public string MessageFromService { get; set; }

        [DisplayName("Дата отправки")]
        public DateTime? SendDate { get; set; }

        public ClientSmsModel(SmsBase sms)
        {
            this.SenderId = sms.SenderId;
            this.ClientId = sms.ClientId;
            this.VisitGroupId = sms.VisitGroupId;
            this.VisitId = sms.VisitId;
            this.Phone = sms.Phone;
            this.Message = sms.Message;
            this.CreateDate = sms.CreateDate;
            this.Comment = sms.Comment;
            this.StatusIdInside = sms.StatusIdInside;
            if (string.IsNullOrEmpty(sms.StatusFromService) && sms.SendDate != null)
            {
                this.StatusFromService = "Успешно";
            }
            else
            {
                this.StatusFromService = sms.StatusFromService;
            }

            if (string.IsNullOrEmpty(sms.MessageFromService) && sms.SendDate != null)
            {
                this.MessageFromService = "Сообщение успешно отправлено";
            }
            else
            {
                this.MessageFromService = sms.MessageFromService;
            }

            this.SendDate = sms.SendDate;
        }
    }
}