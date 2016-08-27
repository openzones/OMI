using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OMInsurance.Entities.Core;

namespace OMInsurance.Entities
{
	public class Document : DataObject
	{
        public ReferenceItem DocumentType { get; set; }
        public string Series { get; set; }
        public string Number { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool IsIssueCase { get; set; }
        public string IssueDepartment { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

		public class SaveData
		{
            public SaveData()
            {
            }
            public SaveData(Document document)
            {
                this.DocumentTypeId = document.DocumentType.Id == 0 ? new long?() : document.DocumentType.Id;
                this.Series = document.Series;
                this.Number = document.Number;
                this.IssueDate = document.IssueDate;
                this.ExpirationDate = document.ExpirationDate;
                this.IsIssueCase = document.IsIssueCase;
                this.IssueDepartment = document.IssueDepartment;
            }
            public long? Id { get; set; }
            public long? DocumentTypeId { get; set; }
			public string Series { get; set; }	
			public string Number { get; set; }	  
			public DateTime? IssueDate { get; set; }
			public DateTime? ExpirationDate { get; set; }	 
			public bool IsIssueCase { get; set; }
			public string IssueDepartment { get; set; }
		}
	}
}
