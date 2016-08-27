using OMInsurance.Entities.Core;

namespace OMInsurance.Entities
{
    public class DeliveryCenter : ReferenceItem
    {
        public string SMO { get; set; }
        public string DisplayName { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string WorkHours { get; set; }
        public bool? IsDigitPolicyAbailable { get; set; }
        public long? ParentId { get; set; }
        public bool? IsMFC { get; set; }
    }
}
