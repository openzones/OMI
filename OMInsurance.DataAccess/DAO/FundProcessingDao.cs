using OMInsurance.DataAccess.Core;
using OMInsurance.DataAccess.Core.Helpers;
using OMInsurance.DataAccess.Materializers;
using OMInsurance.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace OMInsurance.DataAccess.DAO
{
    public class FundProcessingDao : ItemDao, IFundResponseCreator
    {
        private static FundProcessingDao _instance = new FundProcessingDao();

        private FundProcessingDao()
            : base(DatabaseAliases.OMInsurance, new DatabaseErrorHandler())
        {
        }

        public static FundProcessingDao Instance
        {
            get
            {
                return _instance;
            }
        }

        public long FundResponse_Create(FundResponse.CreateData data, DateTime createDate)
        {
            return data.Create(FundProcessingDao.Instance, createDate);
        }

        public List<FundResponse> FundResponse_GetList(long id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@ClientVisitID", SqlDbType.BigInt, id);

            return Execute_GetList<FundResponse>(FundResponseMaterializer.Instance, "FundResponse_GetByClientVisitId", parameters);
        }

        public List<FundRequestRecid> ClientVisitId_GetByFundRequesetRecid(IEnumerable<long> recids)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@Recids", SqlDbType.Structured, DaoHelper.GetObjectIds(recids));

            return Execute_GetList<FundRequestRecid>(FundRequestRecidMaterializer.Instance, "ClientVisitId_GetByFundRequesetRecid", parameters);
        }

        public long FundRequestRecid_GetForClientVisit(long clientVisitId, int DataTypeId, DateTime date)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@ClientVisitID", SqlDbType.BigInt, clientVisitId);
            parameters.AddInputParameter("@DataTypeId", SqlDbType.Int, DataTypeId);
            parameters.AddInputParameter("@CreateDate", SqlDbType.DateTime, date);
            SqlParameter id = parameters.AddOutputParameter("@RECID", SqlDbType.BigInt);
            Execute_StoredProcedure("FundRequestRecid_GetForClientVisit", parameters);
            return (long)id.Value;
        }

        public long Create(S5FundResponse.CreateData data, DateTime createDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@ClientVisitID", SqlDbType.BigInt, data.ClientVisitId);
            parameters.AddInputParameter("@Recid", SqlDbType.BigInt, data.Recid);
            parameters.AddInputParameter("@DataTypeId", SqlDbType.BigInt, data.DataTypeId);
            parameters.AddInputParameter("@PolicyTypeID", SqlDbType.BigInt, data.PolicyTypeId);
            parameters.AddInputParameter("@UnifiedPolicyNumber", SqlDbType.NVarChar, data.UnifiedPolicyNumber);
            parameters.AddInputParameter("@PolicySeries", SqlDbType.NVarChar, data.PolicySeries);
            parameters.AddInputParameter("@PolicyNumber", SqlDbType.NVarChar, data.PolicyNumber);
            parameters.AddInputParameter("@OKATO", SqlDbType.NVarChar, data.OKATO);
            parameters.AddInputParameter("@OGRN", SqlDbType.NVarChar, data.OGRN);
            parameters.AddInputParameter("@StartDate", SqlDbType.DateTime, data.StartDate);
            parameters.AddInputParameter("@ExpirationDate", SqlDbType.DateTime, data.ExpirationDate);
            parameters.AddInputParameter("@FundAnswer", SqlDbType.NVarChar, data.FundAnswer);
            parameters.AddInputParameter("@ErrorMessage", SqlDbType.NVarChar, data.ErrorMessage);
            parameters.AddInputParameter("@OrderNumber", SqlDbType.Int, data.Order);
            parameters.AddInputParameter("@CreateDate", SqlDbType.DateTime, createDate);
            SqlParameter id = parameters.AddOutputParameter("@ID", SqlDbType.BigInt);
            Execute_StoredProcedure("S5FundResponse_Create", parameters);
            return (long)id.Value;
        }

        public long Create(S6FundResponse.CreateData data, DateTime createDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@ClientVisitID", SqlDbType.BigInt, data.ClientVisitId);
            parameters.AddInputParameter("@Recid", SqlDbType.BigInt, data.Recid);
            parameters.AddInputParameter("@DataTypeId", SqlDbType.BigInt, data.DataTypeId);
            parameters.AddInputParameter("@PolicyTypeID", SqlDbType.BigInt, data.PolicyTypeId);
            parameters.AddInputParameter("@UnifiedPolicyNumber", SqlDbType.NVarChar, data.UnifiedPolicyNumber);
            parameters.AddInputParameter("@PolicySeries", SqlDbType.NVarChar, data.PolicySeries);
            parameters.AddInputParameter("@PolicyNumber", SqlDbType.NVarChar, data.PolicyNumber);
            parameters.AddInputParameter("@OKATO", SqlDbType.NVarChar, data.OKATO);
            parameters.AddInputParameter("@OGRN", SqlDbType.NVarChar, data.OGRN);
            parameters.AddInputParameter("@StartDate", SqlDbType.DateTime, data.StartDate);
            parameters.AddInputParameter("@ExpirationDate", SqlDbType.DateTime, data.ExpirationDate);
            parameters.AddInputParameter("@FundAnswer", SqlDbType.NVarChar, data.FundAnswer);
            parameters.AddInputParameter("@ErrorMessage", SqlDbType.NVarChar, data.ErrorMessage);
            parameters.AddInputParameter("@OrderNumber", SqlDbType.Int, data.Order);
            parameters.AddInputParameter("@CreateDate", SqlDbType.DateTime, createDate);
            SqlParameter id = parameters.AddOutputParameter("@ID", SqlDbType.BigInt);
            Execute_StoredProcedure("S6FundResponse_Create", parameters);
            return (long)id.Value;
        }

        public long Create(S9FundResponse.CreateData data, DateTime createDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@ClientVisitID", SqlDbType.BigInt, data.ClientVisitId);
            parameters.AddInputParameter("@Recid", SqlDbType.BigInt, data.Recid);
            parameters.AddInputParameter("@DataTypeId", SqlDbType.BigInt, data.DataTypeId);
            parameters.AddInputParameter("@PolicyTypeID", SqlDbType.BigInt, data.PolicyTypeId);
            parameters.AddInputParameter("@UnifiedPolicyNumber", SqlDbType.NVarChar, data.UnifiedPolicyNumber);
            parameters.AddInputParameter("@PolicySeries", SqlDbType.NVarChar, data.PolicySeries);
            parameters.AddInputParameter("@PolicyNumber", SqlDbType.NVarChar, data.PolicyNumber);
            parameters.AddInputParameter("@OKATO", SqlDbType.NVarChar, data.OKATO);
            parameters.AddInputParameter("@OGRN", SqlDbType.NVarChar, data.OGRN);
            parameters.AddInputParameter("@StartDate", SqlDbType.DateTime, data.StartDate);
            parameters.AddInputParameter("@ExpirationDate", SqlDbType.DateTime, data.ExpirationDate);
            parameters.AddInputParameter("@FundAnswer", SqlDbType.NVarChar, data.FundAnswer);
            parameters.AddInputParameter("@ErrorMessage", SqlDbType.NVarChar, data.ErrorMessage);
            parameters.AddInputParameter("@OrderNumber", SqlDbType.Int, data.Order);
            parameters.AddInputParameter("@CreateDate", SqlDbType.DateTime, createDate);
            SqlParameter id = parameters.AddOutputParameter("@ID", SqlDbType.BigInt);
            Execute_StoredProcedure("S9FundResponse_Create", parameters);
            return (long)id.Value;
        }

        public long Create(SnilsFundResponse.CreateData data, DateTime createDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@ClientVisitID", SqlDbType.BigInt, data.ClientVisitId);
            parameters.AddInputParameter("@Recid", SqlDbType.BigInt, data.Recid);
            parameters.AddInputParameter("@DataTypeId", SqlDbType.BigInt, data.DataTypeId);
            parameters.AddInputParameter("@PolicyTypeID", SqlDbType.BigInt, data.PolicyTypeId);
            parameters.AddInputParameter("@UnifiedPolicyNumber", SqlDbType.NVarChar, data.UnifiedPolicyNumber);
            parameters.AddInputParameter("@PolicySeries", SqlDbType.NVarChar, data.PolicySeries);
            parameters.AddInputParameter("@PolicyNumber", SqlDbType.NVarChar, data.PolicyNumber);
            parameters.AddInputParameter("@OKATO", SqlDbType.NVarChar, data.OKATO);
            parameters.AddInputParameter("@OGRN", SqlDbType.NVarChar, data.OGRN);
            parameters.AddInputParameter("@StartDate", SqlDbType.DateTime, data.StartDate);
            parameters.AddInputParameter("@ExpirationDate", SqlDbType.DateTime, data.ExpirationDate);
            parameters.AddInputParameter("@FundAnswer", SqlDbType.NVarChar, data.FundAnswer);
            parameters.AddInputParameter("@ErrorMessage", SqlDbType.NVarChar, data.ErrorMessage);
            parameters.AddInputParameter("@OrderNumber", SqlDbType.Int, data.Order);
            parameters.AddInputParameter("@CreateDate", SqlDbType.DateTime, createDate);
            SqlParameter id = parameters.AddOutputParameter("@ID", SqlDbType.BigInt);
            Execute_StoredProcedure("SnilsFundResponse_Create", parameters);
            return (long)id.Value;
        }

        public long Create(SvdFundResponse.CreateData data, DateTime date)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@ClientVisitID", SqlDbType.BigInt, data.ClientVisitId);
            parameters.AddInputParameter("@Recid", SqlDbType.BigInt, data.Recid);
            parameters.AddInputParameter("@DataTypeId", SqlDbType.BigInt, data.DataTypeId);
            parameters.AddInputParameter("@UnifiedPolicyNumber", SqlDbType.NVarChar, data.UnifiedPolicyNumber);
            parameters.AddInputParameter("@PolicySeries", SqlDbType.NVarChar, data.PolicySeries);
            parameters.AddInputParameter("@PolicyNumber", SqlDbType.NVarChar, data.PolicyNumber);
            parameters.AddInputParameter("@StartDate", SqlDbType.DateTime, data.StartDate);
            parameters.AddInputParameter("@ExpirationDate", SqlDbType.DateTime, data.ExpirationDate);
            parameters.AddInputParameter("@OmsCode", SqlDbType.NVarChar, data.OmsCode);
            parameters.AddInputParameter("@OGRN", SqlDbType.NVarChar, data.OGRN);
            parameters.AddInputParameter("@Firstname", SqlDbType.NVarChar, data.Firstname);
            parameters.AddInputParameter("@Lastname", SqlDbType.NVarChar, data.Lastname);
            parameters.AddInputParameter("@Secondname", SqlDbType.NVarChar, data.Secondname);
            parameters.AddInputParameter("@Birthday", SqlDbType.DateTime, data.Birthday);
            parameters.AddInputParameter("@Sex", SqlDbType.NVarChar, data.Sex);
            parameters.AddInputParameter("@DocumentTypeID", SqlDbType.BigInt, data.DocumentTypeId);
            parameters.AddInputParameter("@DocumentSeries", SqlDbType.NVarChar, data.DocumentSeries);
            parameters.AddInputParameter("@DocumentNumber", SqlDbType.NVarChar, data.DocumentNumber);
            parameters.AddInputParameter("@DocumentIssueDate", SqlDbType.DateTime, data.DocumentIssueDate);
            parameters.AddInputParameter("@CitizenshipID", SqlDbType.BigInt, data.CitizenshipId);
            parameters.AddInputParameter("@ERZ", SqlDbType.NVarChar, data.ERZ);
            parameters.AddInputParameter("@Snils", SqlDbType.NVarChar, data.Snils);
            parameters.AddInputParameter("@CreateDate", SqlDbType.DateTime, date);
            SqlParameter id = parameters.AddOutputParameter("@ID", SqlDbType.BigInt);
            Execute_StoredProcedure("SvdFundResponse_Create", parameters);
            return (long)id.Value;
        }

        public long Create(GoznakResponse.CreateData data, DateTime date)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@ClientVisitId", SqlDbType.BigInt, data.ClientVisitId);
            parameters.AddInputParameter("@UnifiedPolicyNumber", SqlDbType.NVarChar, data.UnifiedPolicyNumber);
            parameters.AddInputParameter("@Birthday", SqlDbType.Date, data.Birthday);
            parameters.AddInputParameter("@Sex", SqlDbType.NVarChar, data.Sex);
            parameters.AddInputParameter("@Firstname", SqlDbType.NVarChar, data.Firstname);
            parameters.AddInputParameter("@Lastname", SqlDbType.NVarChar, data.Lastname);
            parameters.AddInputParameter("@Secondname", SqlDbType.NVarChar, data.Secondname);
            parameters.AddInputParameter("@DeliveryCenterId", SqlDbType.BigInt, data.DeliveryCenterId);
            parameters.AddInputParameter("@TemporaryPolicyNumber", SqlDbType.NVarChar, data.TemporaryPolicyNumber);
            parameters.AddInputParameter("@SaveDate", SqlDbType.DateTime, date);
            SqlParameter id = parameters.AddOutputParameter("@ID", SqlDbType.BigInt);
            
            Execute_StoredProcedure("GoznakResponse_Save", parameters);
            return (long)id.Value;
        }

        public long Create(FundErrorResponse.CreateData data, DateTime date)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@Recid", SqlDbType.BigInt, data.Recid);
            parameters.AddInputParameter("@ClientVisitId", SqlDbType.BigInt, data.ClientVisitId);
            parameters.AddInputParameter("@FundResponseStageId", SqlDbType.BigInt, data.FundResponseStageId);
            parameters.AddInputParameter("@UnifiedPolicyNumber", SqlDbType.NVarChar, data.UnifiedPolicyNumber);
            parameters.AddInputParameter("@Birthday", SqlDbType.Date, data.Birthday);
            parameters.AddInputParameter("@DeliveryCenterId", SqlDbType.BigInt, data.DeliveryCenterId);
            parameters.AddInputParameter("@OrderNumber", SqlDbType.Int, data.Order);
            parameters.AddInputParameter("@Sex", SqlDbType.NVarChar, data.Sex);
            parameters.AddInputParameter("@ErrorCode", SqlDbType.NVarChar, data.ErrorCode);
            parameters.AddInputParameter("@ErrorText", SqlDbType.NVarChar, data.ErrorText);
            parameters.AddInputParameter("@ExpirationDate", SqlDbType.Date, data.ExpirationDate);
            parameters.AddInputParameter("@Firstname", SqlDbType.NVarChar, data.Firstname);
            parameters.AddInputParameter("@Secondname", SqlDbType.NVarChar, data.Secondname);
            parameters.AddInputParameter("@Lastname", SqlDbType.NVarChar, data.Lastname);
            parameters.AddInputParameter("@LID", SqlDbType.BigInt, data.LID);
            parameters.AddInputParameter("@OGRN", SqlDbType.NVarChar, data.OGRN);
            parameters.AddInputParameter("@PolicyNumber", SqlDbType.NVarChar, data.PolicyNumber);
            parameters.AddInputParameter("@PolicySeries", SqlDbType.NVarChar, data.PolicySeries);
            parameters.AddInputParameter("@PolicyTypeId", SqlDbType.BigInt, data.PolicyTypeId);
            parameters.AddInputParameter("@SMO_ID", SqlDbType.Int, data.SMO_ID);
            parameters.AddInputParameter("@SNILS", SqlDbType.NVarChar, data.SNILS);
            parameters.AddInputParameter("@TemporaryPolicyDate", SqlDbType.DateTime, data.TemporaryPolicyDate);
            parameters.AddInputParameter("@TerritoryCode", SqlDbType.NVarChar, data.TerritoryCode);
            parameters.AddInputParameter("@CreateDate", SqlDbType.DateTime, date);
            SqlParameter id = parameters.AddOutputParameter("@ID", SqlDbType.BigInt);
            Execute_StoredProcedure("FundErrorResponse_Create", parameters);
            return (long)id.Value;
        }

        public FundResponse FundResponse_Get(long responseId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@Id", SqlDbType.BigInt, responseId);
            return Execute_Get<FundResponse>(FundResponseMaterializer.Instance, "FundResponse_Get", parameters);
        }

        public int MonthPackageNumber_Get(DateTime? date)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@Date", SqlDbType.DateTime, date);
            SqlParameter nextNumber = parameters.AddOutputParameter("@NextNumber", SqlDbType.Int);
            Execute_StoredProcedure("NextMonthPackageNumber_Get", parameters);
            return (int)nextNumber.Value;
        }

        public void ClientVisit_SetReadyToFundSubmitRequest(long id, bool isReady, string message, DateTime date)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@ClientVisitID", SqlDbType.BigInt, id);
            parameters.AddInputParameter("@IsReadyToFundSubmitRequest", SqlDbType.Bit, isReady);
            parameters.AddInputParameter("@FundResponseApplyingMessage", SqlDbType.NVarChar, message);
            parameters.AddInputParameter("@SaveDate", SqlDbType.DateTime, date);
            Execute_StoredProcedure("ClientVisit_SetReadyToFundSubmitRequest", parameters);
        }

        public void ClientVisits_SetDifficultCase(long id, bool IsDifficultCase, DateTime date)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@ClientVisitID", SqlDbType.BigInt, id);
            parameters.AddInputParameter("@IsDifficultCase", SqlDbType.Bit, IsDifficultCase);
            parameters.AddInputParameter("@SaveDate", SqlDbType.DateTime, date);
            Execute_StoredProcedure("ClientVisit_SetDufficultCase", parameters);
        }

        public void FundFileHistory_Save(List<FundFileHistory> list)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            FundFileHistoryTableSet set = new FundFileHistoryTableSet(list);
            parameters.AddInputParameter("@Table", SqlDbType.Structured, set.ResultTable);
            Execute_StoredProcedure("FundFileHistory_Save", parameters);
        }

        public List<FundFileHistory> FundFileHistory_Get(long VisitGroupID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@VisitGroupID", SqlDbType.BigInt, VisitGroupID);
            List<FundFileHistory> result = Execute_GetList(FundFileHistoryMaterializer.Instance, "FundFileHistory_Get", parameters);
            return result;
        }
    }
}
