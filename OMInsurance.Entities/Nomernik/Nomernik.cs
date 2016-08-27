using OMInsurance.Entities.Core;
using System;

namespace OMInsurance.Entities
{
    /// <summary>
    /// Номерник (Nomernik)
    /// Класс включает сопроводительную информацию
    /// Сам номерник это: Entities.NOMP и стоп-лист Entities.STOP
    /// </summary>
    public class Nomernik : DataObject
    {

        public long? ClientID { get; set; }
        public long HistoryID { get; set; }
        public string Comment { get; set; }

        /// <summary>
        ///Этот статус не сохраняется в базу, нужен только для логики и вывода результатов       Обновления в БД:
        ///1. не нашли клиента - ничего не делаем                                                    nothing   
        ///2. нашли более 1 клиента - ничего не делаем                                               nothing
        ///3. нашли 1 клиента, данные совпадают - ничего не делаем                                   nothing                              
        ///4. нашли 1 клиента, данные есть везде, но разные - записываем, историю хранить.           insert/update
        ///5. нашли 1 клиента, данных в системе отсутствуют, а в файле есть - записываем             insert
        public int Status { get; set; }

        /// <summary>
        /// Краткая информация по клиенту, необходима для уточнения идентичности клиента
        /// </summary>
        public class ClientShotInfo : Nomernik
        {
            public string Firstname { get; set; }
            public string Secondname { get; set; }
            public string Lastname { get; set; }
            public DateTime? Birthday { get; set; }
        }

        /// <summary>
        /// История номерника и стоп-листа - ведется раздельно в разных таблицах в БД
        /// </summary>
        public class History : DataObject
        {
            public DateTime LoadDate { get; set; }
            public DateTime FileDate { get; set; }
            public long? UserID { get; set; }

            //Кол-во записей: всего, по нашей компании, только изменений
            public long? CountAll { get; set; }
            public long? CountOur { get; set; }
            public long? CountChange { get; set; }

            //Фио пользователя
            public string Lastname { get; set; }
            public string Secondname { get; set; }
            public string Firstname { get; set; }
        }
    }
}
