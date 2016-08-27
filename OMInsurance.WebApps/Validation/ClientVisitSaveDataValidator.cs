using OMInsurance.BusinessLogic;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using OMInsurance.WebApps.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OMInsurance.WebApps.Validation
{
    public class ClientVisitSaveDataValidator : BaseValidator<ClientVisitSaveDataModel>
    {

        public ClientVisitSaveDataValidator()
        {
        }

        public override void Validate(
            ClientVisitSaveDataModel clientVisit,
            ModelValidationContext context)
        {
            ValidateInternalFields(clientVisit, context);
        }

        public override bool IsValid
        {
            get
            {
                return base.IsValid && isValid;
            }
        }

        public override bool IsValidNotCritical
        {
            get
            {
                return base.IsValidNotCritical && isValidNotCritical;
            }
        }

        bool isValid;
        bool isValidNotCritical;

        //создаем экземпляры? либо должны их передать в валидатор?
        ReferenceBusinessLogic referenceBusinessLogic = new ReferenceBusinessLogic();
        BSOBusinessLogic bsoLogic = new BSOBusinessLogic();
        ClientBusinessLogic clientBusinessLogic = new ClientBusinessLogic();

        private void ValidateInternalFields(
            ClientVisitSaveDataModel clientVisit,
            ModelValidationContext context)
        {
            isValid = true;
            isValidNotCritical = true;
            clientVisit.LivingAddress.Validate(context);
            clientVisit.RegistrationAddress.Validate(context);
            clientVisit.NewPolicy.Validate(context);
            clientVisit.OldPolicy.Validate(context);
            clientVisit.NewDocument.Validate(context);
            clientVisit.OldDocument.Validate(context);

            context.listReferenceItem = referenceBusinessLogic.GetReferencesList(Constants.CodFioClassifier);
            clientVisit.NewClientInfo.Validate(context);
            clientVisit.OldClientInfo.Validate(context);
            clientVisit.Representative.Validate(context);

            isValid = clientVisit.LivingAddress.IsValid() &&
            clientVisit.RegistrationAddress.IsValid() &&
            clientVisit.NewPolicy.IsValid() &&
            clientVisit.OldPolicy.IsValid() &&
            clientVisit.NewDocument.IsValid() &&
            clientVisit.OldDocument.IsValid() &&
            clientVisit.NewClientInfo.IsValid() &&
            clientVisit.OldClientInfo.IsValid() &&
            clientVisit.Representative.IsValid();

            isValidNotCritical = clientVisit.LivingAddress.IsValidNotCritical() &&
            clientVisit.RegistrationAddress.IsValidNotCritical() &&
            clientVisit.NewPolicy.IsValidNotCritical() &&
            clientVisit.OldPolicy.IsValidNotCritical() &&
            clientVisit.NewDocument.IsValidNotCritical() &&
            clientVisit.OldDocument.IsValidNotCritical() &&
            clientVisit.NewClientInfo.IsValidNotCritical() &&
            clientVisit.OldClientInfo.IsValidNotCritical() &&
            clientVisit.Representative.IsValidNotCritical();

            ValidateDate(clientVisit);
            ValidateDocument(clientVisit);
            ValidateCitizenship(clientVisit);
            ValidatePassportAndBirthday(clientVisit);
            ValidateBirthCertificate(clientVisit);
            ValidateDeliveryCenter(clientVisit);
            ValidatePhone(clientVisit);

            //if (clientVisit.StatusId == ClientVisitStatuses.SubmitPending.Id) ValidateBSO(clientVisit);
            ValidateBSO(clientVisit);

            //ФЛК по сценарию только исключая роль "Администратор" и только в статусе "Ожидание подачи"
            //если роль не передали(currenUser=null) - все равно проверяем
            if (clientVisit.StatusId == ClientVisitStatuses.SubmitPending.Id)
            {
                if (context.currenUser == null || Role.GetRealRole(context.currenUser).Id != Role.Administrator.Id)
                {
                    ValidateScenario(clientVisit);
                }
            }

            ValidateScenarioForSpecialScenario(clientVisit);
            ValidateSpecialStatus(clientVisit, context);
            ValidateSNILS(clientVisit, context);

        }



        private void ValidateSNILS(ClientVisitSaveDataModel clientVisit, ModelValidationContext context)
        {

            //Не критичное ФЛК
            //При роли Регистратор на сохранение заявки, в случае если значение поля «гражданство» = «Россия» и поле «СНИЛС» не заполнено
            //пока включено для всех ролей
            //if (context.currenUser != null && (Role.GetRealRole(context.currenUser).Id == Role.Registrator.Id))
            {
                List<ReferenceItem> CitizenshipRefItems = referenceBusinessLogic.GetReferencesList(Constants.CitizenshipRef);
                var RussiaId = CitizenshipRefItems.Where(a => a.Code == "RUS").Select(b => b.Id).FirstOrDefault();
                if (string.IsNullOrEmpty(clientVisit.NewClientInfo.SNILS) && clientVisit.NewClientInfo.Citizenship == RussiaId)
                {
                    isValidNotCritical = false;
                    this.MessagesNotCritical.Add("Информация о клиенте: СНИЛС не заполнен.");
                    clientVisit.NewClientInfo.MessagesNotCritical.Add("Информация о клиенте: СНИЛС не заполнен.");
                }
            }

            //Проверка поля СНИЛС
            //Допустимые символы - цифры, пробел, дефис.
            //Проверяется на валидность контрольным числом. СНИЛС имеет вид "XXX-XXX-XXX YY", где XXX-XXX-XXX - номер, а YY - контрольное число.
            //Проверка контрольного числа проводится только для номеров больше номера 001 - 001 - 998.
            //Контрольное число СНИЛС рассчитывается следующим образом:
            //Каждая цифра СНИЛС умножается на номер своей позиции (позиции отсчитываются с конца);
            //Полученные произведения суммируются;
            //Если сумма меньше 100, то контрольное число равно самой сумме;
            //Если сумма равна 100 или 101, то контрольное число равно 00;
            //Если сумма больше 101, то сумма делится нацело на 101 и контрольное число определяется
            //остатком от деления аналогично предыдущим двум пунктам.

            if (!string.IsNullOrEmpty(clientVisit.NewClientInfo.SNILS))
            {
                string snils;
                snils = clientVisit.NewClientInfo.SNILS;
                snils = snils.Trim();

                //проверяем, что снилс  имеет вид XXX-XXX-XXX YY и длину = 14
                if (!(snils.Length == 14 && snils[3] == '-' && snils[7] == '-' && snils[11] == ' '))
                {
                    isValidNotCritical = false;
                    this.MessagesNotCritical.Add("Информация о клиенте: проверьте СНИЛС. Он должен быть вида XXX-XXX-XXX YY.");
                    clientVisit.NewClientInfo.MessagesNotCritical.Add("Информация о клиенте: проверьте СНИЛС. Он должен быть вида XXX-XXX-XXX YY.");
                    return;
                }

                snils = snils.Trim('-', ' ');
                string clearSnils = string.Empty;
                foreach (var item in snils)
                {
                    if (item != '-' & item != ' ')
                    {
                        clearSnils = clearSnils + item;
                    }
                }

                try
                {
                    const int length = 9;
                    bool flagCheck = true;

                    //Проверка контрольного числа проводится только для номеров больше номера 001-001-998
                    if (int.Parse(clearSnils.Substring(0, length)) < 1001998) return;

                    int controlSnils = int.Parse(clearSnils.Substring(length, 2));
                    int[] mainSnils = new int[length];
                    for (int i = 0; i < length; i++)
                    {
                        mainSnils[i] = int.Parse(clearSnils[length - 1 - i].ToString());
                    }

                    int sum = 0;
                    for (int i = 0; i < mainSnils.Length; i++)
                    {
                        sum = sum + mainSnils[i] * (i + 1);
                    }

                    var divisible = sum % 101;
                    if (divisible < 100)
                    {
                        if (divisible != controlSnils)
                        {
                            flagCheck = false;
                        }
                    }
                    else if (divisible == 100 || divisible == 101)
                    {
                        if (controlSnils != 0)
                        {
                            flagCheck = false;
                        }
                    }
                    else //sum > 101  - в эту ветку зайти не сможет
                    {
                        if (controlSnils != (divisible % 101))
                        {
                            flagCheck = false;
                        }
                    }

                    //Дополнительная проверка:
                    //В номере XXX - XXX - XXX не может присутствовать одна и та же цифра три раза подряд.
                    //Дефисы при этой проверке игнорируются, т.е.неверными будут все нижеследующие примеры СНИЛСов:
                    //XXX - 222 - XXX YY
                    //XX2 - 22X - XXX YY
                    int countRepeat = 1;
                    int ch = mainSnils[0];
                    for (int i = 1; i < length; i++)
                    {
                        if (ch == mainSnils[i])
                        {
                            countRepeat++;
                        }
                        else
                        {
                            countRepeat = 1;
                        }

                        if (countRepeat >= 3)
                        {
                            flagCheck = false;
                            break;
                        }
                        ch = mainSnils[i];
                    }

                    if (!flagCheck)
                    {
                        isValid = false;
                        this.Messages.Add("Информация о клиенте: СНИЛС некорректный! Проверьте правильность ввода всех цифр.");
                        clientVisit.NewClientInfo.Messages.Add("Информация о клиенте: СНИЛС некорректный! Проверьте правильность ввода всех цифр.");
                    }
                }
                catch
                {
                    //не смогли распарсить и проверить СНИЛС
                    isValidNotCritical = false;
                    this.MessagesNotCritical.Add("Информация о клиенте: СНИЛС не возможно проверить! Сообщите администратору о возможной ошибке.");
                    clientVisit.NewClientInfo.MessagesNotCritical.Add("Информация о клиенте: СНИЛС не возможно проверить! Сообщите администратору о возможной ошибке.");
                }
            }
        }

        //Разрешить менять статус Регистратору и Ответственному с "внимание комментарий" на "ожидание подачи".
        private void ValidateSpecialStatus(ClientVisitSaveDataModel clientVisit, ModelValidationContext context)
        {
            //если currentUser не передали в валидатор - то не проверяем
            if (context.currenUser != null && (Role.GetRealRole(context.currenUser).Id == Role.Registrator.Id || Role.GetRealRole(context.currenUser).Id == Role.ResponsibleBSO.Id))
            {
                if (!(clientVisit.StatusId == ClientVisitStatuses.SubmitPending.Id || clientVisit.StatusId == ClientVisitStatuses.Comment.Id))
                {
                    if (clientVisit.VisitId != null)
                    {
                        long oldStatusId = clientBusinessLogic.ClientVisit_Get((long)clientVisit.VisitId).Status.Id;
                        if (oldStatusId == ClientVisitStatuses.Comment.Id)
                        {
                            isValid = false;
                            this.Messages.Add("Общая информация по обращению: " +
                                string.Format("В роли {0} статус [{1}] можно изменить только на [{2}]",
                                Role.GetRealRole(context.currenUser).Description, ClientVisitStatuses.Comment.Name, ClientVisitStatuses.SubmitPending.Name));
                            clientVisit.StatusId = ClientVisitStatuses.Comment.Id;
                        }
                    }
                }
            }
        }

        //Если сценарий PI, PT, CI, CT, CR, RI, RT, MP, PRI, PRT, CP и старые документы не заполнены,
        //то копируем данные в старые документы из новых при сохранении.
        private void ValidateScenarioForSpecialScenario(ClientVisitSaveDataModel clientVisit)
        {
            List<ReferenceItem> items = referenceBusinessLogic.GetReferencesList(Constants.ScenarioRef);
            foreach (var item in items)
            {
                item.Code = item.Code.Trim();
            }

            var tempId = items.Where(a => a.Code == "PI" ||
                                          a.Code == "PT" ||
                                          a.Code == "CI" ||
                                          a.Code == "CT" ||
                                          a.Code == "CR" ||
                                          a.Code == "RI" ||
                                          a.Code == "RT" ||
                                          a.Code == "MP" ||
                                          a.Code == "PRI" ||
                                          a.Code == "PRT" ||
                                          a.Code == "CP"
                                     ).Select(b => b.Id).Where(c => c == clientVisit.ScenarioId).FirstOrDefault();
            //Список в списке - предыдущая запись только короче
            //List<string> scn = new List<string>() { "PI", "PT", "CI", "CT", "CR", "RI", "RT", "MP", "PRI", "PRT", "CP" };
            //var tempId1 = items.FindAll(a => scn.Contains(a.Code)).Select(b=>b.Id).Where(c => c == clientVisit.ScenarioId).FirstOrDefault();

            if (tempId != 0)
            {
                if (clientVisit.OldDocument.DocumentTypeID == null &&
                       clientVisit.OldDocument.Series == null &&
                       clientVisit.OldDocument.Number == null &&
                       clientVisit.OldDocument.IssueDate == null &&
                       clientVisit.OldDocument.ExpirationDate == null &&
                       clientVisit.OldDocument.IssueDepartment == null)
                {
                    clientVisit.OldDocument.DocumentTypeID = clientVisit.NewDocument.DocumentTypeID;
                    clientVisit.OldDocument.Series = clientVisit.NewDocument.Series;
                    clientVisit.OldDocument.Number = clientVisit.NewDocument.Number;
                    clientVisit.OldDocument.IssueDate = clientVisit.NewDocument.IssueDate;
                    clientVisit.OldDocument.ExpirationDate = clientVisit.NewDocument.ExpirationDate;
                    clientVisit.OldDocument.IssueDepartment = clientVisit.NewDocument.IssueDepartment;
                }

                if (clientVisit.OldForeignDocument.DocumentTypeID == null &&
                       clientVisit.OldForeignDocument.Series == null &&
                       clientVisit.OldForeignDocument.Number == null &&
                       clientVisit.OldForeignDocument.IssueDate == null &&
                       clientVisit.OldForeignDocument.ExpirationDate == null &&
                       clientVisit.OldForeignDocument.IssueDepartment == null)
                {
                    clientVisit.OldForeignDocument.DocumentTypeID = clientVisit.NewForeignDocument.DocumentTypeID;
                    clientVisit.OldForeignDocument.Series = clientVisit.NewForeignDocument.Series;
                    clientVisit.OldForeignDocument.Number = clientVisit.NewForeignDocument.Number;
                    clientVisit.OldForeignDocument.IssueDate = clientVisit.NewForeignDocument.IssueDate;
                    clientVisit.OldForeignDocument.ExpirationDate = clientVisit.NewForeignDocument.ExpirationDate;
                    clientVisit.OldForeignDocument.IssueDepartment = clientVisit.NewForeignDocument.IssueDepartment;
                }
            }
        }

        //Общая проверка дат на вменяемость. Например 'Дата обращения' не может быть 01.01.1980
        private void ValidateDate(ClientVisitSaveDataModel clientVisit)
        {
            DateTime DateMinimumForDocuments = new DateTime(1991, 1, 1);
            DateTime DateMinimumForPeople = new DateTime(1900, 1, 1);

            //Дата обращения
            if (clientVisit.TemporaryPolicyDate != null && clientVisit.TemporaryPolicyDate < DateMinimumForDocuments)
            {
                isValid = false;
                this.Messages.Add("Общая информация по обращению: " +
                    string.Format("Дата обращения {0} не может быть меньше {1}. Введите правильную дату.",
                    ((DateTime)clientVisit.TemporaryPolicyDate).ToShortDateString(), DateMinimumForDocuments.ToShortDateString()));
            }

            //Дата окончания действия ВС
            if (clientVisit.TemporaryPolicyExpirationDate != null && clientVisit.TemporaryPolicyExpirationDate < DateMinimumForDocuments)
            {
                isValid = false;
                this.Messages.Add("Общая информация по обращению: " +
                    string.Format("Дата окончания действия ВС {0} не может быть меньше {1}. Введите правильную дату.",
                    ((DateTime)clientVisit.TemporaryPolicyExpirationDate).ToShortDateString(), DateMinimumForDocuments.ToShortDateString()));
            }

            //Дата рождения не может быть меньше 1900
            if (clientVisit.NewClientInfo.Birthday != null && clientVisit.NewClientInfo.Birthday < DateMinimumForPeople)
            {
                isValid = false;
                this.Messages.Add("Информация о клиенте: " + Environment.NewLine +
                    string.Format("Дата рождения {0} не может быть меньше {1}. Введите правильную дату.",
                    ((DateTime)clientVisit.NewClientInfo.Birthday).ToShortDateString(), DateMinimumForPeople.ToShortDateString()));
                clientVisit.NewClientInfo.Messages.Add(string.Format("Дата рождения {0} не может быть меньше {1}. Введите правильную дату.",
                    ((DateTime)clientVisit.NewClientInfo.Birthday).ToShortDateString(), DateMinimumForPeople.ToShortDateString()));
            }
            if (clientVisit.OldClientInfo.Birthday != null && clientVisit.OldClientInfo.Birthday < DateMinimumForPeople)
            {
                isValid = false;
                this.Messages.Add("Информация о клиенте: " +
                    string.Format("Дата рождения {0} не может быть меньше {1}. Введите правильную дату.",
                    ((DateTime)clientVisit.OldClientInfo.Birthday).ToShortDateString(), DateMinimumForPeople.ToShortDateString()));
                clientVisit.OldClientInfo.Messages.Add(string.Format("Дата рождения {0} не может быть меньше {1}. Введите правильную дату.",
                    ((DateTime)clientVisit.OldClientInfo.Birthday).ToShortDateString(), DateMinimumForPeople.ToShortDateString()));
            }

            //Дата паспорта/документ иностранного гражданина
            if (clientVisit.NewDocument.IssueDate != null && clientVisit.NewDocument.IssueDate < DateMinimumForPeople)
            {
                isValid = false;
                this.Messages.Add("Документы (Новая информация о документах): " +
                    string.Format("Дата выдачи паспорта {0} не может быть меньше {1}. Введите правильную дату.",
                    ((DateTime)clientVisit.NewDocument.IssueDate).ToShortDateString(), DateMinimumForPeople.ToShortDateString()));
                clientVisit.NewDocument.Messages.Add(string.Format("Дата выдачи паспорта {0} не может быть меньше {1}. Введите правильную дату.",
                    ((DateTime)clientVisit.NewDocument.IssueDate).ToShortDateString(), DateMinimumForPeople.ToShortDateString()));
            }
            if (clientVisit.OldDocument.IssueDate != null && clientVisit.OldDocument.IssueDate < DateMinimumForPeople)
            {
                isValid = false;
                this.Messages.Add("Документы (Старая информация о документах): " +
                    string.Format("Дата выдачи паспорта {0} не может быть меньше {1}. Введите правильную дату.",
                    ((DateTime)clientVisit.OldDocument.IssueDate).ToShortDateString(), DateMinimumForPeople.ToShortDateString()));
                clientVisit.OldDocument.Messages.Add(string.Format("Дата выдачи паспорта {0} не может быть меньше {1}. Введите правильную дату.",
                    ((DateTime)clientVisit.OldDocument.IssueDate).ToShortDateString(), DateMinimumForPeople.ToShortDateString()));
            }
            if (clientVisit.NewForeignDocument.IssueDate != null && clientVisit.NewForeignDocument.IssueDate < DateMinimumForPeople)
            {
                isValid = false;
                this.Messages.Add("Документы (Дополнительный документ иностранного гражданина): " +
                    string.Format("Дата выдачи {0} не может быть меньше {1}. Введите правильную дату.",
                    ((DateTime)clientVisit.NewForeignDocument.IssueDate).ToShortDateString(), DateMinimumForPeople.ToShortDateString()));
                clientVisit.NewForeignDocument.Messages.Add(string.Format("Дата выдачи {0} не может быть меньше {1}. Введите правильную дату.",
                    ((DateTime)clientVisit.NewForeignDocument.IssueDate).ToShortDateString(), DateMinimumForPeople.ToShortDateString()));
            }
            if (clientVisit.OldForeignDocument.IssueDate != null && clientVisit.OldForeignDocument.IssueDate < DateMinimumForPeople)
            {
                isValid = false;
                this.Messages.Add("Документы (Прежний дополнительный документ иностранного гражданина): " +
                    string.Format("Дата выдачи {0} не может быть меньше {1}. Введите правильную дату.",
                    ((DateTime)clientVisit.OldForeignDocument.IssueDate).ToShortDateString(), DateMinimumForPeople.ToShortDateString()));
                clientVisit.OldForeignDocument.Messages.Add(string.Format("Дата выдачи {0} не может быть меньше {1}. Введите правильную дату.",
                    ((DateTime)clientVisit.OldForeignDocument.IssueDate).ToShortDateString(), DateMinimumForPeople.ToShortDateString()));
            }

            //Дата регистрации
            if (clientVisit.RegistrationAddressDate != null && clientVisit.RegistrationAddressDate < DateMinimumForPeople)
            {
                isValid = false;
                this.Messages.Add("Адрес: " +
                    string.Format("Дата регистрации {0} не может быть меньше {1}. Введите правильную дату.",
                    ((DateTime)clientVisit.RegistrationAddressDate).ToShortDateString(), DateMinimumForPeople.ToShortDateString()));
            }

            //Данные полисов
            if (clientVisit.NewPolicy.StartDate != null && clientVisit.NewPolicy.StartDate < DateMinimumForDocuments)
            {
                isValid = false;
                this.Messages.Add("Данные полисов (Новый полис): " +
                    string.Format("Дата начала действия {0} не может быть меньше {1}. Введите правильную дату.",
                    ((DateTime)clientVisit.NewPolicy.StartDate).ToShortDateString(), DateMinimumForDocuments.ToShortDateString()));
                clientVisit.NewPolicy.Messages.Add(string.Format("Дата начала действия {0} не может быть меньше {1}. Введите правильную дату.",
                    ((DateTime)clientVisit.NewPolicy.StartDate).ToShortDateString(), DateMinimumForDocuments.ToShortDateString()));
            }
            if (clientVisit.OldPolicy.StartDate != null && clientVisit.OldPolicy.StartDate < DateMinimumForDocuments)
            {
                isValid = false;
                this.Messages.Add("Данные полисов (Старый полис): " +
                    string.Format("Дата начала действия {0} не может быть меньше {1}. Введите правильную дату.",
                    ((DateTime)clientVisit.OldPolicy.StartDate).ToShortDateString(), DateMinimumForDocuments.ToShortDateString()));
                clientVisit.OldPolicy.Messages.Add(string.Format("Дата начала действия {0} не может быть меньше {1}. Введите правильную дату.",
                    ((DateTime)clientVisit.OldPolicy.StartDate).ToShortDateString(), DateMinimumForDocuments.ToShortDateString()));
            }

            //Представитель
            if (clientVisit.Representative.Birthday != null && clientVisit.Representative.Birthday < DateMinimumForPeople)
            {
                isValid = false;
                this.Messages.Add("Представитель: " +
                    string.Format("Дата рождения {0} не может быть меньше {1}. Введите правильную дату.",
                    ((DateTime)clientVisit.Representative.Birthday).ToShortDateString(), DateMinimumForPeople.ToShortDateString()));
                clientVisit.Representative.Messages.Add(string.Format("Дата рождения {0} не может быть меньше {1}. Введите правильную дату.",
                    ((DateTime)clientVisit.Representative.Birthday).ToShortDateString(), DateMinimumForPeople.ToShortDateString()));
            }
            if (clientVisit.Representative.IssueDate != null && clientVisit.Representative.IssueDate < DateMinimumForPeople)
            {
                isValid = false;
                this.Messages.Add("Представитель: " +
                    string.Format("Дата выдачи документа {0} не может быть меньше {1}. Введите правильную дату.",
                    ((DateTime)clientVisit.Representative.IssueDate).ToShortDateString(), DateMinimumForPeople.ToShortDateString()));
                clientVisit.Representative.Messages.Add(string.Format("Дата выдачи документа {0} не может быть меньше {1}. Введите правильную дату.",
                    ((DateTime)clientVisit.Representative.IssueDate).ToShortDateString(), DateMinimumForPeople.ToShortDateString()));
            }

            //Прикрепление к МО
            if (clientVisit.AttachmentDate != null && clientVisit.AttachmentDate < DateMinimumForDocuments)
            {
                isValid = false;
                this.Messages.Add("Прикрепление к МО: " +
                    string.Format("Дата рождения {0} не может быть меньше {1}. Введите правильную дату.",
                    ((DateTime)clientVisit.AttachmentDate).ToShortDateString(), DateMinimumForDocuments.ToShortDateString()));
            }

        }

        private void ValidateDocument(ClientVisitSaveDataModel clientVisit)
        {
            DateTime newBirthday = clientVisit.NewClientInfo.Birthday.Value;
            int newAge = GetAge(newBirthday, clientVisit.CreateDate);
            DateTime? oldBirthday = clientVisit.OldClientInfo.Birthday;
            int oldAge = 0;
            if (oldBirthday != null)
            {
                oldAge = GetAge((DateTime)oldBirthday, clientVisit.CreateDate);
            }

            string message;
            string message1;
            List<ReferenceItem> ClientCategoryItems = referenceBusinessLogic.GetReferencesList(Constants.ClientCategoryRef);
            List<ReferenceItem> DocumentTypeRefItems = referenceBusinessLogic.GetReferencesList(Constants.DocumentTypeRef);

            //Граждане РФ Москва Code = 00
            //до 14 лет
            //свидетельство о рождении, выданное в Российской Федерации или id = 3
            //свидетельство о рождении, выданное не в Российской Федерации id = 24
            //с 14 лет
            //паспорт гражданина Российской Федерации id = 14
            //временное удостоверение личности гражданина Российской Федерации, выдаваемое на период оформления паспорта id = 13
            if (clientVisit.NewClientInfo.Category == ClientCategoryItems.Where(a => a.Code == "00").Select(b => b.Id).FirstOrDefault())
            {
                string messageLess14 = string.Format("При выбранной категории '{0}' и возрасте менее 14 лет необходимо выбрать '{1}' или '{2}' ",
                            ClientCategoryItems.Where(a => a.Code == "00").Select(b => b.Name).FirstOrDefault(),
                            DocumentTypeRefItems.Where(a => a.Id == 3).Select(b => b.Name).FirstOrDefault(),
                            DocumentTypeRefItems.Where(a => a.Id == 24).Select(b => b.Name).FirstOrDefault());
                string messageMore14 = string.Format("При выбранной категории '{0}' необходимо выбрать '{1}' или '{2}' ",
                            ClientCategoryItems.Where(a => a.Code == "00").Select(b => b.Name).FirstOrDefault(),
                            DocumentTypeRefItems.Where(a => a.Id == 14).Select(b => b.Name).FirstOrDefault(),
                            DocumentTypeRefItems.Where(a => a.Id == 13).Select(b => b.Name).FirstOrDefault());

                if (newAge < 14)
                {
                    if (clientVisit.NewDocument.DocumentTypeID != 3 && clientVisit.NewDocument.DocumentTypeID != 24)
                    {
                        isValid = false;
                        this.Messages.Add("Документы (Новая информация о документах): " + messageLess14);
                        clientVisit.NewDocument.Messages.Add(messageLess14);
                    }
                }
                else
                {
                    if (clientVisit.NewDocument.DocumentTypeID != 14 && clientVisit.NewDocument.DocumentTypeID != 13)
                    {
                        isValid = false;
                        this.Messages.Add("Документы (Новая информация о документах): " + messageMore14);
                        clientVisit.NewDocument.Messages.Add(messageMore14);
                    }
                }

                /*
                if(oldBirthday != null)
                {
                    if (oldAge < 14)
                    {
                        if (clientVisit.OldDocument.DocumentTypeID != 3 && clientVisit.OldDocument.DocumentTypeID != 24)
                        {
                            isValid = false;
                            this.Messages.Add("Документы (Старая информация о документах): " + messageLess14);
                            clientVisit.OldDocument.Messages.Add(messageLess14);
                        }
                    }
                    else
                    {
                        if (clientVisit.OldDocument.DocumentTypeID != 14 && clientVisit.OldDocument.DocumentTypeID != 13)
                        {
                            isValid = false;
                            this.Messages.Add("Документы (Старая информация о документах): " + messageMore14);
                            clientVisit.OldDocument.Messages.Add(messageMore14);
                        }
                    }
                }
                */
            }

            //Граждане РФ, др. субъект РФ, а так же БОМЖ Code = 77
            //менее 14
            //свидетельство о рождении, выданное в Российской Федерации id = 3
            //свидетельство о рождении, выданное не в Российской Федерации id = 24
            //с 14 лет
            //паспорт гражданина Российской Федерации id = 14
            //временное удостоверение личности гражданина Российской Федерации, выдаваемое на период оформления паспорта id = 13
            if (clientVisit.NewClientInfo.Category == ClientCategoryItems.Where(a => a.Code == "77").Select(b => b.Id).FirstOrDefault())
            {

                string messageLess14 = string.Format("При выбранной категории '{0}' и возрасте менее 14 лет необходимо выбрать '{1}' или '{2}' ",
                            ClientCategoryItems.Where(a => a.Code == "77").Select(b => b.Name).FirstOrDefault(),
                            DocumentTypeRefItems.Where(a => a.Id == 3).Select(b => b.Name).FirstOrDefault(),
                            DocumentTypeRefItems.Where(a => a.Id == 24).Select(b => b.Name).FirstOrDefault());

                string messageMore14 = string.Format("При выбранной категории '{0}' необходимо выбрать '{1}' или '{2}' ",
                            ClientCategoryItems.Where(a => a.Code == "77").Select(b => b.Name).FirstOrDefault(),
                            DocumentTypeRefItems.Where(a => a.Id == 14).Select(b => b.Name).FirstOrDefault(),
                            DocumentTypeRefItems.Where(a => a.Id == 13).Select(b => b.Name).FirstOrDefault());

                if (newAge < 14)
                {
                    if (clientVisit.NewDocument.DocumentTypeID != 3 && clientVisit.NewDocument.DocumentTypeID != 24)
                    {
                        isValid = false;
                        this.Messages.Add("Документы (Новая информация о документах): " + messageLess14);
                        clientVisit.NewDocument.Messages.Add(messageLess14);
                    }
                }
                else
                {
                    if (clientVisit.NewDocument.DocumentTypeID != 14 && clientVisit.NewDocument.DocumentTypeID != 13)
                    {
                        isValid = false;
                        this.Messages.Add("Документы (Новая информация о документах): " + messageMore14);
                        clientVisit.NewDocument.Messages.Add(messageMore14);
                    }
                }

                /*
                if (oldBirthday != null)
                {
                    if (oldAge < 14)
                    {
                        if (clientVisit.OldDocument.DocumentTypeID != 3 && clientVisit.OldDocument.DocumentTypeID != 24)
                        {
                            isValid = false;
                            this.Messages.Add("Документы (Старая информация о документах): " + messageLess14);
                            clientVisit.OldDocument.Messages.Add(messageLess14);
                        }
                    }
                    else
                    {
                        if (clientVisit.OldDocument.DocumentTypeID != 14 && clientVisit.OldDocument.DocumentTypeID != 13)
                        {
                            isValid = false;
                            this.Messages.Add("Документы (Старая информация о документах): " + messageMore14);
                            clientVisit.OldDocument.Messages.Add(messageMore14);
                        }
                    }
                }
                */
            }

            //Иностранные граждане / лица без гражданства, зарегистрированные в г.Москве (Имеют вид на жительство РФ) code = 45
            //для всех возрастов
            //(паспорт иностранного гражданина id = 9
            //или документ иностранного гражданина) id =  21
            //Документ лица без гражданства)   id = 22
            //и (вид на жительство) id = 11
            if (clientVisit.NewClientInfo.Category == ClientCategoryItems.Where(a => a.Code == "45").Select(b => b.Id).FirstOrDefault())
            {
                message = string.Format("При выбранной категории '{0}' необходимо выбрать другой тип документа: {1}, {2} или {3}. ",
                                    ClientCategoryItems.Where(a => a.Code == "45").Select(b => b.Name).FirstOrDefault(),
                                    DocumentTypeRefItems.Where(a => a.Id == 9).Select(b => b.Name).FirstOrDefault(),
                                    DocumentTypeRefItems.Where(a => a.Id == 21).Select(b => b.Name).FirstOrDefault(),
                                    DocumentTypeRefItems.Where(a => a.Id == 22).Select(b => b.Name).FirstOrDefault());
                message1 = string.Format("Необходимо выбрать другой тип документа: {0}. ",
                                    DocumentTypeRefItems.Where(a => a.Id == 11).Select(b => b.Name).FirstOrDefault());

                if ((clientVisit.NewDocument.DocumentTypeID == 9 ||
                    clientVisit.NewDocument.DocumentTypeID == 21 ||
                    clientVisit.NewDocument.DocumentTypeID == 22) &&
                    clientVisit.NewForeignDocument.DocumentTypeID == 11)
                {
                    //nothing
                }
                else
                {
                    if (!(clientVisit.NewDocument.DocumentTypeID == 9 || clientVisit.NewDocument.DocumentTypeID == 21 || clientVisit.NewDocument.DocumentTypeID == 22))
                    {
                        isValid = false;
                        this.Messages.Add("Документы (Новая информация о документах): " + message);
                        clientVisit.NewDocument.Messages.Add(message);
                    }

                    if (!(clientVisit.NewForeignDocument.DocumentTypeID == 11))
                    {
                        isValid = false;
                        this.Messages.Add("Документы (Дополнительный документ иностранного гражданина): " + message1);
                        clientVisit.NewForeignDocument.Messages.Add(message1);
                    }
                }

                /*
                if (clientVisit.OldDocument.DocumentTypeID != 9 && clientVisit.OldDocument.DocumentTypeID != 21 &&
                    clientVisit.OldDocument.DocumentTypeID != 11 && clientVisit.OldDocument.DocumentTypeID != 23)
                {
                    isValid = false;
                    this.Messages.Add("Документы (Старая информация о документах):");
                    this.Messages.Add(message);
                    clientVisit.OldDocument.Messages.Add(message);
                }
                */
            }

            //Беженцы / переселенцы Code = 73
            //для всех возрастов
            //Удостоверение беженца id = 12
            //свидетельство о рассмотрении ходатайства о признании беженцем по существу id = 10
            //cвидетельство о предоставлении временного убежища на территории Российской федерации. id = 25
            if (clientVisit.NewClientInfo.Category == ClientCategoryItems.Where(a => a.Code == "73").Select(b => b.Id).FirstOrDefault())
            {

                message = string.Format("При выбранной категории '{0}' необходимо выбрать другой тип документа: {1}, {2} или {3}.",
                                    ClientCategoryItems.Where(a => a.Code == "73").Select(b => b.Name).FirstOrDefault(),
                                    DocumentTypeRefItems.Where(a => a.Id == 12).Select(b => b.Name).FirstOrDefault(),
                                    DocumentTypeRefItems.Where(a => a.Id == 10).Select(b => b.Name).FirstOrDefault(),
                                    DocumentTypeRefItems.Where(a => a.Id == 25).Select(b => b.Name).FirstOrDefault());

                if (clientVisit.NewDocument.DocumentTypeID != 12 &&
                    clientVisit.NewDocument.DocumentTypeID != 10 &&
                    clientVisit.NewDocument.DocumentTypeID != 25)
                {
                    isValid = false;
                    this.Messages.Add("Документы (Новая информация о документах): " + message);
                    clientVisit.NewDocument.Messages.Add(message);
                }

                /*
                if (clientVisit.OldDocument.DocumentTypeID != 12 &&
                    clientVisit.OldDocument.DocumentTypeID != 10 &&
                    clientVisit.OldDocument.DocumentTypeID != 25)
                {
                    isValid = false;
                    this.Messages.Add("Документы (Старая информация о документах): " + message);
                    clientVisit.OldDocument.Messages.Add(message);
                }
                */
            }

            //Иностранные граждане / лица без гражданства, временно проживающие на территории РФ code = 99
            //Паспорт иностранного гражданина  id = 9
            //Документ иностранного гражданина id = 21 
            //документ лица без гражданства)   id = 22
            //(разрешение на временное проживание) id = 23
            if (clientVisit.NewClientInfo.Category == ClientCategoryItems.Where(a => a.Code == "99").Select(b => b.Id).FirstOrDefault())
            {

                message = string.Format("При выбранной категории '{0}' необходимо выбрать другой тип документа: {1}, {2} или {3}.",
                                    ClientCategoryItems.Where(a => a.Code == "99").Select(b => b.Name).FirstOrDefault(),
                                    DocumentTypeRefItems.Where(a => a.Id == 9).Select(b => b.Name).FirstOrDefault(),
                                    DocumentTypeRefItems.Where(a => a.Id == 21).Select(b => b.Name).FirstOrDefault(),
                                    DocumentTypeRefItems.Where(a => a.Id == 22).Select(b => b.Name).FirstOrDefault());
                message1 = string.Format("Необходимо выбрать другой тип документа: {0}. ",
                                    DocumentTypeRefItems.Where(a => a.Id == 23).Select(b => b.Name).FirstOrDefault());

                if ((clientVisit.NewDocument.DocumentTypeID == 9 ||
                    clientVisit.NewDocument.DocumentTypeID == 21 ||
                    clientVisit.NewDocument.DocumentTypeID == 22) &&
                    clientVisit.NewForeignDocument.DocumentTypeID == 23)
                {
                    //nothing
                }
                else
                {
                    if (!(clientVisit.NewDocument.DocumentTypeID == 9 || clientVisit.NewDocument.DocumentTypeID == 21 || clientVisit.NewDocument.DocumentTypeID == 22))
                    {
                        isValid = false;
                        this.Messages.Add("Документы (Новая информация о документах): " + message);
                        clientVisit.NewDocument.Messages.Add(message);
                    }

                    if (!(clientVisit.NewForeignDocument.DocumentTypeID == 23))
                    {
                        isValid = false;
                        this.Messages.Add("Документы (Дополнительный документ иностранного гражданина): " + message1);
                        clientVisit.NewForeignDocument.Messages.Add(message1);
                    }
                }

                /*
                if (clientVisit.OldDocument.DocumentTypeID != 11 &&
                    clientVisit.OldDocument.DocumentTypeID != 22 &&
                    clientVisit.OldDocument.DocumentTypeID != 23)
                {
                    isValid = false;
                    this.Messages.Add("Документы (Старая информация о документах): " + message);
                    clientVisit.OldDocument.Messages.Add(message);
                }
                */
            }
        }

        private void ValidateScenario(ClientVisitSaveDataModel clientVisit)
        {

            List<ReferenceItem> items = referenceBusinessLogic.GetReferencesList(Constants.ScenarioRef);
            foreach (var item in items)
            {
                item.Code = item.Code.Trim();
            }

            //Дата выдачи полиса: поле может быть не пусто только при сценарии POK
            var IdFromBD = items.Where(a => a.Code == "POK").Select(b => b.Id).FirstOrDefault();
            if (IdFromBD == clientVisit.ScenarioId && clientVisit.IssueDate == null)
            {
                isValid = false;
                this.Messages.Add(string.Format("Выбран сценарий {0} => необходимо заполнить поле 'Дата выдачи полиса'",
                    clientVisit.Scenaries.Where(a => a.Value == clientVisit.ScenarioId.ToString()).FirstOrDefault().Text));
            }

            // Временное свидетельство: обязательно при сценариях
            //NB, DP, CR, RI, RT
            var tempId = items.Where(a => a.Code == "NB" ||
                                          a.Code == "DP" ||
                                          a.Code == "CR" ||
                                          a.Code == "RI" ||
                                          a.Code == "RT"
                                     ).Select(b => b.Id).Where(c => c == clientVisit.ScenarioId).FirstOrDefault();
            if (tempId != 0 && clientVisit.TemporaryPolicyNumber == null)
            {
                isValid = false;
                this.Messages.Add(string.Format("Выбран сценарий NB, DP, CR, RI, RT => необходимо заполнить поле 'Временное свидетельство'"));
            }

            //Поля из раздела 'Старая информация о клиенте' должны быть обязательно заполнены для сценариев  CR, RI, RT.
            //Фамилия, Имя, ДР, Пол, Место рождения, Гражданство, Категория клиента - должны быть не пусты при этих сценариях.
            tempId = items.Where(a => a.Code == "CR" ||
                                      a.Code == "RI" ||
                                      a.Code == "RT"
                                 ).Select(b => b.Id).Where(c => c == clientVisit.ScenarioId).FirstOrDefault();
            if (tempId != 0 && (
               string.IsNullOrEmpty(clientVisit.OldClientInfo.Lastname) ||
               (string.IsNullOrEmpty(clientVisit.OldClientInfo.Firstname) && clientVisit.OldClientInfo.FirstnameTypeId != CodFioClassifier.NoSecondnameFirstname.Id) ||
               clientVisit.OldClientInfo.Birthday == null ||
               string.IsNullOrEmpty(clientVisit.OldClientInfo.Sex) ||
               string.IsNullOrEmpty(clientVisit.OldClientInfo.Birthplace) ||
               clientVisit.OldClientInfo.Citizenship == null ||
               clientVisit.OldClientInfo.Category == null
               ))
            {
                string str = "";
                if (string.IsNullOrEmpty(clientVisit.OldClientInfo.Lastname)) str = str + "Фамилия, ";
                if (string.IsNullOrEmpty(clientVisit.OldClientInfo.Firstname) && clientVisit.OldClientInfo.FirstnameTypeId != CodFioClassifier.NoSecondnameFirstname.Id) str = str + "Имя, ";
                if (clientVisit.OldClientInfo.Birthday == null) str = str + "Дата рождения, ";
                if (string.IsNullOrEmpty(clientVisit.OldClientInfo.Sex)) str = str + "Пол, ";
                if (string.IsNullOrEmpty(clientVisit.OldClientInfo.Birthplace)) str = str + "Место рождения, ";
                if (clientVisit.OldClientInfo.Citizenship == null) str = str + "Гражданство, ";
                if (clientVisit.OldClientInfo.Category == null) str = str + "Категория клиента, ";
                str = str.TrimEnd(',', ' ');
                str = str + ".";

                isValid = false;
                this.Messages.Add("Информация о клиенте (Старая информация о застрахованном): " +
                    string.Format("При выбранном сценарии CR, RI, RT необходимо заполнить поля: {0}", str));
                clientVisit.OldClientInfo.Messages.Add(string.Format("При выбранном сценарии CR, RI, RT необходимо заполнить поля {0}", str));
            }

            List<ReferenceItem> PolicyTypeRefItems = referenceBusinessLogic.GetReferencesList(Constants.PolicyTypeRef);
            //Для сценариев  DP,CD,CR,CI,RI,CT,RT  в Данные полисов(Старый полис) возможно выбрать только "ЕНП(П)", "УЭК(К)", "ЭлПолис(Э)"
            var tempPolicyTypeId = PolicyTypeRefItems.Where(a => a.Code == "П" ||
                                                            a.Code == "К" ||
                                                            a.Code == "Э"
                                                       ).Select(b => b.Id).Where(c => c == clientVisit.OldPolicy.PolicyTypeId).FirstOrDefault();
            tempId = items.Where(a => a.Code == "DP" ||
                                      a.Code == "CD" ||
                                      a.Code == "CR" ||
                                      a.Code == "CI" ||
                                      a.Code == "RI" ||
                                      a.Code == "CT" ||
                                      a.Code == "RT"
                                ).Select(b => b.Id).Where(c => c == clientVisit.ScenarioId).FirstOrDefault();


            if (tempId != 0 && tempPolicyTypeId == 0)
            {
                isValid = false;
                this.Messages.Add("Данные полисов (Старый полис): " +
                    string.Format("Выбран сценарий DP,CD,CR,CI,RI,CT или RT => необходимо выбрать правильный тип полиса {0}, {1} или {2}.",
                    PolicyTypeRefItems.Where(a => a.Code == "П").Select(b => b.Name).FirstOrDefault(),
                    PolicyTypeRefItems.Where(a => a.Code == "К").Select(b => b.Name).FirstOrDefault(),
                    PolicyTypeRefItems.Where(a => a.Code == "Э").Select(b => b.Name).FirstOrDefault()));
                clientVisit.OldPolicy.Messages.Add(string.Format("Выбран сценарий DP,CD,CR,CI,RI,CT или RT => необходимо выбрать правильный тип полиса {0}, {1} или {2}.",
                    PolicyTypeRefItems.Where(a => a.Code == "П").Select(b => b.Name).FirstOrDefault(),
                    PolicyTypeRefItems.Where(a => a.Code == "К").Select(b => b.Name).FirstOrDefault(),
                    PolicyTypeRefItems.Where(a => a.Code == "Э").Select(b => b.Name).FirstOrDefault()));
            }

            //Для сценария CD, "Старая информация о Документы" -> документах" обязательно к заполнению поля:
            //Тип документа, Серия, Номер, Дата выдачи, Кем выдан.
            tempId = items.Where(a => a.Code == "CD"
                                ).Select(b => b.Id).Where(c => c == clientVisit.ScenarioId).FirstOrDefault();
            if (tempId != 0 && (clientVisit.OldDocument.DocumentTypeID == null ||
                                string.IsNullOrEmpty(clientVisit.OldDocument.Series) ||
                                string.IsNullOrEmpty(clientVisit.OldDocument.Number) ||
                                clientVisit.OldDocument.IssueDate == null ||
                                string.IsNullOrEmpty(clientVisit.OldDocument.IssueDepartment)
                ))
            {
                isValid = false;
                this.Messages.Add("Документы (Старая информация о документах): " +
                    "Выбран сценарий CD => необходимо заполнить поля.");

                if (clientVisit.OldDocument.DocumentTypeID == null)
                {
                    clientVisit.OldDocument.Messages.Add("Выбран сценарий CD => необходимо указать [Тип документа]");
                }
                if (string.IsNullOrEmpty(clientVisit.OldDocument.Series))
                {
                    clientVisit.OldDocument.Messages.Add("Выбран сценарий CD => необходимо указать [Серия]");
                }
                if (string.IsNullOrEmpty(clientVisit.OldDocument.Number))
                {
                    clientVisit.OldDocument.Messages.Add("Выбран сценарий CD => необходимо указать [Номер]");
                }
                if (clientVisit.OldDocument.IssueDate == null)
                {
                    clientVisit.OldDocument.Messages.Add("Выбран сценарий CD => необходимо указать [Дата выдачи]");
                }
                if (string.IsNullOrEmpty(clientVisit.OldDocument.IssueDepartment))
                {
                    clientVisit.OldDocument.Messages.Add("Выбран сценарий CD => необходимо указать [Кем выдан]");
                }
            }

            //Для сценария "NB - Первичное изготовление ЕНП" возможно "значение не выбрано", "полис старого образца".
            if (clientVisit.OldPolicy.PolicyTypeId != null)
            {
                tempPolicyTypeId = PolicyTypeRefItems.Where(a => a.Code == "С"
                                                           ).Select(b => b.Id).Where(c => c == clientVisit.OldPolicy.PolicyTypeId).FirstOrDefault();
                tempId = items.Where(a => a.Code == "NB"
                                    ).Select(b => b.Id).Where(c => c == clientVisit.ScenarioId).FirstOrDefault();

                if (tempId != 0 && tempPolicyTypeId == 0)
                {
                    isValid = false;
                    this.Messages.Add("Данные полисов (Старый полис): " +
                        string.Format("Выбран сценарий NB => необходимо выбрать правильный тип полиса 'значение не выбрано' или {0}.",
                        PolicyTypeRefItems.Where(a => a.Code == "С").Select(b => b.Name).FirstOrDefault()));
                    clientVisit.OldPolicy.Messages.Add(string.Format("Выбран сценарий NB => необходимо выбрать правильный тип полиса 'значение не выбрано' или {0}.",
                        PolicyTypeRefItems.Where(a => a.Code == "С").Select(b => b.Name).FirstOrDefault()));
                }
            }

            //Номер партии полиса - обязательно для Типа полиса "полис старого образца" Код С
            var OldPolicyIdId = PolicyTypeRefItems.Where(a => a.Code == "С").Select(b => b.Id).FirstOrDefault();
            if (clientVisit.OldPolicy.PolicyTypeId == OldPolicyIdId && string.IsNullOrEmpty(clientVisit.OldPolicy.Number))
            {
                isValid = false;
                this.Messages.Add("Данные полисов (Старый полис): " +
                    "Введите номер полиса.");
                clientVisit.OldPolicy.Messages.Add("Введите номер полиса.");
            }


            //ЕНП обязательно для сценариев DP,CD,CR,CI,RI,CT,RT
            if (clientVisit.OldPolicy.PolicyTypeId != null)
            {
                tempId = items.Where(a => a.Code == "DP" ||
                      a.Code == "CD" ||
                      a.Code == "CR" ||
                      a.Code == "CI" ||
                      a.Code == "RI" ||
                      a.Code == "CT" ||
                      a.Code == "RT"
                ).Select(b => b.Id).Where(c => c == clientVisit.ScenarioId).FirstOrDefault();
                if (tempId != 0 && string.IsNullOrEmpty(clientVisit.OldPolicy.UnifiedPolicyNumber))
                {
                    this.Messages.Add("Данные полисов (Старый полис): " +
                        "Для сценариев DP,CD,CR,CI,RI,CT,RT необходимо указать ЕНП.");
                    clientVisit.OldPolicy.Messages.Add("Для сценариев DP,CD,CR,CI,RI,CT,RT необходимо указать ЕНП.");
                }
            }
        }

        private void ValidateCitizenship(ClientVisitSaveDataModel clientVisit)
        {
            List<ReferenceItem> ClientCategoryItems = referenceBusinessLogic.GetReferencesList(Constants.ClientCategoryRef);
            List<ReferenceItem> CitizenshipRefItems = referenceBusinessLogic.GetReferencesList(Constants.CitizenshipRef);
            var RussiaId = CitizenshipRefItems.Where(a => a.Code == "RUS").Select(b => b.Id).FirstOrDefault();

            //При выборе категорий Граждане РФ Москва, Граждане РФ, зарег. в иных субъектах РФ, БОМЖ,
            //возможное значение - только Россия
            var tempId = ClientCategoryItems.Where(a => a.Code == "00" ||
                                                        a.Code == "77"
                                                   ).Select(b => b.Id).Where(c => c == clientVisit.NewClientInfo.Category).FirstOrDefault();
            if (tempId != 0 && clientVisit.NewClientInfo.Citizenship != RussiaId)
            {
                isValid = false;
                this.Messages.Add("Информация о клиенте:");
                this.Messages.Add("Для категорий 'граждане РФ' возможно только гражданство РОССИЯ. Выберите правильный вариант.");
                clientVisit.NewClientInfo.Messages.Add("Для категорий 'граждане РФ' возможно только гражданство РОССИЯ. Выберите правильный вариант.");
            }
            /*
            tempId = ClientCategoryItems.Where(a => a.Code == "00" ||
                                                    a.Code == "77"
                                               ).Select(b => b.Id).Where(c => c == clientVisit.OldClientInfo.Category).FirstOrDefault();
            if (tempId != 0 && clientVisit.OldClientInfo.Citizenship != RussiaId)
            {
                isValid = false;
                this.Messages.Add("Информация о клиенте: " +
                    "Для категорий 'граждане РФ' возможно только гражданство РОССИЯ. Выберите правильный вариант.");
                clientVisit.OldClientInfo.Messages.Add("Для категорий 'граждане РФ' возможно только гражданство РОССИЯ. Выберите правильный вариант.");
            }
            */


            //Для категорий 99, 45, 73 - гражданство Россия недоступно
            tempId = ClientCategoryItems.Where(a => a.Code == "99" ||
                                                    a.Code == "45" ||
                                                    a.Code == "73"
                                               ).Select(b => b.Id).Where(c => c == clientVisit.NewClientInfo.Category).FirstOrDefault();
            if (tempId != 0 && clientVisit.NewClientInfo.Citizenship == RussiaId)
            {
                isValid = false;
                this.Messages.Add("Информация о клиенте: " +
                    "Для категорий 'беженцы/переселенцы', 'иностранные граждане' гражданство РОССИЯ не доступно. Выберите другой вариант.");
                clientVisit.NewClientInfo.Messages.Add("Для категорий 'беженцы/переселенцы', 'иностранные граждане' гражданство РОССИЯ не доступно. Выберите другой вариант.");
            }
            /*
            tempId = ClientCategoryItems.Where(a => a.Code == "99" ||
                                                    a.Code == "45" ||
                                                    a.Code == "73"
                                               ).Select(b => b.Id).Where(c => c == clientVisit.OldClientInfo.Category).FirstOrDefault();
            if (tempId != 0 && clientVisit.OldClientInfo.Citizenship == RussiaId)
            {
                isValid = false;
                this.Messages.Add("Информация о клиенте: " + 
                    "Для категорий 'беженцы/переселенцы', 'иностранные граждане' гражданство РОССИЯ не доступно. Выберите другой вариант.");
                clientVisit.OldClientInfo.Messages.Add("Для категорий 'беженцы/переселенцы', 'иностранные граждане' гражданство РОССИЯ не доступно. Выберите другой вариант.");
            }
            */
        }

        private void ValidateBSO(ClientVisitSaveDataModel clientVisit)
        {
            BSO bso = bsoLogic.BSO_GetByNumber(clientVisit.TemporaryPolicyNumber);
            if (string.IsNullOrEmpty(clientVisit.TemporaryPolicyNumber))
            {
                return;
            }
            if (bso != null)
            {

                //Проверка БСО, чтобы не могли ввести 2 разных БСО в одно обращение
                try
                {
                    if (clientVisit.ClientId.HasValue)
                    {
                        Client client = clientBusinessLogic.Client_Get(new Entities.User { Roles = { Entities.Core.Role.Administrator } }, clientVisit.ClientId.Value);
                        ClientVisitInfo VG = client.Visits.Where(a => a.VisitGroupId == clientVisit.VisitGroupId).Where(b => !string.IsNullOrEmpty(b.TemporaryPolicyNumber)).FirstOrDefault();
                        if (VG != null)
                        {
                            BSO BSOinVG = bsoLogic.BSO_GetByNumber(VG.TemporaryPolicyNumber);
                            if (client == null || VG == null || string.IsNullOrEmpty(VG.TemporaryPolicyNumber) || BSOinVG == null)
                            {
                                //nothing
                            }
                            else
                            {
                                if (BSOinVG.TemporaryPolicyNumber != clientVisit.TemporaryPolicyNumber)
                                {
                                    if (BSOinVG.Status.Id == (long)ListBSOStatusID.OnClient)
                                    {
                                        isValidNotCritical = false;
                                        this.MessagesNotCritical.Add(
                                            "ВНИМАНИЕ! На данное обращение уже выдан БСО №[" + BSOinVG.TemporaryPolicyNumber + "]." + Environment.NewLine +
                                              "При необходимости создайте новое обращение. Если бланк испорчен, отметьте это."
                                            );
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {//если что-то пошло не так - забиваем на проверку
                    isValidNotCritical = false;
                    this.MessagesNotCritical.Add("Что-то при проверке БСО произошло, сообщите об этой ошибке администратору!");
                }


                //Если поле 'Временное свидетельство' не пусто - делаем автозаполнение поля 'Дата окончания временного свидетельства'
                //30 рабочих дней ~ 45 календарных
                if (clientVisit.TemporaryPolicyExpirationDate == null && clientVisit.TemporaryPolicyDate != null)
                {
                    clientVisit.TemporaryPolicyExpirationDate = ((DateTime)clientVisit.TemporaryPolicyDate).AddDays(45);
                    //this.Messages.Add("Обнаружено, что при наличии ВС поле 'Дата окончания действия ВС' не заполнено!");
                    //this.Messages.Add(string.Format("Автоматически заполнено датой {0}", ((DateTime)clientVisit.TemporaryPolicyExpirationDate).ToShortDateString()));
                }

                //Поле Форма полиса должно быть заполнено, если поле "Временное свидетельство" не пустое
                if (clientVisit.CarrierId == null)
                {
                    isValid = false;
                    this.Messages.Add("Общая информация по обращению: " + "Форма полиса должно быть заполнено при введённом ВС.");
                }

                ///Происходит редактирование заявки, без изменения бсо(вс)
                ///то ничего не проверяем
                if (bso.Status.Id == (long)ListBSOStatusID.OnClient
                    && bso.TemporaryPolicyNumber == clientVisit.TemporaryPolicyNumber
                    && bso.DeliveryPointId == clientVisit.DeliveryPointId
                    && bso.VisitGroupId == clientVisit.VisitGroupId)
                {
                    return;
                }

                ///Если выдан клиенту
                if (bso.Status.Id == (long)ListBSOStatusID.OnClient)
                {   //и совпадают ID обращения
                    if (bso.VisitGroupId == clientVisit.VisitGroupId
                        && (bso.TemporaryPolicyNumber != clientVisit.TemporaryPolicyNumber || bso.DeliveryPointId != clientVisit.DeliveryPointId))
                    {
                        isValid = false;
                        this.Messages.Add(string.Format("БСО [{0}] уже выдан этому клиенту! ID обращения: {1}. Место выдачи: {2}. Исправьте данные в заявке.",
                            bso.TemporaryPolicyNumber, bso.VisitGroupId, bso.DeliveryPoint));
                        return;
                    }
                    else
                    {
                        isValid = false;
                        this.Messages.Add(string.Format("БСО [{0}] уже выдан клиенту! ID обращения: {1}. Место выдачи: {2}. Введите другой номер БСО.",
                            bso.TemporaryPolicyNumber, bso.VisitGroupId, bso.DeliveryPoint));
                        return;
                    }
                }

                ///Проверка статуса "На точке"
                if (bso.Status.Id != (long)ListBSOStatusID.OnDelivery)
                {
                    isValid = false;
                    this.Messages.Add(string.Format("Статус данного БСО [{0}] должен быть: На точке. Сейчас он в статусе [{1}]",
                        bso.TemporaryPolicyNumber, bso.Status.Name));
                }
                ///на этом ли пункте/точке находится свободный БСО?
                if (bso.DeliveryPointId != clientVisit.DeliveryPointId)
                {
                    isValid = false;
                    if (string.IsNullOrEmpty(bso.DeliveryPoint))
                    {
                        bso.DeliveryPoint = "неизвестно";
                    }
                    this.Messages.Add(string.Format("Данный БСО [{0}] находится на точке: {1}.",
                        bso.TemporaryPolicyNumber, bso.DeliveryPoint));
                }
                ///не привязан ли этот БСО уже к другому обращению?
                if (bso.VisitGroupId != clientVisit.VisitGroupId && bso.VisitGroupId.HasValue)
                {
                    isValid = false;
                    this.Messages.Add("Этот БСО № " + clientVisit.TemporaryPolicyNumber + " уже привязан к обращению ID " + bso.VisitGroupId + ".\n");
                }
                if (bso.VisitGroupId != null)
                {
                    isValid = false;
                    this.Messages.Add("Данный БСО № " + bso.TemporaryPolicyNumber + " уже привязан к обращению ID " + bso.VisitGroupId + ".\n");
                }
            }
            else
            {
                isValid = false;
                this.Messages.Add("Вводимый вами номер БСО (Временное свидетельство) " + clientVisit.TemporaryPolicyNumber + " отсутствует в базе.");
            }
        }

        private void ValidatePhone(ClientVisitSaveDataModel clientVisit)
        {
            if (string.IsNullOrEmpty(clientVisit.Phone))
            {
                isValid = false;
                this.Messages.Add("Не заполнен номер телефона");
            }
        }

        private void ValidateDeliveryCenter(ClientVisitSaveDataModel clientVisit)
        {
            if (!clientVisit.DeliveryCenterId.HasValue ||
                clientVisit.DeliveryCenterId.HasValue && clientVisit.DeliveryCenterId.Value == 0)
            {
                isValid = false;
                this.Messages.Add("Не заполнен пункт регистрации");
            }
        }

        private void ValidateBirthCertificate(ClientVisitSaveDataModel clientVisit)
        {
            DocumentModel document = clientVisit.NewDocument;
            if (document.DocumentTypeID == Constants.BirthCertificateDocumentId)
            {
                DateTime birthday = clientVisit.NewClientInfo.Birthday.Value;
                DateTime expireDate = GetBirthCertificateExpireDate(birthday);
                if (expireDate < (clientVisit.CreateDate ?? DateTime.Now))
                {
                    isValid = false;
                    document.Messages.Add("Истек срок действия свидетельства о рождении");
                }
            }
        }

        private void ValidatePassportAndBirthday(ClientVisitSaveDataModel clientVisit)
        {
            DateTime birthday = clientVisit.NewClientInfo.Birthday.Value;
            int age = GetAge(birthday, clientVisit.CreateDate);
            List<ReferenceItem> ApplicationMethodRefItems = referenceBusinessLogic.GetReferencesList(Constants.ApplicationMethodRef);

            DocumentModel document = clientVisit.NewDocument;
            if (document.DocumentTypeID == Constants.RussianFederationPassportDocumentId || document.DocumentTypeID == Constants.USSRPasportDocumentId)
            {

                DateTime? issueDate = clientVisit.NewDocument.IssueDate;
                if (age < 14)
                {
                    isValid = false;
                    this.Messages.Add("Документы: " +
                        string.Format("Возраст клиента ({0}) не предполагает использование паспорта.", age));
                    document.Messages.Add(string.Format("Возраст клиента ({0}) не предполагает использование паспорта", age));
                }
                //DateTime expireDate = GetPassportExpireDate(birthday, clientVisit.CreateDate);
                //if (issueDate > expireDate)
                //{
                //    isValid = false;
                //    document.Messages.Add(string.Format("Истёр срок действия паспорта: {0}", expireDate.ToString("dd.MM.yyyy")));
                //}
            }

            //Для клиентов с возрастом моложе 18 лет, не может быть выбран способ подачи "лично". Code = 1 в ApplicationMethodRef.
            if (age < 18 && clientVisit.ApplicationMethodId == ApplicationMethodRefItems.Where(a => a.Code == "1").Select(b => b.Id).FirstOrDefault())
            {
                isValid = false;
                this.Messages.Add("Общая информация по обращению: " +
                    string.Format("Возраст клиента ({0}) не предполагает способ подачи заявления 'лично'.", age));
            }

            //Заполнение всех полей раздела Представитель обязательно для способа подачи заявления "ч/з представителя".
            //В остальных случаях не заполняется.
            //через представителя Code = 2
            if (clientVisit.ApplicationMethodId == ApplicationMethodRefItems.Where(a => a.Code == "2").Select(b => b.Id).FirstOrDefault())
            {
                if (clientVisit.Representative.RepresentativeTypeId == null ||
                    string.IsNullOrEmpty(clientVisit.Representative.Lastname) ||
                    string.IsNullOrEmpty(clientVisit.Representative.Firstname) ||
                    clientVisit.Representative.Birthday == null ||
                    clientVisit.Representative.DocumentTypeId == null ||
                    string.IsNullOrEmpty(clientVisit.Representative.Series) ||
                    string.IsNullOrEmpty(clientVisit.Representative.Number) ||
                    clientVisit.Representative.IssueDate == null ||
                    string.IsNullOrEmpty(clientVisit.Representative.IssueDepartment)
                    )
                {
                    string str = "";
                    if (clientVisit.Representative.RepresentativeTypeId == null) str = str + "Тип представителя, ";
                    if (string.IsNullOrEmpty(clientVisit.Representative.Lastname)) str = str + "Фамилия, ";
                    if (string.IsNullOrEmpty(clientVisit.Representative.Firstname)) str = str + "Имя, ";
                    if (clientVisit.Representative.Birthday == null) str = str + "Дата рождения, ";
                    if (clientVisit.Representative.DocumentTypeId == null) str = str + "Тип документа, ";
                    if (string.IsNullOrEmpty(clientVisit.Representative.Series)) str = str + "Серия документа, ";
                    if (string.IsNullOrEmpty(clientVisit.Representative.Number)) str = str + "Номер документа, ";
                    if (clientVisit.Representative.IssueDate == null) str = str + "Дата выдачи, ";
                    if (string.IsNullOrEmpty(clientVisit.Representative.IssueDepartment)) str = str + "Кем выдан, ";
                    str = str.TrimEnd(',', ' ');
                    str = str + ".";

                    isValid = false;
                    this.Messages.Add("Представитель: " + string.Format("Необходимо заполнить поля: {0}", str));
                    clientVisit.Representative.Messages.Add(string.Format("Необходимо заполнить поля {0}", str));
                }

            }

            //      Должно быть на граждан РФ
            //1)	дата рождения < 14 лет + 30 дней – смена св. о рождении на паспорт
            //2)	14 лет < дата рождения < 20 лет + 30 дней - замена паспорта
            //3)	в 45 лет + 30 дней - замена паспорта
            List<ReferenceItem> CitizenshipRefItems = referenceBusinessLogic.GetReferencesList(Constants.CitizenshipRef);
            var RussiaId = CitizenshipRefItems.Where(a => a.Code == "RUS").Select(b => b.Id).FirstOrDefault();
            if (clientVisit.NewClientInfo.Citizenship == RussiaId &&
                    (clientVisit.NewDocument.DocumentTypeID == Constants.BirthCertificateDocumentId ||
                     clientVisit.NewDocument.DocumentTypeID == Constants.RussianFederationPassportDocumentId
                ))
            {
                var flag = CountDay(DateTime.Now, clientVisit.NewClientInfo.Birthday.Value.AddYears(14).AddDays(30));
                if (flag < 0)
                {
                    //еще не исполнилось 14
                    //nothing
                }
                else
                {
                    //14+
                    if (clientVisit.NewDocument.DocumentTypeID == Constants.BirthCertificateDocumentId)
                    {
                        //необходима смена свидетельства на паспорт
                        isValidNotCritical = false;
                        this.MessagesNotCritical.Add("Необходима замена свидетельства о рождении на паспорт.");
                        clientVisit.NewClientInfo.MessagesNotCritical.Add("Необходима замена свидетельства о рождении на паспорт.");
                    }

                    if (clientVisit.NewDocument.DocumentTypeID == Constants.RussianFederationPassportDocumentId)
                    {
                        flag = CountDay(DateTime.Now, clientVisit.NewClientInfo.Birthday.Value.AddYears(20).AddDays(30));
                        if (flag < 0)
                        {
                            //еще не исполнилось 20
                            //nothing
                        }
                        else
                        {
                            flag = CountDay(DateTime.Now, clientVisit.NewClientInfo.Birthday.Value.AddYears(45).AddDays(30));
                            if (flag < 0)
                            {
                                //20 - 45
                                //Ключевая точка
                                var KeyPoint = clientVisit.NewClientInfo.Birthday.Value.AddYears(20);
                                if (clientVisit.NewDocument.IssueDate.HasValue && clientVisit.NewDocument.IssueDate.Value < KeyPoint)
                                {
                                    //необходима 1ая замена паспорта
                                    isValidNotCritical = false;
                                    this.MessagesNotCritical.Add("Клиенту необходима замена паспорта, т.к. ему больше 20 лет.");
                                    clientVisit.NewDocument.MessagesNotCritical.Add("Клиенту необходима замена паспорта, т.к. ему больше 20 лет.");
                                }
                            }
                            else
                            {
                                //уже больше 45
                                //Ключевая точка
                                var KeyPoint = clientVisit.NewClientInfo.Birthday.Value.AddYears(45);
                                if (clientVisit.NewDocument.IssueDate.HasValue && clientVisit.NewDocument.IssueDate.Value < KeyPoint)
                                {
                                    //необходима 2ая замена паспорта
                                    isValidNotCritical = false;
                                    this.MessagesNotCritical.Add("Клиенту необходима замена паспорта, т.к. ему больше 45 лет.");
                                    clientVisit.NewDocument.MessagesNotCritical.Add("Клиенту необходима замена паспорта, т.к. ему больше 45 лет.");
                                }
                            }
                        }
                    }

                }
            }

            //2 последние цифры серии должны быть равны году выдачи паспорта РФ
            //серия 34 15
            //дата выдачи 2015 или 2016 год
            //иначе выдавать предупреждение
            if (clientVisit.NewDocument.DocumentTypeID == Constants.RussianFederationPassportDocumentId)
            {
                if (!string.IsNullOrEmpty(clientVisit.NewDocument.Series) && clientVisit.NewDocument.IssueDate.HasValue)
                {
                    try
                    {
                        var flag = clientVisit.NewDocument.Series.Trim()
                          .EndsWith(clientVisit.NewDocument.IssueDate.Value.Year.ToString().Substring(2, 2));

                        if (!(flag))
                        {
                            isValidNotCritical = false;
                            this.MessagesNotCritical.Add("Две последние цифры серии паспорта не равны году выдачи паспорта.");
                            clientVisit.NewDocument.MessagesNotCritical.Add("Две последние цифры серии паспорта не равны году выдачи паспорта.");
                        }
                    }
                    catch
                    {
                        isValidNotCritical = false;
                        this.MessagesNotCritical.Add("Невозможно распарсить серию паспорта и/или год рождения. Сообщите об этой ошибке администратору.");
                        clientVisit.NewDocument.MessagesNotCritical.Add("Невозможно распарсить серию паспорта и/или год рождения. Сообщите об этой ошибке администратору.");
                    }
                }
            }

        }

        /// <summary>
        /// Кол-во дней между двумя датами.
        /// Из date вычитаем date2 и возвращаем разницу в днях
        /// </summary>
        /// <param name="date"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        private static int CountDay(DateTime date, DateTime? date2 = null)
        {
            if (!date2.HasValue)
            {
                date2 = DateTime.Now;
            }
            TimeSpan time = (DateTime)date - (DateTime)date2;
            return time.Days;
        }

        private static DateTime GetPassportExpireDate(DateTime birthday, DateTime? createDate)
        {
            int age = GetAge(birthday, createDate);

            if (age >= 20 && age < 45)
            {
                return birthday.AddYears(20).AddDays(30);
            }

            if (age >= 45)
            {
                return birthday.AddYears(45).AddDays(30);
            }

            return DateTime.MaxValue;
        }

        private static DateTime GetBirthCertificateExpireDate(DateTime birthday)
        {
            return birthday.AddYears(14).AddDays(30);
        }

        private static DateTime GetPassportExpireDate(DateTime birthday)
        {
            return birthday.AddYears(14).AddDays(30);
        }

        private static int GetAge(DateTime birthday, DateTime? createDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthday.Year;
            if (birthday > today.AddYears(-age)) age--;
            return age;
        }
    }
}