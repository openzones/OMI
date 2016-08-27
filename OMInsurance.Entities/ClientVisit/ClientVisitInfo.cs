using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMInsurance.Entities
{
    public class ClientVisitInfo : DataObject
    {
        public ClientVisitInfo()
        {
        }
        public long VisitGroupId { get; set; }
        public long ClientId { get; set; }
        public DateTime StatusDate { get; set; }
        public string Firstname { get; set; }
        public string Secondname { get; set; }
        public string Lastname { get; set; }
        public string Sex { get; set; }
        public DateTime? Birthday { get; set; }
        public string TemporaryPolicyNumber { get; set; }
        public DateTime? TemporaryPolicyDate { get; set; }
        public string PolicySeries { get; set; }
        public string PolicyNumber { get; set; }
        public string FundResponseApplyingMessage { get; set; }
        public bool IsReadyToFundSubmitRequest { get; set; }
        public bool IsDifficultCase { get; set; }
        public string UnifiedPolicyNumber { get; set; }
        public string SNILS { get; set; }
        public DateTime? PolicyIssueDate { get; set; }
        public ReferenceItem Status { get; set; }
        public string PolicyParty { get; set; }
        public string Comment { get; set; }
        public ReferenceItem DeliveryCenter { get; set; }
        public string DeliveryPoint { get; set; }
        public string Phone { get; set; }
        public ReferenceItem Scenario { get; set; }
        public long? UserId { get; set; }
        public string ClientAcquisitionEmployee { get; set; }
        public string Citizenship { get; set; }
    }
}
