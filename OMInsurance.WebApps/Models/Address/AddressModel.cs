using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using OMInsurance.WebApps.Models.Core;
using OMInsurance.WebApps.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OMInsurance.WebApps.Models
{
    public class AddressModel : ValidatableModel<AddressModel>
    {
        #region Constructors

        public AddressModel()
        {
            validator = new AddressModelValidator();
        }
        
        public AddressModel(Address address, AddressType type)
            : this(type)
        {
            Id = address.Id;
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
            FullAddressString = address.FullAddressString;
        }

        public AddressModel(AddressType type)
            : this()
        {
            if (type == AddressType.Living)
            {
                RegionId = "0c5b2444-70a0-4932-980c-b4dc0d3f02b5";
                Region = "г Москва";
            } 
            AddressType = type;        
        }

        #endregion

        #region Properties
        public long? Id { get; set; }

        [DisplayName("Код территории")]
        [StringLength(2, ErrorMessage = "Максимальная длина - 2 символа")]
        public string TerritoryCode { get; set; }

        [DisplayName("Код региона")]
        [StringLength(3, ErrorMessage = "Максимальная длина - 3 символа")]
        public string RegionCode { get; set; }

        [DisplayName("Регион")]
        [StringLength(60, ErrorMessage = "Максимальная длина - 60 символов")]
        public string Region { get; set; }
        public string RegionId { get; set; }

        [DisplayName("Район")]
        [StringLength(60, ErrorMessage = "Максимальная длина - 60 символов")]
        public string Area { get; set; }
        public string AreaId { get; set; }

        [DisplayName("Город")]
        [StringLength(60, ErrorMessage = "Максимальная длина - 60 символов")]
        public string City { get; set; }
        public string CityId { get; set; }

        [DisplayName("Населенный пункт")]
        [StringLength(60, ErrorMessage = "Максимальная длина - 60 символов")]
        public string Locality { get; set; }
        public string LocalityId { get; set; }

        [DisplayName("Улица")]
        [StringLength(60, ErrorMessage = "Максимальная длина - 60 символов")]
        public string Street { get; set; }

        [DisplayName("Дом")]
        [RegularExpression(Constants.LatinCyrillicNumber, ErrorMessage = "Неверное значение")]
        [StringLength(7, ErrorMessage = "Максимальная длина - 7 символов")]
        public string House { get; set; }

        [DisplayName("Корпус")]
        [RegularExpression(Constants.LatinCyrillicNumber, ErrorMessage = "Неверное значение")]
        [StringLength(5, ErrorMessage = "Максимальная длина - 5 символов")]
        public string Housing { get; set; }

        [DisplayName("Строение")]
        [RegularExpression(Constants.LatinCyrillicNumber, ErrorMessage = "Неверное значение")]
        [StringLength(5, ErrorMessage = "Максимальная длина - 5 символов")]
        public string Building { get; set; }

        [DisplayName("Квартира")]
        [RegularExpression(Constants.LatinCyrillicNumber, ErrorMessage = "Неверное значение")]
        [StringLength(5, ErrorMessage = "Максимальная длина - 5 символов")]
        public string Appartment { get; set; }

        [DisplayName("Код улицы")]
        [StringLength(6, ErrorMessage = "Максимальная длина - 5 символов")]
        public string StreetCode { get; set; }

        [DisplayName("Индекс")]
        [RegularExpression(Constants.Number, ErrorMessage = "Неверное значение")]
        [StringLength(6, ErrorMessage = "Индекс")]
        public string PostIndex { get; set; }

        [DisplayName("Полный адрес")]
        public string FullAddressString { get; set; }

        public AddressType AddressType { get; set; }

        #endregion

        #region Methods

        public Address.SaveData GetForBLL()
        {
            Address.SaveData data = new Address.SaveData();
            data.Id = this.Id;
            data.TerritoryCode = this.TerritoryCode;
            data.RegionCode = this.RegionCode;
            data.Region = this.Region;
            data.RegionId = this.RegionId;
            data.Area = this.Area;
            data.AreaId = this.AreaId;
            data.Locality = this.Locality;
            data.LocalityId = this.LocalityId;
            data.City = this.City;
            data.CityId = this.CityId;
            data.Street = this.Street;
            data.House = this.House;
            data.Housing = this.Housing;
            data.Building = this.Building;
            data.Appartment = this.Appartment;
            data.StreetCode = this.StreetCode;
            data.FullAddressString = this.FullAddressString;
            return data;
        }
        
        #endregion
    }
}