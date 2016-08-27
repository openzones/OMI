using System;
using System.Collections.Generic;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace OMInsurance.WebApps.Models
{
    public class ReferenceUniversalItemModel
    {
        public static List<ReferenceUniversalItemModel> ListReferenceUniversalItem = new List<ReferenceUniversalItemModel>();
        public static string ReferenceDisplayName { get; set; }

        [DisplayName("Название справочника")]
        public string ReferenceName { get; set; }

        [Required]
        [Display(Name = "Id", Prompt = "Id в таблице не должны дублироваться!")]
        public long? Id { get; set; }

        [Required]
        [DisplayName("Название")]
        public string Name { get; set; }

        [DisplayName("Код")]
        public string Code { get; set; }

        [DisplayName("Начало действия")]
        public DateTime? StartDate { get; set; }

        [DisplayName("Конец действия")]
        public DateTime? EndDate { get; set; }

        [Required]
        [DisplayName("Доступно для Регистратора")]
        public bool? IsEnabledForRegistrator { get; set; }

        [Required]
        [DisplayName("Доступно для Оператора")]
        public bool? IsEnabledForOperator { get; set; }

        public string SMO { get; set; }

        [DisplayName("Отображаемое название")]
        public string DisplayName { get; set; }

        [DisplayName("Округ/город")]
        public string District { get; set; }

        [DisplayName("Адрес")]
        public string Address { get; set; }

        [DisplayName("Телефон")]
        public string Phone { get; set; }

        [DisplayName("Часы работы")]
        public string WorkHours { get; set; }

        [Required]
        [DisplayName("Доступен эл.полис")]
        public bool? IsDigitPolicyAbailable { get; set; }

        [DisplayName("Родительский пункт")]
        public long? ParentId { get; set; }

        [Required]
        [DisplayName("Это МФЦ?")]
        public bool? IsMFC { get; set; }

        [DisplayName("Пункт (Id)")]
        public long? DeliveryCenterId { get; set; }

        [Display(Name = "Ответственный (Id)")]
        public long? DeliveryPointHeadId { get; set; }

        [Required]
        [DisplayName("Смс оповещение")]
        public bool? SendSms { get; set; }

        public long? LPU_ID_AIS { get; set; }
        public long? FIL_ID { get; set; }

        [StringLength(7, ErrorMessage = "Максимальная длина - 7 символов")]
        public string MCOD { get; set; }

        [StringLength(120, ErrorMessage = "Максимальная длина - 120 символов")]
        [DisplayName("Полное название")]
        public string FULLNAME { get; set; }

        [StringLength(15, ErrorMessage = "Максимальная длина - 15 символов")]
        [DisplayName("ОГРН")]
        public string OGRN { get; set; }

        [StringLength(6, ErrorMessage = "Максимальная длина - 6 символов")]
        public string FCOD { get; set; }

        [StringLength(50, ErrorMessage = "Максимальная длина - 50 символов")]
        [DisplayName("Фамилия")]
        public string Lastname { get; set; }

        [StringLength(50, ErrorMessage = "Максимальная длина - 50 символов")]
        [DisplayName("Имя")]
        public string Firstname { get; set; }

        [StringLength(50, ErrorMessage = "Максимальная длина - 50 символов")]
        [DisplayName("Отчество")]
        public string Secondname { get; set; }

        [StringLength(50, ErrorMessage = "Максимальная длина - 50 символов")]
        [DisplayName("Код ошибки (Er_c)")]
        public string ErrCode { get; set; }


        public List<SelectListItem> DeliveryCenters { get; set; }
        public List<SelectListItem> BSOResponsibles { get; set; }


        public static void FillListReferenceUniversal(List<ReferenceUniversalItem> reference)
        {
            ListReferenceUniversalItem = new List<ReferenceUniversalItemModel>();
            foreach (var item in reference)
            {
                ReferenceUniversalItemModel r = new ReferenceUniversalItemModel(item);
                ListReferenceUniversalItem.Add(r);
            }
        }

        public ReferenceUniversalItemModel()
        {
        }

        public ReferenceUniversalItemModel(ReferenceUniversalItem item)
        {
            ReferenceName = item.ReferenceName;
            Id = item.Id;
            Name = item.Name;
            DisplayName = item.DisplayName;
            Code = item.Code;
            District = item.District;
            SMO = item.SMO;
            Address = item.Address;
            Phone = item.Phone;
            WorkHours = item.WorkHours;
            IsDigitPolicyAbailable = item.IsDigitPolicyAbailable;
            ParentId = item.ParentId;
            StartDate = item.StartDate;
            EndDate = item.EndDate;
            IsEnabledForOperator = item.IsEnabledForOperator;
            IsEnabledForRegistrator = item.IsEnabledForRegistrator;
            IsMFC = item.IsMFC;
            SendSms = item.SendSms;
            DeliveryCenterId = item.DeliveryCenterId;
            DeliveryPointHeadId = item.DeliveryPointHeadId;
            LPU_ID_AIS = item.LPU_ID_AIS;
            FIL_ID = item.FIL_ID;
            MCOD = item.MCOD;
            FULLNAME = item.FULLNAME;
            OGRN = item.OGRN;
            FCOD = item.FCOD;
            Lastname = item.Lastname;
            Firstname = item.Firstname;
            Secondname = item.Secondname;
            ErrCode = item.ErrCode;
        }

        public void FillReferenceForView()
        {
            DeliveryCenters = ReferencesProvider.GetReferences(Constants.DeliveryCenterRef, null, true);
            BSOResponsibles = StatusBSOProvider.GetListBSOResponsibles(true);
        }
    }
}