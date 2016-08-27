using OMInsurance.Entities.Core;
using System.Runtime.Serialization;

namespace OMInsurance.Entities
{
    [DataContract(Name = "PolicyStatus")]
    public class PolicyStatus
    {
        [DataMember(Name="Id")]
        public long Id { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }
    }
}
