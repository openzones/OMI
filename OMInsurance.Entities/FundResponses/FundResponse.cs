using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
namespace OMInsurance.Entities
{
    public abstract class FundResponse
    {
        public const string OK_Answer = "OK";
        /// <summary>
        /// Response Identifier
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Identifier that is equal to identifier of corresponding client visit
        /// </summary>
        public long ClientVisitId { get; set; }

        /// <summary>
        /// Identifier that is equal to identifier of corresponding client visit
        /// </summary>
        public long ClientVisitGroupId { get; set; }

        public DateTime CreateDate { get; set; }

        public abstract class CreateData
        {
            public long ClientVisitId { get; set; }
            public long? Recid { get; set; }
            public int? DataTypeId { get; set; }
            public int Order { get; set; }
            public abstract string GetResponseTypeName();

            public abstract long Create(IFundResponseCreator filler, DateTime date);

            public virtual string GetFullname()
            {
                return string.Empty;
            }
        }

        public class UploadReportData
        {
            public long ClientVisitId { get; set; }
            public long? Recid { get; set; }
            public string ResponseTypeName { get; set; }
            public string UploadResult { get; set; }

            public long VisitGroupId { get; set; }
            public long ClientId { get; set; }
            public string Fullname { get; set; }
            public string Sex { get; set; }
            public DateTime? Birthday { get; set; }
            public string TemporaryPolicyNumber { get; set; }
            public DateTime? TemporaryPolicyDate { get; set; }
            public string PolicySeries { get; set; }
            public string PolicyNumber { get; set; }
            public string UnifiedPolicyNumber { get; set; }
            public ReferenceItem Status { get; set; }
            public DateTime StatusDate { get; set; }
            public string PolicyParty { get; set; }
            public ReferenceItem DeliveryCenter { get; set; }
        }

        public virtual void Apply(
            ClientVisit.SaveData data,
            List<FundResponseFields> newFields,
            List<FundResponseFields> oldFields)
        {
        }
    }
}