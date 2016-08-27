using OMInsurance.Entities.Core;
using System;

namespace OMInsurance.Entities.SMS
{
    public class SmsResult : DataObject
    {
        /// <summary>
        /// наш внутренний статус
        /// </summary>
        public long? StatusIdInside { get; set; }

        /// <summary>
        /// Статус отправки/Код ошибки. Если null, то отправка успешна
        /// 
        /// </summary>
        public string StatusFromService { get; set; }

        /// <summary>
        /// Сообщение от сервиса
        /// </summary>
        public string MessageFromService { get; set; }

        /// <summary>
        /// Дата отправки
        /// </summary>
        public DateTime? SendDate { get; set; }

        /// <summary>
        /// True if sending was successful, otherwise is false
        /// </summary>
        public bool IsSuccess { get; set; }
    }
}
