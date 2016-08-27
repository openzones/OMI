using OMInsurance.DataAccess.Core;
using OMInsurance.Entities;
using System.Collections.Generic;
using System.Linq;

namespace OMInsurance.DataAccess.Materializers
{
    public class AddressMaterializer : IMaterializer<Address>
    {
        private static readonly AddressMaterializer _instance = new AddressMaterializer();

        public static AddressMaterializer Instance
        {
            get
            {
                return _instance;
            }
        }
        public Address Materialize(DataReaderAdapter dataReader)
        {
            return Materialize_List(dataReader).FirstOrDefault();
        }

        public List<Address> Materialize_List(DataReaderAdapter dataReader)
        {
            List<Address> items = new List<Address>();

            while (dataReader.Read())
            {
                Address obj = ReadItemFields(dataReader);
                items.Add(obj);
            }

            return items;
        }

        public Address ReadItemFields(DataReaderAdapter dataReader, Address item = null)
        {
            if (item == null)
            {
                item = new Address();
            }
            item.Id = dataReader.GetInt64("ID");

            item.RegionCode = dataReader.GetString("RegionCode");
            item.TerritoryCode = dataReader.GetString("TerritoryCode");
            item.AreaId = dataReader.GetString("AreaId");
            item.RegionId = dataReader.GetString("RegionId");
            item.CityId = dataReader.GetString("CityId");
            item.LocalityId = dataReader.GetString("LocalityId");
            item.Area = dataReader.GetString("Area");
            item.Region = dataReader.GetString("Region");
            item.Locality = dataReader.GetString("Locality");
            item.City = dataReader.GetString("City");
            item.Street = dataReader.GetString("Street");
            item.House = dataReader.GetString("House");
            item.Housing = dataReader.GetString("Housing");
            item.Building = dataReader.GetString("Building");
            item.Appartment = dataReader.GetString("Appartment");
            item.StreetCode = dataReader.GetString("StreetCode");
            item.PostIndex = dataReader.GetString("PostIndex");
            item.FullAddressString = dataReader.GetString("FullAddressString");

            return item;
        }
    }
}
