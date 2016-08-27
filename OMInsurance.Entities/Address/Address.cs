using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OMInsurance.Entities.Core;

namespace OMInsurance.Entities
{
    public class Address : DataObject
    {
        #region Properties

        public string TerritoryCode { get; set; }
        public string RegionCode { get; set; }
        public string Region { get; set; }
        public string RegionId { get; set; }
        public string Area { get; set; }
        public string AreaId { get; set; }
        public string City { get; set; }
        public string CityId { get; set; }
        public string Locality { get; set; }
        public string LocalityId { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Housing { get; set; }
        public string Building { get; set; }
        public string Appartment { get; set; }
        public string StreetCode { get; set; }
        public string FullAddressString { get; set; }
        public string PostIndex { get; set; }
        #endregion

        #region Constructors

        #endregion

        public class SaveData
        {
            public SaveData()
            {
            }
            public SaveData(Address address)
            {
                TerritoryCode = address.TerritoryCode;
                RegionCode = address.RegionCode;
                Region = address.Region;
                RegionId = address.RegionId;
                Area = address.Area;
                AreaId = address.AreaId;
                City = address.City;
                CityId = address.CityId;
                Locality = address.Locality;
                LocalityId = address.LocalityId;
                Street = address.Street;
                House = address.House;
                Housing = address.Housing;
                Building = address.Building;
                Appartment = address.Appartment;
                StreetCode = address.StreetCode;
                PostIndex = address.PostIndex;
                FullAddressString = address.FullAddressString;
            }
            public long? Id { get; set; }
            public string TerritoryCode { get; set; }
            public string RegionCode { get; set; }
            public string Region { get; set; }
            public string RegionId { get; set; }
            public string Area { get; set; }
            public string AreaId { get; set; }
            public string City { get; set; }
            public string CityId { get; set; }
            public string Locality { get; set; }
            public string LocalityId { get; set; }
            public string Street { get; set; }
            public string House { get; set; }
            public string Housing { get; set; }
            public string Building { get; set; }
            public string Appartment { get; set; }
            public string StreetCode { get; set; }
            public string PostIndex { get; set; }
            private string fulladdress;
            public string FullAddressString
            {
                get
                {
                    if (string.IsNullOrEmpty(fulladdress))
                    {
                        fulladdress = this.FillFullAddress();
                    }
                    return fulladdress;
                }
                set
                { 
                    fulladdress = value; 
                }
            }

            private string FillFullAddress()
            {
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(this.Region))
                {
                    sb.Append(this.Region);
                }
                if (!string.IsNullOrEmpty(this.Area))
                {
                    sb.Append(", " + this.Area);
                }
                if (!string.IsNullOrEmpty(this.City))
                {
                    sb.Append(", " + this.City);
                }
                if (!string.IsNullOrEmpty(this.Locality))
                {
                    sb.Append(", " + this.Locality);
                }
                if (!string.IsNullOrEmpty(this.Street))
                {
                    sb.Append(", " + this.Street);
                }
                if (!string.IsNullOrEmpty(this.House))
                {
                    sb.Append(", д " + this.House);
                }
                if (!string.IsNullOrEmpty(this.Housing))
                {
                    sb.Append(", к " + this.Housing);
                }
                if (!string.IsNullOrEmpty(this.Building))
                {
                    sb.Append(", стр " + this.Building);
                }
                if (!string.IsNullOrEmpty(this.Appartment))
                {
                    sb.Append(", кв " + this.Appartment);
                }
                return sb.ToString();
            }
        }
    }
}
