using NPOI.SS.UserModel;
using OMInsurance.Entities;
using OMInsurance.PrintedForms.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OMInsurance.PrintedForms
{
    public class BSOInvoice : PrintedForm
    {
        private List<string> ListBsoNumber;
        private string FIO;
        private string DeliveryCenter;

        #region Marks
        protected const string deliveryCenterConst = "$DeliveryCenter$";
        protected const string nowDateTimeConst = "$NowDataTime$";
        protected const string FIOConst = "$FIO$";
        protected const string numbersConst = "$Numbers$";
        protected const string Count = "$Count$";
        protected const string SUM = "$SUM$";
        #endregion

        public BSOInvoice(List<string> listBsoNumber, string fio, string deliveryCenter)
        {
            ListBsoNumber = listBsoNumber;
            FIO = fio;
            DeliveryCenter = deliveryCenter;
            TemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Templates", "BSOInvoice.xls");
        }

        protected override void Process(ISheet table)
        {
            int rowFIO = table.FindCellByMacros(FIOConst).RowIndex;
            int rowNumbers = table.FindCellByMacros(numbersConst).RowIndex;
            int colNumbers = table.FindCellByMacros(numbersConst).ColumnIndex;
            int rowPrice = table.FindCellByMacros(SUM).RowIndex;
            double price;
            try
            {
                IRow r = table.GetRow(rowPrice);
                price = r.GetCell(table.FindCellByMacros(SUM).ColumnIndex - 1).NumericCellValue;
            }
            catch
            {
                price = 0.0;
            }
            
            table.FindCellByMacros(deliveryCenterConst).SetValue(DeliveryCenter);
            table.FindCellByMacros(deliveryCenterConst).SetValue(DeliveryCenter);
            table.FindCellByMacros(nowDateTimeConst).SetValue(DateTime.Now.ToShortDateString());
            table.FindCellByMacros(FIOConst).SetValue(FIO);
            table.FindCellByMacros(Count).SetValue(ListBsoNumber.Count.ToString());
            table.FindCellByMacros(Count).SetValue(ListBsoNumber.Count.ToString());
            int rowIndex = table.FindCellByMacros(SUM).RowIndex;
            int columnIndex = table.FindCellByMacros(numbersConst).ColumnIndex;

            if(ListBsoNumber.Count <= 1)
            {
                table.FindCellByMacros(SUM).SetValue((price * ListBsoNumber.Count).ToString());
                rowIndex = table.FindCellByMacros(numbersConst).RowIndex;
                table.SetCellValue(rowIndex, columnIndex, ListBsoNumber.FirstOrDefault());
            }
            else
            {
                List<long> listLong = new List<long>(ListBsoNumber.Count);
                try
                {
                    //создаем список чисел (парсим номера БСО в long)
                    foreach (var elem in ListBsoNumber)
                    {
                        listLong.Add(long.Parse(elem));
                    }
                    //сортируем
                    listLong.Sort((a, b) => a.CompareTo(b));
                    //создаем сортированный список БСО
                    List<string> newListBsoNumber = new List<string>(listLong.Count);
                    foreach(long l in listLong)
                    {
                        newListBsoNumber.Add( string.Format("{0,9:D9}", l));
                    }

                    //группируем по диапазонам
                    Dictionary<string, long> dic = new Dictionary<string, long>();
                    long temp = listLong.FirstOrDefault();
                    long count = 0;
                    dic.Add("с    " + newListBsoNumber.FirstOrDefault(), 0);
                    for(int i = 0; i< listLong.Count; i++)
                    {
                        if (listLong[i] - temp > 1) { dic.Add("по "+ newListBsoNumber[i-1], count); dic.Add("с    " + newListBsoNumber[i], 0); count = 0; }
                        temp = listLong[i];
                        count++;
                    }
                    dic.Add("по " + newListBsoNumber.LastOrDefault(), count);

                    //если много данных - двигаем 2 последние строчки еще ниже
                    if (dic.Count > 10)
                    {
                        table.CopyRow(rowFIO, rowFIO+dic.Count-9);
                        table.CopyRow(rowFIO + 1, rowFIO + dic.Count-8);
                        table.CreateRow(rowFIO);
                        table.CreateRow(rowFIO+1);
                    }

                    ICellStyle style = table.Workbook.CreateCellStyle();
                    style.BorderRight = BorderStyle.Thin;
                    style.BorderBottom = BorderStyle.Thin;
                    style.BorderTop = BorderStyle.Thin;
                    style.BorderLeft = BorderStyle.Thin;

                    //выводим весь этот колохоз
                    int r = rowNumbers;
                    foreach (var element in dic)
                    {
                        IRow row = table.CreateRow(r);
                        row.CreateCell(0).CellStyle = style;
                        row.CreateCell(1).CellStyle = style;
                        row.CreateCell(2).CellStyle = style;
                        row.CreateCell(3).CellStyle = style;
                        row.CreateCell(4).CellStyle = style;
                        row.CreateCell(5).CellStyle = style;
                        row.CreateCell(6).CellStyle = style;
                        row.CreateCell(7).CellStyle = style;
                        row.CreateCell(8).CellStyle = style;
                        row.CreateCell(9).CellStyle = style;
                        row.CreateCell(10).CellStyle = style;

                        row.GetCell(3).SetValue(element.Key);
                        
                        if (element.Value > 0)
                        {//2ая строка в группе
                            row.GetCell(2).SetValue("Временные свидетельства"); row.GetCell(2).CellStyle = style;
                            row.GetCell(5).SetValue("шт.");
                            row.GetCell(6).SetValue(element.Value.ToString());
                            row.GetCell(7).SetValue(element.Value.ToString());
                            row.GetCell(8).SetValue(price.ToString());
                            row.GetCell(9).SetValue((price * element.Value).ToString());
                        }
                        else
                        {
                            //1ая строка
                        }
                        r++;
                    }
                }
                catch
                {
                }
            }
        }
    }
}
