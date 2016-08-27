using OMInsurance.Entities;

namespace OMInsurance.WebApps.Models
{
    public class ClientVersionInfoModel 
    {
        public long Id { get; set; }
        public string Fullname { get; set; }

        public ClientVersionInfoModel()
        {
        }

        public ClientVersionInfoModel(ClientVersionInfo clientVersion)
        {
            this.Id = clientVersion.Id;
            this.Fullname = clientVersion.Fullname;
        }
    }
}