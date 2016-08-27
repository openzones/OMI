using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMInsurance.Entities.Core
{
    public class PolicyTypeRef
    {
        public static ReferenceItem OldPolicy;
        public static ReferenceItem TemporaryPolicy;
        public static ReferenceItem UnifiedPolicy;
        public static ReferenceItem UEK;
        public static ReferenceItem DigitalPolicy;

        static PolicyTypeRef()
        {
            OldPolicy = new ReferenceItem() { Id = 1, Code = "С", Name = "Полис старого образца" };
            TemporaryPolicy = new ReferenceItem() { Id = 2, Code = "В", Name = "ВС" };
            UnifiedPolicy = new ReferenceItem() { Id = 3, Code = "П", Name = "ЕНП" };
            UEK = new ReferenceItem() { Id = 4, Code = "К", Name = "УЭК" };
            DigitalPolicy = new ReferenceItem() { Id = 5, Code = "Э", Name = "ЭлПолис" };
        }
    }
}
