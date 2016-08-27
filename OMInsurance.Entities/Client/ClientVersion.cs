using OMInsurance.Entities.Core;
using System;

namespace OMInsurance.Entities
{
	public class ClientVersion : DataObject
	{
		#region Properties

        public string Firstname { get; set; }
        public string Secondname { get; set; }
        public string Lastname { get; set; }

        public ReferenceItem FirstnameType { get; set; }
        public ReferenceItem SecondnameType { get; set; }
        public ReferenceItem LastnameType { get; set; }

        public DateTime? Birthday { get; set; }
        public string Sex { get; set; }
        public string SNILS { get; set; }
        public ReferenceItem Citizenship { get; set; }
        public string Birthplace { get; set; }
        public ReferenceItem Category { get; set; }

		#endregion

		public class SaveData
        {
            public SaveData()
            {
            }
            public SaveData(ClientVersion clientVersion)
            {
                this.Birthday = clientVersion.Birthday;
                this.Firstname = clientVersion.Firstname;
                this.Secondname = clientVersion.Secondname;
                this.Lastname = clientVersion.Lastname;
                this.Sex = string.IsNullOrEmpty(clientVersion.Sex) ? new char?() : clientVersion.Sex[0];
                this.SNILS = clientVersion.SNILS;
                this.Citizenship = clientVersion.Citizenship.Id == 0 ? new long?() : clientVersion.Citizenship.Id;
                this.Birthplace = clientVersion.Birthplace;
                this.Category = clientVersion.Category.Id == 0 ? new long?() : clientVersion.Category.Id;
                this.FirstnameTypeId = clientVersion.FirstnameType.Id == 0 ? new long?() : clientVersion.FirstnameType.Id;
                this.SecondnameTypeId = clientVersion.SecondnameType.Id == 0 ? new long?() : clientVersion.SecondnameType.Id;
                this.LastnameTypeId = clientVersion.LastnameType.Id == 0 ? new long?() : clientVersion.LastnameType.Id;
            }
            public long? Id { get; set; }
			public string Firstname { get; set; }
			public string Secondname { get; set; }
			public string Lastname { get; set; }
			public DateTime? Birthday { get; set; }
			public char? Sex { get; set; }
			public string SNILS { get; set; }
			public long? Citizenship { get; set; }  
			public string Birthplace { get; set; }
			public long? Category { get; set; }

            public long? FirstnameTypeId { get; set; }
            public long? SecondnameTypeId { get; set; }
            public long? LastnameTypeId { get; set; }
        }

        public string Fullname
        {
            get
            {
                return string.Format("{0} {1} {2}",
                    Lastname ?? string.Empty,
                    Firstname ?? string.Empty,
                    Secondname ?? string.Empty);
            }
        }
    }
}
