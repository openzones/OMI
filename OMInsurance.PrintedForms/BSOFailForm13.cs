using NPOI.SS.UserModel;
using OMInsurance.Entities;
using OMInsurance.PrintedForms.Helpers;
using System;
using System.Collections.Generic;
using System.IO;

namespace OMInsurance.PrintedForms
{
    public class BSOFailForm13 : PrintedForm
    {
        private List<BSOInfo> ListBSOInfo;
        private DateTime DateTimeFrom;
        private DateTime DateTimeTo;
        private string DeliveryCenter;
        private string FIO;

        #region Marks
        protected const string dateTimeConst = "$DateTime$";
        protected const string deliveryCenterConst = "$DeliveryCenter$";
        protected const string numberFailConst = "$NumberFail$";
        protected const string numberFail1Const = "$NumberFail1$";
        protected const string numbersConst = "$Numbers$";
        protected const string FIOConst = "$FIO$";
        protected const string nowDateTimeConst = "$NowDataTime$";

        #endregion
        public BSOFailForm13(List<BSOInfo> listBsoInfo, DateTime dateFrom, DateTime dateTo, string deliveryCenter, string fio)
        {
            ListBSOInfo = listBsoInfo;
            DateTimeFrom = dateFrom;
            DateTimeTo = dateTo;
            DeliveryCenter = deliveryCenter;
            FIO = fio;
            TemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Templates", "BSOFailForm13.xls");
        }

        protected override void Process(ISheet table)
        {
            string dateTime = string.Format("за отчетный период с {0} г. по {1} г.", DateTimeFrom.ToShortDateString(), DateTimeTo.ToShortDateString());
            string numbers = null;
            foreach(BSOInfo b in ListBSOInfo)
            {
                numbers = numbers + b.TemporaryPolicyNumber + ", ";
            }

            table.FindCellByMacros(dateTimeConst).SetValue(dateTime);
            table.FindCellByMacros(deliveryCenterConst).SetValue(DeliveryCenter);
            table.FindCellByMacros(numberFailConst).SetValue(ListBSOInfo.Count.ToString());
            table.FindCellByMacros(numberFail1Const).SetValue(ListBSOInfo.Count.ToString());
            table.FindCellByMacros(numbersConst).SetValue(numbers);
            table.FindCellByMacros(FIOConst).SetValue(FIO);
            table.FindCellByMacros(nowDateTimeConst).SetValue(DateTime.Now.ToShortDateString());
        }
    }
}
