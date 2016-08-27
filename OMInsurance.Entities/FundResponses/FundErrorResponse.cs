using OMInsurance.Entities.Core;
using System;

namespace OMInsurance.Entities
{
    public class FundErrorResponse : FundResponse
    {
        public const string Name = "FundError";

        public ReferenceItem FundResponseStage { get; set; }
        public long LID { get; set; }
        public int OrderNumber { get; set; }
        public string UnifiedPolicyNumber { get; set; }
        public string SNILS { get; set; }
        public ReferenceItem PolicyType { get; set; }
        public ReferenceItem DeliveryCenter { get; set; }
        public string PolicySeries { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime? TemporaryPolicyDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int SMO_ID { get; set; }
        public string Firstname { get; set; }
        public string Secondname { get; set; }
        public string Lastname { get; set; }
        public string Sex { get; set; }
        public DateTime? Birthday { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorText { get; set; }
        public string TerritoryCode { get; set; }
        public string OGRN { get; set; }


        public new class CreateData : FundResponse.CreateData
        {
            public long FundResponseStageId { get; set; }
            public long LID { get; set; }
            public string UnifiedPolicyNumber { get; set; }
            public string SNILS { get; set; }
            public long PolicyTypeId { get; set; }
            public string PolicySeries { get; set; }
            public string PolicyNumber { get; set; }
            public DateTime? TemporaryPolicyDate { get; set; }
            public DateTime? ExpirationDate { get; set; }
            public int SMO_ID { get; set; }
            public string Firstname { get; set; }
            public string Secondname { get; set; }
            public string Lastname { get; set; }
            public string Sex { get; set; }
            public DateTime? Birthday { get; set; }
            public long? DeliveryCenterId { get; set; }
            public string ErrorCode { get; set; }
            public string ErrorText { get; set; }
            public string TerritoryCode { get; set; }
            public string OGRN { get; set; }

            public override long Create(IFundResponseCreator creator, DateTime date)
            {
                return creator.Create(this, date);
            }

            public override string GetResponseTypeName()
            {
                return "Ответ фонда";
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
