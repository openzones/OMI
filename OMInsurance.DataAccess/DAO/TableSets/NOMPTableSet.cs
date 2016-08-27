using OMInsurance.Entities;
using System;
using System.Collections.Generic;
using System.Data;


namespace OMInsurance.DataAccess.DAO
{
    public class NOMPTableSet
    {
        private readonly DataTable nompResultTable;

        public DataTable NOMPResultTable
        {
            get { return nompResultTable; }
        }

        public NOMPTableSet(IEnumerable<NOMP> listNOMP)
        {
            nompResultTable = new DataTable()
            {
                Columns =
                {
                    new DataColumn("S_CARD", typeof(string)),
                    new DataColumn("N_CARD", typeof(string)),
                    new DataColumn("UnifiedPolicyNumber", typeof(string)),
                    new DataColumn("TemporaryPolicyNumber", typeof(string)),
                    new DataColumn("LPU_ID", typeof(long)),
                    new DataColumn("DATE_IN", typeof(DateTime)),
                    new DataColumn("SPOS", typeof(int)),
                    new DataColumn("ClientID", typeof(long)),
                    new DataColumn("HistoryID", typeof(long))
                }
            };
            FillTable(listNOMP);
        }

        private void FillTable(IEnumerable<NOMP> listNOMP)
        {
            foreach (NOMP nomp in listNOMP)
            {
                nompResultTable.Rows.Add(nomp.S_CARD, nomp.N_CARD, nomp.ENP , nomp.VSN , nomp.LPU_ID, nomp.DATE_IN, nomp.SPOS, nomp.ClientID, nomp.HistoryID);
            }
        }
    }
}
