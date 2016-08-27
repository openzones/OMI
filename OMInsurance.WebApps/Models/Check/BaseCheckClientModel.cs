using OMInsurance.Entities;
using OMInsurance.Entities.Check;
using OMInsurance.Entities.Core;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Web.Mvc;
using System.Linq;

namespace OMInsurance.WebApps.Models
{
    public class BaseCheckClientModel
    {
        //список ClientId - которые скрыты из оображения
        public static List<long> ListHideClientId = new List<long>();
        public BaseCheckClientModel()
        {
            ListCheckClientModel = new List<CheckClientModel>();
            SearchCheckClient = new SearchCheckClientModel();
            ViewColumn = new ViewColumnModel();
            IsHideClientId = true;
            PageSize = 100;
        }

        [DisplayName("Скрывать отмеченные")]
        public bool IsHideClientId { get; set; }

        /// <summary>
        /// Кол-во клиентов кот. будем объединять
        /// </summary>
        [DisplayName("Клиентов в количестве")]
        public int CountMerge { get; set; }

        /// <summary>
        /// Кол-во возможных дубликатов
        /// </summary>
        public long CountDublicate { get; set; }

        /// <summary>
        /// Общее кол-во извлеченных записей из БД
        /// </summary>
        public long CountRow { get; set; }
        public List<CheckClientModel> ListCheckClientModel { get; set; }
        public SearchCheckClientModel SearchCheckClient { get; set; }
        public ViewColumnModel ViewColumn { get; set; }

        [DisplayName("Отображать")]
        public long PageSize { get; set; }


        /// <summary>
        /// Отмечаем скрытые, список которых находится в ListHideClientId
        /// </summary>
        /// <returns></returns>
        public BaseCheckClientModel SetHide()
        {
            var list = this.ListCheckClientModel.FindAll(a => ListHideClientId.Contains(a.Id));
            foreach(var item in list)
            {
                item.IsHide = true;
            }
            return this;
        }

        /// <summary>
        /// Определяем возможное кол-во дубликатов
        /// </summary>
        /// <param name="list"></param>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public static long SumDublicate(List<CheckClient> list, SearchCheckClientModel searchCriteria)
        {
            long sumDublicate = 0;
            if (list == null)
            {
                return sumDublicate;
            }
            var ch = list.FirstOrDefault();

            foreach (var item in list)
            {
                if (CheckCondition(ch, item, searchCriteria))
                {
                    //nothing
                }
                else
                {
                    sumDublicate++;
                    ch = item;
                }
            }
            return sumDublicate;
        }

        /// <summary>
        /// ищем клиентов, кот. надо объединить
        /// </summary>
        /// <param name="list"></param>
        /// <param name="searchCriteria"></param>
        /// <param name="countMerge"></param>
        /// <returns></returns>
        public Dictionary<long, List<long>> FindDuplicate(List<CheckClient> list, SearchCheckClientModel searchCriteria, long countMerge)
        {
            Dictionary<long, List<long>> dic = new Dictionary<long, List<long>>();

            if (list == null)
            {
                return dic;
            }

            var ch = list.FirstOrDefault();
            var key = list.FirstOrDefault().Id;
            List<long> listLong = new List<long>();
            dic.Add(key, listLong);
            foreach (var item in list)
            {
                if (CheckCondition(ch, item, searchCriteria))
                {
                    if (key != item.Id)
                    {
                        listLong.Add(item.Id);
                    }
                }
                else
                {
                    if (dic.Count >= countMerge)
                    {
                        //Если оказалось, что клиент в списке без дубликатов, то фильтруем/выбрасываем его
                        dic = dic.Where(a => a.Value.Count != 0).ToDictionary(b => b.Key, b => b.Value);
                        if(dic.Count >= countMerge)
                        {
                            return dic;
                        }
                    }

                    ch = item;
                    key = item.Id;
                    listLong = new List<long>();
                    dic.Add(key, listLong);
                }
            }

            //Если оказалось, что клиент в списке без дубликатов, то фильтруем/выбрасываем его
            dic = dic.Where(a => a.Value.Count != 0).ToDictionary(b => b.Key, b => b.Value);
            return dic;
        }

        private static bool CheckCondition(CheckClient ch, CheckClient item, SearchCheckClientModel searchCriteria)
        {
            //Закомментированный код = незакомменчиному
            //bool flagLastname = false;
            //bool flagFirstname = false;
            //...

            //if (searchCriteria.IsLastname && (ch.Lastname.ToLower() == item.Lastname.ToLower()))
            //{
            //    flagLastname = true;
            //}

            //if (searchCriteria.IsFirstname && (ch.Firstname.ToLower() == item.Firstname.ToLower()))
            //{
            //    flagFirstname = true;
            //}
            //...

            //if (!(searchCriteria.IsLastname ^ flagLastname) &&
            //    !(searchCriteria.IsFirstname ^ flagFirstname) &&
            //      ...
            //    )
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}

            if (string.IsNullOrEmpty(ch.Lastname)) ch.Lastname = string.Empty;
            if (string.IsNullOrEmpty(ch.Firstname)) ch.Firstname = string.Empty;
            if (string.IsNullOrEmpty(ch.Secondname)) ch.Secondname = string.Empty;
            if (string.IsNullOrEmpty(item.Lastname)) item.Lastname = string.Empty;
            if (string.IsNullOrEmpty(item.Firstname)) item.Firstname = string.Empty;
            if (string.IsNullOrEmpty(item.Secondname)) item.Secondname = string.Empty;

            if (
                !(searchCriteria.IsLastname ^ (searchCriteria.IsLastname && (ch.Lastname.ToLower() == item.Lastname.ToLower()))) &&
                !(searchCriteria.IsFirstname ^ (searchCriteria.IsFirstname && (ch.Firstname.ToLower() == item.Firstname.ToLower()))) &&
                !(searchCriteria.IsSecondname ^ (searchCriteria.IsSecondname && (ch.Secondname.ToLower() == item.Secondname.ToLower()))) &&
                !(searchCriteria.IsSex ^ (searchCriteria.IsSex && (ch.Sex == item.Sex))) &&
                !(searchCriteria.IsBirthday ^ (searchCriteria.IsBirthday && (ch.Birthday == item.Birthday))) &&
                !(searchCriteria.IsPolicySeries ^ (searchCriteria.IsPolicySeries && (ch.PolicySeries == item.PolicySeries))) &&
                !(searchCriteria.IsPolicyNumber ^ (searchCriteria.IsPolicyNumber && (ch.PolicyNumber == item.PolicyNumber))) &&
                !(searchCriteria.IsUnifiedPolicyNumber ^ (searchCriteria.IsUnifiedPolicyNumber && (ch.UnifiedPolicyNumber == item.UnifiedPolicyNumber))) &&
                !(searchCriteria.IsDocumentSeries ^ (searchCriteria.IsDocumentSeries && (ch.DocumentSeries == item.DocumentSeries))) &&
                !(searchCriteria.IsDocumentNumber ^ (searchCriteria.IsDocumentNumber && (ch.DocumentNumber == item.DocumentNumber)))
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}