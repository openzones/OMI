using NPOI.SS.UserModel;
using OMInsurance.Entities;
using OMInsurance.PrintedForms.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OMInsurance.PrintedForms
{
    public class AllocationBSO : PrintedForm
    {
        private List<ClientVisitInfo> Visits;
        #region Marks

        protected const string title = "$Title$";

        #endregion

        public AllocationBSO(List<ClientVisitInfo> visits)
        {
            Visits = visits;
            TemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Templates", "AllocationBSO.xlsx");
        }

        protected override void Process(ISheet table)
        {
            var firstVisit = Visits.FirstOrDefault();
            if (firstVisit == null)
            {
                table.FindCellByMacros(title).SetValue(GetNotNullString("Отчет по распределению БСО. Данных нет!"));
                return;
            }
            string partyNumber = firstVisit.PolicyParty;
            table.FindCellByMacros(title).SetValue(GetNotNullString(string.Format("Отчет по распределению БСО")));
            int rowNumber = 3;
            for (int i = 0; i < Visits.Count; i++)
            {
                var clientVisit = Visits[i];
                IRow row = table.CreateRow(rowNumber);
                row.CreateCell(0).SetValue(GetNotNullString((i + 1).ToString()));
                row.CreateCell(1).SetValue(GetNotNullString(clientVisit.DeliveryCenter != null ? clientVisit.DeliveryCenter.Code : string.Empty));
                row.CreateCell(2).SetValue(GetNotNullString(clientVisit.DeliveryPoint));
                row.CreateCell(3).SetValue(GetNotNullString(clientVisit.PolicyParty));
                row.CreateCell(4).SetValue(GetNotNullString(clientVisit.TemporaryPolicyNumber));
                row.CreateCell(5).SetValue(GetDateString(clientVisit.TemporaryPolicyDate, "dd.MM.yyyy"));
                row.CreateCell(6).SetValue(GetNotNullString(clientVisit.UnifiedPolicyNumber));
                row.CreateCell(7).SetValue(GetDateString(clientVisit.PolicyIssueDate, "dd.MM.yyyy"));
                string fio = string.Format("{0} {1} {2}",
                    clientVisit.Lastname ?? string.Empty,
                    clientVisit.Firstname ?? string.Empty,
                    clientVisit.Secondname ?? string.Empty);
                row.CreateCell(8).SetValue(GetNotNullString(fio));
                row.CreateCell(9).SetValue(GetDateString(clientVisit.Birthday, "dd.MM.yyyy"));
                row.CreateCell(10).SetValue(GetNotNullString(clientVisit.Phone));
                if (!string.IsNullOrEmpty(clientVisit.Comment)) clientVisit.Comment = clientVisit.Comment.Replace("  ", string.Empty);
                row.CreateCell(11).SetValue(GetNotNullString(clientVisit.Comment));
                rowNumber++;
            }

        }
    }
}
