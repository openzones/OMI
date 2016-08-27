using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using System.Collections.Generic;
using System.Linq;

namespace OMInsurance.DataAccess.Materializers
{
    internal class ClientMaterializer : IMaterializer<Client>
    {
        private static readonly ClientMaterializer _instance = new ClientMaterializer();

        public static ClientMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }

        public Client Materialize(DataReaderAdapter reader)
        {
            return Materialize_List(reader).FirstOrDefault();
        }

        public List<Client> Materialize_List(DataReaderAdapter reader)
        {
            List<Client> items = new List<Client>();
            Dictionary<long, Client> clientsById = new Dictionary<long, Client>();

            while (reader.Read())
            {
                Client obj = ReadItemFields(reader);
                clientsById.Add(obj.Id, obj);
                items.Add(obj);
            }

            reader.NextResult();

            while (reader.Read())
            {
                long clientId = reader.GetInt64("ClientId");
                Client obj = clientsById[clientId];
                obj.Versions.Add(ClientVersionMaterializer.Instance.ReadItemFields(reader));
            }

            reader.NextResult();

            while (reader.Read())
            {
                long clientId = reader.GetInt64("ClientId");
                Client obj = clientsById[clientId];
                obj.Visits.Add(ClientVisitInfoMaterializer.Instance.ReadItemFields(reader));
            }

            reader.NextResult();

            while (reader.Read())
            {
                long clientId = reader.GetInt64("ClientId");
                Client obj = clientsById[clientId];
                obj.ListSms.Add(SmsMaterializer.Instance.ReadItemFields(reader));
            }

            return items;
        }

        public Client ReadItemFields(DataReaderAdapter reader, Client item = null)
        {
            if (item == null)
            {
                item = new Client();
            }

            item.Id = reader.GetInt64("ID");
            item.ActualVersion = ClientVersionMaterializer.Instance.ReadItemFields(reader);

            return item;
        }
    }
}
