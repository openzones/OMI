using System;

namespace OMInsurance.Entities
{
    public class FundFileHistory
    {
        public long ClientID { get; set; }
        public long VisitGroupID { get; set; }
        public long ClientVisitID { get; set; }
        public long StatusID { get; set; }
        public DateTime Date { get; set; }
        public long UserID { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }

        public FundFileHistory()
        {
        }

        public FundFileHistory(ClientVisitInfo clientVisit, FundFileHistory template)
        {
            this.ClientID = clientVisit.ClientId;
            this.VisitGroupID = clientVisit.VisitGroupId;
            this.ClientVisitID = clientVisit.Id;
            this.StatusID = template.StatusID;
            this.Date = DateTime.Now;
            this.UserID = template.UserID;
            this.FileName = template.FileName;
            this.FileUrl = template.FileUrl;
        }

        public FundFileHistory(ClientVisit.UpdateResultData item, FundFileHistory template)
        {
            this.ClientID = item.ClientId;
            this.VisitGroupID = item.ClientVisitGroupId;
            this.ClientVisitID = item.Id;
            this.StatusID = template.StatusID;
            this.Date = DateTime.Now;
            this.UserID = template.UserID;
            this.FileName = template.FileName;
            this.FileUrl = template.FileUrl;
        }

        public FundFileHistory(FundResponse.UploadReportData item, FundFileHistory template)
        {
            this.ClientID = item.ClientId;
            this.VisitGroupID = item.VisitGroupId;
            this.ClientVisitID = item.ClientVisitId;
            this.StatusID = template.StatusID;
            this.Date = DateTime.Now;
            this.UserID = template.UserID;
            this.FileName = template.FileName;
            this.FileUrl = template.FileUrl;
        }
    }
}
