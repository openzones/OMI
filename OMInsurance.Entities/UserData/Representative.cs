using OMInsurance.Entities.Core;
using System;

namespace OMInsurance.Entities
{
    public class Representative : DataObject
    {
        public long? RepresentativeTypeId { get; set; }
        public ReferenceItem DocumentType { get; set; }
        public string Firstname { get; set; }
        public string Secondname { get; set; }
        public string Lastname { get; set; }
        public DateTime? Birthday { get; set; }
        public string Series { get; set; }
        public string Number { get; set; }
        public DateTime? IssueDate { get; set; }
        public string IssueDepartment { get; set; }

        public class SaveData
        {
            public SaveData()
            {

            }
            public SaveData(Representative representative)
            {
                this.RepresentativeTypeId = representative.RepresentativeTypeId;
                this.DocumentTypeId = representative.DocumentType != null ? representative.DocumentType.Id : new long?();
                this.Firstname = representative.Firstname;
                this.Secondname = representative.Secondname;
                this.Lastname = representative.Lastname;
                this.Birthday = representative.Birthday;
                this.Series = representative.Series;
                this.Number = representative.Number;
                this.IssueDate = representative.IssueDate;
                this.IssueDepartment = representative.IssueDepartment;
            }
            public long? Id { get; set; }
            public long? RepresentativeTypeId { get; set; }
            public long? DocumentTypeId { get; set; }
            public string Firstname { get; set; }
            public string Secondname { get; set; }
            public string Lastname { get; set; }
            public DateTime? Birthday { get; set; }
            public string Series { get; set; }
            public string Number { get; set; }
            public DateTime? IssueDate { get; set; }
            public string IssueDepartment { get; set; }
        }
    }
}
