using NPOI.SS.UserModel;
using OMInsurance.Entities.SMS;
using OMInsurance.PrintedForms.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OMInsurance.PrintedForms
{
    public class SMSBaseReport : PrintedForm
    {
        private List<SmsBase> ListSmsBase;
        private DateTime CreateDateFrom;
        private DateTime CreateDateTo;

        #region Marks

        protected const string title = "$Title$";

        #endregion

        public SMSBaseReport(List<SmsBase> listSmsBase, DateTime createDateFrom, DateTime createDateTo)
        {
            ListSmsBase = listSmsBase;
            CreateDateFrom = createDateFrom;
            CreateDateTo = createDateTo;
            TemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Templates", "SmsBaseReport.xls");
        }

        protected override void Process(ISheet table)
        {
            table.FindCellByMacros(title).SetValue("Отчет по отправленным смс с "+CreateDateFrom.ToShortDateString()+ " по " + CreateDateTo.ToShortDateString());
            int rowNumber = 3;
            for (int i = 0; i < ListSmsBase.Count; i++)
            {
                var sms = ListSmsBase[i];
                IRow row = table.CreateRow(rowNumber);
                row.CreateCell(0).SetValue(GetNotNullString((i + 1).ToString()));
                row.CreateCell(1).SetValue(GetNotNullString(sms.SenderId));
                row.CreateCell(2).SetValue(GetNotNullString(sms.ClientId.ToString()));
                row.CreateCell(3).SetValue(GetNotNullString(sms.VisitGroupId.ToString()));
                row.CreateCell(4).SetValue(GetNotNullString(sms.VisitId.ToString()));
                row.CreateCell(5).SetValue(GetNotNullString(sms.Phone));
                row.CreateCell(6).SetValue(GetNotNullString(sms.Message));
                row.CreateCell(7).SetValue(sms.CreateDate.ToString());
                row.CreateCell(8).SetValue(GetNotNullString(sms.Comment));
                row.CreateCell(9).SetValue(GetNotNullString(sms.StatusIdInside.ToString()));
                row.CreateCell(10).SetValue(GetNotNullString(sms.StatusFromService));
                row.CreateCell(11).SetValue(GetNotNullString(sms.MessageFromService));
                row.CreateCell(12).SetValue(sms.SendDate.ToString());
                row.CreateCell(13).SetValue(GetDateString(sms.StatuRepeatDate, "dd.MM.yyyy"));
                row.CreateCell(14).SetValue(GetNotNullString(sms.StatusIdRepeat.ToString()));
                rowNumber++;
            }
        }
    }
}
