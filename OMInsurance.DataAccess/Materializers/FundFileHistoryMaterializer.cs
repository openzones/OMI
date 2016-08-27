using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using System.Collections.Generic;
using System.Linq;

namespace OMInsurance.DataAccess.Materializers
{
    public class FundFileHistoryMaterializer : IMaterializer<FundFileHistory>
    {
        private static readonly FundFileHistoryMaterializer _instance = new FundFileHistoryMaterializer();

        public static FundFileHistoryMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }

        public FundFileHistory Materialize(DataReaderAdapter reader)
        {
            return Materialize_List(reader).FirstOrDefault();
        }

        public List<FundFileHistory> Materialize_List(DataReaderAdapter reader)
        {
            List<FundFileHistory> items = new List<FundFileHistory>();

            while (reader.Read())
            {
                FundFileHistory obj = ReadItemFields(reader);
                items.Add(obj);
            }
            return items;
        }

        public FundFileHistory ReadItemFields(DataReaderAdapter reader, FundFileHistory item = null)
        {
            if (item == null)
            {
                item = new FundFileHistory();
            }
            item.ClientID = reader.GetInt64("ClientID");
            item.VisitGroupID = reader.GetInt64("VisitGroupID");
            item.ClientVisitID = reader.GetInt64("ClientVisitID");
            item.StatusID = reader.GetInt64("StatusID");
            item.Date = reader.GetDateTime("Date");
            item.UserID = reader.GetInt64("UserID");
            item.FileName = reader.GetString("FileName");
            item.FileUrl = reader.GetString("FileUrl");
            
            return item;
        }
    }
}
