using OMInsurance.Entities.Core;

namespace OMInsurance.Entities
{
    public class SMO : DataObject
    {
        public string OKATO { get; set; }
        public string RegionCode { get; set; }
        public string TerritoryName { get; set; }
        public string Shortname { get; set; }
        public string Fullname { get; set; }
        public string OGRN { get; set; }
        public string SMOCode { get; set; }
    }
}
