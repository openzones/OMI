using OMInsurance.BusinessLogic;
using OMInsurance.Entities.Core;
using OMInsurance.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OMInsurance.Entities;

namespace OMInsurance.WebApps.Models
{
    public static class StatusBSOProvider
    {
        private static IBSOBusinessLogic bso_bl = new BSOBusinessLogic();
        private static IUserBusinessLogic userBusinessLogic = new UserBusinessLogic();

        public static List<SelectListItem> GetBSOListStatus(bool withDefaultEmpty = false)
        {
            List<BSOStatusRef> listStatus = bso_bl.BSO_GetListStatus();
            List<SelectListItem> listBSOStatuses = new List<SelectListItem>();
            if (withDefaultEmpty)
            {
                listBSOStatuses.Add(new SelectListItem()
                {
                    Text = "Значение не выбрано",
                    Value = ""
                });
            }
            foreach (BSOStatusRef item in listStatus)
            {
                listBSOStatuses.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }

            return listBSOStatuses;
        }

        public static List<SelectListItem> GetAvailableBSOStatus(long? statusId, bool withDefaultEmpty = false)
        {
            List<BSOStatusRef> listStatusfromDB = bso_bl.BSO_GetListStatus();
            List<BSOStatusRef> listAvailable = new List<BSOStatusRef>();
            List<long> listLong = BSOStatusValidator.GetAvailableBSOStatus(statusId);
            foreach (long item in listLong)
            {
                listAvailable.Add(listStatusfromDB.Where(a => a.Id == item).FirstOrDefault());
            }

            List<SelectListItem> listBSOStatuses = new List<SelectListItem>();
            if (withDefaultEmpty)
            {
                listBSOStatuses.Add(new SelectListItem()
                {
                    Text = "Значение не выбрано",
                    Value = ""
                });
            }

            foreach (BSOStatusRef item in listAvailable)
            {
                listBSOStatuses.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }

            return listBSOStatuses;
        }

        public static List<SelectListItem> GetListBSOResponsibles(bool withDefaultEmpty = false)
        {
            Role role = new Role() { Id = 5 };
            List<User> listAllUser = userBusinessLogic.Find("");
            listAllUser.Sort((a, b) => a.Lastname.CompareTo(b.Lastname));

            List<User> listResponsible = new List<User>();
            foreach (var item in listAllUser)
            {
                foreach (var r in item.Roles)
                {
                    if (r.Id == 5)
                    {
                        listResponsible.Add(item);
                    }
                }
            }

            List<SelectListItem> listBSOResponsibles = new List<SelectListItem>();
            if (withDefaultEmpty)
            {
                listBSOResponsibles.Add(new SelectListItem()
                {
                    Text = "Значение не выбрано",
                    Value = ""
                });
            }

            foreach (User user in listResponsible)
            {
                listBSOResponsibles.Add(new SelectListItem()
                {
                    Value = user.Id.ToString(),
                    Text = user.Lastname + " " + user.Firstname.Remove(1) + ". " + user.Secondname.Remove(1) + "."
                });
            }
            return listBSOResponsibles;
        }
    }
}