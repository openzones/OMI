using NPOI.SS.UserModel;
using OMInsurance.Entities;
using OMInsurance.PrintedForms.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OMInsurance.PrintedForms
{
    public class PolicyExtraitionSignatureList : PrintedForm
    {
        private List<ClientVisitInfo> Visits;
        #region Marks

        protected const string title = "$Title$";

        #endregion

        public PolicyExtraitionSignatureList(List<ClientVisitInfo> visits)
        {
            Visits = visits;
            TemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Templates", "PolicyExtraitionSignatureList.xls");
        }

        protected override void Process(ISheet table)
        {
            var firstVisit = Visits.FirstOrDefault();
            if (firstVisit == null)
            {
                table.FindCellByMacros(title).SetValue(GetNotNullString("Журнал выдачи полисов ОМС"));
                return;
            }
            string partyNumber = firstVisit.PolicyParty;
            table.FindCellByMacros(title).SetValue(GetNotNullString(string.Format("Журнал выдачи полисов ОМС (заявка {0})", partyNumber)));

            int rowNumber = 3;
            ICellStyle style = table.Workbook.CreateCellStyle();
            style.BorderRight = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;

            for (int i = 0; i < Visits.Count; i++)
            {
                var clientVisit = Visits[i];
                IRow row = table.CreateRow(rowNumber);
                row.CreateCell(0).SetValue(GetNotNullString((i + 1).ToString()));
                row.CreateCell(1).SetValue(GetNotNullString(clientVisit.TemporaryPolicyNumber));
                row.CreateCell(2).SetValue(GetDateString(clientVisit.TemporaryPolicyDate, "dd.MM.yyyy"));
                row.CreateCell(3).SetValue(GetNotNullString(clientVisit.UnifiedPolicyNumber));
                string fio = string.Format("{0} {1} {2}",
                    clientVisit.Lastname ?? string.Empty,
                    clientVisit.Firstname ?? string.Empty,
                    clientVisit.Secondname ?? string.Empty);
                row.CreateCell(4).SetValue(GetNotNullString(fio));
                row.CreateCell(5).SetValue(GetDateString(clientVisit.Birthday, "dd.MM.yyyy"));
                row.CreateCell(6).SetValue(GetNotNullString(clientVisit.DeliveryPoint));
                row.CreateCell(7).SetValue(GetNotNullString(clientVisit.DeliveryCenter.Name));
                row.CreateCell(8);
                row.CreateCell(9);
                if (!string.IsNullOrEmpty(clientVisit.Comment)) clientVisit.Comment = clientVisit.Comment.Replace("  ", string.Empty);
                row.CreateCell(10).SetValue(GetNotNullString(clientVisit.Comment));

                row.GetCell(0).CellStyle = style;
                row.GetCell(1).CellStyle = style;
                row.GetCell(2).CellStyle = style;
                row.GetCell(3).CellStyle = style;
                row.GetCell(4).CellStyle = style;
                row.GetCell(5).CellStyle = style;
                row.GetCell(6).CellStyle = style;
                row.GetCell(7).CellStyle = style;
                row.GetCell(8).CellStyle = style;
                row.GetCell(9).CellStyle = style;
                row.GetCell(10).CellStyle = style;
                rowNumber++;
            }

        }
    }
}
