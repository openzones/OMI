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
    public class BSOOperativeInformation : PrintedForm
    {
        private List<BSOSumStatus> ListBSOSumStatus;
        private DateTime Date;

        #region Marks
        protected const string dateTimeConst = "$DateTime$";
        protected const string OnStorageConst = "$OnStorage$";
        protected const string OnDeliveryConst = "$OnDelivery$";
        protected const string OnClientConst = "$OnClient$";
        protected const string GotoStorageConst = "$GotoStorage$";
        protected const string FailConst = "$Fail$";
        protected const string FailOnStorageConst = "$FailOnStorage$";
        protected const string DeleteConst = "$Delete$";
        protected const string SumConst = "$Sum$";
        #endregion

        public BSOOperativeInformation( DateTime date, List<BSOSumStatus> list)
        {
            ListBSOSumStatus = list;
            Date = date;
            TemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Templates", "BSOOperativeInformation.xls");
        }

        protected override void Process(ISheet table)
        {
            long sum = 0; foreach (var a in ListBSOSumStatus) { sum = sum + a.Count; }

            IRow r = table.GetRow(2);
            ICellStyle style = r.GetCell(0).CellStyle;

            IRow rowTitle = table.CreateRow(2);
            rowTitle.HeightInPoints = 26;
            IRow rowData = table.CreateRow(3);
            rowTitle.CreateCell(0).SetValue("Дата");
            rowTitle.GetCell(0).CellStyle = style;
            rowData.CreateCell(0).SetValue(Date.ToShortDateString());

            int i = 1;
            foreach (var item in ListBSOSumStatus)
            {
                rowTitle.CreateCell(i).SetValue(item.Name);
                rowTitle.GetCell(i).CellStyle = style;
                rowData.CreateCell(i).SetCellValue(item.Count);
                i++;
            }

            rowTitle.CreateCell(i).SetValue("Всего");
            rowTitle.GetCell(i).CellStyle = style;
            rowData.CreateCell(i).SetCellValue(sum);

        }
    }
}
