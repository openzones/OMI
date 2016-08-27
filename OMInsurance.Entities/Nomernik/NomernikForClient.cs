using OMInsurance.Entities.Core;
using System;
namespace OMInsurance.Entities
{
    public class NomernikForClient : DataObject
    {

        public string S_CARD { get; set; }
        public string N_CARD { get; set; }
        public string ENP { get; set; }
        public string VSN { get; set; }
        public long? LPU_ID { get; set; }
        public DateTime? DATE_IN { get; set; }
        public int? SPOS { get; set; }

        public string SCENARIO { get; set; }
        public long? QZ { get; set; }
        public DateTime? DATE_END { get; set; }
        public DateTime? DATE_ARC { get; set; }
        public string IST { get; set; }


        public long? ClientID { get; set; }
        public DateTime LoadDate { get; set; }
        public DateTime FileDate { get; set; }
        public string Lastname { get; set; }
        public string Secondname { get; set; }
        public string Firstname { get; set; }
    }
}
