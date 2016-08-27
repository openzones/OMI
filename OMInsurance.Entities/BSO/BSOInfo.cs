using OMInsurance.Entities.Core;
using System;

namespace OMInsurance.Entities
{
    public class BSOInfo : DataObject
    {
        /// <summary>
        /// Номер бланка БСО
        /// </summary>
        public string TemporaryPolicyNumber { get; set; }

        /// <summary>
        /// Номер бланка БСО
        /// </summary>
        public string PolicyPartyNumber { get; set; }

        /// <summary>
        /// Статус бланка БСО
        /// </summary>
        public BSOStatusRef Status { get; set; }

        /// <summary>
        /// Дата последнего статус БСО
        /// </summary>
        public DateTime? StatusDate { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Идентификатор пункта выдачи бланка БСО
        /// </summary>
        public long? DeliveryCenterId { get; set; }
        public string DeliveryCenter { get; set; }

        /// <summary>
        /// Идентификатор точки выдачи
        /// </summary>
        public long? DeliveryPointId { get; set; }
        public string DeliveryPoint { get; set; }

        /// <summary>
        /// Ответственный
        /// </summary>
        public long? ResponsibleID { get; set; }
        public string ResponsibleName { get; set; }

        /// <summary>
        /// Дата изменений
        /// </summary>
        public DateTime? ChangeDate { get; set; }
    }
}
