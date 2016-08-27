using OMInsurance.DataAccess.Core;
using OMInsurance.DataAccess.Core.Helpers;
using OMInsurance.DataAccess.Materializers;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using OMInsurance.Entities.Searching;
using OMInsurance.Entities.Sorting;

namespace OMInsurance.DataAccess.DAO
{
    public class BSODao : ItemDao
    {
        private static BSODao _instance = new BSODao();

        private BSODao()
            : base(DatabaseAliases.OMInsurance, new DatabaseErrorHandler())
        {
        }

        public static BSODao Instance
        {
            get
            {
                return _instance;
            }
        }


        public List<BSOHistoryItem> BSO_GetListHistory(DateTime StatusDateFrom, DateTime StatusDateTo, List<long> BSO_IDs)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@StatusDateFrom", SqlDbType.DateTime, StatusDateFrom);
            parameters.AddInputParameter("@StatusDateTo", SqlDbType.DateTime, StatusDateTo);
            var temp = DaoHelper.GetObjectIds(BSO_IDs);
            parameters.AddInputParameter("@BSO_IDs", SqlDbType.Structured, DaoHelper.GetObjectIds(BSO_IDs));
            List<BSOHistoryItem> result = Execute_GetList(BSOHistoryItemMaterializer.Instance, "BSO_GetListHistory", parameters);
            return result;
        }

        public List<BSOSumStatus> BSO_GetSumAllStatus(DateTime data)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@StatusDate", SqlDbType.DateTime, data);
            List<BSOSumStatus> result = Execute_GetList(BSOSumStatusMaterializer.Instance, "BSO_GetSumAllStatus", parameters);
            return result;
        }


        public long BSO_Save(BSO.SaveData data)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            //parameters.AddInputParameter("@BSO_Id", SqlDbType.BigInt, data.Id);
            parameters.AddInputParameter("@TemporaryPolicyNumber", SqlDbType.NVarChar, data.TemporaryPolicyNumber);
            parameters.AddInputParameter("@PolicyPartyNumber", SqlDbType.NVarChar, data.PolicyPartyNumber);
            parameters.AddInputParameter("@StatusId", SqlDbType.BigInt, data.StatusId);
            parameters.AddInputParameter("@StatusDate", SqlDbType.DateTime, data.StatusDate);
            parameters.AddInputParameter("@DeliveryCenterId", SqlDbType.BigInt, data.DeliveryCenterId);
            parameters.AddInputParameter("@DeliveryPointId", SqlDbType.BigInt, data.DeliveryPointId);
            parameters.AddInputParameter("@VisitGroupId", SqlDbType.BigInt, data.VisitGroupId);
            parameters.AddInputParameter("@Comment", SqlDbType.NVarChar, data.Comment);
            parameters.AddInputParameter("@UserID", SqlDbType.BigInt, data.UserId);
            parameters.AddInputParameter("@ResponsibleID", SqlDbType.BigInt, data.ResponsibleID);
            parameters.AddInputParameter("@ChangeDate", SqlDbType.DateTime, DateTime.Now);
            SqlParameter BSO_ID = parameters.AddInputOutputParameter("@BSO_ID", SqlDbType.BigInt, data.Id);
            Execute_StoredProcedure("BSO_Save", parameters);
            return (long)BSO_ID.Value;
        }

        public void BSO_Set(BSO.SaveData data)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@BSO_Id", SqlDbType.BigInt, data.Id);
            parameters.AddInputParameter("@StatusId", SqlDbType.BigInt, data.StatusId);
            parameters.AddInputParameter("@DeliveryPointId", SqlDbType.BigInt, data.DeliveryPointId);
            parameters.AddInputParameter("@StatusDate", SqlDbType.DateTime, data.StatusDate);
            parameters.AddInputParameter("@Comment", SqlDbType.NVarChar, data.Comment);
            parameters.AddInputParameter("@UserID", SqlDbType.BigInt, data.UserId);
            parameters.AddInputParameter("@ResponsibleID", SqlDbType.BigInt, data.ResponsibleID);
            parameters.AddInputParameter("@ChangeDate", SqlDbType.DateTime, DateTime.Now);
            Execute_StoredProcedure("BSO_Set", parameters);
        }

        public  BSO BSO_GetByID(long bso_id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@BSO_ID", SqlDbType.BigInt, bso_id);
            BSO bso = Execute_Get(BSOMaterializer.Instance, "BSO_GetByID", parameters);
            return bso;
        }

        public BSO BSO_GetByNumber(string temporaryPolicyNumber)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@TemporaryPolicyNumber", SqlDbType.NVarChar, temporaryPolicyNumber);
            BSO bso = Execute_Get(BSOMaterializer.Instance, "BSO_GetByNumber", parameters);
            return bso;
        }

        public List<BSOStatusRef> BSO_GetListStatus()
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<BSOStatusRef> listBsoStatus = Execute_GetList(BSOStatusRefMaterializer.Instance, "BSO_GetListStatus", parameters);
            return listBsoStatus;

        }

         public DataPage<BSOInfo> BSO_Find(
            BSOSearchCriteria criteria,
            List<SortCriteria<BSOSortField>> sortCriteria,
            PageRequest pageRequest)
        { 
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@TemporaryPolicyNumberFrom", SqlDbType.NVarChar, criteria.TemporaryPolicyNumberFrom);
            parameters.AddInputParameter("@TemporaryPolicyNumberTo", SqlDbType.NVarChar, criteria.TemporaryPolicyNumberTo);
            parameters.AddInputParameter("@PolicyPartyNumber", SqlDbType.NVarChar, criteria.PolicyPartyNumber);
            parameters.AddInputParameter("@StatusId", SqlDbType.BigInt, criteria.StatusId);
            parameters.AddInputParameter("@ResponsibleID", SqlDbType.BigInt, criteria.ResponsibleID);
            parameters.AddInputParameter("@StatusDateFrom", SqlDbType.Date, criteria.StatusDateFrom);
            parameters.AddInputParameter("@StatusDateTo", SqlDbType.Date, criteria.StatusDateTo);
            parameters.AddInputParameter("@ChangeDateFrom", SqlDbType.DateTime, criteria.ChangeDateFrom);
            parameters.AddInputParameter("@ChangeDateTo", SqlDbType.DateTime, criteria.ChangeDateTo);

            parameters.AddInputParameter("@DeliveryCenterIds", SqlDbType.Structured, DaoHelper.GetObjectIds(criteria.DeliveryCenterIds));
            parameters.AddInputParameter("@DeliveryPointIds", SqlDbType.Structured, DaoHelper.GetObjectIds(criteria.DeliveryPointIds));
            //parameters.AddInputParameter("@ResponsibleIDs", SqlDbType.Structured, DaoHelper.GetObjectIds(criteria.ResponsibleIDs));

            SqlParameter totalCountParameter = parameters.AddOutputParameter("@total_count", SqlDbType.Int);
            parameters.AddInputParameter("@sort_criteria", SqlDbType.Structured, DaoHelper.GetSortFieldsTable(sortCriteria));
            parameters.AddInputParameter("@Page_size", SqlDbType.Int, pageRequest.PageSize);
            parameters.AddInputParameter("@Page_number", SqlDbType.Int, pageRequest.PageNumber);

            List<BSOInfo> bsos = Execute_GetList(BSOInfoMaterializer.Instance, "BSO_Find", parameters);
            return DaoHelper.GetDataPage(bsos, totalCountParameter, pageRequest);
         }
    }
}
