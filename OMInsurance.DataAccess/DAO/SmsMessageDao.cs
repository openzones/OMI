using OMInsurance.DataAccess.Core;
using OMInsurance.Entities.SMS;
using OMInsurance.DataAccess.Core.Helpers;
using OMInsurance.DataAccess.Materializers;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System;

namespace OMInsurance.DataAccess.DAO
{
    public class SmsMessageDao : ItemDao
    {
        private static SmsMessageDao _instance = new SmsMessageDao();

        private SmsMessageDao()
            : base(DatabaseAliases.OMInsurance, new DatabaseErrorHandler())
        {
        }

        public static SmsMessageDao Instance
        {
            get
            {
                return _instance;
            }
        }

        public List<SmsBase> SMSBase_GetAll(SmsBase.SmsBaseGet smsGet)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@CreateDateFrom", SqlDbType.DateTime, smsGet.CreateDateFrom);
            parameters.AddInputParameter("@CreateDateTo", SqlDbType.DateTime, smsGet.CreateDateTo);
            List<SmsBase> result = Execute_GetList(SmsMaterializer.Instance, "SMSBase_GetAll", parameters);
            return result;
        }

        public SmsTemplate SmsTemplate_Get()
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter SenderId = parameters.AddInputOutputParameter("@SenderId", SqlDbType.NVarChar, "sdfds");
            SqlParameter Message = parameters.AddInputOutputParameter("@Message", SqlDbType.NVarChar, "fsdf");
            SqlParameter StatusId = parameters.AddInputOutputParameter("@StatusId", SqlDbType.BigInt, 9);
            SmsTemplate SmsTemplate = Execute_Get(SmsTemplateMaterializer.Instance, "SMSTemplate_Get", parameters);
            return SmsTemplate;
        }

        public void SmsTemplate_Set(SmsTemplate smsTemplate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@SenderId", SqlDbType.NVarChar, smsTemplate.SenderId);
            parameters.AddInputParameter("@Message", SqlDbType.NVarChar, smsTemplate.Message);
            parameters.AddInputParameter("@Phone", SqlDbType.NVarChar, smsTemplate.Phone);
            parameters.AddInputParameter("@CreateDate", SqlDbType.DateTime, DateTime.Now);
            parameters.AddInputParameter("@StatusId", SqlDbType.BigInt, smsTemplate.StatusId);
            Execute_StoredProcedure("SMSTemplate_Set", parameters);
        }

        /// <summary>
        /// Заполняем таблицу SMSBase
        /// </summary>
        /// <param name="data"></param>
        public void SMSBaseSet(SmsBase.SmsBaseSet data)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@SenderId", SqlDbType.NVarChar, data.SenderId);
            parameters.AddInputParameter("@StatusId", SqlDbType.BigInt, data.StatusId);
            parameters.AddInputParameter("@StatusDate", SqlDbType.DateTime, data.StatusDate);
            parameters.AddInputParameter("@CreateDate", SqlDbType.DateTime, data.CreateDate);
            parameters.AddInputParameter("@Message", SqlDbType.NVarChar, data.Message);
            parameters.AddInputParameter("@Comment", SqlDbType.NVarChar, data.Comment);
            Execute_StoredProcedure("SMSBase_Set", parameters);
        }

        /// <summary>
        /// Retuns list of messages that should be sent
        /// </summary>
        /// <returns>List of SmsMessages</returns>
        public List<SMSMessage> GetList(long? StatusIdInside = null)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@StatusIdInside", SqlDbType.BigInt, StatusIdInside);
            List<SMSMessage> listSms = Execute_GetList(SmsMessageMaterializer.Instance, "SMSBase_GetList", parameters);
            return listSms;
        }

        /// <summary>
        /// Sets messages results to spacified messages
        /// </summary>
        public void SetMessageResult(IEnumerable<SmsResult> results)
        {
            SmsResultTableSet smsSet = new SmsResultTableSet(results);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@smsTable", SqlDbType.Structured, smsSet.SmsResultTable);
            Execute_StoredProcedure("SMSBase_SetResults", parameters);
        }

        /// <summary>
        /// Sets message results to spacified message
        /// </summary>
        public void SetMessageResult(SmsResult result)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@MessageID", SqlDbType.BigInt, result.Id);
            parameters.AddInputParameter("@StatusIdInside", SqlDbType.BigInt, result.StatusIdInside);
            parameters.AddInputParameter("@StatusFromService", SqlDbType.NVarChar, result.StatusFromService);
            parameters.AddInputParameter("@MessageFromService", SqlDbType.NVarChar, result.MessageFromService);
            parameters.AddInputParameter("@SendDate", SqlDbType.DateTime, result.SendDate);
            Execute_StoredProcedure("SMSBase_SetResult", parameters);
        }
    }
}
