using OMInsurance.Entities;
using System;
using System.Collections.Generic;
using System.Data;

namespace OMInsurance.DataAccess.DAO
{
    public class STOPTableSet
    {
        private readonly DataTable stopResultTable;

        public DataTable STOPResultTable
        {
            get { return stopResultTable; }
        }

        public STOPTableSet(IEnumerable<STOP> listSTOP)
        {
            stopResultTable = new DataTable()
            {
                Columns =
                {
                    new DataColumn("SCENARIO", typeof(string)),
                    new DataColumn("S_CARD", typeof(string)),
                    new DataColumn("N_CARD", typeof(string)),
                    new DataColumn("UnifiedPolicyNumber", typeof(string)),
                    new DataColumn("TemporaryPolicyNumber", typeof(string)),
                    new DataColumn("QZ", typeof(long)),
                    new DataColumn("DATE_END", typeof(DateTime)),
                    new DataColumn("DATE_ARC", typeof(DateTime)),
                    new DataColumn("IST", typeof(string)),
                    new DataColumn("ClientID", typeof(long)),
                    new DataColumn("HistoryID", typeof(long))
                }
            };
            FillTable(listSTOP);
        }

        private void FillTable(IEnumerable<STOP> listSTOP)
        {
            foreach (STOP stop in listSTOP)
            {
                stopResultTable.Rows.Add(stop.SCENARIO, stop.S_CARD, stop.N_CARD, stop.ENP, stop.VSN, stop.QZ, stop.DATE_END, stop.DATE_ARC, stop.IST, stop.ClientID, stop.HistoryID);
            }
        }

    }
}
