using OMInsurance.DataAccess.Core;
using OMInsurance.DataAccess.Materializers;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace OMInsurance.DataAccess.DAO
{
    public class ReferencesDao : ItemDao
    {
        private static ReferencesDao _instance = new ReferencesDao();

        private ReferencesDao()
            : base(DatabaseAliases.OMInsurance, new DatabaseErrorHandler())
        {
        }

        public static ReferencesDao Instance
        {
            get
            {
                return _instance;
            }
        }

        public List<ReferenceItem> GetList(string referenceName)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@ReferenceName", SqlDbType.NVarChar, referenceName);
            List<ReferenceItem> items = Execute_GetList<ReferenceItem>(ReferencesMaterializer.Instance, "Reference_GetList", parameters);
            return items;
        }

        public List<ReferenceUniversalItem> GetUniversalList(string referenceName)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@ReferenceName", SqlDbType.NVarChar, referenceName);
            List<ReferenceUniversalItem> items = Execute_GetList<ReferenceUniversalItem>(ReferenceUniversalItemMaterializer.Instance, "Reference_GetUniversalList", parameters);
            return items;
        }

        public void SaveUniversalReferenceItem(ReferenceUniversalItem item, string referenceName, bool flagUpdateOrInsert = false)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@ReferenceName", SqlDbType.NVarChar, referenceName);
            parameters.AddInputParameter("@FlagUpdateOrInsert", SqlDbType.Bit, flagUpdateOrInsert);

            parameters.AddInputParameter("@ID", SqlDbType.BigInt, item.Id);
            parameters.AddInputParameter("@Name", SqlDbType.NVarChar, item.Name );
            parameters.AddInputParameter("@DisplayName", SqlDbType.NVarChar, item.DisplayName);
            parameters.AddInputParameter("@Code", SqlDbType.NVarChar, item.Code);
            parameters.AddInputParameter("@District", SqlDbType.NVarChar, item.District);
            parameters.AddInputParameter("@SMO", SqlDbType.NVarChar, item.SMO);
            parameters.AddInputParameter("@Address", SqlDbType.NVarChar, item.Address);
            parameters.AddInputParameter("@Phone", SqlDbType.NVarChar, item.Phone);
            parameters.AddInputParameter("@WorkHours", SqlDbType.NVarChar, item.WorkHours);
            parameters.AddInputParameter("@IsDigitPolicyAbailable", SqlDbType.Bit, item.IsDigitPolicyAbailable);
            parameters.AddInputParameter("@ParentID", SqlDbType.BigInt, item.ParentId);
            parameters.AddInputParameter("@IsEnabledForOperator", SqlDbType.Bit, item.IsEnabledForOperator);
            parameters.AddInputParameter("@IsEnabledForRegistrator", SqlDbType.Bit, item.IsEnabledForRegistrator);
            parameters.AddInputParameter("@IsMFC", SqlDbType.Bit, item.IsMFC);
            parameters.AddInputParameter("@StartDate", SqlDbType.DateTime, item.StartDate);
            parameters.AddInputParameter("@EndDate", SqlDbType.DateTime, item.EndDate);
            parameters.AddInputParameter("@DeliveryCenterId", SqlDbType.BigInt, item.DeliveryCenterId);
            parameters.AddInputParameter("@DeliveryPointHeadId", SqlDbType.BigInt, item.DeliveryPointHeadId);
            parameters.AddInputParameter("@SendSms", SqlDbType.Bit, item.SendSms);

            parameters.AddInputParameter("@LPU_ID_AIS", SqlDbType.BigInt, item.LPU_ID_AIS);
            parameters.AddInputParameter("@FIL_ID", SqlDbType.BigInt, item.FIL_ID);
            parameters.AddInputParameter("@MCOD", SqlDbType.NVarChar, item.MCOD);
            parameters.AddInputParameter("@FULLNAME", SqlDbType.NVarChar, item.FULLNAME);
            parameters.AddInputParameter("@OGRN", SqlDbType.NVarChar, item.OGRN);
            parameters.AddInputParameter("@FCOD", SqlDbType.NVarChar, item.FCOD);

            parameters.AddInputParameter("@Lastname", SqlDbType.NVarChar, item.Lastname);
            parameters.AddInputParameter("@Firstname", SqlDbType.NVarChar, item.Firstname);
            parameters.AddInputParameter("@Secondname", SqlDbType.NVarChar, item.Secondname);

            parameters.AddInputParameter("@ErrCode", SqlDbType.NVarChar, item.ErrCode);

            Execute_StoredProcedure("Reference_SaveItem", parameters);
        }

        public void DeleteReferenceItem(long id, string referenceName)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@ReferenceName", SqlDbType.NVarChar, referenceName);
            parameters.AddInputParameter("@ID", SqlDbType.BigInt, id);
            Execute_StoredProcedure("Reference_DeleteItem", parameters);
        }

        public List<DeliveryCenter> GetDeliveryCenterList()
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<DeliveryCenter> items = Execute_GetList<DeliveryCenter>(DeliveryCenterMaterializer.Instance, "DeliveryCenter_GetList", parameters);
            return items;
        }

        public HashSet<DateTime> GetHolidays(int? year)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@TargetYear", SqlDbType.Int, year);
            HashSet<DateTime> dates = new HashSet<DateTime>();
            Execute_Reader("Holidays_Get", parameters,
                 (reader) =>
                 {
                     while (reader.Read())
                     {
                         dates.Add(reader.GetDateTime("Date"));
                     }
                 });
            return dates;
        }

        public HashSet<DateTime> GetExceptionalWorkingDays(int? year)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@TargetYear", SqlDbType.Int, year);
            HashSet<DateTime> dates = new HashSet<DateTime>();
            Execute_Reader("WorkingDates_Get", parameters,
                 (reader) =>
                 {
                     while (reader.Read())
                     {
                         dates.Add(reader.GetDateTime("Date"));
                     }
                 });
            return dates;
        }
    }
}
