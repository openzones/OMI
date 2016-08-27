using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMInsurance.DataAccess.Materializers
{
    class ClientVisitHistoryItemMaterializer : IMaterializer<ClientVisitHistoryItem>
    {
        private static readonly ClientVisitHistoryItemMaterializer _instance = new ClientVisitHistoryItemMaterializer();

        public static ClientVisitHistoryItemMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }

        public ClientVisitHistoryItem Materialize(DataReaderAdapter reader)
        {
            return Materialize_List(reader).FirstOrDefault();
        }

        public List<ClientVisitHistoryItem> Materialize_List(DataReaderAdapter reader)
        {
            List<ClientVisitHistoryItem> items = new List<ClientVisitHistoryItem>();

            while (reader.Read())
            {
                ClientVisitHistoryItem obj = ReadItemFields(reader);
                items.Add(obj);
            }
            return items;
        }

        public ClientVisitHistoryItem ReadItemFields(DataReaderAdapter reader, ClientVisitHistoryItem item = null)
        {
            if (item == null)
            {
                item = new ClientVisitHistoryItem();
            }

            item.ClientVisitId = reader.GetInt64("ClientVisitId");
            item.Status = ReferencesMaterializer.Instance.ReadItemFields(reader, "StatusId", "StatusCode", "StatusName");
            item.StatusDate = reader.GetDateTime("StatusDate");
            item.UserId = reader.GetInt64Null("UserId");
            item.UserLogin = reader.GetString("UserLogin");
            item.UserFirstname = reader.GetString("UserFirstname");
            item.UserLastname = reader.GetString("UserLastname");
            item.UserSecondname = reader.GetString("UserSecondname");

            return item;
        }
    }
}