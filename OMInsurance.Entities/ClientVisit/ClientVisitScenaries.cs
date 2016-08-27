using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMInsurance.Entities
{
    public static class ClientVisitScenaries
    {
        /// <summary>
        /// POK
        /// </summary>
        public static ReferenceItem PolicyExtradition;
        /// <summary>
        /// CD
        /// </summary>
        public static ReferenceItem ChangeDocument;
        /// <summary>
        /// CI
        /// </summary>
        public static ReferenceItem ReregistrationMoscowENPWithoutFIO;
        /// <summary>
        /// CT
        /// </summary>
        public static ReferenceItem ReregistrationRegionalENPWithoutFIO;

        /// <summary>
        /// DP
        /// </summary>
        public static ReferenceItem LostENPWithoutFIO;
        /// <summary>
        /// NB
        /// </summary>
        public static ReferenceItem FirstRequestENP;
        /// <summary>
        /// CR
        /// </summary>
        public static ReferenceItem RequestENPSameSMOChangeFIO;
        /// <summary>
        /// RI
        /// </summary>
        public static ReferenceItem ReregistrationMoscowENPWithFIO;
        /// <summary>
        /// RT
        /// </summary>
        public static ReferenceItem ReregistrationRegionalENPWithFIO;
        /// <summary>
        /// PT
        /// </summary>
        public static ReferenceItem ReregistrationRegionalOldPolicyWithoutFIO;
        /// <summary>
        /// PRT
        /// </summary>
        public static ReferenceItem ReregistrationRegionalOldPolicyWithFIO;        
        /// <summary>
        /// PI 
        /// </summary>
        public static ReferenceItem NewUnifiedPolicyNumberByKMSOtherSMO;
        /// <summary>
        /// PRI 
        /// </summary>
        public static ReferenceItem NewUnifiedPolicyNumberByKMSOtherSMOWithFIO;
        /// <summary>
        /// CP 
        /// </summary>
        public static ReferenceItem NewUnifiedPolicyNumberByKMS;
        /// <summary>
        /// PR 
        /// </summary>
        public static ReferenceItem NewUnifiedPolicyNumberByOldPolicy;
        /// <summary>
        /// MP 
        /// </summary>
        public static ReferenceItem PolicyMerge;
        /// <summary>
        /// RD 
        /// </summary>
        public static ReferenceItem PolicySeparation;
        /// <summary>
        /// CLR
        /// </summary>
        public static ReferenceItem RemoveFromRegister;
        /// <summary>
        /// CPV
        /// </summary>
        public static ReferenceItem ChangeSexOrBirthdaySMO;
        /// <summary>
        /// CPV
        /// </summary>
        public static ReferenceItem PolicyRecovery;
        static ClientVisitScenaries()
        {
            PolicyExtradition = new ReferenceItem() { Id = 2, Code = "POK", Name = "Выдача ЕНП на руки" };
            ReregistrationRegionalOldPolicyWithoutFIO = new ReferenceItem() { Id = 3, Code = "PT", Name = "Замена старого регионального полиса без изменения Ф,И,О, ДУЛ" };
            ReregistrationRegionalOldPolicyWithFIO = new ReferenceItem() { Id = 4, Code = "PRT", Name = "Изгот.ЕНП по старому регион.полису с изменением Ф,И,О, ДУЛ" };
            NewUnifiedPolicyNumberByOldPolicy = new ReferenceItem() { Id = 5, Code = "PR", Name = "замена старого полиса на новый с изменением реквизитов застрахованного" };
            ChangeDocument = new ReferenceItem() { Id = 10, Code = "CD", Name = "Изменение ДУЛ, добавление СНИЛС" };
            ReregistrationMoscowENPWithoutFIO = new ReferenceItem() { Id = 15, Code = "CI", Name = "Перерегистрация московского ЕНП без изменения Ф,И,О, ДУЛ" };
            ReregistrationRegionalENPWithoutFIO = new ReferenceItem() { Id = 17, Code = "CT", Name = "Перерегистрация регионального ЕНП без изменения Ф,И,О, ДУЛ" };
            LostENPWithoutFIO = new ReferenceItem() { Id = 6, Code = "DP", Name = "Изгот.ЕНП при порче,утрате ЕНП без изм.Ф,И,О,ДУЛ при наличии ЕНП или КМС своей СМО" };
            FirstRequestENP = new ReferenceItem() { Id = 13, Code = "NB", Name = "Первичное изготовление ЕНП" };
            RequestENPSameSMOChangeFIO = new ReferenceItem() { Id = 14, Code = "CR", Name = "Переизготовление ЕНП своей СМО с изменением Ф,И,О, ДУЛ" };
            ReregistrationMoscowENPWithFIO = new ReferenceItem() { Id = 16, Code = "RI", Name = "Перерегистрация московского ЕНП с изменением Ф,И,О, ДУЛ" };
            ReregistrationRegionalENPWithFIO = new ReferenceItem() { Id = 18, Code = "RT", Name = "Перерегистрация регионального ЕНП с изменением Ф,И,О, ДУЛ" };
            NewUnifiedPolicyNumberByKMS = new ReferenceItem() { Id = 9, Code = "CP", Name = "Изготовление ЕНП по своей КМС без изменения Ф,И,О, ДУЛ" };
            NewUnifiedPolicyNumberByKMSOtherSMO = new ReferenceItem() { Id = 7, Code = "PI", Name = "Изготовление ЕНП по КМС другой СМО без изменения Ф,И,О, ДУЛ" };
            NewUnifiedPolicyNumberByKMSOtherSMOWithFIO = new ReferenceItem() { Id = 8, Code = "PRI", Name = "Изготовление ЕНП по КМС другой СМО с изменением Ф,И,О, ДУЛ" };
            PolicyMerge = new ReferenceItem() { Id = 12, Code = "MP", Name = "Объединение двух полисов" };
            PolicySeparation = new ReferenceItem() { Id = 19, Code = "RD", Name = "Разъединение двух полисов" };
            RemoveFromRegister = new ReferenceItem() { Id = 20, Code = "CLR", Name = "Снятие с учета ЕНП или КМС своей СМО" };
            ChangeSexOrBirthdaySMO = new ReferenceItem() { Id = 11, Code = "CPV", Name = "Изменение пола, даты рожд. в ЕНП или КМС своей СМО - промежуточное действие" };
            PolicyRecovery = new ReferenceItem() { Id = 1, Code = "AD", Name = "Восстановление КМС или ЕНП своей СМО в РС ЕРЗЛ по положит.ответу св.5" };
        }
    }
}
