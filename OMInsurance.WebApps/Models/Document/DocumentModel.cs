using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using OMInsurance.WebApps.Models.Core;
using OMInsurance.WebApps.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace OMInsurance.WebApps.Models
{
    public class DocumentModel : ValidatableModel<DocumentModel>
    {
        #region Properties

        public long? Id { get; set; }

        public DocumentType DocumentType { get; set; }

        [DisplayName("Тип документа")]
        public long? DocumentTypeID { get; set; }
        public List<SelectListItem> DocumentTypes { get; set; }

        [DisplayName("Серия")]
        [StringLength(9, ErrorMessage = "Максимальная длина - 9 символов")]
        public string Series { get; set; }

        [DisplayName("Номер")]
        [StringLength(16, ErrorMessage = "Максимальная длина - 16 символов")]
        public string Number { get; set; }

        [DisplayName("Дата выдачи")]
        public DateTime? IssueDate { get; set; }

        [DisplayName("Срок окончания действия")]
        public DateTime? ExpirationDate { get; set; }

        [DisplayName("Особый случай подачи документа")]
        public bool IsIssueCase { get; set; }

        [DisplayName("Кем выдан")]
        public string IssueDepartment { get; set; }

        #endregion

        #region Constructors
        public DocumentModel()
        {
            DocumentTypes = ReferencesProvider.GetReferences(Constants.DocumentTypeRef, null, true);
            validator = new DocumentModelValidator();
        }

        public DocumentModel(DocumentType type)
            : this()
        {
            if (type == DocumentType.New)
            {
                this.DocumentTypeID = Constants.RussianFederationPassportDocumentId;
            }
            if (type == DocumentType.NewForeign || type == DocumentType.OldForeign)
            {
                DocumentTypes.RemoveAll(item => item.Value != string.Empty && item.Value != "11" && item.Value != "23");
            }
            this.DocumentType = type;
        }

        public DocumentModel(Document document, DocumentType type) : this(type)
        {
            this.Id = document.Id;
            this.DocumentTypeID = document.DocumentType != null ? document.DocumentType.Id : 0;
            this.Series = document.Series;
            this.Number = document.Number;
            this.DocumentType = type;
            this.IssueDate = document.IssueDate;
            this.ExpirationDate = document.ExpirationDate;
            this.IsIssueCase = document.IsIssueCase;
            this.IssueDepartment = document.IssueDepartment;
        }
        #endregion

        #region Methods
        public Document.SaveData GetForBLL()
        {
            Document.SaveData data = new Document.SaveData();
            data.Id = this.Id;
            data.DocumentTypeId = this.DocumentTypeID == 0 ? new long?() : this.DocumentTypeID;
            data.Series = this.Series;
            data.Number = this.Number;
            data.IssueDate = this.IssueDate;
            data.ExpirationDate = this.ExpirationDate;
            data.IsIssueCase = this.DocumentTypeID == Constants.USSRPasportDocumentId;
            data.IssueDepartment = this.IssueDepartment;
            return data;
        }

        #endregion
    }
}