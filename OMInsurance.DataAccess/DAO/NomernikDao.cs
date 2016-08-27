using OMInsurance.DataAccess.Core;
using OMInsurance.DataAccess.Core.Helpers;
using OMInsurance.DataAccess.Materializers;
using OMInsurance.DBUtils;
using OMInsurance.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
namespace OMInsurance.DataAccess.DAO
{
    public class NomernikDao :ItemDao
    {
        private static NomernikDao _instance = new NomernikDao();

        private NomernikDao()
            : base(DatabaseAliases.OMInsurance, new DatabaseErrorHandler())
        {
        }

        public static NomernikDao Instance
        {
            get
            {
                return _instance;
            }
        }

        public List<NomernikForClient> NomernikClientNOMP_Get(long ClientID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@ClientID", SqlDbType.BigInt, ClientID);
            List<NomernikForClient> list = Execute_GetList(NomernikClientNOMPMaterializer.Instance, "NomernikClientNOMP_Get", parameters);
            return list;
        }

        public List<NomernikForClient> NomernikClientSTOP_Get(long ClientID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@ClientID", SqlDbType.BigInt, ClientID);
            List<NomernikForClient> list = Execute_GetList(NomernikClientSTOPMaterializer.Instance, "NomernikClientSTOP_Get", parameters);
            return list;
        }

        public void NOMP_Save(IEnumerable<NOMP> listNOMP, Nomernik.History nompHistory)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            NOMPTableSet set = new NOMPTableSet(listNOMP);
            parameters.AddInputParameter("@nompTable", SqlDbType.Structured, set.NOMPResultTable);
            parameters.AddInputParameter("@LoadDate", SqlDbType.DateTime, nompHistory.LoadDate);
            parameters.AddInputParameter("@FileDate", SqlDbType.DateTime, nompHistory.FileDate);
            parameters.AddInputParameter("@UserID", SqlDbType.BigInt, nompHistory.UserID);
            parameters.AddInputParameter("@CountAll", SqlDbType.BigInt, nompHistory.CountAll);
            parameters.AddInputParameter("@CountOur", SqlDbType.BigInt, nompHistory.CountOur);
            parameters.AddInputParameter("@CountChange", SqlDbType.BigInt, listNOMP.Count());
            //в процедуре идет запись данных в таблицу NOMP и истории в NOMPHistory
            Execute_StoredProcedure("NOMP_Save", parameters);
        }

