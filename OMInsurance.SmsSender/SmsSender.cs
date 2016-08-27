using System;
using System.Collections.Generic;
using OMInsurance.Entities.SMS;
using OMInsurance.SmsSender.ServiceReferenceSms;
using System.IO;
using System.Linq;
using System.Threading;

namespace OMInsurance.SmsSender
{
    public class SmsSender
    {
        /// <summary>
        /// Очищаем телефон от символов и приводим к формату 81234567890 - 11 символов
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static string ClearPhone(string phone)
        {
            string[] trimChars = { "-", "(", ")" };
            phone = "8" + phone;
            foreach (var ch in trimChars)
            {
                phone = phone.Replace(ch, "");
            }
            return phone;
        }

        public static List<SmsResult> Send(List<SMSMessage> list)
        {
            List<SmsMessage> items = new List<SmsMessage>();
            foreach (var sms in list)
            {
                SmsMessage item = new SmsMessage();
                item.MessageId = sms.Id;
                if (string.IsNullOrEmpty(sms.SenderId))
                {
                    item.SenderId = string.Empty;
                }
                else
                {
                    item.SenderId = sms.SenderId.Trim();
                }

                if (string.IsNullOrEmpty(sms.Phone))
                {
                    item.Phone = string.Empty;
                }
                else
                {
                    item.Phone = ClearPhone(sms.Phone);
                }

                if (string.IsNullOrEmpty(sms.Message))
                {
                    item.Message = string.Empty;
                }
                else
                {
                    item.Message = sms.Message.Trim();
                }
                items.Add(item);
            }

            if (items.Count == 0) return null;

            List<SmsSendingResult> resultService = new List<SmsSendingResult>();
            using (EasySmsServiceClient client = new EasySmsServiceClient())
            {
                client.Open();
                resultService.AddRange(client.SendMessages(items));
                client.Close();
            }
            List<SmsResult> results = new List<SmsResult>();
            foreach (var sms in resultService)
            {
                results.Add(new SmsResult()
                {
                    Id = sms.MessageId,
                    StatusIdInside = 1, //смс было отправлено
                    StatusFromService = sms.ErrorCode,
                    MessageFromService = sms.ErrorMessage,
                    SendDate = DateTime.Now,
                    IsSuccess = sms.IsSuccess
                });
            }

            //WriteFileItems(items);
            //WriteFileResult(resultService);
            //results = Stab(items);
            return results;
        }


        /// <summary>
        /// программная заглушка
        /// </summary>
        /// <returns></returns>
        public static List<SmsResult> Stab(List<SmsMessage> items)
        {

            Dictionary<string, string> dictonary = new Dictionary<string, string>();
            dictonary.Add("201", "Неизвестная ошибка");
            dictonary.Add("202", "Неправильный формат документа");
            dictonary.Add("203", "Ошибка авторизации");
            dictonary.Add("204", "Неизвестное имя отправителя");
            dictonary.Add("205", "Недостаточно денег на счете");
            dictonary.Add("206", "Неверный формат телефона получателя");
            dictonary.Add("207", "Сообщение с sync_id уже отправлено");
            dictonary.Add("208", "Пользователь заблокирован");
            dictonary.Add("211", "Указанный текст не разрешен к отправке");
            dictonary.Add("213", "Отправка смс в заданную страну не разрешена");
            dictonary.Add("", "Успешная отправка");

            List<SmsResult> results = new List<SmsResult>();
            Random r = new Random();
            foreach (var sms in items)
            {
                int temp = r.Next(dictonary.Count);
                results.Add(new SmsResult()
                {
                    Id = sms.MessageId,
                    StatusIdInside = 1,
                    StatusFromService = dictonary.ElementAt(temp).Key,
                    MessageFromService = dictonary.ElementAt(temp).Value,
                    SendDate = DateTime.Now,
                    IsSuccess = true
                });
            }
            return results;
        }


        public static void WriteFileResult(List<SmsSendingResult> resultService)
        {
            StreamWriter file;
            using (file = new StreamWriter(new FileStream("C:\\Items.log", System.IO.FileMode.Append)))
            {
                file.WriteLine("Result elements " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                foreach (var elem in resultService)
                {
                    file.WriteLine(elem.MessageId + " " + elem.ErrorCode + " " + elem.ErrorMessage + " " + elem.IsSuccess);
                }
                file.Flush();
                file.Close();
            }
        }

        public static void WriteFileItems(List<SmsMessage> items)
        {
            StreamWriter file;
            using (file = new StreamWriter(new FileStream("C:\\Items.log", System.IO.FileMode.Append)))
            {
                file.WriteLine("Input elements " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                foreach (var elem in items)
                {
                    file.WriteLine(elem.MessageId + " " + elem.Phone);
                }
                file.Flush();
                file.Close();
            }
        }


        public static string SendOneSms(SMSMessage sms)
        {
            if (!string.IsNullOrEmpty(sms.SenderId)) sms.SenderId.Trim();

            SmsMessage input = new SmsMessage()
            {
                MessageId = sms.Id,
                SenderId = sms.SenderId,
                Phone = sms.Phone,
                Message = sms.Message
            };

            SmsSendingResult result = new SmsSendingResult();
            using (EasySmsServiceClient client = new EasySmsServiceClient())
            {
                client.Open();
                result = client.SendMessage(input);
                client.Close();
            }
            return result.ErrorMessage;
        }
    }
}
