using OMInsurance.Configuration;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using OMInsurance.WebApps.Models.Core;
using OMInsurance.WebApps.Models.Heplers;
using OMInsurance.WebApps.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace OMInsurance.WebApps.Models
{
    public class ClientPretensionModel
    {
        public ClientPretensionModel()
        {
            this.M_expertS = ReferencesProvider.GetExpert(true);
            this.M_osn230 = ReferencesProvider.GetCodeDefect(true);
        }

        public ClientPretensionModel ClientPretensionGeneration(ClientPretension pretension, Client client, User user = null)
        {
            ClientPretensionModel data = new ClientPretensionModel();
            this.ClientId = pretension.ClientID;
            this.Generator = pretension.Generator;
            this.LPU_ID = pretension.LPU_ID;
            this.DATE_IN = pretension.DATE_IN;
            this.M_nakt = pretension.M_nakt;
            this.MedicalCenterId = pretension.MedicalCenterId;
            this.M_mo = pretension.M_mo;
            this.M_mcod = pretension.M_mcod;
            this.M_period = pretension.M_period;
            this.M_snpol = pretension.M_snpol;

            this.M_fd = "заявление " +
                        client.ActualVersion.Lastname + " " +
                        client.ActualVersion.Firstname + " " +
                        client.ActualVersion.Secondname + " " +
                        (client.ActualVersion.Birthday.HasValue ? ((DateTime)client.ActualVersion.Birthday).ToShortDateString() : string.Empty) +
                        " г. или ее представителя о выборе " +
                        this.M_mo +
                        " для оказания первичной медико-санитарной помощи";

            this.M_nd1 = "заявление " +
                        client.ActualVersion.Lastname + " " +
                        client.ActualVersion.Firstname + " " +
                        client.ActualVersion.Secondname + " " +
                        (client.ActualVersion.Birthday.HasValue ? ((DateTime)client.ActualVersion.Birthday).ToShortDateString() : string.Empty) +
                        " г. или ее представителя о выборе " +
                        this.M_mo +
                        " для оказания первичной медико-санитарной помощи от " +
                        (this.DATE_IN.HasValue ? ((DateTime)this.DATE_IN).ToShortDateString() : string.Empty) +
                        " г. ";

            this.M_nd2 = "Прикрепление застрахованного к " +
                         this.M_mo +
                         " от " +
                         (this.DATE_IN.HasValue ? ((DateTime)this.DATE_IN).ToShortDateString() : string.Empty) +
                         " г. признано ";

            if (user != null)
            {
                this.UserId = user.Id;
                this.UserPosition = user.Position;
                this.UserFIO = user.Fullname;
            }
            this.CreateDate = DateTime.Now;
            //this.M_expertS = ReferencesProvider.GetExpert(true);
            //this.M_osn230 = ReferencesProvider.GetCodeDefect(true);
            return this;
        }

        public ClientPretensionModel(ClientPretension pretension)
        {
            this.sevenSimbolGeneraton = pretension.sevenSimbolGeneration;
            this.Generator = pretension.Generator;
            this.ClientId = pretension.ClientID;
            this.LPU_ID = pretension.LPU_ID; 
            this.DATE_IN = pretension.DATE_IN;
            this.M_nakt = pretension.M_nakt;
            this.M_dakt = pretension.M_dakt;
            this.M_dakt2 = pretension.M_dakt;
            this.M_expert_Id = pretension.M_expert_Id;
            this.MedicalCenterId = pretension.MedicalCenterId;
            this.M_mo = pretension.M_mo; //
            this.M_mcod = pretension.M_mcod; //
            this.M_period = pretension.M_period;
            this.M_snpol = pretension.M_snpol;
            this.M_fd = pretension.M_fd;
            this.M_nd1 = pretension.M_nd1;
            this.M_nd2 = pretension.M_nd2;
            this.IsConfirm = pretension.IsConfirm;
            this.M_osn230_Id = pretension.M_osn230_Id;
            this.M_straf = pretension.M_straf;
            this.PeriodFrom = pretension.PeriodFrom;
            this.PeriodTo = pretension.PeriodTo;
            this.Coefficient = pretension.Coefficient;
            this.UserId = pretension.UserId;
            this.UserFIO = pretension.UserFIO; //
            this.CreateDate = pretension.CreateDate;
            this.UserPosition = pretension.UserPosition; //
            this.UpdateUserId = pretension.UpdateUserId;
            this.UpdateUserFIO = pretension.UpdateUserFIO; //
            this.UpdateDate = pretension.UpdateDate ?? (DateTime)pretension.UpdateDate; //

            this.FileNameLPU = pretension.FileNameLPU ?? string.Empty;
            this.FileName2 = pretension.FileName2 ?? string.Empty;
            this.FileUrlLPU = pretension.FileUrlLPU ?? string.Empty;
            this.FileUrl2 = pretension.FileUrl2 ?? string.Empty;

            IsExistFileLPU = System.IO.File.Exists(Path.Combine(ConfiguraionProvider.FileStorageFolder, this.FileUrlLPU));
            if (IsExistFileLPU)
            {
                this.SizeFileLPU = GetFileSizeToString((float)new System.IO.FileInfo(Path.Combine(ConfiguraionProvider.FileStorageFolder, this.FileUrlLPU)).Length);
            }

            IsExistFile2 = System.IO.File.Exists(Path.Combine(ConfiguraionProvider.FileStorageFolder, this.FileUrl2));
            if (IsExistFile2)
            {
                this.SizeFile2 = GetFileSizeToString((float)new System.IO.FileInfo(Path.Combine(ConfiguraionProvider.FileStorageFolder, this.FileUrl2)).Length);
            }

            M_expertS = ReferencesProvider.GetExpert(true);
            M_osn230 = ReferencesProvider.GetCodeDefect(true);
        }

        public ClientPretension GetClientPretension()
        {
            ClientPretension pretension = new ClientPretension();

            pretension.sevenSimbolGeneration = this.sevenSimbolGeneraton;

            pretension.Generator = this.Generator;
            pretension.ClientID = this.ClientId;
            pretension.LPU_ID = this.LPU_ID;
            pretension.DATE_IN = this.DATE_IN;
            pretension.M_nakt = this.M_nakt;
            pretension.M_dakt = this.M_dakt;
            pretension.M_expert_Id = this.M_expert_Id;
            pretension.M_expert = this.M_expertS.Where(a => a.Value == this.M_expert_Id.ToString()).Select(b => b.Text).FirstOrDefault();
            pretension.MedicalCenterId = this.MedicalCenterId;
            pretension.M_mo = this.M_mo;
            pretension.M_mcod = this.M_mcod; 
            pretension.M_period = this.M_period;
            pretension.M_snpol = this.M_snpol;
            pretension.M_fd = this.M_fd;
            pretension.M_nd1 = this.M_nd1;
            pretension.M_nd2 = this.M_nd2;
            pretension.IsConfirm = this.IsConfirm;
            pretension.M_osn230_Id = this.M_osn230_Id;
            pretension.M_osn230Ref = ReferencesProvider.GetUniversalReference(Constants.Defect);
            pretension.M_straf = this.M_straf;
            pretension.PeriodFrom = this.PeriodFrom;
            pretension.PeriodTo = this.PeriodTo;
            pretension.Coefficient = this.Coefficient;
            pretension.UserId = this.UserId;
            pretension.UserFIO = this.UserFIO;
            pretension.UserPosition = this.UserPosition;
            pretension.UpdateUserId = this.UpdateUserId;
            //this.UpdateUserFIO = pretension.UpdateUserFIO; //
            pretension.MCOD = ReferencesProvider.GetUniversalReference(Constants.MedicalCenterRef).Where(a => a.Id == this.MedicalCenterId).Select(b => b.MCOD).FirstOrDefault();

            pretension.CreateDate = this.CreateDate;
            pretension.UpdateDate = this.UpdateDate;

            pretension.FileNameLPU = this.FileNameLPU;
            pretension.FileName2 = this.FileName2;
            pretension.FileUrlLPU = this.FileUrlLPU;
            pretension.FileUrl2 = this.FileUrl2;

            return pretension;
        }

        /// <summary>
        ///семь сгенерированных символов, используется в имени файла
        /// </summary>
        public string sevenSimbolGeneraton { get; set; }

        /// <summary>
        /// используется при генерации 4 цифр
        /// </summary>
        public long Generator { get; set; }
        public long ClientId { get; set; }
        public long? LPU_ID { get; set; }
        public DateTime? DATE_IN { get; set; }


        //Акт медико-экономической экспертизы (целевой)

        /// <summary>
        ///номер акта - генерируется "№P2"LPU_ID"121"4 генерирующихся цифры"/p"
        ///пример: №P212341210000/p
        /// </summary>
        [DisplayName("Номер акта")]
        public string M_nakt { get; set; }

        /// <summary>
        /// дата акта - выбирается вручную
        /// </summary>
        [DisplayName("Дата акта")]
        public DateTime? M_dakt { get; set; }
        public DateTime? M_dakt2 { get; set; }

        [DisplayName("Эксперт")]
        public long? M_expert_Id { get; set; }
        public List<SelectListItem> M_expertS { get; set; }

        public long? MedicalCenterId { get; set; }
        [DisplayName("Наименование МО")]
        public string M_mo { get; set; }

        [DisplayName("Код МО")]
        public string M_mcod { get; set; }

        /// <summary>
        /// период - из номерника DATE_IN из которого год и месяц (в такой очередности) без точек, только 6 цифр
        /// </summary>
        [DisplayName("Период")]
        public string M_period { get; set; }

        /// <summary>
        /// номер документа ОМС (ЕНП или Номер полиса)
        /// </summary>
        [DisplayName("Номер документа ОМС")]
        public string M_snpol { get; set; }

        /// <summary>
        ///Генерируемая фраза:
        ///заявление 'ФИО полностью' д.р. 'дата рождения' г. или ее представителя о выборе 'ЛПУ из справочника по номернику' для оказания первичной медико-санитарной помощи
        /// </summary>
        [DisplayName("Генерируемая фраза")]
        public string M_fd { get; set; }

        /// <summary>
        ///Генерируемая фраза:
        ///заявление 'ФИО' д.р. 'дата рождения' г. или ее представителя о выборе 'ЛПУ по справочнику из номерника' для оказания первичной медико-санитарной помощи
        ///от 'дата из номерника' г. не представлено. Прикрепление застрахованного к ФБЛПУ "Поликлиника ФНС России" от 16.02.2016г. признано необоснованным
        /// </summary>
        [DisplayName("Генерируемая фраза")]
        public string M_nd1 { get; set; }

        [DisplayName("Генерируемая фраза")]
        public string M_nd2 { get; set; }

        /// <summary>
        /// не представлено / представлено
        /// необосновано /обосновано
        /// </summary>
        [DisplayName("Обосновано/необосновано")]
        public bool? IsConfirm { get; set; }

        /// <summary>
        /// код дефекта - из справочника
        /// </summary>
        [DisplayName("Код дефекта/нарушения (osn230)")]
        public long? M_osn230_Id { get; set; }
        public List<SelectListItem> M_osn230 { get; set; }

        /// <summary>
        /// код дефекта в другой кодировке - из справочника
        /// </summary>
        [DisplayName("Код дефекта (err)")]
        public string M_er_c { get; set; }

        /// <summary>
        /// сумма штрафа - справочник, по DATE_IN из номерника определяем дату, и выставляем соответствующую сумму по периоду
        /// </summary>
        [DisplayName("Сумма штрафа")]
        public float? M_straf { get; set; }


        //Предписание
        //поле 1 - генератор                    long     generator
        //поле 2 - дата акта                    DateTime m_dakt
        //поле 3 - название МО из справочника   string m_mo

        //поле 4 и поле 5
        //период с... по...
        public DateTime? PeriodFrom { get; set; }
        public DateTime? PeriodTo { get; set; }

        //поле 6 - справочник, по DATE_IN из номерника определяем дату, и выставляем соответствующий коэффициент определив по периоду
        [DisplayName("Коэффициент")]
        public float? Coefficient { get; set; }

        public long? UserId { get; set; }

        //поле 7 - должность из учетной записи, того кто делает
        [DisplayName("Должность")]
        public string UserPosition { get; set; }

        [DisplayName("Полное ФИО")]
        //поле 8 - Фамилия и инициалы из учетной записи, того кто делает
        public string UserFIO { get; set; }

        [DisplayName("Дата создания:")]
        public DateTime CreateDate { get; set; }

        public long? UpdateUserId { get; set; }

        [DisplayName("Обновил:")]
        public string UpdateUserFIO { get; set; }

        [DisplayName("Дата обновления:")]
        public DateTime UpdateDate { get; set; }


        public bool IsExistFileLPU { get; set; }
        public string FileNameLPU { get; set; }
        public string FileUrlLPU { get; set; }

        [DisplayName("Размер")]
        public string SizeFileLPU { get; set; }

        public bool IsExistFile2 { get; set; }
        public string FileName2 { get; set; }
        public string FileUrl2 { get; set; }

        [DisplayName("Размер")]
        public string SizeFile2 { get; set; }













        private string GetFileSizeToString(float size)
        {
            if ((size / 1024 / 1024 / 1024) > 1)
            {
                return string.Format("{0:F2} Гб", size / 1024 / 1024 / 1024);
            }
            else
            {
                if ((size / 1024 / 1024) > 1)
                {
                    return string.Format("{0:F2} Мб", size / 1024 / 1024);
                }
                else
                {
                    if ((size / 1024) > 1)
                    {
                        return string.Format("{0:F2} Кб", size / 1024);
                    }
                    else
                    {
                        return string.Format("{0:F2} байт", size);
                    }
                }
            }
        }

    }
}