        public void STOP_Save(IEnumerable<STOP> listSTOP, Nomernik.History nompHistory)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            STOPTableSet set = new STOPTableSet(listSTOP);
            parameters.AddInputParameter("@stopTable", SqlDbType.Structured, set.STOPResultTable);
            parameters.AddInputParameter("@LoadDate", SqlDbType.DateTime, nompHistory.LoadDate);
            parameters.AddInputParameter("@FileDate", SqlDbType.DateTime, nompHistory.FileDate);
            parameters.AddInputParameter("@UserID", SqlDbType.BigInt, nompHistory.UserID);
            parameters.AddInputParameter("@CountAll", SqlDbType.BigInt, nompHistory.CountAll);
            parameters.AddInputParameter("@CountOur", SqlDbType.BigInt, nompHistory.CountOur);
            parameters.AddInputParameter("@CountChange", SqlDbType.BigInt, listSTOP.Count());
            //в процедуре идет запись данных в таблицу STOP и истории в STOPHistory
            Execute_StoredProcedure("STOP_Save", parameters);
        }

        public List<Nomernik.ClientShotInfo> ClientsShotInfo_Get(IEnumerable<long> listClientIds)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.AddInputParameter("@ClientIDs", SqlDbType.Structured, DaoHelper.GetObjectIds(listClientIds));
            List<Nomernik.ClientShotInfo> list = Execute_GetList(NomernikClientShotInfoMaterializer.Instance, "NomernikClientsShotInfo_Get", parameters);
            return list;
        }

        public Nomernik.History NOMPHistory_Get()
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter result = parameters.AddInputOutputParameter("@result", SqlDbType.BigInt, 0);
            try
            {
                Nomernik.History history = Execute_Get(NomernikHistoryMaterializer.Instance, "NOMPHistory_Get", parameters);
                return history;
            }
            catch
            {
                return null;
            }
        }

        public Nomernik.History STOPHistory_Get()
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter result = parameters.AddInputOutputParameter("@result", SqlDbType.BigInt, 0);
            try
            {
                Nomernik.History history = Execute_Get(NomernikHistoryMaterializer.Instance, "STOPHistory_Get", parameters);
                return history;
            }
            catch
            {
                return null;
            }
        }

        public List<NOMP> NOMP_Find(List<NOMP> list)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            NOMPTableSet set = new NOMPTableSet(list);
            parameters.AddInputParameter("@ENP", SqlDbType.Structured, set.NOMPResultTable);
            List<NOMP> result = Execute_GetList(NomernikNOMPMaterializer.Instance, "NOMP_Find", parameters);
            return result;
        }

        public List<STOP> STOP_Find(List<STOP> list)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            STOPTableSet set = new STOPTableSet(list);
            parameters.AddInputParameter("@ENP", SqlDbType.Structured, set.STOPResultTable);
            List<STOP> result = Execute_GetList(NomernikSTOPMaterializer.Instance, "STOP_Find", parameters);
            return result;
        }

        public List<NOMP> GetDataFromNOMPDbf(string filename)
        {
            DBFProcessor pr = new DBFProcessor();
            List<NOMP> result = new List<NOMP>();
            try
            {
                DataTable table = DBFProcessor.GetDataTable(filename, string.Format("select * from \"{0}\" where Q='P2';", filename));
                foreach (DataRow row in table.Rows)
                {
                    NOMP data = new NOMP();
                    data.S_CARD = (string)(row["S_CARD"]);
                    data.N_CARD = ((decimal)row["N_CARD"]).ToString();
                    data.ENP = (string)row["ENP"];
                    data.VSN = (string)row["VSN"];
                    data.LPU_ID = (long?)((decimal)row["LPU_ID"]);
                    data.DATE_IN = (DateTime?)row["DATE_IN"];
                    data.SPOS = (int?)((decimal)row["SPOS"]);
                    result.Add(data);
                }
            }
            finally
            {
                File.Delete(filename);
            }
            return result;
        }

        public List<STOP> GetDataFromSTOPDbf(string filename)
        {
            DBFProcessor pr = new DBFProcessor();
            List<STOP> result = new List<STOP>();
            try
            {
                DataTable table = DBFProcessor.GetDataTable(filename, string.Format("select * from \"{0}\" where Q=3386;", filename));
                foreach (DataRow row in table.Rows)
                {
                    STOP data = new STOP();
                    data.SCENARIO = (string)(row["SCENARIO"]);
                    data.S_CARD = (string)(row["S_CARD"]);
                    data.N_CARD = ((decimal)row["N_CARD"]).ToString();
                    data.ENP = (string)row["ENP"];
                    data.VSN = ((string)row["VSN"]).Trim();
                    data.QZ = (long?)((decimal)row["QZ"]);
                    data.DATE_END = (DateTime?)row["DATE_END"];
                    data.DATE_ARC = (DateTime?)row["DATE_ARC"];
                    data.IST = (string)row["IST"];
                    result.Add(data);
                }
            }
            finally
            {
                File.Delete(filename);
            }
            return result;
        }

        public long? GetAllRowCount(string filename)
        {
            try
            {
                DataTable table = DBFProcessor.GetDataTable(filename, string.Format("select count(1) from \"{0}\" ;", filename));
                return (long)((decimal)((table.Rows[0])[0]));
            }
            catch
            {
                return null;
            }
        }

        public long? GetOurRowCount(string filename, string param)
        {
            try
            {
                DataTable table = DBFProcessor.GetDataTable(filename, string.Format("select count(1) from \"{0}\" where Q={1};", filename, param));
                return (long)((decimal)((table.Rows[0])[0]));
            }
            catch
            {
                return null;
            }
        }
    }
}
