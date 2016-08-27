using OMInsurance.BusinessLogic;
using OMInsurance.Entities.Core;
using OMInsurance.Entities;
using OMInsurance.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OMInsurance.WebApps
{
    public static class ReferencesProvider
    {
        private static IReferenceBusinessLogic referenceBusinessLogic = new ReferenceBusinessLogic();
        private static IUserBusinessLogic userBusinessLogic = new UserBusinessLogic();
        private static ConcurrentDictionary<string, List<ReferenceItem>> referencesPool = new ConcurrentDictionary<string, List<ReferenceItem>>();
        private static HashSet<DateTime> holidays;
        private static HashSet<DateTime> exceptionalWorkingDays;

        public static List<SelectListItem> GetReferences(string referenceName, string selectedValue = null, bool withDefaultEmpty = false)
        {
            List<ReferenceItem> items = GetReferenceItems(referenceName);
            //CodFioClassifier - не сортировать
            return GetSelectListItems(referenceName, selectedValue, withDefaultEmpty, items);
        }

        public static List<ReferenceItem> GetReferenceItems(string referenceName)
        {
            List<ReferenceItem> items;
            if (referencesPool.ContainsKey(referenceName))
            {
                items = referencesPool[referenceName];
            }
            else
            {
                if (referenceName == Constants.DeliveryCenterForOperatorRef)
                {
                    items = referenceBusinessLogic.GetDeliveryCenterList()
                        .Where(item => !string.IsNullOrEmpty(item.DisplayName))
                        .Select(item => item as ReferenceItem).ToList();
                }
                else
                {
                    items = referenceBusinessLogic.GetReferencesList(referenceName);
                }
                referencesPool.AddOrUpdate(referenceName, items, (key, value) => value);
            }
            return items;
        }

        public static List<ReferenceUniversalItem> GetUniversalReference(string referenceName)
        {
            List<ReferenceUniversalItem> listRefItemUniversal = referenceBusinessLogic.GetUniversalList(referenceName);
            return listRefItemUniversal;
        }

        

        public static List<SelectListItem> GetRoles()
        {
            return userBusinessLogic.Role_GetList().Select(role => new SelectListItem() { Text = role.Description, Value = role.Id.ToString() }).ToList();
        }

        public static List<SelectListItem> GetUsers(string selectedValue = null, bool withDefaultEmpty = false)
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            List<Entities.User> listUser = userBusinessLogic.Find("");
            listUser.Sort((a, b) => a.Lastname.CompareTo(b.Lastname));

            if (withDefaultEmpty)
            {
                listItems.Add(new SelectListItem()
                {
                    Text = "Значение не выбрано",
                    Value = ""
                });
            }

            foreach (var user in listUser)
            {
                listItems.Add(new SelectListItem()
                {
                    Text = user.Lastname + " " + user.Firstname + " " + user.Secondname,
                    Value = user.Id.ToString(),
                    Selected = !string.IsNullOrEmpty(selectedValue) && selectedValue == user.Id.ToString()
                });
            }
            return listItems;
        }

        public static List<SelectListItem> GetReferencesDisplayName(bool withDefaultEmpty = false)
        {
            List<SelectListItem> listItems = new List<SelectListItem>();

            if (withDefaultEmpty)
            {
                listItems.Add(new SelectListItem()
                {
                    Text = "Значение не выбрано",
                    Value = ""
                });
            }

            List<ReferenceUniversalItem> listRefUniversal = new List<ReferenceUniversalItem>();
            listRefUniversal = referenceBusinessLogic.GetUniversalList(Constants.ReferenceRef);
            listRefUniversal.Sort((a, b) => a.Name.CompareTo(b.Name));

            listItems.AddRange(listRefUniversal.Select(item => new SelectListItem()
            {
                Text = item.DisplayName + " [" + item.Name + "]",
                Value = item.Id.ToString()
            }).ToList());

            return listItems;
        }

        /// <summary>
        /// Получаем список Точек прикреплённых к ответственному.
        /// </summary>
        /// <param name="responsibleId"></param>
        /// <param name="withDefaultEmpty"></param>
        /// <returns></returns>
        public static List<SelectListItem> GetDeliveryPointsForResponsibleBSO(long userId, bool withDefaultEmpty = false)
        {
            List<ReferenceUniversalItem> listRefItem = referenceBusinessLogic.GetUniversalList(Constants.DeliveryPointRef).FindAll(a => a.DeliveryPointHeadId == userId);
            List<SelectListItem> listItems = new List<SelectListItem>();

            if(listRefItem == null || listRefItem.Count() < 1) 
            {
                return listItems;
            }
            else
            {
                if (withDefaultEmpty)
                {
                    listItems.Add(new SelectListItem()
                    {
                        Text = "Значение не выбрано",
                        Value = ""
                    });
                }

                listRefItem.Sort((a, b) => a.Name.CompareTo(b.Name));
                listItems.AddRange(listRefItem.Select(item => new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                }).ToList());
            }

            return listItems;
        }

        /// <summary>
        /// Получаем список Экспертов
        /// </summary>
        /// <param name="withDefaultEmpty"></param>
        /// <returns></returns>
        public static List<SelectListItem> GetExpert(bool withDefaultEmpty = false)
        {
            List<ReferenceUniversalItem> listRefItem = referenceBusinessLogic.GetUniversalList(Constants.ClientPretensionExpert);
            List<SelectListItem> listItems = new List<SelectListItem>();

            if (listRefItem == null || listRefItem.Count() < 1)
            {
                return listItems;
            }
            else
            {
                if (withDefaultEmpty)
                {
                    listItems.Add(new SelectListItem()
                    {
                        Text = "Значение не выбрано",
                        Value = ""
                    });
                }

                listRefItem.Sort((a, b) => a.Lastname.CompareTo(b.Lastname));
                listItems.AddRange(listRefItem.Select(item => new SelectListItem()
                {
                    Text = item.Lastname + " " + item.Firstname + " " + item.Secondname,
                    Value = item.Id.ToString()
                }).ToList());
            }
            return listItems;
        }

        /// <summary>
        /// Список кодов дефектов
        /// </summary>
        /// <param name="withDefaultEmpty"></param>
        /// <returns></returns>
        public static List<SelectListItem> GetCodeDefect(bool withDefaultEmpty = false)
        {
            List<ReferenceUniversalItem> listRefItem = referenceBusinessLogic.GetUniversalList(Constants.Defect);
            List<SelectListItem> listItems = new List<SelectListItem>();

            if (listRefItem == null || listRefItem.Count() < 1)
            {
                return listItems;
            }
            else
            {
                if (withDefaultEmpty)
                {
                    listItems.Add(new SelectListItem()
                    {
                        Text = "Значение не выбрано",
                        Value = ""
                    });
                }

                listRefItem.Sort((a, b) => a.Code.CompareTo(b.Code));
                listItems.AddRange(listRefItem.Select(item => new SelectListItem()
                {
                    Text = item.Code + " [" + item.ErrCode + "] " + item.Name,
                    Value = item.Id.ToString()
                }).ToList());
            }
            return listItems;
        }


        public static List<SelectListItem> GetListClientAcquisitionEmployee(string selectedValue = null, bool withDefaultEmpty = false)
        {
            List<SelectListItem> listItems = new List<SelectListItem>();

            if (withDefaultEmpty)
            {
                listItems.Add(new SelectListItem()
                {
                    Text = "Значение не выбрано",
                    Value = ""
                });
            }

            listItems.AddRange(userBusinessLogic.ClientAcquisitionEmployee_Get(null).Select(user => new SelectListItem()
            {
                Text = user.Lastname + " " + user.Firstname + " " + user.Secondname,
                Value = user.Lastname + " " + user.Firstname + " " + user.Secondname,
                Selected = !string.IsNullOrEmpty(selectedValue) && selectedValue == user.UserId.ToString()
            }).ToList());
            return listItems;
        }


        public static HashSet<DateTime> GetHolidays()
        {
            if (holidays == null)
            {
                holidays = referenceBusinessLogic.GetHolidays(null);
            }

            return holidays;
        }

        public static HashSet<DateTime> GetExceptionalWorkingDays()
        {
            if (exceptionalWorkingDays == null)
            {
                exceptionalWorkingDays = referenceBusinessLogic.GetExceptionalWorkingDays(null);
            }
            return exceptionalWorkingDays;
        }

        public static List<SelectListItem> GetReferences(
            string referenceName,
            List<Role> roles,
            DateTime? date,
            string selectedValue = null,
            bool withDefaultEmpty = false)
        {
            List<ReferenceItem> references = GetReferenceItems(referenceName);
            references.Sort((a, b) => a.Name.CompareTo(b.Name));
            List<ReferenceItem> filteredReferences = new List<ReferenceItem>();

            foreach (var refItem in references)
            {
                if (date.HasValue && (refItem.EndDate < date || refItem.StartDate > date))
                {
                    continue;
                }

                if (roles != null && refItem.IsEnabledForRegistrator.HasValue
                    && !refItem.IsEnabledForRegistrator.Value
                    && roles.Contains(Role.Registrator)
                    && !roles.Contains(Role.Administrator)
                    && !roles.Contains(Role.OperatorSG))
                {
                    continue;
                }

                if (roles != null && refItem.IsEnabledForOperator.HasValue
                    && !refItem.IsEnabledForOperator.Value
                    && roles.Contains(Role.OperatorSG)
                    && !roles.Contains(Role.Administrator))
                {
                    continue;
                }

                filteredReferences.Add(refItem);
            }
            return GetSelectListItems(referenceName, selectedValue, withDefaultEmpty, filteredReferences);
        }

        private static List<SelectListItem> GetSelectListItems(string referenceName, string selectedValue, bool withDefaultEmpty, List<ReferenceItem> filteredReferences)
        {
            List<SelectListItem> listItems = new List<SelectListItem>();

            if (referenceName != Constants.CodFioClassifier)
            {
                filteredReferences.Sort((a, b) => a.Name.CompareTo(b.Name));
            }

            if (withDefaultEmpty)
            {
                listItems.Add(new SelectListItem()
                {
                    Text = "Значение не выбрано",
                    Value = ""
                });
            }

            //справочник "ScenarioRef" выводим с кодом Code
            if (referenceName == Constants.ScenarioRef)
            {
                listItems.AddRange(filteredReferences.Select(item => new SelectListItem()
                {
                    Text = String.Format("[ {0} ]  {1}", item.Code, item.Name),
                    Value = item.Id.ToString(),
                    Selected = !string.IsNullOrEmpty(selectedValue) && selectedValue == item.Code.ToString()
                }));
            }
            else //остальные справочники выводим, как обычно без кодов
            {
                listItems.AddRange(filteredReferences.Select(item => new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = !string.IsNullOrEmpty(selectedValue) && selectedValue == item.Code.ToString()
                }));
            }

            return listItems;
        }

    }
}