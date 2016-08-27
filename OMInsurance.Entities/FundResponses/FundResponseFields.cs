using System.ComponentModel.DataAnnotations;
namespace OMInsurance.Entities
{
    public enum FundResponseFields
    {
        [Display(Name="ЕНП")]
        UnifiedPolicyNumber,

        [Display(Name = "Тип полиса")]
        PolicyType,

        [Display(Name = "Серия полиса")]
        PolicySeries,

        [Display(Name = "Номер полиса")]
        PolicyNumber,

        [Display(Name = "ОКАТО")]
        OKATO,

        [Display(Name = "ОГРН")]
        OGRN,

        [Display(Name = "Дата начала действия")]
        StartDate,

        [Display(Name = "Дата окончания действия")]
        ExpirationDate,

        [Display(Name = "Фамилия")]
        Lastname,
        [Display(Name = "Имя")]
        Firstname,
        [Display(Name = "Отчество")]
        Secondname,
        [Display(Name = "Дата рождения")]
        Birthday,
        [Display(Name = "Пол")]
        Sex,
        [Display(Name = "Тип документа")]
        DocumentType,
        [Display(Name = "Серия документа")]
        DocumentSeries,
        [Display(Name = "Номер документа")]
        DocumentNumber,
        [Display(Name = "Гражданство")]
        Citizenship,
        [Display(Name = "СНИЛС")]
        Snils,
        [Display(Name = "ЕНП в № полиса")]
        UnifierPolicyNumberToPolicyNumber
    }
}
