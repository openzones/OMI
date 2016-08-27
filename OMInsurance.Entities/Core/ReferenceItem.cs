using System;
namespace OMInsurance.Entities.Core
{
    public class ReferenceItem : DataObject
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsEnabledForRegistrator { get; set; }
        public bool? IsEnabledForOperator { get; set; }
    }
}
