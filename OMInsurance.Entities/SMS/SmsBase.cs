using System;
using OMInsurance.Entities.Core;

namespace OMInsurance.Entities.SMS
{
    public class SmsBase : DataObject
    {
        public string SenderId { get; set; }
        public long ClientId { get; set; }
        public long VisitGroupId { get; set; }
        public long VisitId { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// причина создания записи
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        ///статус отправки внутренний
        /// </summary>
        public long? StatusIdInside { get; set; }

        /// <summary>
        /// id статуса из сервиса
        /// </summary>
        public string StatusFromService { get; set; }

        /// <summary>
        /// сообщение статуса из сервиса
        /// </summary>
        public string MessageFromService { get; set; }

        /// <summary>
        /// дата и время отправки
        /// </summary>
        public DateTime? SendDate { get; set; }

        /// <summary>
        /// повторный статус
        /// </summary>
        public long? StatusIdRepeat { get; set; }

        /// <summary>
        /// дата повторного статуса
        /// </summary>
        public DateTime? StatuRepeatDate { get; set; }


        /// <summary>
        /// входные параметры для формирования данных в таблицы SMSBase
        /// </summary>
        public class SmsBaseSet
        {
            public SmsBaseSet()
            {
            }

            public SmsBaseSet(SmsTemplate smsTemplate)
            {
                this.SenderId = smsTemplate.SenderId;
                this.StatusId = smsTemplate.StatusId;
                this.StatusDate = DateTime.Now.AddDays(-5);//Установка. Выбираем по текущей дате -5 дней
                this.CreateDate = DateTime.Now;
                this.Message = smsTemplate.Message;
                if (smsTemplate.StatusId == 9) this.Comment = "Полис готов";

            }
            /// <summary>
            /// по умолчанию MSK-UralSib
            /// </summary>
            public string SenderId { get; set; }

            /// <summary>
            /// Статус полиса (готов)
            /// </summary>
            public long? StatusId { get; set; }

            /// <summary>
            /// Статус готовности полиса - 5 дней
            /// </summary>
            public DateTime StatusDate { get; set; }

            /// <summary>
            /// Дата записи. Обычно DateTime.Now
            /// </summary>
            public DateTime CreateDate { get; set; }

            /// <summary>
            /// Сообщение для пользователя. "Ваш полис ОМС изготовлен. Подробности на сайте: msk-uralsib.ru"
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// причина создания записи. Например: "Полис готов."
            /// </summary>
            public string Comment { get; set; }

        }

        public class SmsBaseGet
        {
            public DateTime CreateDateFrom { get; set; }
            public DateTime CreateDateTo { get; set; }
        }


    }
}
