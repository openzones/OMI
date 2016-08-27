namespace OMInsurance.Entities.Core
{
    public static class Constants
    {
        #region Text Length Limitations

        /// <summary>
        /// Maximal length that string containing a code may have.
        /// </summary>
        public const int CodeMaxLength = 16;

        /// <summary>
        /// Maximal length that string containing a short name may have.
        /// </summary>
        public const int ShortNameMaxLength = 32;

        /// <summary>
        /// Maximal length that string containing a name may have.
        /// </summary>
        public const int NameMaxLength = 255;

        /// <summary>
        /// Maximal length that string containing a middle name may have.
        /// </summary>
        public const int MiddleNameMaxLength = 128;

        /// <summary>
        /// Maximal length that string containing a long name may have.
        /// </summary>
        public const int LongNameMaxLength = 1024;

        /// <summary>
        /// Maximal length that string containing a big name may have.
        /// </summary>
        public const int BigNameMaxLength = 450;

        /// <summary>
        /// Maximal length that string containing some textual description may have.
        /// </summary>
        public const int DescriptionMaxLength = 4000;

        public const int ExperimentalPlanDescriptionMaxLength = 1073741824;

        #endregion Text Length Limitations

        #region References names

        public const string CodFioClassifier = "CodFioClassifier";
        public const string CitizenshipRef = "CitizenshipRef";
        public const string ClientCategoryRef = "ClientCategoryRef";
        public const string DocumentTypeRef = "DocumentTypeRef";
        public const string PolicyTypeRef = "PolicyTypeRef";
        public const string GOZNAKTypeRef = "GOZNAKTypeRef";
        public const string ApplicationMethodRef = "ApplicationMethodRef";
        public const string CarriersRef = "CarriersRef";

        public const string PolicyAttachmentTypeRef = "PolicyAttachmentTypeRef";
        public const string DeliveryCenterRef = "DeliveryCenterRef";
        public const string DeliveryCenterForOperatorRef = "DeliveryCenterForOperatorRef";
        public const string DeliveryPointRef = "DeliveryPointRef";
        public const string ClientVisitStatusRef = "ClientVisitStatusRef";
        public const string ScenarioRef = "ScenarioRef";
        public const string RepresentativeTypeRef = "RepresentativeTypeRef";
        public const string UralsibClientCategoryRef = "UralsibClientCategoryRef";
        public const string MedicalCenterRef = "MedicalCenterRef";
        public const string UralsibRegionsRef = "UralsibRegionsRef";
        public const string ReferenceRef = "ReferenceRef";
        public const string FundFileHistoryStatusRef = "FundFileHistoryStatusRef";

        public const string ClientPretensionExpert = "ClientPretensionExpert";
        public const string Defect = "Defect";



        #endregion

        #region Default values

        public const int DefaultPageNumber = 1;
        public const int DefaultPageSize = 10;

        #endregion
        public const string ApplicationName = "OMInsurance";

        #region Documents ids

        public const long USSRPasportDocumentId = 1;
        public const long RussianFederationPassportDocumentId = 14;
        public const long BirthCertificateDocumentId = 3;
        public const string EmailRegex = @"^(([^<>()[\]\\.,;:\s@\""]+"
                                       + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                                       + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                                       + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                                       + @"[a-zA-Z]{2,}))$";
        public const string PhoneRegex = @"^\(([0-9]{3})\)([0-9]{3})-([0-9]{2})-([0-9]{2})$";
        public const string LatinCyrillicNumber = @"^[а-яА-ЯёЁa-zA-Z0-9\\\/]+$";
        public const string Number = @"^[0-9]+$";

        #endregion

        #region Fias ids

        public const string MoscowRegionFiasId = "0c5b2444-70a0-4932-980c-b4dc0d3f02b5";

        #endregion

        #region Statuses

        public const int PolicyReceived = 4;

        #endregion

        #region Logfiles for SmsWindowsService
        public const string LogFileSmsWindowsService = "C:\\Temp\\SmsWindowsService.log";
        #endregion

        #region Path save Fund files
        public const string SverkaRequest = "FundFiles\\SverkiRequest";
        public const string SverkaResponse = "FundFiles\\SverkiResponse";
        public const string FundRequest = "FundFiles\\FundRequest";
        public const string FundResponse = "FundFiles\\FundResponse";
        public const string Filial = "FundFiles\\Filial";
        public const string Migrate = "FundFiles\\Migrate";
        public const string MFC = "FundFiles\\MFC";
        public const string Pretension = "Pretension";
        #endregion



        #region Count field in excel file from region
        public const int DefaultCountFieldInRegionExcel = 29;
        public const int CountFieldInUfa = 19;
        public const int CountFieldInMoscowObl = 24;
        #endregion

        public const string MoscowOKATO = "45000";
    }
}
