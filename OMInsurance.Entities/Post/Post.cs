using System;
using OMInsurance.Entities.Core;

namespace OMInsurance.Entities.Post
{
    public class Post : DataObject
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime? CreateDate { get; set; }
        public User CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public User UpdateUser { get; set; }
        public bool Disabled { get; set; }

    }
}
