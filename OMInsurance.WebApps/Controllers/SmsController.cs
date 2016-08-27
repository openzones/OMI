using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using OMInsurance.Entities.Searching;
using OMInsurance.WebApps.Models;
using OMInsurance.WebApps.Security;
using OMInsurance.Interfaces;
using OMInsurance.BusinessLogic;
using OMInsurance.WebApps.Validation;
using OMInsurance.Entities.SMS;
using OMInsurance.SmsSender;

namespace OMInsurance.WebApps.Controllers
{
    [AuthorizeUser]
    [AuthorizeUser(Roles = "Administrator")]
    public class SmsController : OMInsuranceController
    {
        ISmsBusinessLogic smsBusinessLogic;

        public SmsController()
        {
            smsBusinessLogic = new SmsBusinessLogic();
        }

        public ActionResult Index()
        {
            SmsTemplate sms = smsBusinessLogic.SmsTemplate_Get();
            SmsTemplateModel model = new SmsTemplateModel();
            model.GetSmsTemplate(sms);
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(SmsTemplateModel model)
        {
            SmsTemplate smsTemplate = new SmsTemplate();
            smsTemplate = model.SetSmsTemplate();
            smsBusinessLogic.SmsTemplate_Set(smsTemplate);
            SmsTemplate sms = smsBusinessLogic.SmsTemplate_Get();
            model = new SmsTemplateModel();
            model.GetSmsTemplate(sms);
            return View(model);
        }

        public ActionResult SendTestSms(SmsTemplateModel model)
        {
            SmsTemplate smsTemplate = smsBusinessLogic.SmsTemplate_Get();
            smsTemplate.Phone = model.Phone;
            model.GetSmsTemplate(smsTemplate);
            smsTemplate = model.SetSmsTemplate();

            SMSMessage smsMessage = new SMSMessage() {
                Id = 1,//в тестовой отправке смс ID не критично
                SenderId = smsTemplate.SenderId,
                Phone = smsTemplate.Phone,
                Message = smsTemplate.Message
            };
            if (!string.IsNullOrEmpty(smsMessage.Phone))
            {
                model.Result = SmsSender.SmsSender.SendOneSms(smsMessage);
                if (string.IsNullOrEmpty(model.Result)) model.Result = "Смс успешно отправлено.";
            }
            else
            {
                model.Result = "Введите номер телефона!";
            }

            return View("Index", model);
        }

    }
}