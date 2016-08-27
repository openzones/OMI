using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System;
using System.Linq;
using System.ComponentModel;

namespace OMInsurance.WebApps.Models
{
    public class ClientVisitUpdateResultModel
    {
        [DisplayName("ЕНП")]
        public string UnifiedPolicyNumber { get; set; }

        [DisplayName("Фамилия")]
        public string Lastname { get; set; }

        [DisplayName("Имя")]
        public string Firstname { get; set; }

        [DisplayName("Отчество")]
        public string Secondname { get; set; }

        [DisplayName("Пол")]
        public string Sex { get; set; }

        [DisplayName("Дата рождения")]
        public DateTime? Birthday { get; set; }

        [DisplayName("Статус")]
        public string Status { get; set; }

        [DisplayName("Выгружено успешно")]
        public bool IsSuccess { get; set; }
        
        [DisplayName("Результат")]
        public string Message { get; set; }

        public long ClientId { get; set; }
        
        public ClientVisitUpdateResultModel(ClientVisit.UpdateResultData data)
        {
            ClientId = data.ClientId;
            UnifiedPolicyNumber = data.UnifiedPolicyNumber;
            Lastname = data.Lastname;
            Firstname = data.Firstname;
            Secondname = data.Secondname;
            Sex = data.Sex == 1 ? "Мужской" : "Женский";
            Birthday = data.Birthday;
            Status = data.Status == null || data.Status.Id == 0 
                ? string.Empty
                : ReferencesProvider.GetReferenceItems(Constants.ClientVisitStatusRef).FirstOrDefault(item => item.Id == data.Status.Id).Name;
            IsSuccess = data.IsSuccess;
            Message = data.Message;
        }
    }
}