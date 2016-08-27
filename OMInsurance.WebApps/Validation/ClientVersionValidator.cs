using OMInsurance.Entities;
using OMInsurance.WebApps.Models;
using System.Linq;

namespace OMInsurance.WebApps.Validation
{
    public class ClientVersionValidator : BaseValidator<ClientVersionEditModel>
    {
        public ClientVersionValidator()
        {
        }

        public override void Validate(
            ClientVersionEditModel clientVisit,
            ModelValidationContext context)
        {
            ValidateInternalFields(clientVisit, context);
        }

        public override bool IsValid
        {
            get
            {
                return base.IsValid;
            }
        }

        private void ValidateInternalFields(
            ClientVersionEditModel clientVersion,
            ModelValidationContext context)
        {
            TrimFIO(clientVersion);

            //проверка ФИО
            //Автоопределение значения полей - task 9203

            //ID	Name	                                        CODE
            //1   Стандартная запись (пробел)
            //2   Нет отчества / имени                                0
            //3   Одна буква в фамилии / имени / отчестве             1
            //4   Пробел в фамилии / имени / отчестве                 2
            //5   Одна буква + пробел в фамилии / имени / отчестве    3
            //6   Повтор реквизитов у разных физических лиц *         9


            //	Если одна буква в фам/им/от – одна буква в фам/им/от
            if (OneLetter(clientVersion.Lastname)) clientVersion.LastnameTypeId = context.listReferenceItem.Where(a => a.Code == "1").Select(b => b.Id).FirstOrDefault();
            if (OneLetter(clientVersion.Secondname)) clientVersion.SecondnameTypeId = context.listReferenceItem.Where(a => a.Code == "1").Select(b => b.Id).FirstOrDefault();
            if (OneLetter(clientVersion.Firstname)) clientVersion.FirstnameTypeId = context.listReferenceItem.Where(a => a.Code == "1").Select(b => b.Id).FirstOrDefault();

            //Если поле фам/им/от пустое – нет от/им, только на ИО
            if (!string.IsNullOrEmpty(clientVersion.Lastname))
            {
                if (string.IsNullOrEmpty(clientVersion.Secondname))
                {
                    clientVersion.SecondnameTypeId = context.listReferenceItem.Where(a => a.Code == "0").Select(b => b.Id).FirstOrDefault();
                }
                if (string.IsNullOrEmpty(clientVersion.Firstname) && clientVersion.FirstnameTypeId != CodFioClassifier.NoSecondnameFirstname.Id)
                {
                    clientVersion.FirstnameTypeId = context.listReferenceItem.Where(a => a.Code == "0").Select(b => b.Id).FirstOrDefault();
                }
            }

            //Если в фамилии/имени/отчестве есть дефис или пробел – пробел в фам/им/от
            if (Space(clientVersion.Lastname))
            {
                if (SpaceAndOneLetter(clientVersion.Lastname))
                {
                    clientVersion.LastnameTypeId = context.listReferenceItem.Where(a => a.Code == "3").Select(b => b.Id).FirstOrDefault();
                }
                else
                {
                    clientVersion.LastnameTypeId = context.listReferenceItem.Where(a => a.Code == "2").Select(b => b.Id).FirstOrDefault();
                }
            }

            if (Space(clientVersion.Secondname))
            {
                if (SpaceAndOneLetter(clientVersion.Secondname))
                {
                    clientVersion.SecondnameTypeId = context.listReferenceItem.Where(a => a.Code == "3").Select(b => b.Id).FirstOrDefault();
                }
                else
                {
                    clientVersion.SecondnameTypeId = context.listReferenceItem.Where(a => a.Code == "2").Select(b => b.Id).FirstOrDefault();
                }
            }

            if (Space(clientVersion.Firstname))
            {
                if (SpaceAndOneLetter(clientVersion.Firstname))
                {
                    clientVersion.FirstnameTypeId = context.listReferenceItem.Where(a => a.Code == "3").Select(b => b.Id).FirstOrDefault();
                }
                else
                {
                    clientVersion.FirstnameTypeId = context.listReferenceItem.Where(a => a.Code == "2").Select(b => b.Id).FirstOrDefault();
                }
            }

            //Стандартная запись
            if (!Space(clientVersion.Lastname) && !SpaceAndOneLetter(clientVersion.Lastname) && !OneLetter(clientVersion.Lastname) && !string.IsNullOrEmpty(clientVersion.Lastname) && clientVersion.LastnameTypeId != 6)
            {
                clientVersion.LastnameTypeId = context.listReferenceItem.Where(a => a.Code == " ").Select(b => b.Id).FirstOrDefault();
            }
            if (!Space(clientVersion.Firstname) && !SpaceAndOneLetter(clientVersion.Firstname) && !OneLetter(clientVersion.Firstname) && !string.IsNullOrEmpty(clientVersion.Firstname) && clientVersion.FirstnameTypeId != 6)
            {
                clientVersion.FirstnameTypeId = context.listReferenceItem.Where(a => a.Code == " ").Select(b => b.Id).FirstOrDefault();
            }
            if (!Space(clientVersion.Secondname) && !SpaceAndOneLetter(clientVersion.Secondname) && !OneLetter(clientVersion.Secondname) && !string.IsNullOrEmpty(clientVersion.Secondname) && clientVersion.SecondnameTypeId != 6)
            {
                clientVersion.SecondnameTypeId = context.listReferenceItem.Where(a => a.Code == " ").Select(b => b.Id).FirstOrDefault();
            }
        }

        private void TrimFIO(ClientVersionEditModel clientVersion)
        {
            clientVersion.Lastname = string.IsNullOrEmpty(clientVersion.Lastname) ? string.Empty : clientVersion.Lastname.Trim();
            clientVersion.Firstname = string.IsNullOrEmpty(clientVersion.Firstname) ? string.Empty : clientVersion.Firstname.Trim();
            clientVersion.Secondname = string.IsNullOrEmpty(clientVersion.Secondname) ? string.Empty : clientVersion.Secondname.Trim();
        }

        private bool OneLetter(string name)
        {
            if ((!string.IsNullOrEmpty(name)) && name.Length == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool Space(string name)
        {
            foreach (var ch in name)
            {
                if (ch == ' ' || ch == '-') return true;
            }
            return false;
        }

        private bool SpaceAndOneLetter(string name)
        {
            if (Space(name))
            {
                if (name.Remove(2) != name.Remove(2).Trim()) return true;
            }
            return false;
        }
    }
}