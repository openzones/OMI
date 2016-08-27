using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System;
using System.ComponentModel;

namespace OMInsurance.WebApps.Models
{
    public class ClientVersionModel
    {
        #region Constructors

        public ClientVersionModel()
        {
        }

        public ClientVersionModel(ClientVersion version)
        {
            this.Firstname = version.Firstname;
            this.Secondname = version.Secondname;
            this.Lastname = version.Lastname;
            this.FirstnameType = version.FirstnameType;
            this.SecondnameType = version.SecondnameType;
            this.LastnameType = version.LastnameType;
            this.Birthday = version.Birthday;
            this.Sex = version.Sex;
            this.SNILS = version.SNILS;
            this.Citizenship = version.Citizenship;
            this.Birthplace = version.Birthplace;
            this.Category = version.Category;
        }

        #endregion

        #region Properties
        public long ID { get; set; }

        [DisplayName("Имя")]
        public string Firstname { get; set; }

        [DisplayName("Отчество")]
        public string Secondname { get; set; }

        [DisplayName("Фамилия")]
        public string Lastname { get; set; }

        public ReferenceItem FirstnameType { get; set; }
        public ReferenceItem SecondnameType { get; set; }
        public ReferenceItem LastnameType { get; set; }
        
        [DisplayName("Дата рождения")]
        public DateTime? Birthday { get; set; }

        [DisplayName("Пол")]
        public string Sex { get; set; }

        [DisplayName("СНИЛС")]
        public string SNILS { get; set; }

        [DisplayName("Гражданство")]
        public ReferenceItem Citizenship { get; set; }

        [DisplayName("Место рождения")]
        public string Birthplace { get; set; }

        [DisplayName("Категория")]
        public ReferenceItem Category { get; set; }

        #endregion
    }
}