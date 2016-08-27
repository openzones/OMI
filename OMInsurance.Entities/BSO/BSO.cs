using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;

namespace OMInsurance.Entities
{
    /// <summary>
    /// Бланк строгой отчетности БСО
    /// </summary>
    public class BSO : DataObject
    {
        public BSO()
        {
            History = new List<BSOHistoryItem>();
            Status = new BSOStatusRef();
        }

        /// <summary>
        /// Номер бланка БСО
        /// </summary>
        public string TemporaryPolicyNumber { get; set; }

        /// <summary>
        /// Номер партии бланка БСО
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
        /// Идентификатор пункта выдачи бланка БСО
        /// </summary>
        public long? DeliveryCenterId { get; set; }
        public string DeliveryCenter { get; set; }

        /// <summary>
        /// Идентификатор места выдачи
        /// </summary>
        public long? DeliveryPointId { get; set; }
        public string DeliveryPoint { get; set; }

        /// <summary>
        /// Идентификатор обращения
        /// </summary>
        public long? VisitGroupId { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Пользователь изменивший статус
        /// </summary>
        public long? UserId { get; set; }


        /// <summary>
        /// Ответственный БСО
        /// </summary>
        public long? ResponsibleID { get; set; }

        /// <summary>
        /// История БСО
        /// </summary>
        public List<BSOHistoryItem> History { get; set; }

        /// <summary>
        /// Дата изменений
        /// </summary>
        public DateTime? ChangeDate { get; set; }

        /// <summary>
        /// Данные для создания или обновления данных о бланке БСО
        /// </summary>
        public class SaveData
        {
            public SaveData()
            {
            }

            public SaveData(BSO bso)
            {
                this.Id = bso.Id;
                this.TemporaryPolicyNumber = bso.TemporaryPolicyNumber;
                this.PolicyPartyNumber = bso.PolicyPartyNumber;
                this.StatusId = bso.Status.Id;
                this.StatusDate = bso.StatusDate;
                this.DeliveryCenterId = bso.DeliveryCenterId;
                this.DeliveryPointId = bso.DeliveryPointId;
                this.VisitGroupId = bso.VisitGroupId;
                this.Comment = bso.Comment;
                this.UserId = bso.UserId;
                this.ResponsibleID = bso.ResponsibleID;
                this.ChangeDate = bso.ChangeDate;
            }

            /// <summary>
            /// Идентификатор бланка, который необходимо обновить
            /// </summary>
            public long? Id { get; set; }
            
            /// <summary>
            /// Номер бланка БСО
            /// </summary>
            public string TemporaryPolicyNumber { get; set; }

            /// <summary>
            /// Номер партии бланка БСО
            /// </summary>
            public string PolicyPartyNumber { get; set; }

            /// <summary>
            /// Статус бланка БСО
            /// </summary>
            public long? StatusId { get; set; }

            /// <summary>
            /// Дата последнего статус БСО
            /// </summary>
            public DateTime? StatusDate { get; set; }

            /// <summary>
            /// Идентификатор пункта выдачи бланка БСО
            /// </summary>
            public long? DeliveryCenterId { get; set; }
            public string DeliveryCenter { get; set; }

            /// <summary>
            /// Идентификатор обращения
            /// </summary>
            public long? VisitGroupId { get; set; }

            /// <summary>
            /// Комментарий
            /// </summary>
            public string Comment { get; set; }

            /// <summary>
            /// Идентификатор места выдачи
            /// </summary>
            public long? DeliveryPointId { get; set; }

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

   
}
