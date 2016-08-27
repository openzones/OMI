namespace OMInsurance.WebApps.Models.PrintedForms
{
    public class PrintedFormsModel
    {
        public PrintedFormsModel()
        {
            PartyJournal = new PartyJournalModel();
            BSOFailForm13 = new BSOFailForm13Model();
            BSOOperativeInformation = new BSOOperativeInformationModel();
            BSOReportForm10 = new BSOReportForm10Model();
            BSOReportForm10Full = new BSOReportForm10FullModel();
            SMSBaseReport = new SMSBaseReportModel();
            AllocationBSO = new AllocationBSOModel();
            BSOMoveReportYear = new BSOMoveReportYearModel();
            ScenarioForm2 = new ScenarioForm2Model();
            ClientVisitReport = new ClientVisitReportModel();
            SNILSReport = new SNILSReportModel();
            StatusReport = new StatusReportModel();
        }

        public string Message { get; set; }
        public PartyJournalModel PartyJournal { get; set; }

        public BSOFailForm13Model BSOFailForm13 { get; set; }

        public BSOOperativeInformationModel BSOOperativeInformation { get; set;}

        public BSOReportForm10Model BSOReportForm10 { get; set; }

        public BSOReportForm10FullModel BSOReportForm10Full { get; set; }

        public SMSBaseReportModel SMSBaseReport { get; set; }

        public AllocationBSOModel AllocationBSO { get;  set; }

        public BSOMoveReportYearModel BSOMoveReportYear { get; set; }

        //Отчет по сценариям за месяц, task-9137
        public ScenarioForm2Model ScenarioForm2 { get; set; }

        public ClientVisitReportModel ClientVisitReport { get; set; }

        //Отчет - Заявки без СНИЛС, task 10084
        public SNILSReportModel SNILSReport { get; set; }

        public StatusReportModel StatusReport { get; set; }
    }
}