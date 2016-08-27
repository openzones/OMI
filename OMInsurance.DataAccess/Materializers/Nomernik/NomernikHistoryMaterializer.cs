using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using System.Collections.Generic;
using System.Linq;

namespace OMInsurance.DataAccess.Materializers
{
    public class NomernikHistoryMaterializer : IMaterializer<Nomernik.History>
    {
        private static readonly NomernikHistoryMaterializer _instance = new NomernikHistoryMaterializer();

        public static NomernikHistoryMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }
        public Nomernik.History Materialize(DataReaderAdapter reader)
        {
            return Materialize_List(reader).FirstOrDefault();
        }

        public List<Nomernik.History> Materialize_List(DataReaderAdapter reader)
        {
            List<Nomernik.History> items = new List<Nomernik.History>();

            while (reader.Read())
            {
                Nomernik.History obj = ReadItemFields(reader);
                items.Add(obj);
            }
            return items;
        }

        public Nomernik.History ReadItemFields(DataReaderAdapter reader, Nomernik.History item = null)
        {
            if (item == null)
            {
                item = new Nomernik.History();
            }
            try
            {
                item.Id = reader.GetInt64("ID");
                item.LoadDate = reader.GetDateTime("LoadDate");
                item.FileDate = reader.GetDateTime("FileDate");
                item.UserID = reader.GetInt64Null("UserID");
                item.CountAll = reader.GetInt64Null("CountAll");
                item.CountOur = reader.GetInt64Null("CountOur");
                item.CountChange = reader.GetInt64Null("CountChange");
                item.Firstname = reader.GetString("Firstname");
                item.Secondname = reader.GetString("Secondname");
                item.Lastname = reader.GetString("Lastname");
            }
            catch
            {
                item = null;
            }
            return item;
        }
    }
}
