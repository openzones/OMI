using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using System.Collections.Generic;
using System.Linq;

namespace OMInsurance.DataAccess.Materializers
{
    public class BSOSumStatusMaterializer : IMaterializer<BSOSumStatus>
    {
        private static readonly BSOSumStatusMaterializer _instance = new BSOSumStatusMaterializer();

        public static BSOSumStatusMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }

        public BSOSumStatus Materialize(DataReaderAdapter reader)
        {
            return Materialize_List(reader).FirstOrDefault();
        }

        public List<BSOSumStatus> Materialize_List(DataReaderAdapter reader)
        {
            List<BSOSumStatus> items = new List<BSOSumStatus>();

            while (reader.Read())
            {
                BSOSumStatus obj = ReadItemFields(reader);
                items.Add(obj);
            }
            return items;
        }

        public BSOSumStatus ReadItemFields(DataReaderAdapter reader, BSOSumStatus item = null)
        {
            if (item == null)
            {
                item = new BSOSumStatus();
            }

            item.Id = reader.GetInt64("StatusId");
            item.Name = reader.GetString("Name");
            item.Count= (long)reader.GetInt32("StatusCount");
            return item;
        }
    }
}
