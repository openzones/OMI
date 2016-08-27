using OMInsurance.Entities.Core;
using System;
using System.ComponentModel;

namespace OMInsurance.WebApps.Models
{
    public class ClientVisitHistoryItemModel
    {
        public ClientVisitHistoryItemModel(Entities.ClientVisitHistoryItem item)
        {
            this.ClientVisitId = item.ClientVisitId;
            this.Status = item.Status;
            this.StatusDate = item.StatusDate;
            this.UserId = item.UserId;
            this.UserLogin = item.UserLogin;
            this.UserFirstname = item.UserFirstname;
            this.UserSecondname = item.UserSecondname;
            this.UserLastname = item.UserLastname;
        }
        [DisplayName("Номер заявки")]
        public long ClientVisitId { get; set; }

        [DisplayName("Статус")]
        public ReferenceItem Status { get; set; }

        [DisplayName("Дата статуса")]
        public DateTime StatusDate { get; set; }

        [DisplayName("UserId")]
        public long? UserId { get; set; }

        [DisplayName("Логин пользователя")]
        public string UserLogin { get; set; }

        [DisplayName("Имя пользователя")]
        public string UserFirstname { get; set; }
        
        [DisplayName("Отчество пользователя")]
        public string UserSecondname { get; set; }

        [DisplayName("Фамилия пользователя")]
        public string UserLastname { get; set; }
    }
}