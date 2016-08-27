using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using System.Collections.Generic;
using System.Linq;

namespace OMInsurance.DataAccess.Materializers
{
    public class ClientPretensionMaterializer : IMaterializer<ClientPretension>
    {
        private static readonly ClientPretensionMaterializer _instance = new ClientPretensionMaterializer();

        public static ClientPretensionMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }

        public ClientPretension Materialize(DataReaderAdapter dataReader)
        {
            return Materialize_List(dataReader).FirstOrDefault();
        }

        public List<ClientPretension> Materialize_List(DataReaderAdapter dataReader)
        {
            List<ClientPretension> items = new List<ClientPretension>();

            while (dataReader.Read())
            {
                ClientPretension obj = ReadItemFields(dataReader);
                items.Add(obj);
            }
            return items;
        }

        public ClientPretension ReadItemFields(DataReaderAdapter dataReader, ClientPretension item = null)
        {
            if (item == null)
            {
                item = new ClientPretension();
            }
            item.Generator = dataReader.GetInt64("Generator");
            item.ClientID = dataReader.GetInt64("ClientID");
            item.LPU_ID = dataReader.GetInt64Null("LPU_ID");
            item.DATE_IN = dataReader.GetDateTimeNull("DATE_IN");
            item.M_nakt = dataReader.GetString("M_nakt");
            item.M_dakt = dataReader.GetDateTimeNull("M_dakt");
            item.M_expert_Id = dataReader.GetInt64Null("M_expert_Id");
            item.MedicalCenterId = dataReader.GetInt64Null("MedicalCenterId");
            item.M_mo = dataReader.GetString("M_mo");
            item.M_mcod = dataReader.GetString("M_mcod");
            item.MCOD = dataReader.GetString("MCOD");
            item.M_period = dataReader.GetString("M_period");
            item.M_snpol = dataReader.GetString("M_snpol");
            item.M_fd = dataReader.GetString("M_fd");
            item.M_nd1 = dataReader.GetString("M_nd1");
            item.M_nd2 = dataReader.GetString("M_nd2");
            item.IsConfirm = dataReader.GetBooleanNull("IsConfirm");
            item.M_osn230_Id = dataReader.GetInt64Null("M_osn230_Id");
            item.M_straf = dataReader.GetFloatNull("M_straf");
            item.PeriodFrom = dataReader.GetDateTimeNull("PeriodFrom");
            item.PeriodTo = dataReader.GetDateTimeNull("PeriodTo");
            item.Coefficient = dataReader.GetFloatNull("Coefficient");
            item.UserId = dataReader.GetInt64Null("UserId");
            item.UserFIO = dataReader.GetString("UserFIO");
            item.UserPosition = dataReader.GetString("Position");
            item.CreateDate = dataReader.GetDateTime("CreateDate");
            item.UpdateUserId = dataReader.GetInt64Null("UpdateUserId");
            item.UpdateUserFIO = dataReader.GetString("UpdateUserFIO");
            item.UpdateDate = dataReader.GetDateTimeNull("UpdateDate");

            item.FileNameLPU = dataReader.GetString("FileNameLPU");
            item.FileUrlLPU = dataReader.GetString("FileUrlLPU");
            item.FileName2 = dataReader.GetString("FileName2");
            item.FileUrl2 = dataReader.GetString("FileUrl2");

            return item;
        }
    }
}
