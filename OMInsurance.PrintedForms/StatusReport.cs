using NPOI.SS.UserModel;
using OMInsurance.Entities;
using OMInsurance.PrintedForms.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OMInsurance.PrintedForms
{
    public class StatusReport : PrintedForm
    {
        private List<ClientVisitInfo> Visits;
        private DateTime? StatusDateFrom;
        private DateTime? StatusDateTo;
        private List<string> ListStatus;
        #region Marks

        protected const string ConstTitle = "$Title$";
        protected const string ConstStatuses = "$Statuses$";

        #endregion

        public StatusReport(List<ClientVisitInfo> visits, DateTime? statusDateFrom, DateTime? statusDateTo, List<string> listStatus)
        {
            Visits = visits;
            StatusDateFrom = statusDateFrom;
            StatusDateTo = statusDateTo;
            ListStatus = listStatus;
            TemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Templates", "StatusReport.xlsx");
        }

        protected override void Process(ISheet table)
        {
            var firstVisit = Visits.FirstOrDefault();
            if (firstVisit == null)
            {
                table.FindCellByMacros(ConstTitle).SetValue(GetNotNullString("Отчет по статусам. Данных нет!"));
                return;
            }

            table.FindCellByMacros(ConstTitle).SetValue(GetNotNullString("Отчет по статусам c " + GetDateString(StatusDateFrom, "dd.MM.yyyy") +
                " по " + GetDateString(StatusDateTo, "dd.MM.yyyy")));
            string statuses = "Выбранные статусы: ";
            if(ListStatus.Count > 0)
            {
                foreach (var item in ListStatus)
                {
                    statuses = statuses + item + ", ";
                }
            }
            else
            {
                statuses = statuses + "Все";
            }
            statuses = statuses.TrimEnd(' ', ',');
            table.FindCellByMacros(ConstStatuses).SetValue(GetNotNullString(statuses));

            int rowNumber = 4;
            for (int i = 0; i < Visits.Count; i++)
            {
                var clientVisit = Visits[i];
                IRow row = table.CreateRow(rowNumber);
                row.CreateCell(0).SetValue(GetNotNullString((i + 1).ToString()));
                row.CreateCell(1).SetCellValue(clientVisit.ClientId);
                row.CreateCell(2).SetCellValue(clientVisit.VisitGroupId);
                row.CreateCell(3).SetValue(GetNotNullString(clientVisit.DeliveryCenter != null ? clientVisit.DeliveryCenter.Code : string.Empty));
                row.CreateCell(4).SetValue(GetNotNullString(clientVisit.DeliveryCenter.Name));
                row.CreateCell(5).SetValue(GetNotNullString(clientVisit.DeliveryPoint));
                row.CreateCell(6).SetValue(GetNotNullString(clientVisit.PolicyParty));
                row.CreateCell(7).SetValue(GetNotNullString(clientVisit.TemporaryPolicyNumber));
                row.CreateCell(8).SetValue(GetNotNullString(clientVisit.UnifiedPolicyNumber));
                row.CreateCell(9).SetValue(GetNotNullString(clientVisit.Lastname));
                row.CreateCell(10).SetValue(GetNotNullString(clientVisit.Firstname));
                row.CreateCell(11).SetValue(GetNotNullString(clientVisit.Secondname));
                row.CreateCell(12).SetValue(GetDateString(clientVisit.Birthday, "dd.MM.yyyy"));
                row.CreateCell(13).SetValue(GetNotNullString(clientVisit.Phone));
                if (!string.IsNullOrEmpty(clientVisit.Comment)) clientVisit.Comment = clientVisit.Comment.Replace("  ", string.Empty);
                row.CreateCell(14).SetValue(GetNotNullString(clientVisit.Comment));
                
                rowNumber++;
            }


        }

    }
}
