using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OMInsurance.Entities;
using System.ComponentModel;
using OMInsurance.Entities.SMS;
using System.ComponentModel.DataAnnotations;
using OMInsurance.Entities.Core;
using System.Web.Mvc;

namespace OMInsurance.WebApps.Models
{
    public class SmsTemplateModel
    {
        [DisplayName("SenderId")]
        public string SenderId { get; set; }

        [DisplayName("Мобильный телефон")]
        [RegularExpression(Constants.PhoneRegex, ErrorMessage = "Неверное значение")]
        public string Phone { get; set; }

        [DisplayName("Шаблон сообщения")]
        public string Message { get; set; }

        [DisplayName("Дата последнего обновления шаблона")]
        public DateTime? CreateDate { get; set; }

        [DisplayName("Статус отправки")]
        public string Result { get; set; }

        [DisplayName("Статус заявки (полиса)")]
        public long? StatusId { get; set; }
        public List<SelectListItem> Statuses { get; set; }

        public SmsTemplateModel GetSmsTemplate(SmsTemplate sms)
        {
            if(sms != null)
            {
                this.SenderId = sms.SenderId;
                this.Phone = sms.Phone;
                this.Message = sms.Message;
                this.CreateDate = sms.CreateDate;
                this.Result = sms.Result;
                this.StatusId = sms.StatusId;
                this.Statuses = ReferencesProvider.GetReferences(Constants.ClientVisitStatusRef, null, true);
            }
            else
            {
                this.SenderId = "";
                this.Phone = "";
                this.Message = "";
                this.CreateDate = null;
                this.Result = "";
                this.StatusId = 9;
                this.Statuses = ReferencesProvider.GetReferences(Constants.ClientVisitStatusRef, null, true);
            }

            return this;
        }

        public SmsTemplate SetSmsTemplate ()
        {
            SmsTemplate sms = new SmsTemplate();
            sms.SenderId = this.SenderId.Trim();
            sms.Message = this.Message;
            sms.CreateDate = DateTime.Now;
            sms.Result = this.Result;
            sms.StatusId = this.StatusId;
            if (string.IsNullOrEmpty(this.Phone))
            {
                sms.Phone = this.Phone;
            }
            else
            {
                string str = this.Phone;
                string[] trimChars = { "-", "(", ")" };
                str = "8" + str.Trim();
                foreach (var ch in trimChars)
                {
                    str = str.Replace(ch, "");
                }
                sms.Phone = str;
            }
            return sms;
        }
    }
}