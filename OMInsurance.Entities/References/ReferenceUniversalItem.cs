using OMInsurance.Entities.Core;

namespace OMInsurance.Entities
{
    public class ReferenceUniversalItem : ReferenceItem
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
        public long? DeliveryCenterId { get; set; }
        public long? DeliveryPointHeadId { get; set; }
        public bool? SendSms { get; set; }

        public long? LPU_ID_AIS { get; set; }
        public long? FIL_ID { get; set; }
        public string MCOD { get; set; }
        public string FULLNAME { get; set; }
        public string OGRN { get; set; }
        public string FCOD { get; set; }

        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Secondname { get; set; }

        //для справочника 
        public string ErrCode { get; set; }

        /// <summary>
        /// Название справочника
        /// </summary>
        public string ReferenceName { get; set; }
    }
}
