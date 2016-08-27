using System;
using OMInsurance.Entities.Core;

namespace OMInsurance.Entities
{
    public class PolicyFromRegion : DataObject
    {
        public PolicyFromRegion()
        {
            PolicyStatus = new PolicyStatus();
            DeliveryCenter = new DeliveryCenter();
            DeliveryPoint = new DeliveryPoint();
        }

        public long RegionId { get; set; }
        public string TemporaryPolicyNumber { get; set; }
        public string UnifiedPolicyNumber { get; set; }
        public string PolicySeries { get; set; }
        public string PolicyNumber { get; set; }
        public PolicyStatus PolicyStatus { get; set; }
        public DeliveryCenter DeliveryCenter { get; set; }
        public DeliveryPoint DeliveryPoint { get; set; }
        public DateTime? ClientVisitDate { get; set; }
        public string ApplicationMethod { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Secondname { get; set; }
        public string Sex { get; set; }
        public string Phone { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? StatusDate { get; set; }
        public string Category { get; set; }
        public string Citizenship { get; set; }
        public string NomernikStatus { get; set; }
        public string LPU { get; set; }
        public DateTime? AttachmentDate { get; set; }
        public string AttachmentMethod { get; set; }
        public string BlankNumber { get; set; }
        public DateTime? SaveDate { get; set; }
    }
}
