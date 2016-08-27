using OMInsurance.DataAccess.DAO;
using OMInsurance.Entities;
using OMInsurance.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace OMInsurance.BusinessLogic
{
    public class NomernikBusinessLogic : INomernikBusinessLogic
    {
        public Nomernik.History NOMPHistory_Get()
        {
            return NomernikDao.Instance.NOMPHistory_Get();
        }

        public Nomernik.History STOPHistory_Get()
        {
            return NomernikDao.Instance.STOPHistory_Get();
        }

        public List<NOMP> GetDataFromNOMPdbf(string dbfFilePath, Nomernik.History nompHistory)
        {
            List<NOMP> dataFromDBF = NomernikDao.Instance.GetDataFromNOMPDbf(dbfFilePath);
            //добавляем нули, т.к.
            //в нашей базе номер старого полиса хранится  с нулями с начала строки, т.е. 0000000001
            foreach (var item in dataFromDBF)
            {
                if (!string.IsNullOrEmpty(item.N_CARD))
                {
                    if (item.N_CARD.ToString().Length > 0 && item.N_CARD.ToString().Length < 10)
                    {
                        while (item.N_CARD.ToString().Length != 10)
                        {
                            item.N_CARD = "0" + item.N_CARD;
                        }
                    }
                }
            }

            List<NOMP> dataFromDB = new List<NOMP>(dataFromDBF.Count);
            dataFromDB = NomernikDao.Instance.NOMP_Find(dataFromDBF);

            int j = 0;
            foreach (var elementDBF in dataFromDBF)
            {
                //группируем по ENP и N_CARD
                List<NOMP> group = new List<NOMP>();
                group.AddRange(dataFromDB.FindAll(a => a.ENP == elementDBF.ENP));
                group.AddRange(dataFromDB.FindAll(a => a.N_CARD == elementDBF.N_CARD));
                //ищем в группе ID !=0  Это значит, что запись уже есть в истории номерников и выбираем последнюю запись из этой подгруппы
                NOMP lastInNOMP = group.FindAll(a => a.Id != 0).OrderBy(b => b.Id).LastOrDefault();
                if (lastInNOMP != null)
                {
                    //обрабатываем в результаты 4.3 - данные не изменились и 4.4 - данные изменились
                    //флаг -> были ли изменения? По умолчанию - нет
                    bool flagChange = false;
                    if (elementDBF.S_CARD != lastInNOMP.S_CARD) flagChange = true;
                    if (elementDBF.N_CARD != lastInNOMP.N_CARD) flagChange = true;
                    if (elementDBF.VSN != lastInNOMP.VSN) flagChange = true;
                    if (elementDBF.LPU_ID != lastInNOMP.LPU_ID) flagChange = true;
                    if (elementDBF.DATE_IN != lastInNOMP.DATE_IN) flagChange = true;
                    if (elementDBF.SPOS != lastInNOMP.SPOS) flagChange = true;

                    if (flagChange)
                    {
                        elementDBF.Status = 4;
                    }
                    else
                    {
                        elementDBF.Status = 3;
                    }
                    elementDBF.ClientID = lastInNOMP.ClientID;
                }
                else
                {
                    if (group.Count > 0)
                    {
                        if (group.Count == 1)
                        {
                            //клиент найден, уникален, записей в таблицу NOMP еще не было  -> 4.5
                            elementDBF.Status = 5;
                            elementDBF.ClientID = group.FirstOrDefault().ClientID;
                        }

                        if (group.Count > 1)
                        {
                            //проверяем на уникальность
                            int countClientId = 1;
                            long? ClientId = group.FirstOrDefault().ClientID;
                            foreach (var elem in group)
                            {
                                if (elem.ClientID != ClientId)
                                {
                                    countClientId++;
                                }
                            }

                            //ClientId уникален? 
                            if (countClientId == 1)
                            {
                                NOMP temp = group.OrderBy(b => b.DATE_IN).LastOrDefault();
                                //уникален -> 4.5
                                elementDBF.Status = 5;
                                elementDBF.ClientID = temp.ClientID;
                            }
                            else
                            {
                                //в результаты говорим, что найденный у нас клиент не уникален 4.2
                                elementDBF.Status = 2;
                                //пытаемся понять: может все-таки это один клиент несмотря на то, что у него разные ClientID
                                //сравниваем фио и дату рождения
                                List<Nomernik.ClientShotInfo> listClient = NomernikDao.Instance.ClientsShotInfo_Get(group.Select(a => (long)a.ClientID).Distinct());
                                elementDBF.Comment = elementDBF.Comment + string.Format("Найдены {0} клиента(ов). ", listClient.Count());
                                elementDBF.Comment = elementDBF.Comment + "ClientID = ";
                                bool flag = true;
                                string firstname = listClient.FirstOrDefault().Firstname;
                                string secondname = listClient.FirstOrDefault().Secondname;
                                string lastname = listClient.FirstOrDefault().Lastname;
                                DateTime? birthday = listClient.FirstOrDefault().Birthday;
                                foreach (var el in listClient)
                                {
                                    elementDBF.Comment = elementDBF.Comment + string.Format("{0}, ", el.ClientID);
                                    if (el.Firstname != firstname) flag = false;
                                    if (el.Secondname != secondname) flag = false;
                                    if (el.Lastname != lastname) flag = false;
                                    if (el.Birthday != birthday) flag = false;

                                    firstname = el.Firstname;
                                    secondname = el.Secondname;
                                    lastname = el.Lastname;
                                    birthday = el.Birthday;
                                }
                                elementDBF.Comment = elementDBF.Comment.TrimEnd(',', ' ');
                                elementDBF.Comment = elementDBF.Comment + ". ";

                                if (flag)
                                {
                                    elementDBF.Comment = elementDBF.Comment + "Есть предположение, что это один клиент. ФИО и дата рождения совпадают.";
                                }
                                else
                                {
                                    elementDBF.Comment = elementDBF.Comment + "Это разные клиенты. ФИО и/или дата рождения не совпадают.";
                                }
                            }
                        }
                    }
                    else
                    {
                        //в результаты - клиент в системе не найден 4.1
                        elementDBF.Status = 1;
                    }
                }

                foreach (var elem in group)
                {
                    dataFromDB.Remove(elem);
                }
                j++;
            }

            //записать в БД надо только со статусом 4 и 5
            var items = dataFromDBF.Where(i => (i.Status == 4 || i.Status == 5));
            NomernikDao.Instance.NOMP_Save(items, nompHistory);

            return dataFromDBF;
        }

        public List<STOP> GetDataFromSTOPdbf(string dbfFilePath, Nomernik.History nompHistory)
        {
            List<STOP> dataFromDBF = NomernikDao.Instance.GetDataFromSTOPDbf(dbfFilePath);
            //добавляем нули, т.к.
            //в нашей базе номер старого полиса хранится  с нулями с начала строки, т.е. 0000000001
            foreach (var item in dataFromDBF)
            {
                if (!string.IsNullOrEmpty(item.N_CARD))
                {
                    if (item.N_CARD.ToString().Length > 0 && item.N_CARD.ToString().Length < 10)
                    {
                        while (item.N_CARD.ToString().Length != 10)
                        {
                            item.N_CARD = "0" + item.N_CARD;
                        }
                    }
                }
            }

            List<STOP> dataFromDB = new List<STOP>(dataFromDBF.Count);
            dataFromDB = NomernikDao.Instance.STOP_Find(dataFromDBF);

            int j = 0;
            foreach (var elementDBF in dataFromDBF)
            {
                //группируем по ENP и по N_CARD
                List<STOP> group = new List<STOP>();
                group.AddRange(dataFromDB.FindAll(a => a.ENP == elementDBF.ENP));
                group.AddRange(dataFromDB.FindAll(a => a.N_CARD == elementDBF.N_CARD));
                //ищем в группе ID !=0  Это значит, что запись уже есть в истории номерников и выбираем последнюю запись из этой подгруппы
                STOP lastInNOMP = group.FindAll(a => a.Id != 0).OrderBy(b => b.Id).LastOrDefault();
                if (lastInNOMP != null)
                {
                    //обрабатываем в результаты 4.3 - данные не изменились и 4.4 - данные изменились
                    //флаг -> были ли изменения? По умолчанию - нет
                    bool flagChange = false;
                    if (elementDBF.SCENARIO != lastInNOMP.SCENARIO) flagChange = true;
                    if (elementDBF.S_CARD != lastInNOMP.S_CARD) flagChange = true;
                    if (elementDBF.N_CARD != lastInNOMP.N_CARD) flagChange = true;
                    if (elementDBF.VSN != lastInNOMP.VSN) flagChange = true;
                    if (elementDBF.QZ != lastInNOMP.QZ) flagChange = true;
                    if (elementDBF.DATE_END != lastInNOMP.DATE_END) flagChange = true;
                    if (elementDBF.DATE_ARC != lastInNOMP.DATE_ARC) flagChange = true;
                    if (elementDBF.IST != lastInNOMP.IST) flagChange = true;

                    if (flagChange)
                    {
                        elementDBF.Status = 4;
                    }
                    else
                    {
                        elementDBF.Status = 3;
                    }
                    elementDBF.ClientID = lastInNOMP.ClientID;
                }
                else
                {
                    if (group.Count > 0)
                    {
                        if (group.Count == 1)
                        {
                            //клиент найден, уникален, записей в таблицу STOP еще не было  -> 4.5
                            elementDBF.Status = 5;
                            elementDBF.ClientID = group.FirstOrDefault().ClientID;
                        }

                        if (group.Count > 1)
                        {
                            //проверяем на уникальность
                            int countClientId = 1;
                            long? ClientId = group.FirstOrDefault().ClientID;
                            foreach (var elem in group)
                            {
                                if (elem.ClientID != ClientId)
                                {
                                    countClientId++;
                                }
                            }

                            //ClientId уникален? 
                            if (countClientId == 1)
                            {
                                STOP temp = group.OrderBy(b => b.DATE_ARC).LastOrDefault();
                                //уникален -> 4.5
                                elementDBF.Status = 5;
                                elementDBF.ClientID = temp.ClientID;
                            }
                            else
                            {
                                //в результаты говорим, что найденный у нас клиент не уникален 4.2
                                elementDBF.Status = 2;
                                //пытаемся понять: может все-таки это один клиент несмотря на то, что у него разные ClientID
                                //сравниваем фио и дату рождения
                                List<Nomernik.ClientShotInfo> listClient = NomernikDao.Instance.ClientsShotInfo_Get(group.Select(a => (long)a.ClientID).Distinct());
                                elementDBF.Comment = elementDBF.Comment + string.Format("Найдены {0} клиента(ов). ", listClient.Count());
                                elementDBF.Comment = elementDBF.Comment + "ClientID = ";
                                bool flag = true;
                                string firstname = listClient.FirstOrDefault().Firstname;
                                string secondname = listClient.FirstOrDefault().Secondname;
                                string lastname = listClient.FirstOrDefault().Lastname;
                                DateTime? birthday = listClient.FirstOrDefault().Birthday;
                                foreach (var el in listClient)
                                {
                                    elementDBF.Comment = elementDBF.Comment + string.Format("{0}, ", el.ClientID);
                                    if (el.Firstname != firstname) flag = false;
                                    if (el.Secondname != secondname) flag = false;
                                    if (el.Lastname != lastname) flag = false;
                                    if (el.Birthday != birthday) flag = false;

                                    firstname = el.Firstname;
                                    secondname = el.Secondname;
                                    lastname = el.Lastname;
                                    birthday = el.Birthday;
                                }
                                elementDBF.Comment = elementDBF.Comment.TrimEnd(',', ' ');
                                elementDBF.Comment = elementDBF.Comment + ". ";

                                if (flag)
                                {
                                    elementDBF.Comment = elementDBF.Comment + "Есть предположение, что это один клиент. ФИО и дата рождения совпадают.";
                                }
                                else
                                {
                                    elementDBF.Comment = elementDBF.Comment + "Это разные клиенты. ФИО и/или дата рождения не совпадают.";
                                }
                            }
                        }
                    }
                    else
                    {
                        //в результаты - клиент в системе не найден 4.1
                        elementDBF.Status = 1;
                    }
                }

                foreach (var elem in group)
                {
                    dataFromDB.Remove(elem);
                }
                j++;
            }

            //записать в БД надо только со статусом 4 и 5
            var items = dataFromDBF.Where(i => (i.Status == 4 || i.Status == 5));
            NomernikDao.Instance.STOP_Save(items, nompHistory);

            return dataFromDBF;
        }


        /// <summary>
        /// Получаем общее кол-во записей(строк) в dbf
        /// </summary>
        /// <param name="dbfFilePath"></param>
        /// <returns></returns>
        public long? GetAllRowCount(string dbfFilePath)
        {
            return NomernikDao.Instance.GetAllRowCount(dbfFilePath);
        }

        /// <summary>
        /// Получаем кол-во записей(строк) в файле dbf только по нашей компании
        /// </summary>
        /// <param name="dbfFilePath"></param>
        /// <returns></returns>
        public long? GetOurRowCount(string dbfFilePath, string param)
        {
            return NomernikDao.Instance.GetOurRowCount(dbfFilePath, param);
        }

        public List<NomernikForClient> NomernikClientNOMP_Get(long ClientID)
        {
            return NomernikDao.Instance.NomernikClientNOMP_Get(ClientID);
        }

        public List<NomernikForClient> NomernikClientSTOP_Get(long ClientID)
        {
            return NomernikDao.Instance.NomernikClientSTOP_Get(ClientID);
        }

    }
}
