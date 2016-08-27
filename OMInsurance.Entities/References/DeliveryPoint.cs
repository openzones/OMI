using OMInsurance.Entities.Core;

namespace OMInsurance.Entities
{
    public class DeliveryPoint : ReferenceItem
    {
        public long? DeliveryCenterId { get; set; }
        public long? DeliveryPointHeadId { get; set; }
    }
}
