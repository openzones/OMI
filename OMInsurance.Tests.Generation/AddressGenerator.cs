using OMInsurance.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMInsurance.Tests.Generation
{
    public static class AddressGenerator
    {
        public static Address.SaveData GetAddressSaveData(long? addressId)
        {
            Address.SaveData data = new Address.SaveData();
            data.Id = addressId;
            data.Appartment = "1";
            data.Region = "Москва г";
            data.RegionId = "Москва г";
            data.Building = "3";
            data.Area = "Московский";
            data.House = "5";
            data.Housing = "6";
            data.Locality = "13";
            data.RegionCode = "65";
            data.Street = "Московская";
            data.StreetCode = "14";
            data.TerritoryCode = "12";
            data.FullAddressString = string.Format("{0} {1} {2} {3} {4}", data.Region, data.Building, data.Area, data.Locality, data.Street);
            return data;
        }
    }
}
