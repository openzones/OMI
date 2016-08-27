using System;
using OMInsurance.Entities.Core;

namespace OMInsurance.Entities.SMS
{
    public class SmsTemplate
    {
        public string SenderId { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Result { get; set; }
        public long? StatusId { get; set; }
    }
}
