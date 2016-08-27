using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using OMInsurance.WebApps.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace OMInsurance.WebApps.Models
{
    public class RepresentativeEditModel : ValidatableModel<RepresentativeEditModel>
    {
        #region Constructors

        public RepresentativeEditModel()
        {
            validator = new RepresentativeEditModelValidator();
            RepresentativeTypes = ReferencesProvider.GetReferences(Constants.RepresentativeTypeRef, null, true);
            DocumentTypes = ReferencesProvider.GetReferences(Constants.DocumentTypeRef, null, true);
        }

        public RepresentativeEditModel(Representative representative) : this()
        {
            if (representative != null)
            {
                Id = representative.Id;
                DocumentTypeId = representative.DocumentType != null ? representative.DocumentType.Id : new Nullable<long>();
                RepresentativeTypeId = representative.RepresentativeTypeId;
                Firstname = representative.Firstname;
                Secondname = representative.Secondname;
                Lastname = representative.Lastname;
                Birthday = representative.Birthday;
                IssueDate = representative.IssueDate;
                IssueDepartment = representative.IssueDepartment;
                Series = representative.Series;
                Number = representative.Number;
            }
        }

        #endregion

        #region Properties

        public long? Id { get; set; }

        [Display(Name = "Тип представителя")]
        public long? RepresentativeTypeId { get; set; }
        public List<SelectListItem> RepresentativeTypes { get; set; }

        [Display(Name = "Тип документа")]
        public long? DocumentTypeId { get; set; }
        public List<SelectListItem> DocumentTypes { get; set; }

        [Display(Name = "Имя")]
        [StringLength(50, ErrorMessage = "Максимальная длина - 50 символов")]
        public string Firstname { get; set; }

        [Display(Name = "Отчество")]
        [StringLength(50, ErrorMessage = "Максимальная длина - 50 символов")]
        public string Secondname { get; set; }

        [Display(Name = "Фамилия")]
        [StringLength(50, ErrorMessage = "Максимальная длина - 50 символов")]
        public string Lastname { get; set; }

        [Display(Name = "Дата рождения")]
        public DateTime? Birthday { get; set; }

        [Display(Name = "Серия документа")]
        [StringLength(8, ErrorMessage = "Максимальная длина - 50 символов")]
        public string Series { get; set; }

        [Display(Name = "Номер документа")]
        [StringLength(9, ErrorMessage = "Максимальная длина - 50 символов")]
        public string Number { get; set; }

        [Display(Name = "Дата выдачи")]
        public DateTime? IssueDate { get; set; }

        [Display(Name = "Кем выдан")]
        [StringLength(100, ErrorMessage = "Максимальная длина - 100 символов")]
        public string IssueDepartment { get; set; }

        #endregion

        #region Methods

        public Representative.SaveData GetForBLL()
        {
            Representative.SaveData data = new Representative.SaveData();
            data.Id = this.Id;
            data.DocumentTypeId = this.DocumentTypeId.HasValue ? this.DocumentTypeId.Value : new Nullable<long>();
            data.RepresentativeTypeId = this.RepresentativeTypeId.HasValue ? this.RepresentativeTypeId.Value : new Nullable<long>();
            data.Firstname = this.Firstname;
            data.Secondname = this.Secondname;
            data.Lastname = this.Lastname;
            data.Birthday = this.Birthday;
            data.IssueDate = this.IssueDate;
            data.IssueDepartment = this.IssueDepartment;
            data.Series = this.Series;
            data.Number = this.Number;
            return data;
        }

        #endregion
    }
}