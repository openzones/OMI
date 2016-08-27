using OMInsurance.Entities.Core;
using System;

namespace OMInsurance.Entities
{
    public class GoznakResponse : FundResponse
    {
        public const string Name = "Goznak";

        public string UnifiedPolicyNumber { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Secondname { get; set; }
        public DateTime? Birthday { get; set; }
        public string Sex { get; set; }
        public ReferenceItem DeliveryCenter { get; set; }
        public string TemporaryPolicyNumber { get; set; }
        public DateTime UpdateDate { get; set; }

        public new class CreateData : FundResponse.CreateData
        {
            public string UnifiedPolicyNumber { get; set; }
            public DateTime? Birthday { get; set; }
            public string Lastname { get; set; }
            public string Firstname { get; set; }
            public string Secondname { get; set; }
            public string Sex { get; set; }
            public string TemporaryPolicyNumber { get; set; }
            public long DeliveryCenterId { get; set; }

            public override long Create(IFundResponseCreator creator, DateTime date)
            {
                return creator.Create(this, date);
            }

            public override string GetResponseTypeName()
            {
                return "Z-файл";
            }

            public override string GetFullname()
            {
                return string.Format("{0} {1} {2}",
                    Lastname ?? string.Empty,
                    Firstname ?? string.Empty,
                    Secondname ?? string.Empty);
            }
        }
    }
}
