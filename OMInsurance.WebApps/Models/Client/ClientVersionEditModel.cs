using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using OMInsurance.WebApps.Validation;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace OMInsurance.WebApps.Models
{
    public class ClientVersionEditModel : ValidatableModel<ClientVersionEditModel>
    {
        #region Constructors
        public ClientVersionEditModel()
        {
            validator = new ClientVersionValidator();
            CodFioClassifier = ReferencesProvider.GetReferences(Constants.CodFioClassifier, " ", false);
            Citizenships = ReferencesProvider.GetReferences(Constants.CitizenshipRef, null, true);
            Categories = ReferencesProvider.GetReferences(Constants.ClientCategoryRef, null, true);
        }

        public ClientVersionEditModel(EntityType type)
            : this()
        {
            EntityType = type;
            if (type == Entities.Core.EntityType.New)
            {
                //Default Russia
                Citizenship = 1;
                //Default client category
                Category = 1;
            }
        }

        public ClientVersionEditModel(ClientVersion clientVersion, EntityType type)
            : this(type)
        {
            Id = clientVersion.Id;
            Firstname = clientVersion.Firstname;
            FirstnameTypeId = clientVersion.FirstnameType != null ? clientVersion.FirstnameType.Id : 0;
            Secondname = clientVersion.Secondname;
            SecondnameTypeId = clientVersion.SecondnameType != null ? clientVersion.SecondnameType.Id : 0;
            Lastname = clientVersion.Lastname;
            LastnameTypeId = clientVersion.LastnameType != null ? clientVersion.LastnameType.Id : 0;
            Birthday = clientVersion.Birthday;
            Sex = clientVersion.Sex;
            SNILS = clientVersion.SNILS;
            Citizenship = clientVersion.Citizenship != null ? clientVersion.Citizenship.Id : 0;
            Birthplace = clientVersion.Birthplace;
            Category = clientVersion.Category != null ? clientVersion.Category.Id : 0;
        }

        #endregion

        #region Properties

        public long? Id { get; set; }

        public EntityType EntityType { get; set; }

        [DisplayName("Имя")]
        [StringLength(50, ErrorMessage = "Максимальная длина - 50 символов")]
        public string Firstname { get; set; }

        [DisplayName("Отчество")]
        [StringLength(50, ErrorMessage = "Максимальная длина - 50 символов")]
        public string Secondname { get; set; }

        [DisplayName("Фамилия")]
        [StringLength(50, ErrorMessage = "Максимальная длина - 50 символов")]
        public string Lastname { get; set; }

        [DisplayName("Особый случай в записи имени")]
        public long FirstnameTypeId { get; set; }
        public List<SelectListItem> CodFioClassifier { get; set; }

        [DisplayName("Особый случай в записи отчестве")]
        public long SecondnameTypeId { get; set; }

        [DisplayName("Особый случай в записи фамилии")]
        public long LastnameTypeId { get; set; }

        [DisplayName("Дата рождения")]
        public DateTime? Birthday { get; set; }

        [DisplayName("Пол")]
        public string Sex { get; set; }

        [DisplayName("СНИЛС")]
        [StringLength(14, ErrorMessage = "Максимальная длина - 14 символов")]
        public string SNILS { get; set; }

        [DisplayName("Гражданство")]
        public long? Citizenship { get; set; }
        public List<SelectListItem> Citizenships { get; set; }

        [DisplayName("Место рождения")]
        [StringLength(150, ErrorMessage = "Максимальная длина - 150 символов")]
        public string Birthplace { get; set; }

        [DisplayName("Категория")]
        public long? Category { get; set; }
        public List<SelectListItem> Categories { get; set; }

        #endregion

        #region Methods

        public ClientVersion.SaveData GetForBLL()
        {
            ClientVersion.SaveData data = new ClientVersion.SaveData()
            {
                Id = this.Id,
                Birthday = this.Birthday,
                Birthplace = this.Birthplace,
                Category = this.Category == 0 ? new long?() : this.Category,
                Citizenship = this.Citizenship == 0 ? new long?() : this.Citizenship,
                Firstname = this.Firstname,
                Lastname = this.Lastname,
                Secondname = this.Secondname,
                FirstnameTypeId = this.FirstnameTypeId == 0 ? new long?() : this.FirstnameTypeId,
                LastnameTypeId = this.LastnameTypeId == 0 ? new long?() : this.LastnameTypeId,
                SecondnameTypeId = this.SecondnameTypeId == 0 ? new long?() : this.SecondnameTypeId,
                Sex = !string.IsNullOrEmpty(this.Sex) && this.Sex.Length > 0 ? this.Sex[0] : new char?(),
                SNILS = this.SNILS
            };
            return data;
        }

        public override void Validate(ModelValidationContext context)
        {
            validator.Validate(this, context);
        }
        #endregion
    }
}