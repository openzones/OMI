using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;

namespace OMInsurance.Entities
{
    public class SvdFundResponse : FundResponse
    {
        public const string Name = "svd";

        #region Properties

        public string UnifiedPolicyNumber { get; set; }
        public string PolicySeries { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string OmsCode { get; set; }
        public string OGRN { get; set; }
        public string Firstname { get; set; }
        public string Secondname { get; set; }
        public string Lastname { get; set; }
        public DateTime? Birthday { get; set; }
        public EntityType DataType { get; set; }
        public string Sex { get; set; }
        public ReferenceItem DocumentType { get; set; }
        public string DocumentSeries { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime? DocumentIssueDate { get; set; }
        public ReferenceItem Citizenship { get; set; }
        public string ERZ { get; set; }
        public string Snils { get; set; }

        #endregion

        public override void Apply(
            ClientVisit.SaveData data,
            List<FundResponseFields> newFields,
            List<FundResponseFields> oldFields)
        {
            UpdatePolicy(data.NewPolicy, newFields, EntityType.New);
            UpdatePolicy(data.OldPolicy, oldFields, EntityType.Old);
            UpdateClientData(data.NewClientInfo, newFields);
            UpdateClientData(data.OldClientInfo, oldFields);
            UpdateDocument(data.NewDocument, newFields);
            UpdateDocument(data.OldDocument, oldFields);
        }

        private void UpdateClientData(ClientVersion.SaveData saveData, List<FundResponseFields> fields)
        {
            if (fields.Contains(FundResponseFields.Lastname))
            {
                saveData.Lastname = Lastname;
            }
            if (fields.Contains(FundResponseFields.Firstname))
            {
                saveData.Firstname = Firstname;
            }
            if (fields.Contains(FundResponseFields.Secondname))
            {
                saveData.Secondname = Secondname;
            }
            if (fields.Contains(FundResponseFields.Birthday))
            {
                saveData.Birthday = Birthday;
            }
            if (fields.Contains(FundResponseFields.Sex))
            {
                saveData.Sex = Sex.Length > 0 ? Sex[0] : new char?();
            }
            if (fields.Contains(FundResponseFields.Citizenship))
            {
                saveData.Citizenship = Citizenship.Id != 0 ? Citizenship.Id : new long?();
            }
            if (fields.Contains(FundResponseFields.Snils))
            {
                saveData.SNILS = Snils;
            }
        }
        private void UpdateDocument(Document.SaveData saveData, List<FundResponseFields> fields)
        {
            if (fields.Contains(FundResponseFields.DocumentType))
            {
                saveData.DocumentTypeId = DocumentType.Id != 0 ? DocumentType.Id : new long?();
            }
            if (fields.Contains(FundResponseFields.DocumentSeries))
            {
                saveData.Series = this.DocumentSeries;
            }
            if (fields.Contains(FundResponseFields.DocumentNumber))
            {
                saveData.Number = this.DocumentNumber;
            }
        }
        private void UpdatePolicy(PolicyInfo.SaveData saveData, List<FundResponseFields> fields, EntityType type = EntityType.General)
        {
            if (fields.Contains(FundResponseFields.ExpirationDate))
            {
                saveData.EndDate = ExpirationDate ?? new DateTime(2099, 12, 31);
            }
            if (fields.Contains(FundResponseFields.OGRN))
            {
                saveData.OGRN = OGRN;
            }
            if (fields.Contains(FundResponseFields.OKATO))
            {
                saveData.OKATO = Constants.MoscowOKATO;
            }
            if (fields.Contains(FundResponseFields.PolicyNumber))
            {
                saveData.Number = PolicyNumber;
            }
            if (fields.Contains(FundResponseFields.UnifierPolicyNumberToPolicyNumber))
            {
                saveData.Number = UnifiedPolicyNumber;
            }
            if (fields.Contains(FundResponseFields.PolicySeries))
            {
                saveData.Series = PolicySeries;
            }
            if (fields.Contains(FundResponseFields.StartDate))
            {
                saveData.StartDate = StartDate;
            }
            if (fields.Contains(FundResponseFields.UnifiedPolicyNumber))
            {
                saveData.UnifiedPolicyNumber = UnifiedPolicyNumber;
            }
            if (type == EntityType.Old)
            {
                saveData.PolicyTypeId = PolicyTypeRef.OldPolicy.Id;
            }
        }

        public new class CreateData : FundResponse.CreateData
        {
            public string UnifiedPolicyNumber { get; set; }
            public string PolicySeries { get; set; }
            public string PolicyNumber { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? ExpirationDate { get; set; }
            public string OmsCode { get; set; }
            public string OGRN { get; set; }
            public string Firstname { get; set; }
            public string Secondname { get; set; }
            public string Lastname { get; set; }
            public DateTime? Birthday { get; set; }
            public string Sex { get; set; }
            public string Snils { get; set; }
            public long? DocumentTypeId { get; set; }
            public string DocumentSeries { get; set; }
            public string DocumentNumber { get; set; }
            public DateTime? DocumentIssueDate { get; set; }
            public long? CitizenshipId { get; set; }
            public string ERZ { get; set; }

            public override long Create(IFundResponseCreator creator, DateTime date)
            {
                return creator.Create(this, date);
            }

            public override string GetResponseTypeName()
            {
                return "Сверка SVD";
            }
        }
    }
}