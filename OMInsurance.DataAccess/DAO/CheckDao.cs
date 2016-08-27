using OMInsurance.DataAccess.Core;
using OMInsurance.DataAccess.Core.Helpers;
using OMInsurance.DataAccess.Materializers;
using OMInsurance.Entities;
using OMInsurance.Entities.Check;
using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using OMInsurance.Entities.Searching;
using OMInsurance.Entities.Sorting;


namespace OMInsurance.DataAccess.DAO
{
    public class CheckDao : ItemDao
    {
        private static CheckDao _instance = new CheckDao();

        private CheckDao()
            : base(DatabaseAliases.OMInsurance, new DatabaseErrorHandler())
        {
        }

        public static CheckDao Instance
        {
            get
            {
                return _instance;
            }
        }

        public List<CheckClient> Check_Client(CheckClientSearchCriteria criteria)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@IsLastname", SqlDbType.Bit, criteria.IsLastname);
            parameters.AddInputParameter("@IsFirstname", SqlDbType.Bit, criteria.IsFirstname);
            parameters.AddInputParameter("@IsSecondname", SqlDbType.Bit, criteria.IsSecondname);
            parameters.AddInputParameter("@IsBirthday", SqlDbType.Bit, criteria.IsBirthday);
            parameters.AddInputParameter("@IsSex", SqlDbType.Bit, criteria.IsSex);
            parameters.AddInputParameter("@IsPolicySeries", SqlDbType.Bit, criteria.IsPolicySeries);
            parameters.AddInputParameter("@IsPolicyNumber", SqlDbType.Bit, criteria.IsPolicyNumber);
            parameters.AddInputParameter("@IsUnifiedPolicyNumber", SqlDbType.Bit, criteria.IsUnifiedPolicyNumber);
            parameters.AddInputParameter("@IsDocumentSeries", SqlDbType.Bit, criteria.IsDocumentSeries);
            parameters.AddInputParameter("@IsDocumentNumber", SqlDbType.Bit, criteria.IsDocumentNumber);
            List<CheckClient> result = Execute_GetList(CheckClientMaterializer.Instance, "Check_Client", parameters);
            return result;
        }

        public List<FundFileHistory> FundFileHistory_Find(CheckFileHistorySearchCriteria criteria)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@DateFrom", SqlDbType.DateTime, criteria.DateFrom);
            parameters.AddInputParameter("@DateTo", SqlDbType.DateTime, criteria.DateTo);
            parameters.AddInputParameter("@StatusId", SqlDbType.BigInt, criteria.StatusId);
            parameters.AddInputParameter("@UserId", SqlDbType.BigInt, criteria.UserId);
            List<FundFileHistory> result = Execute_GetList(FundFileHistoryMaterializer.Instance, "FundFileHistory_Find", parameters);
            return result;
        }

        public List<ClientPretension> ClientPretension_Find(CheckPretensionSearchCriteria criteria)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@M_daktFrom", SqlDbType.DateTime, criteria.M_daktFrom);
            parameters.AddInputParameter("@M_daktTo", SqlDbType.DateTime, criteria.M_daktTo);
            parameters.AddInputParameter("@CreateDateFrom", SqlDbType.DateTime, criteria.CreateDateFrom);
            parameters.AddInputParameter("@CreateDateTo", SqlDbType.DateTime, criteria.CreateDateTo);
            parameters.AddInputParameter("@UserId", SqlDbType.BigInt, criteria.UserId);
            List<ClientPretension> result = Execute_GetList(ClientPretensionMaterializer.Instance, "ClientPretension_Find", parameters);
            return result;
        }
    }
}
