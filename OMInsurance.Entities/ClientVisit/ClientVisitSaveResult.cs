namespace OMInsurance.Entities
{
    public class ClientVisitSaveResult
    {
        public long ClientID { get; set; }
        public long ClientVisitID { get; set; }
        public long NewClientVersionID { get; set; }
        public long VisitGroupId { get; set; }
    }
}
