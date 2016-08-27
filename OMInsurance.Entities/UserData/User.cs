using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;

namespace OMInsurance.Entities
{
    public class User : DataObject
    {
        public User()
        {
            Roles = new List<Role>();
        }

        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public ReferenceItem Department { get; set; }
        public string DepartmentDisplayName { get; set; }
        public ReferenceItem DeliveryPoint { get; set; }
        public string Firstname { get; set; }
        public string Secondname { get; set; }
        public string Lastname { get; set; }
        public bool? IsBlocked { get; set; }
        public List<Role> Roles { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public class SaveData
        {
            public long? Id { get; set; }
            public string Login { get; set; }
            public string PasswordHash { get; set; }
            public long DepartmentId { get; set; }
            public long DeliveryPointId { get; set; }
            public List<long> Roles { get; set; }
            public string Firstname { get; set; }
            public string Secondname { get; set; }
            public string Lastname { get; set; }
            public string Position { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
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
