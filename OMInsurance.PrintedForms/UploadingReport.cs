using NPOI.SS.UserModel;
using OMInsurance.Entities;
using OMInsurance.PrintedForms.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OMInsurance.PrintedForms
{
    public class UploadingReport : PrintedForm
    {
        private List<ClientVisit.UpdateResultData> reportItems;
        string reportTitle;
        #region Marks

        protected const string title = "$Title$";

        #endregion

        public UploadingReport(string title, List<ClientVisit.UpdateResultData> visits)
        {
            reportItems = visits;
            this.reportTitle = title;
            TemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Templates", "UploadingReport.xls");
        }

        protected override void Process(ISheet table)
        {
            table.FindCellByMacros(title).SetValue(string.Format("Отчет по загрузке файла {0}", reportTitle));
            int rowNumber = 3;
            for(int i = 0; i < reportItems.Count; i++)
            {
                ClientVisit.UpdateResultData reportItem = reportItems[i];
                IRow row = table.CreateRow(rowNumber);
                row.CreateCell(0).SetValue(GetNotNullString((i + 1).ToString()));
                row.CreateCell(1).SetValue(GetNotNullString(reportItem.UnifiedPolicyNumber));
                row.CreateCell(2).SetValue(GetNotNullString(reportItem.Lastname));
                row.CreateCell(3).SetValue(GetNotNullString(reportItem.Firstname));
                row.CreateCell(4).SetValue(GetNotNullString(reportItem.Secondname));
                row.CreateCell(5).SetValue(GetDateString(reportItem.Birthday, "dd.MM.yyyy"));
                row.CreateCell(6).SetValue(GetNotNullString(reportItem.Message));
                row.CreateCell(7).SetValue(GetNotNullString(reportItem.Status != null ? reportItem.Status.Id.ToString() : string.Empty));
                row.CreateCell(8).SetValue(GetNotNullString(reportItem.RECID.ToString()));
                row.CreateCell(9).SetValue(GetNotNullString(reportItem.Id.ToString()));
                row.CreateCell(10).SetValue(GetNotNullString(reportItem.ClientId.ToString()));
                row.CreateCell(11).SetValue(GetNotNullString(reportItem.ClientVisitGroupId.ToString()));
                rowNumber++;
            }
        }
    }
}
