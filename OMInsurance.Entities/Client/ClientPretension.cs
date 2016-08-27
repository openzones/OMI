using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;

namespace OMInsurance.Entities
{
    public class ClientPretension : DataObject
    {

        //семь сгенерированных символов, используется в имени файла
        public string sevenSimbolGeneration { get; set; }

        /// <summary>
        /// используется при генерации 4 цифр
        /// </summary>
        public long Generator { get; set; }
        public long ClientID { get; set; }
        public long? LPU_ID { get; set; }
        public DateTime? DATE_IN { get; set; }

        //Акт медико-экономической экспертизы (целевой)

        /// <summary>
        ///номер акта - генерируется "№P2"LPU_ID"121"4 генерирующихся цифры"/p"
        ///пример: №P212341210000/p
        /// </summary>
        public string M_nakt { get; set; }

        /// <summary>
        /// дата акта - выбирается вручную
        /// </summary>
        public DateTime? M_dakt { get; set; }

        /// <summary>
        /// эксперт
        /// </summary>
        public long? M_expert_Id { get; set; }
        public string M_expert { get; set; }


        public long? MedicalCenterId { get; set; }

        /// <summary>
        ///  наименование МО
        /// </summary>
        public string M_mo { get; set; }

        /// <summary>
        /// код МО
        /// </summary>
        public string M_mcod { get; set; }

        /// <summary>
        /// код MCOD
        /// </summary>
        public string MCOD { get; set; }

        /// <summary>
        /// период - из номерника DATE_IN из которого год и месяц (в такой очередности) без точек, только 6 цифр
        /// </summary>
        public string M_period { get; set; }

        /// <summary>
        /// номер документа ОМС (ЕНП или Номер полиса)
        /// </summary>
        public string M_snpol { get; set; }

        /// <summary>
        ///Генерируемая фраза:
        ///заявление 'ФИО полностью' д.р. 'дата рождения' г. или ее представителя о выборе 'ЛПУ из справочника по номернику' для оказания первичной медико-санитарной помощи
        /// </summary>
        public string M_fd { get; set; }

        /// <summary>
        ///Генерируемая фраза:
        ///заявление 'ФИО' д.р. 'дата рождения' г. или ее представителя о выборе 'ЛПУ по справочнику из номерника' для оказания первичной медико-санитарной помощи
        ///от 'дата из номерника' г. не представлено. Прикрепление застрахованного к ФБЛПУ "Поликлиника ФНС России" от 16.02.2016г. признано необоснованным
        /// </summary>
        public string M_nd1 { get; set; }
        public string M_nd2 { get; set; }

        /// <summary>
        /// не представлено / представлено
        /// необосновано /обосновано
        /// </summary>
        public bool? IsConfirm { get; set; }

        /// <summary>
        /// код дефекта - из справочника
        /// </summary>
        public long? M_osn230_Id { get; set; }
        public List<ReferenceUniversalItem> M_osn230Ref { get; set; }

        /// <summary>
        /// код дефекта в другой кодировке - из справочника
        /// </summary>
        //public string m_er_c { get; set; }

        /// <summary>
        /// сумма штрафа - справочник, по DATE_IN из номерника определяем дату, и выставляем соответствующую сумму по периоду
        /// </summary>
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
        public float? Coefficient { get; set; }

        public long? UserId { get; set; }
        public string UserPosition { get; set; }
        public string UserFIO { get; set; }


        public long? UpdateUserId { get; set; }
        public string UpdateUserFIO { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public string FileNameLPU { get; set; }
        public string FileUrlLPU { get; set; }
        public string FileName2 { get; set; }
        public string FileUrl2 { get; set; }

    }
}
