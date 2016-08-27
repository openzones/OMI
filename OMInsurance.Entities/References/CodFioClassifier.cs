using OMInsurance.Entities.Core;

namespace OMInsurance.Entities
{
    public static class CodFioClassifier
    {
        public static ReferenceItem Standard;
        public static ReferenceItem NoSecondnameFirstname;
        public static ReferenceItem OneLetter;
        public static ReferenceItem Whitespace;
        public static ReferenceItem OneLetterWhitespace;
        public static ReferenceItem Repeat;
        static CodFioClassifier()
        {
            Standard = new ReferenceItem() { Id = 1, Code = " ", Name = "Стандартная запись (пробел)" };
            NoSecondnameFirstname = new ReferenceItem() { Id = 2, Code = "0", Name = "Нет отчества / имени" };
            OneLetter = new ReferenceItem() { Id = 3, Code = "1", Name = "Одна буква в фамилии /имени /отчестве" };
            Whitespace = new ReferenceItem() { Id = 4, Code = "2", Name = "Пробел в фамилии /имени /отчестве" };
            OneLetterWhitespace = new ReferenceItem() { Id = 5, Code = "3", Name = "Одна буква + пробел в фамилии /имени /отчестве" };
            Repeat = new ReferenceItem() { Id = 6, Code = "9", Name = "Повтор реквизитов у разных физических лиц" };
        }
    }
}
