using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using System.Collections.Generic;
using System.Linq;


namespace OMInsurance.DataAccess.Materializers
{
    public class ClientPretensionGenerationMaterializer : IMaterializer<ClientPretension>
    {
        private static readonly ClientPretensionGenerationMaterializer _instance = new ClientPretensionGenerationMaterializer();

        public static ClientPretensionGenerationMaterializer Instance
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
            item.Id = dataReader.GetInt64("ID");
            item.Generator = dataReader.GetInt64("Generator");
            item.ClientID = dataReader.GetInt64("ClientID");
            item.LPU_ID = dataReader.GetInt64Null("LPU_ID");
            item.DATE_IN = dataReader.GetDateTimeNull("DATE_IN");
            item.M_nakt = dataReader.GetString("M_nakt");
            item.MedicalCenterId = dataReader.GetInt64Null("MedicalCenterId");
            item.M_mo = dataReader.GetString("M_mo");
            item.M_mcod = dataReader.GetString("M_mcod");
            item.M_period = dataReader.GetString("M_period");
            item.M_snpol = dataReader.GetString("M_snpol");

            return item;
        }
    }
}
