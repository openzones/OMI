namespace OMInsurance.Entities.Core
{
    public class FiasEntry
    {
        /// <summary>
        /// Main identifier (not GUID because there are some invalid GUID could be)
        /// </summary>
        public string Id { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public FiasType FiasType { get; set; }
        public string RegionCode { get; set; }
        public string AreaCode { get; set; }
    }
}
