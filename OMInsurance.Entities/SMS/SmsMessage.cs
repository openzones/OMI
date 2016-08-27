using OMInsurance.Entities.Core;
namespace OMInsurance.Entities.SMS
{
    public class SMSMessage : DataObject
    {
        public string SenderId { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
    }
}
