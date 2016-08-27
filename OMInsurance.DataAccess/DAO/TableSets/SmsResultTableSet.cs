using OMInsurance.Entities.SMS;
using System;
using System.Collections.Generic;
using System.Data;

namespace OMInsurance.DataAccess.DAO
{
    public class SmsResultTableSet
    {
        private readonly DataTable smsResultTable;

        public DataTable SmsResultTable
        {
            get { return smsResultTable; }
        }

        public SmsResultTableSet(IEnumerable<SmsResult> listSms)
        {
            smsResultTable = new DataTable()
            {
                Columns =
                {
                    new DataColumn("MessageID", typeof(long)),
                    new DataColumn("StatusIdInside", typeof(long)),
                    new DataColumn("StatusFromService", typeof(string)),
                    new DataColumn("MessageFromService", typeof(string)),
                    new DataColumn("SendDate", typeof(DateTime))
                }
            };
            FillTable(listSms);
        }

        private void FillTable(IEnumerable<SmsResult> listSms)
        {
            foreach (SmsResult sms in listSms)
            {
                smsResultTable.Rows.Add(sms.Id, sms.StatusIdInside, sms.StatusFromService, sms.MessageFromService, sms.SendDate);
            }
        }
    }
}
