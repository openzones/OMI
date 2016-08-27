using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMInsurance.Entities
{
    public class ClientVisitHistoryItem
    {
        public long ClientVisitId { get; set; }
        public ReferenceItem Status { get; set; }
        public DateTime StatusDate { get; set; }
        public long? UserId { get; set; }
        public string UserLogin { get; set; }
        public string UserFirstname { get; set; }
        public string UserSecondname { get; set; }
        public string UserLastname { get; set; }
    }
}
