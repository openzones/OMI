using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;

namespace OMInsurance.Entities
{
    public class ClientVisitStatuses : IEqualityComparer<ReferenceItem>
    {
        public static ReferenceItem SubmitPending;
        public static ReferenceItem AnswerPending;
        public static ReferenceItem Processed;
        public static ReferenceItem PolicyIssued;
        public static ReferenceItem ErrorEntry;
        public static ReferenceItem PolicyMadeByAnotherCompany;
        public static ReferenceItem ReregistrationDone;
        public static ReferenceItem Comment;
        public static ReferenceItem PolicyReadyForClient;
        public static ReferenceItem ImportedPolicy;
        public static ReferenceItem SentToGoznak;
        public static ReferenceItem PolicyIssuedAndSentToTheFond;
        public static ReferenceItem Reconciliation;
        public static ReferenceItem FundError;

        static ClientVisitStatuses()
        {
            SubmitPending = new ReferenceItem() { Id = 1, Code = "1", Name = "Ожидание подачи" };
            AnswerPending = new ReferenceItem() { Id = 2, Code = "2", Name = "Ожидание ответа" };
            Processed = new ReferenceItem() { Id = 3, Code = "3", Name = "Обработана" };
            PolicyIssued = new ReferenceItem() { Id = 4, Code = "5", Name = "Полис получен" };
            ErrorEntry = new ReferenceItem() { Id = 5, Code = "6", Name = "Ошибочная запись" };
            PolicyMadeByAnotherCompany = new ReferenceItem() { Id = 6, Code = "7", Name = "Полис изготовлен другой компанией" };
            ReregistrationDone = new ReferenceItem() { Id = 7, Code = "8", Name = "Перерегистрация завершена" };
            Comment = new ReferenceItem() { Id = 8, Code = "9", Name = "Внимание-комментарий" };
            PolicyReadyForClient = new ReferenceItem() { Id = 9, Code = "10", Name = "Полис изготовлен" };
            PolicyIssuedAndSentToTheFond = new ReferenceItem() { Id = 11, Code = "11", Name = "Полис выдан и отправлен в фонд" };
            ImportedPolicy = new ReferenceItem() { Id = 12, Code = "12", Name = "Импортирован из сторонней системы" };
            SentToGoznak = new ReferenceItem() { Id = 10, Code = "4", Name = "Заявка на изготовление полиса отправлена ГОЗНАК" };
            Reconciliation = new ReferenceItem() { Id = 13, Code = "13", Name = "Сверка" };
            FundError = new ReferenceItem() { Id = 14, Code = "14", Name = "Ошибка посылки в ФОМС" };
        }

        public bool Equals(ReferenceItem x, ReferenceItem y)
        {
            return x.Code == y.Code && x.Id == y.Id;
        }

        public int GetHashCode(ReferenceItem obj)
        {
            if (string.IsNullOrEmpty(obj.Code))
            {
                return obj.Id.GetHashCode();
            };
            return obj.Id.GetHashCode() ^ (obj.Code.GetHashCode());
        }

        //Must be refactored
        public static bool MigrationNeedUpdate(ReferenceItem newStatus, ReferenceItem oldStatus)
        {
            int newIndex = GetMigrationIdComparableIndex(newStatus.Id);
            int oldIndex = GetMigrationIdComparableIndex(oldStatus.Id);
            return newIndex != -1 && oldIndex != -1 && newIndex > oldIndex;
        }

        public static bool MigrationNeedUpdate(long newStatusId, long oldStatusId)
        {
            int newIndex = GetMigrationIdComparableIndex(newStatusId);
            int oldIndex = GetMigrationIdComparableIndex(oldStatusId);
            return newIndex != -1 && oldIndex != -1 && newIndex > oldIndex;
        }

        private static Dictionary<long, int> MirgrationIdRemap = new Dictionary<long, int> { 
            { 1, 0 }, { 2, 1 }, { 5, 2 }, { 7, 3 }, { 10, 4 },  { 9, 5 }, { 4, 6 }, { 11, 7 }
        };

        private static int GetMigrationIdComparableIndex(long id)
        {
            if (MirgrationIdRemap.ContainsKey(id))
                return MirgrationIdRemap[id];
            return -1;
        }
    }
}
