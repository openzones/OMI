using NPOI.SS.UserModel;
using OMInsurance.Entities;
using OMInsurance.PrintedForms.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace OMInsurance.PrintedForms
{
    public class ClientVisitReport : PrintedForm
    {
        private List<ClientVisitInfo> ClientVisits;
        private List<User> ListUser;
        private DateTime DateFrom;
        private DateTime DateTo;

        #region Marks
        protected const string title = "$Title$";
        #endregion

        public ClientVisitReport(List<ClientVisitInfo> clientVisits, DateTime dateFrom, DateTime dateTo, List<User> listUser)
        {
            ClientVisits = clientVisits;
            ListUser = listUser;
            DateFrom = dateFrom;
            DateTo = dateTo;
            TemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Templates", "ClientVisitReport.xls");
        }

        protected override void Process(ISheet table)
        {
            table.FindCellByMacros(title).SetValue("Сводный отчет по обращениям с " + DateFrom.ToShortDateString() + " по " + DateTo.ToShortDateString());
            int rowNumber = 3;


            var VisitGroup = ClientVisits.GroupBy(a => a.VisitGroupId);
            List<ClientVisitInfo> clearClientVisit = new List<ClientVisitInfo>();
            foreach(var item in VisitGroup)
            {
                clearClientVisit.Add(item.OrderBy(a => a.StatusDate).FirstOrDefault());
                //clearClientVisit.Add(item.OrderBy(a => a.StatusDate).LastOrDefault());
            }

            var GroupByDeliveryCenter = clearClientVisit.GroupBy(a => a.DeliveryCenter.Id);

            foreach(var elem in GroupByDeliveryCenter)
            {
                var GroupByDeliveryPoint = elem.OrderBy(b=>b.DeliveryPoint).GroupBy(a => a.DeliveryPoint);

                foreach (var item in GroupByDeliveryPoint)
                {
                    var GroupByUserId = item.GroupBy(a => a.UserId);
                    foreach (var item1 in GroupByUserId)
                    {
                        var GroupByClientAcquisitionEmployee = item1.GroupBy(a => a.ClientAcquisitionEmployee);
                        foreach (var item2 in GroupByClientAcquisitionEmployee)
                        {
                            IRow row = table.CreateRow(rowNumber);
                            row.CreateCell(0).SetCellValue(rowNumber - 2);
                            row.CreateCell(1).SetCellValue(item2.FirstOrDefault().DeliveryCenter.Name);
                            row.CreateCell(2).SetCellValue(item2.FirstOrDefault().DeliveryPoint);
                            row.CreateCell(3).SetCellValue(ListUser.Where(a=>a.Id == item2.FirstOrDefault().UserId).Select(b=>b.Fullname).FirstOrDefault());
                            row.CreateCell(4).SetCellValue(item2.FirstOrDefault().ClientAcquisitionEmployee);
                            row.CreateCell(5).SetCellValue(item2.Count());
                            rowNumber++;
                        }
                    }
                }
            }
            

            
        }

    }
}
