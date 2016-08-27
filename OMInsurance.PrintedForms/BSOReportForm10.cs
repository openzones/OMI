using NPOI.SS.UserModel;
using OMInsurance.Entities;
using OMInsurance.PrintedForms.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OMInsurance.PrintedForms
{
    public class BSOReportForm10 : PrintedForm
    {
        private List<BSOInfo> ListBSOInfo;
        private DateTime DateTimeFrom;
        private DateTime DateTimeTo;
        private string DeliveryPoint;
        private string FIO;

        #region Marks
        protected const string dateTimeConst = "$DateTime$";
        protected const string deliveryPointConst = "$DeliveryPoint$";
        protected const string FIOConst = "$FIO$";
        protected const string nowDateTimeConst = "$NowDateTime$";
        #endregion

        public BSOReportForm10(List<BSOInfo> listBsoInfo, DateTime dateFrom, DateTime dateTo, string deliveryPoint, string fio)
        {
            ListBSOInfo = listBsoInfo;
            DateTimeFrom = dateFrom;
            DateTimeTo = dateTo;
            DeliveryPoint = deliveryPoint;
            FIO = fio;
            TemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Templates", "BSOReportForm10.xls");
        }

        protected override void Process(ISheet table)
        {
            //cсохранем низ документа
            int t = 47;
            IRow rowTemp1 = table.GetRow(t);
            IRow rowTemp2 = table.GetRow(t + 3);
            IRow rowTemp3 = table.GetRow(t + 4);
            IRow rowTemp4 = table.GetRow(t + 6);
            IRow rowTemp5 = table.GetRow(t + 7);
            IRow rowTemp6 = table.GetRow(t + 10);
            IRow rowTemp7 = table.GetRow(t + 11);
            IRow rowTemp8 = table.GetRow(t + 13);
            IRow rowTemp9 = table.GetRow(t + 14);
            IRow rowTemp10 = table.GetRow(t + 16);

            string dateTime = string.Format("за отчетный период с {0} г. по {1} г.", DateTimeFrom.ToShortDateString(), DateTimeTo.ToShortDateString());
            table.FindCellByMacros(dateTimeConst).SetValue(dateTime);
            table.FindCellByMacros(deliveryPointConst).SetValue(DeliveryPoint);

            int rowNumber = 14;
            foreach( var element in ListBSOInfo)
            {
                IRow row = table.CreateRow(rowNumber);
                row.CreateCell(0).SetValue(element.TemporaryPolicyNumber);
                row.CreateCell(1).SetValue(element.Status.Name);
                row.CreateCell(2).SetValue(((DateTime)element.StatusDate).ToShortDateString());
                row.CreateCell(3).SetValue(element.ResponsibleName);
                row.CreateCell(4).SetValue(element.DeliveryCenter);
                row.CreateCell(5).SetValue(element.DeliveryPoint);
                rowNumber++;
            }
            if (rowNumber < 45)
            {
                table.FindCellByMacros(FIOConst).SetValue(FIO);
                table.FindCellByMacros(nowDateTimeConst).SetValue(DateTime.Now.ToShortDateString());
            }
            else
            {
                //выводим сохраненный низ документа
                rowNumber = rowNumber + 2;
                IRow row1 = table.CreateRow(rowNumber);
                row1.CreateCell(0).SetValue(rowTemp1.GetCell(0).StringCellValue);

                rowNumber = rowNumber + 3;
                IRow row2 = table.CreateRow(rowNumber);
                row2.CreateCell(0).SetValue(rowTemp2.GetCell(0).StringCellValue);

                rowNumber = rowNumber + 1;
                IRow row3 = table.CreateRow(rowNumber);
                row3.CreateCell(4).SetValue(rowTemp3.GetCell(4).StringCellValue);

                rowNumber = rowNumber + 2;
                IRow row4 = table.CreateRow(rowNumber);
                row4.CreateCell(1).SetValue(rowTemp4.GetCell(1).StringCellValue); row4.CreateCell(4).SetValue(rowTemp4.GetCell(4).StringCellValue);

                rowNumber = rowNumber + 1;
                IRow row5 = table.CreateRow(rowNumber);
                row5.CreateCell(2).SetValue(rowTemp5.GetCell(2).StringCellValue); row5.CreateCell(5).SetValue(rowTemp5.GetCell(5).StringCellValue);

                rowNumber = rowNumber + 3;
                IRow row6 = table.CreateRow(rowNumber);
                row6.CreateCell(0).SetValue(rowTemp6.GetCell(0).StringCellValue);

                rowNumber = rowNumber + 1;
                IRow row7 = table.CreateRow(rowNumber);
                row7.CreateCell(4).SetValue(rowTemp7.GetCell(4).StringCellValue);

                rowNumber = rowNumber + 3;
                IRow row8 = table.CreateRow(rowNumber);
                row8.CreateCell(1).SetValue(rowTemp8.GetCell(1).StringCellValue); row8.CreateCell(4).SetValue(FIO);

                rowNumber = rowNumber + 1;
                IRow row9 = table.CreateRow(rowNumber);
                row9.CreateCell(2).SetValue(rowTemp9.GetCell(2).StringCellValue); row9.CreateCell(5).SetValue(rowTemp9.GetCell(5).StringCellValue);

                rowNumber = rowNumber + 2;
                IRow row10 = table.CreateRow(rowNumber);
                row10.CreateCell(0).SetValue(DateTime.Now.ToShortDateString());
            }
        }
    }
}
