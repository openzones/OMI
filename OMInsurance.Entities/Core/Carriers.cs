
namespace OMInsurance.Entities.Core
{
    public class Carriers
    {
        public static ReferenceItem UEK;
        public static ReferenceItem DigitalPolicy;
        public static ReferenceItem PaperPolicy;

        static Carriers()
        {
            PaperPolicy = new ReferenceItem() { Id = 1, Code = "1", Name = "Бумажный" };
            DigitalPolicy = new ReferenceItem() { Id = 7, Code = "2", Name = "ЭлПолис" };
            UEK = new ReferenceItem() { Id = 2, Code = "3", Name = "УЭК" };
        }
    }
}
