using OMInsurance.Entities.Core;

namespace OMInsurance.Entities
{
    //используется для получения сумм по каждому статусу из базы
    public class BSOSumStatus : DataObject
    {
        public string Name { get; set; }

        //сумма статусов
        //используется в отчете BSOOperativeInformation (Оперативная информация о расходовании БСО (временных свидетельств))
        public long Count { get; set; }
    }
}
