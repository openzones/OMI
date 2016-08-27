using OMInsurance.Entities.Core;
using System;

namespace OMInsurance.Entities
{
    /// <summary>
    /// Класс, описывающий действия с БСО
    /// </summary>
    public class BSOHistoryItem : DataObject
    {
        /// <summary>
        /// Статус бланка
        /// </summary>
        public BSOStatusRef Status { get; set; } 

        /// <summary>
        /// Дата статуса
        /// </summary>
        public DateTime? StatusDate { get; set; } 
        
        /// <summary>
        /// Идентификатор обращения
        /// </summary>
        public long? VisitGroupId { get; set; }
        
        /// <summary>
        /// Комментарий
        /// </summary>
        public string Comment { get; set; }
        
        /// <summary>
        /// Идентификатор пункта
        /// </summary>
        public long? DeliveryCenterId { get; set; }
        public string DeliveryCenter { get; set; }

        /// <summary>
        /// Идентификатор места выдачи
        /// </summary>
        public long? DeliveryPointId { get; set; }
        public string DeliveryPoint { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// Ответственный БСО
        /// </summary>
        public long? ResponsibleID { get; set; }

        /// <summary>
        /// Дата изменений
        /// </summary>
        public DateTime? ChangeDate { get; set; }
    }
}
