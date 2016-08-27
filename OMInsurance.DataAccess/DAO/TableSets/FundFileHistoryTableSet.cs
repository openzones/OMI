using OMInsurance.Entities;
using System;
using System.Collections.Generic;
using System.Data;

namespace OMInsurance.DataAccess.DAO
{
    public class FundFileHistoryTableSet
    {
        private readonly DataTable resultTable;

        public DataTable ResultTable
        {
            get { return resultTable; }
        }

        public FundFileHistoryTableSet(IEnumerable<FundFileHistory> list)
        {
            resultTable = new DataTable()
            {
                Columns =
                {
                    new DataColumn("ClientID", typeof(long)),
                    new DataColumn("VisitGroupID", typeof(long)),
                    new DataColumn("ClientVisitID", typeof(long)),
                    new DataColumn("StatusID", typeof(long)),
                    new DataColumn("Date", typeof(DateTime)),
                    new DataColumn("UserID", typeof(long)),
                    new DataColumn("FileName", typeof(string)),
                    new DataColumn("FileUrl", typeof(string))
                }
            };
            FillTable(list);
        }

        private void FillTable(IEnumerable<FundFileHistory> list)
        {
            foreach (FundFileHistory item in list)
            {
                resultTable.Rows.Add(
                    item.ClientID,
                    item.VisitGroupID,
                    item.ClientVisitID,
                    item.StatusID,
                    item.Date,
                    item.UserID,
                    item.FileName,
                    item.FileUrl);
            }
        }
    }
}
