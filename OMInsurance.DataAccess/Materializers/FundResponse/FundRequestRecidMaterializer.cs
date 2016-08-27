using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMInsurance.DataAccess.Materializers
{
    public class FundRequestRecidMaterializer : IMaterializer<FundRequestRecid>
    {
        private static readonly FundRequestRecidMaterializer _instance = new FundRequestRecidMaterializer();

        public static FundRequestRecidMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }
        public FundRequestRecid Materialize(DataReaderAdapter dataReader)
        {
            return Materialize_List(dataReader).FirstOrDefault();
        }

        public List<FundRequestRecid> Materialize_List(DataReaderAdapter dataReader)
        {
            List<FundRequestRecid> items = new List<FundRequestRecid>();

            while (dataReader.Read())
            {
                FundRequestRecid obj = ReadItemFields(dataReader);
                items.Add(obj);
            }

            return items;
        }

        public FundRequestRecid ReadItemFields(DataReaderAdapter dataReader, FundRequestRecid item = null)
        {
            if (item == null)
            {
                item = new FundRequestRecid();
            }

            item.ClientVisitId = dataReader.GetInt32("ClientVisitId");
            item.DataTypeId = dataReader.GetInt32("DataType");
            item.Recid = dataReader.GetInt32("Recid");
            item.CreateDate = dataReader.GetDateTime("CreateDate");
            return item;
        }
    }

}
