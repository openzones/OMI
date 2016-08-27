using OMInsurance.Entities.Core;

namespace OMInsurance.Entities
{
    public static class FundFileHistoryStatus
    {
        public static ReferenceItem SverkaRequest;
        public static ReferenceItem SverkaResponse;
        public static ReferenceItem FundRequest;
        public static ReferenceItem FundResponse;
        public static ReferenceItem Filial;
        public static ReferenceItem Migrate;
        public static ReferenceItem MFC;

        static FundFileHistoryStatus()
        {
            //данные должны соответствовать справочнику FundFileHistoryStatusRef
            SverkaRequest = new ReferenceItem() { Id = 1, Code = "1", Name = "Сверка" };
            SverkaResponse = new ReferenceItem() { Id = 2, Code = "2", Name = "Ответ на сверку" };
            FundRequest = new ReferenceItem() { Id = 3, Code = "3", Name = "Посылка в фонд" };
            FundResponse = new ReferenceItem() { Id = 4, Code = "4", Name = "Ответ из фонда" };
            Filial = new ReferenceItem() { Id = 5, Code = "5", Name = "Загрузка из филиалов" };
            Migrate = new ReferenceItem() { Id = 6, Code = "6", Name = "Миграция" };
            MFC = new ReferenceItem() { Id = 7, Code = "7", Name = "Загрузка из МФЦ" };
        }
    }
}
