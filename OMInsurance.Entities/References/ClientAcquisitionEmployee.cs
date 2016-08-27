using OMInsurance.Entities.Core;

namespace OMInsurance.Entities
{
    public class ClientAcquisitionEmployee : DataObject
    {
        public long? UserId { get; set; }
        public string Firstname { get; set; }
        public string Secondname { get; set; }
        public string Lastname { get; set; }
    }
}
