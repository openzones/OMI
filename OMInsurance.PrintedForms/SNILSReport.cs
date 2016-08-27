using NPOI.SS.UserModel;
using OMInsurance.Entities;
using OMInsurance.PrintedForms.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OMInsurance.PrintedForms
{
    public class SNILSReport : PrintedForm
    {
        private List<ClientVisitInfo> ListClientVisitInfo;
        private DateTime DateTimeFrom;
        private DateTime DateTimeTo;
        private string DeliveryPoint;
        private string FIO;
        private bool? ChoiceSNILS;
        private List<User> ListUser;

        #region Marks
        protected const string dateTimeConst = "$DateTime$";
        protected const string deliveryPointConst = "$DeliveryPoint$";
        protected const string FIOConst = "$FIO$";
        protected const string nowDateTimeConst = "$NowDateTime$";
        protected const string SNILSConst = "$SNILS$";
        #endregion

        public SNILSReport(List<ClientVisitInfo> listClientVisitInfo,
            DateTime dateFrom,
            DateTime dateTo,
            string deliveryPoint,
            string fio,
            bool? choiceSNILS,
             List<User> listUser
                                    )
        {
            ListClientVisitInfo = listClientVisitInfo;
            DateTimeFrom = dateFrom;
            DateTimeTo = dateTo;
            DeliveryPoint = deliveryPoint;
            FIO = fio;
            ChoiceSNILS = choiceSNILS;
            ListUser = listUser;
            TemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Templates", "SNILSReport.xlsx");
        }

        protected override void Process(ISheet table)
        {
            string dateTime = string.Format("За период с {0} г. по {1} г.", DateTimeFrom.ToShortDateString(), DateTimeTo.ToShortDateString());
            table.FindCellByMacros(dateTimeConst).SetValue(dateTime);
            table.FindCellByMacros(deliveryPointConst).SetValue(DeliveryPoint);
            FIO = FIO + " (" + DateTime.Now.ToShortDateString() + ")";
            table.FindCellByMacros(FIOConst).SetValue(FIO);
            if(ChoiceSNILS == null)
            {
                table.FindCellByMacros(SNILSConst).SetValue("Все значения");
            }
            else if(ChoiceSNILS == true)
            {
                table.FindCellByMacros(SNILSConst).SetValue("Есть");
            }
            else
            {
                table.FindCellByMacros(SNILSConst).SetValue("Нет");
            }

            int rowNumber = 6;

            foreach( var item in ListClientVisitInfo)
            {
                IRow row = table.CreateRow(rowNumber);
                row.CreateCell(0).SetValue(item.Lastname);
                row.CreateCell(1).SetValue(item.Firstname);
                row.CreateCell(2).SetValue(item.Secondname);
                row.CreateCell(3).SetValue(item.Sex);
                row.CreateCell(4).SetValue(GetDateString(item.Birthday, "dd.MM.yyyy"));
                row.CreateCell(5).SetValue(item.Citizenship);
                row.CreateCell(6).SetValue(item.SNILS);
                row.CreateCell(7).SetValue(item.Phone);
                row.CreateCell(8).SetValue(item.TemporaryPolicyNumber);
                row.CreateCell(9).SetValue(GetDateString(item.TemporaryPolicyDate, "dd.MM.yyyy"));
                row.CreateCell(10).SetValue(item.Status.Name);
                row.CreateCell(11).SetValue(GetDateString(item.StatusDate, "dd.MM.yyyy"));
                row.CreateCell(12).SetValue(item.Scenario.Code);
                row.CreateCell(13).SetValue(item.Scenario.Name);
                row.CreateCell(14).SetValue(item.DeliveryCenter.Name);
                row.CreateCell(15).SetValue(item.DeliveryPoint);
                row.CreateCell(16).SetValue(ListUser.Where(a => a.Id == item.UserId).FirstOrDefault().Fullname);
                rowNumber++;
            }
        }
    }
}
