using OMInsurance.Entities.Core;
using System;

namespace OMInsurance.Entities
{
    public class NOMP : Nomernik
    {   /// <summary>
        ///Переменные соответствуют полям в номернике (dbf)
        /// </summary>
        
        /// <summary>
        ///Cерия полиса старого образца
        /// </summary>
        public string S_CARD { get; set;}

        /// <summary>
        ///Номер полиса старого образца, генерируется всегда Фондом
        ///</summary>
        //public long? N_CARD { get; set; }
        public string N_CARD { get; set; }

        /// <summary>
        ///ENP = UnifiedPolicyNumber
        /// </summary>
        public string ENP { get; set; }

        /// <summary>
        ///VSN = TemporaryPolicyNumber
        /// </summary>
        public string VSN { get; set; }

        /// <summary>
        ///ID медицинской организации (МО)
        /// </summary>
        public long? LPU_ID { get; set; }

        /// <summary>
        ///Дата прикрепления к страховой
        /// </summary>
        public DateTime? DATE_IN { get; set; }

        /// <summary>
        ///Способ прикрепления к МО
        /// </summary>
        public int? SPOS { get; set; }
    }
}
