using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
using System.Data;

namespace OMInsurance.DataAccess.DAO
{
    public class LogMessageTableSet
    {
        private readonly DataTable logMessageTable;

        public DataTable LogMessageTable
        {
            get { return logMessageTable; }
        }

        public LogMessageTableSet(IEnumerable<LogMessage> messages)
        {
            logMessageTable = new DataTable()
            {
                Columns = 
                { 
                    new DataColumn("ClassName", typeof(string)),
                    new DataColumn("MethodName", typeof(string)),
                    new DataColumn("Message", typeof(string)),
                    new DataColumn("ErrorSeverity", typeof(string)),
                    new DataColumn("AppName", typeof(string)),
                    new DataColumn("Stacktrace", typeof(string)),
                    new DataColumn("CreateDate", typeof(DateTime))
                }
            };
            FillTable(messages);
        }

        private void FillTable(IEnumerable<LogMessage> messages)
        {
            foreach (LogMessage message in messages)
            {
                logMessageTable.Rows.Add(message.ClassName, message.MethodName, message.Message,
                    message.Severity, message.AppName, message.StackTrace, message.CreateDate);
            }
        }
    }
}
