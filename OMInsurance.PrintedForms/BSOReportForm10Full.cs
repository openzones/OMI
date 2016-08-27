using NPOI.SS.UserModel;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using OMInsurance.PrintedForms.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OMInsurance.PrintedForms
{
    public class BSOReportForm10Full : PrintedForm
    {
        private List<BSOInfo> ListBSOBeforeDate;
        private List<BSOInfo> ListBSOinRangeDate;
        private List<BSOInfo> ListBSOAllDate;

        //список выданных
        private List<BSOInfo> ListBSOGet;

        private List<BSOHistoryItem> ListHistory;
        List<string> ListDelivery;

        private DateTime DateTimeFrom;
        private DateTime DateTimeTo;
        private string DeliveryPoint;
        private string FIO;
        private string Position;

        #region Marks
        protected const string dateTimeConst = "$DateTime$";
        protected const string deliveryPointConst = "$DeliveryPoint$";
        protected const string FIOConst = "$FIO$";
        protected const string PositionConst = "$Position$";
        protected const string nowDateTimeConst = "$NowDateTime$";
        #endregion

        public BSOReportForm10Full(List<BSOHistoryItem> listHistory, List<BSOInfo> listBSOBeforeDate, List<BSOInfo> listBSOinRangeDate, List<BSOInfo> listBSOAllDate,
                                    List<string> listDelivery, DateTime dateFrom, DateTime dateTo, User currentUser)
        {
            ListBSOGet = new List<BSOInfo>();
            ListBSOinRangeDate = listBSOinRangeDate;

            ListBSOBeforeDate = listBSOBeforeDate;//new List<BSOInfo>();
            ListBSOAllDate = listBSOAllDate;
            ListHistory = listHistory;
            ListDelivery = listDelivery;
            DateTimeFrom = dateFrom;
            DateTimeTo = dateTo;
            FIO = currentUser.Fullname;
            Position = currentUser.Position;

            if (ListDelivery.Count > 0)
            {
                DeliveryPoint = string.Empty;
                foreach (var item in ListDelivery)
                {
                    DeliveryPoint = DeliveryPoint + " " + item;
                }
            }
            else
            {
                DeliveryPoint = "Все точки";
            }


            TemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Templates", "BSOReportForm10Full.xls");
        }

        protected override void Process(ISheet table)
        {
            int rowNumber = 16;
            int rowNumberDown = 27;

            string dateTime = string.Format("за отчетный период с {0} г. по {1} г.", DateTimeFrom.ToShortDateString(), DateTimeTo.ToShortDateString());
            table.FindCellByMacros(dateTimeConst).SetValue(dateTime);
            table.FindCellByMacros(deliveryPointConst).SetValue(DeliveryPoint);

            var ListHistoryGroupByBSO_IDs = ListHistory.GroupBy(a => a.Id);
            foreach (var item in ListHistoryGroupByBSO_IDs)
            {
                BSOHistoryItem temp = item.OrderBy(a => a.StatusDate).Where(b => b.Status.Id == (long)ListBSOStatusID.OnDelivery || b.Status.Id == (long)ListBSOStatusID.OnResponsible).LastOrDefault();

                if (temp != null)
                {
                    if (temp.StatusDate < DateTimeFrom)
                    {
                        BSOInfo bsoInfo = new BSOInfo()
                        {
                            TemporaryPolicyNumber = ListBSOinRangeDate.Where(a => a.Id == temp.Id).Select(b => b.TemporaryPolicyNumber).LastOrDefault(),
                            //TemporaryPolicyNumber = ListBSOAllDate.Where(a => a.Id == temp.Id).Select(b => b.TemporaryPolicyNumber).LastOrDefault(),
                            Status = new BSOStatusRef() { Id = temp.Status.Id }
                        };
                        if(!string.IsNullOrEmpty(bsoInfo.TemporaryPolicyNumber))
                            ListBSOBeforeDate.Add(bsoInfo);
                    }
                    else
                    {
                        BSOInfo bsoInfo = new BSOInfo()
                        {
                            TemporaryPolicyNumber = ListBSOinRangeDate.Where(a => a.Id == temp.Id).Select(b => b.TemporaryPolicyNumber).LastOrDefault(),
                            //TemporaryPolicyNumber = ListBSOAllDate.Where(a => a.Id == temp.Id).Select(b => b.TemporaryPolicyNumber).LastOrDefault(),
                            Status = new BSOStatusRef() { Id = temp.Status.Id }
                        };
                        if (!string.IsNullOrEmpty(bsoInfo.TemporaryPolicyNumber))
                            ListBSOGet.Add(bsoInfo);
                    }
                }
            }

            Dictionary<string, long> dicBefore = SortingAndGroup(ListBSOBeforeDate, new List<long>() { (long)ListBSOStatusID.OnDelivery, (long)ListBSOStatusID.OnResponsible });
            Dictionary<string, long> dicRange = SortingAndGroup(ListBSOGet, new List<long>() { (long)ListBSOStatusID.OnDelivery, (long)ListBSOStatusID.OnResponsible });
            Dictionary<string, long> dicAll = SortingAndGroup(ListBSOAllDate, new List<long>() { (long)ListBSOStatusID.OnDelivery, (long)ListBSOStatusID.OnResponsible });
            Dictionary<string, long> dicOnClient = SortingAndGroup(ListBSOinRangeDate, new List<long>() { (long)ListBSOStatusID.OnClient });
            Dictionary<string, long> dicFail = SortingAndGroup(ListBSOinRangeDate, new List<long>() { (long)ListBSOStatusID.FailOnStorage, (long)ListBSOStatusID.FailOnResponsible, (long)ListBSOStatusID.FailGotoStorage });

            //определяем в каком словаре максимальное кол-во элементов, для того чтоб насоздавать строки
            List<int> countDictionary = new List<int>();
            countDictionary.Add(dicBefore == null ? 0 : dicBefore.Count);
            countDictionary.Add(dicRange == null ? 0 : dicRange.Count);
            countDictionary.Add(dicAll == null ? 0 : dicAll.Count);
            countDictionary.Add(dicOnClient == null ? 0 : dicOnClient.Count);
            countDictionary.Add(dicFail == null ? 0 : dicFail.Count);
            var max = countDictionary.Max() / 2;

            //если много данных - двигаем последние строчки еще ниже
            if (max > 10)
            {
                for (int i = 0; i < 10; i++)
                {
                    table.CopyRow(i + rowNumberDown, i + rowNumberDown + max);
                }
            }

            ICellStyle style = table.Workbook.CreateCellStyle();
            style.BorderRight = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;

            List<IRow> rows = new List<IRow>();
            for (int i = 0; i < countDictionary.Max() / 2; i++)
            {
                IRow row = table.CreateRow(i + rowNumber);
                for (int j = 0; j <= 14; j++)
                {
                    row.CreateCell(j).CellStyle = style;
                }
                rows.Add(row);
            }

            Output(dicBefore, rows, 0);
            Output(dicRange, rows, 3);
            Output(dicOnClient, rows, 6);
            Output(dicFail, rows, 9);
            Output(dicAll, rows, 12);

            //Итого:
            IRow rowSum = table.CreateRow(max + rowNumber);
            rowSum.CreateCell(0).SetValue("Итого:");
            rowSum.CreateCell(2).SetCellFormula(string.Format("SUM(C{0}:C{1})", rowNumber + 1, rowNumber + max));
            rowSum.CreateCell(5).SetCellFormula(string.Format("SUM(F{0}:F{1})", rowNumber + 1, rowNumber + max));
            rowSum.CreateCell(8).SetCellFormula(string.Format("SUM(I{0}:I{1})", rowNumber + 1, rowNumber + max));
            rowSum.CreateCell(11).SetCellFormula(string.Format("SUM(L{0}:L{1})", rowNumber + 1, rowNumber + max));
            rowSum.CreateCell(14).SetCellFormula(string.Format("SUM(O{0}:O{1})", rowNumber + 1, rowNumber + max));

            table.FindCellByMacros(FIOConst).SetValue(FIO);
            table.FindCellByMacros(PositionConst).SetValue(Position);
            table.FindCellByMacros(nowDateTimeConst).SetValue(DateTime.Now.ToShortDateString());
        }

        //даем список БСО и список статусов по которым надо сгрупировать
        private Dictionary<string, long> SortingAndGroup(List<BSOInfo> listBso, List<long> listStatusId)
        {
            if (listBso == null) return null;
            //парсим все номера БСО в long
            List<long> listLong = new List<long>(listBso.Count);
            foreach (var elem in listBso)
            {
                foreach (long status in listStatusId)
                {
                    if (elem.Status.Id == status)
                    {
                        listLong.Add(long.Parse(elem.TemporaryPolicyNumber));
                    }
                }
            }

            //если список, который мы должны сгруппировать пуст, то выходим
            if (listLong.Count == 0) return null;
            //сортируем
            listLong.Sort((a, b) => a.CompareTo(b));
            //создаем сортированный список БСО
            List<string> listString = new List<string>(listLong.Count);
            foreach (long l in listLong)
            {
                listString.Add(string.Format("{0,9:D9}", l));
            }
            //группируем по диапазонам
            Dictionary<string, long> dic = new Dictionary<string, long>();
            long temp = listLong.FirstOrDefault();
            long count = 0;
            dic.Add(" " + listString.FirstOrDefault(), 0);
            for (int i = 0; i < listLong.Count; i++)
            {
                if (listLong[i] - temp > 1) { dic.Add(listString[i - 1], count); dic.Add(" " + listString[i], 0); count = 0; }
                temp = listLong[i];
                count++;
            }
            dic.Add(listString.LastOrDefault(), count);

            return dic;
        }

        private void Output(Dictionary<string, long> dic, List<IRow> rows, int position, ICellStyle style = null)
        {
            if (dic == null) return;

            for (int i = 0, j = 0; i < dic.Count / 2; i++, j = j + 2)
            {
                rows.ElementAt(i).GetCell(position).SetCellValue(dic.ElementAt(j).Key.Trim());
                rows.ElementAt(i).GetCell(position + 1).SetCellValue(dic.ElementAt(j + 1).Key);
                rows.ElementAt(i).GetCell(position + 2).SetCellValue(dic.ElementAt(j + 1).Value);
            }
        }

    }
}
