using OMInsurance.Entities.Core;
using System;

namespace OMInsurance.Entities
{
    public class STOP : Nomernik
    {  /// <summary>
       ///Переменные соответствуют полям в стоп-листе (dbf)
       /// </summary>

        /// <summary>
        ///Код сценария
        /// </summary>
        public string SCENARIO { get; set; }

        /// <summary>
        ///Cерия полиса старого образца
        /// </summary>
        public string S_CARD { get; set; }

        /// <summary>
        ///Номер полиса старого образца, генерируется всегда Фондом
        ///</summary>
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
        /// ID компании, куда перешел клиент
        /// </summary>
        public long? QZ { get; set; }

        /// <summary>
        ///Дата завершения обслуживания
        /// </summary>
        public DateTime? DATE_END { get; set; }

        /// <summary>
        ///Дата закрытия клиента
        /// </summary>
        public DateTime? DATE_ARC { get; set; }

        /// <summary>
        /// Причина закрытия клиента
        /// </summary>
        public string IST { get; set; }
    }
}
