using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMInsurance.Entities.Core
{
    public enum ListBSOStatusID : long
    {
        /// <summary>
        /// Данные статусы соответствуют StatusID в dbo.BSOStatusRef
        /// </summary>
        OnStorage = 1,          //На складе ЦО
        OnDelivery = 2,         //На точке
        OnClient = 3,           //ВС выдано клиенту
        GotoStorage = 4,        //Передан в ЦО (не поврежден)
        FailOnResponsible = 5,  //Испорчен (испорчен, утерян, похищен), на ответственном
        FailGotoStorage = 6,    //Испорчен, передан в ЦО
        FailOnStorage = 7,      //Испорчен, на складе в ЦО
        Delete = 8,             //Утилизирован
        OnResponsible = 10      //На ответственном
    }

    public class BSOStatusValidator
    {
        public static List<long> ListAllBSOStatus = new List<long>() {
            (long)ListBSOStatusID.OnStorage,
            (long)ListBSOStatusID.OnDelivery,
            (long)ListBSOStatusID.OnClient,
            (long)ListBSOStatusID.GotoStorage,
            (long)ListBSOStatusID.FailOnResponsible,
            (long)ListBSOStatusID.FailGotoStorage,
            (long)ListBSOStatusID.FailOnStorage,
            (long)ListBSOStatusID.Delete,
            (long)ListBSOStatusID.OnResponsible
        };

        /// <summary>
        /// Валидатор смены статуса БСО
        /// Если мы возвращаем null, то валидация успешна.
        /// </summary>
        /// <param name="currentStatus"></param>
        /// <param name="newStatus"></param>
        /// <returns></returns>
        public static string Validator(long currentStatus, long newStatus, User currentUser = null)
        {
            string message = ValidatorStatus(currentStatus, newStatus);
            //если валидация Статуса была успешна проверяем Роль
            if (string.IsNullOrEmpty(message))
            {
                message = ValidatorRole(newStatus, currentUser);
            }
            return message;
        }

        //возвращает список доступных для изменения статусов + сам текущий статус
        //т.е. если statusId = 1, то вернет список {1,2,8}
        public static List<long> GetAvailableBSOStatus(long? statusId)
        {
            if (statusId == (long)ListBSOStatusID.OnStorage) return new List<long>() { (long)ListBSOStatusID.OnStorage, (long)ListBSOStatusID.OnDelivery, (long)ListBSOStatusID.OnResponsible };
            if (statusId == (long)ListBSOStatusID.OnDelivery) return new List<long>() { (long)ListBSOStatusID.OnDelivery, (long)ListBSOStatusID.OnClient, (long)ListBSOStatusID.OnResponsible, (long)ListBSOStatusID.FailOnResponsible };
            if (statusId == (long)ListBSOStatusID.OnClient) return new List<long>() { (long)ListBSOStatusID.OnClient, (long)ListBSOStatusID.FailOnResponsible };
            if (statusId == (long)ListBSOStatusID.GotoStorage) return new List<long>() { (long)ListBSOStatusID.GotoStorage, (long)ListBSOStatusID.OnStorage };
            if (statusId == (long)ListBSOStatusID.FailOnResponsible) return new List<long>() { (long)ListBSOStatusID.FailOnResponsible, (long)ListBSOStatusID.FailGotoStorage };
            if (statusId == (long)ListBSOStatusID.FailGotoStorage) return new List<long>() { (long)ListBSOStatusID.FailGotoStorage, (long)ListBSOStatusID.FailOnStorage };
            if (statusId == (long)ListBSOStatusID.FailOnStorage) return new List<long>() { (long)ListBSOStatusID.FailOnStorage, (long)ListBSOStatusID.Delete };
            if (statusId == (long)ListBSOStatusID.Delete) return new List<long>() { (long)ListBSOStatusID.Delete };
            if (statusId == (long)ListBSOStatusID.OnResponsible) return new List<long>() { (long)ListBSOStatusID.OnResponsible, (long)ListBSOStatusID.OnDelivery, (long)ListBSOStatusID.GotoStorage };

            return ListAllBSOStatus;
        }

        //Валидатор статуса
        private static string ValidatorStatus(long currentStatus, long newStatus)
        {
            //по умолчанию валидация провалена - fail
            string message = "Не верная смена статуса. ";

            /// <summary>
            ///Статус не изменился
            /// </summary>
            if (currentStatus == newStatus) return message = null;

            /// <summary>
            ///На складе ЦО
            /// </summary>
            if (currentStatus == (long)ListBSOStatusID.OnStorage)
            {
                if (newStatus == (long)ListBSOStatusID.OnDelivery) return message = null;
                if (newStatus == (long)ListBSOStatusID.OnResponsible) return message = null;
                message = message + "Статус [На складе ЦО] можно изменить только -> [На точке], [На ответственном]";
                return message;
            }

            /// <summary>
            ///На точке
            /// </summary>
            if (currentStatus == (long)ListBSOStatusID.OnDelivery)
            {
                if (newStatus == (long)ListBSOStatusID.OnClient) return message = null;
                if (newStatus == (long)ListBSOStatusID.OnResponsible) return message = null;
                if (newStatus == (long)ListBSOStatusID.FailOnResponsible) return message = null;
                message = message + "Статус [На точке] можно изменить только на -> [Выдан клиенту], [На ответственном], [Испорчен, на ответственном]";
                return message;
            }

            /// <summary>
            ///Выдан клиенту
            /// </summary>
            if (currentStatus == (long)ListBSOStatusID.OnClient)
            {
                //Если выдан клиенту, его можно сменить только на Испорчен у ответственного
                if (newStatus == (long)ListBSOStatusID.FailOnResponsible) return message = null;
                message = message + "Статус [Выдан клиенту] можно изменить только на -> [Испорчен, на ответственном]";
                return message;
            }

            /// <summary>
            ///Передан в ЦО (не поврежден)
            /// </summary>
            if (currentStatus == (long)ListBSOStatusID.GotoStorage)
            {
                if (newStatus == (long)ListBSOStatusID.OnStorage) return message = null;
                message = message + "Статус [Передан в ЦО] можно изменить только на -> [На складе ЦО]";
                return message;
            }

            /// <summary>
            ///Испорчен (испорчен, утерян, похищен), на ответственном
            /// </summary>
            if (currentStatus == (long)ListBSOStatusID.FailOnResponsible)
            {
                if (newStatus == (long)ListBSOStatusID.FailGotoStorage) return message = null;
                message = message + "Статус [Испорчен, на ответственном] можно изменить только на -> [Испорчен, передан в ЦО]";
                return message;
            }

            /// <summary>
            ///Испорчен, передан в ЦО
            /// </summary>
            if (currentStatus == (long)ListBSOStatusID.FailGotoStorage)
            {
                if (newStatus == (long)ListBSOStatusID.FailOnStorage) return message = null;
                message = message + "Статус [Испорчен, передан в ЦО] можно изменить только на -> [Испорчен, на складе в ЦО]";
                return message;
            }

            /// <summary>
            ///Испорчен (на складе ЦО)
            /// </summary>
            if (currentStatus == (long)ListBSOStatusID.FailOnStorage)
            {
                if (newStatus == (long)ListBSOStatusID.Delete) return message = null;
                message = message + "Статус [Испорчен, на складе в ЦО] можно изменить только на -> [Утилизирован]";
                return message;
            }
            /// <summary>
            ///Утилизирован
            /// </summary>
            if (currentStatus == (long)ListBSOStatusID.Delete)
            {
                //БСО утилизирован
                message = message + "Статус [Утилизирован] изменить нельзя.";
                return message;
            }

            /// <summary>
            ///На ответственном
            /// </summary>
            if (currentStatus == (long)ListBSOStatusID.OnResponsible)
            {
                if (newStatus == (long)ListBSOStatusID.OnDelivery) return message = null;
                if (newStatus == (long)ListBSOStatusID.GotoStorage) return message = null;
                message = message + "Статус [На ответственном] можно изменить только на -> [На точке], [Передан в ЦО]";
                return message;
            }
            return message;
        }

        //Валидатор роли
        private static string ValidatorRole(long newStatus, User currentUser)
        {
            //по умолчанию валидация провалена - fail
            string message = "Не верная смена статуса. ";

            //если в валидатор не передали CurrenUser - то ничего не проверяем
            if (currentUser == null) return null;

            //ищем максимальный "допуск" у пользователя в ранжировании, указанном в Role
            Role realRole = Role.GetRealRole(currentUser);


            //Администраторам разрешены любые действия
            if (realRole == Role.Administrator || realRole == Role.AdministratorBSO)
            {
                return null;
            }

            //Есть исключения, когда один человек и Ответственный и Регистратор
            //тогда он совмещает 2 роли сразу
            if (currentUser.Roles.Where(a => a.Id == Role.Registrator.Id || a.Id == Role.ResponsibleBSO.Id).Count() == 2)
            {
                if (newStatus == (long)ListBSOStatusID.OnDelivery) return null;
                if (newStatus == (long)ListBSOStatusID.GotoStorage) return null;
                if (newStatus == (long)ListBSOStatusID.FailGotoStorage) return null;
                if (newStatus == (long)ListBSOStatusID.OnClient) return null;
                if (newStatus == (long)ListBSOStatusID.FailOnResponsible) return null;
                if (newStatus == (long)ListBSOStatusID.OnResponsible) return null;
                message = message + string.Format("В вашей роли [{0}] и [{1}] можно изменить статус только на [На ответственном], [На точке], [Передан в ЦО], [Испорчен, передан в ЦО], [Выдан клиенту], [Испорчен (утерян, похищен) у ответственного].",
                    Role.ResponsibleBSO.Description, Role.Registrator.Description);
                return message;
            }

            //'Ответственный' может сменить только на На ответственном, На точке, Передан в ЦО, Испорчен (утерян, похищен), передан в ЦО
            if (realRole == Role.ResponsibleBSO)
            {
                if (newStatus == (long)ListBSOStatusID.OnDelivery) return null;
                if (newStatus == (long)ListBSOStatusID.GotoStorage) return null;
                if (newStatus == (long)ListBSOStatusID.FailGotoStorage) return null;
                if (newStatus == (long)ListBSOStatusID.OnResponsible) return null;
                message = message + string.Format("В вашей роли [{0}] можно изменить статус только на [На ответственном], [На точке], [Передан в ЦО], [Испорчен, передан в ЦО].",
                    Role.ResponsibleBSO.Description);
                return message;
            }

            //'Регистратор' может сменить только на На ответственном, Выдан клиенту, Испорчен (утерян, похищен) у ответственного
            if (realRole == Role.Registrator)
            {
                if (newStatus == (long)ListBSOStatusID.OnClient) return null;
                if (newStatus == (long)ListBSOStatusID.FailOnResponsible) return null;
                if (newStatus == (long)ListBSOStatusID.OnResponsible) return null;
                message = message + string.Format("В вашей роли [{0}] можно изменить статус только на [На ответственном], [Выдан клиенту], [Испорчен (утерян, похищен) у ответственного].",
                    Role.Registrator.Description);
                return message;
            }
            return message;
        }
    }
}
