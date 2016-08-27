using NPOI.SS.UserModel;
using OMInsurance.Entities;
using OMInsurance.PrintedForms.Helpers;
using System;
using System.IO;

namespace OMInsurance.PrintedForms
{
    public class ChangeInsuranceCompanyApplication : PrintedForm
    {
        public ClientVisit ClientVisit { get; set; }

        #region Marks

        protected const string newFirstname = "$NewFirstname$";
        protected const string newSecondname = "$NewSecondname$";
        protected const string newLastname = "$NewLastname$";
        protected const string isNewMen = "$IsNewMen$";
        protected const string isNewFemale = "$IsNewFemale$";
        protected const string birthdate = "$NewBirthdate$";
        protected const string birthplace = "$NewBirthplace$";
        protected const string newDocumentType = "$NewDocumentType$";
        protected const string newDocumentSeries = "$NewDocumentSeries$";
        protected const string newDocumentNumber = "$NewDocumentNumber$";
        protected const string newDocumentIssueDate = "$NewDocumentIssueDate$";
        protected const string newDocumentIssueDepartment = "$NewDocumentIssueDepartment$";
        protected const string citizenship = "$Citizenship$";
        protected const string isBOMZH = "$IsBOMZH$";
        protected const string registrationPostIndex = "$RegistrationPostIndex$";
        protected const string registrationRegion = "$RegistrationRegion$";
        protected const string registrationArea = "$RegistrationArea$";
        protected const string registrationCity = "$RegistrationCity$";
        protected const string registrationLocality = "$RegistrationLocality$";
        protected const string registrationStreet = "$RegistrationStreet$";
        protected const string registrationHouse = "$RegistrationHouse$";
        protected const string registrationBuilding = "$RegistrationBuilding$";
        protected const string registrationHousing = "$RegistrationHousing$";
        protected const string registrationAppartment = "$RegistrationAppartment$";
        protected const string registrationDate = "$RegistrationDate$";
        protected const string livingPostIndex = "$LivingPostIndex$";
        protected const string livingRegion = "$LivingRegion$";
        protected const string livingArea = "$LivingArea$";
        protected const string livingCity = "$LivingCity$";
        protected const string livingLocality = "$LivingLocality$";
        protected const string livingStreet = "$LivingStreet$";
        protected const string livingHouse = "$LivingHouse$";
        protected const string livingBuilding = "$LivingBuilding$";
        protected const string livingHousing = "$LivingHousing$";
        protected const string livingAppartment = "$LivingAppartment$";
        protected const string foreignDocumentStartDate = "$ForeignDocumentStartDate$";
        protected const string foreignDocumentExpirationDate = "$ForeignDocumentExpirationDate$";
        protected const string SNILS = "$SNILS$";
        protected const string phone = "$Phone$";
        protected const string email = "$Email$";
        protected const string representativeLastname = "$RepresentativeLastname$";
        protected const string representativeFirstname = "$RepresentativeFirstname$";
        protected const string representativeSecondname = "$RepresentativeSecondname$";
        protected const string RepresentativeDocumentType = "$RepresentativeDocumentType$";
        protected const string RepresentativeDocumentSeries = "$RepresentativeDocumentSeries$";
        protected const string RepresentativeDocumentNumber = "$RepresentativeDocumentNumber$";
        protected const string RepresentativeDocumentIssueDate = "$RepresentativeDocumentIssueDate$";
        protected const string FIO = "$FIO$";
        protected const string SignatureDate = "$SignatureDate$";
        protected const string SignatureDate2 = "$SignatureDate2$";
        protected const string Registrator = "$Registrator$";
        protected const string TemporaryPolicyNumber = "$TemporaryPolicyNumber$";
        protected const string IssueDate = "$IssueDate$";
        protected const string enp = "$enp$";

        #endregion

        public ChangeInsuranceCompanyApplication(ClientVisit visit)
        {
            TemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Templates", "ChangeInsuranceCompanyApplication.xls");
            this.ClientVisit = visit;
        }

        protected override void Process(ISheet table)
        {
            table.FindCellByMacros(newFirstname).SetValue(GetNotNullString(ClientVisit.NewClientInfo.Firstname));
            table.FindCellByMacros(newSecondname).SetValue(GetNotNullString(ClientVisit.NewClientInfo.Secondname));
            table.FindCellByMacros(newLastname).SetValue(GetNotNullString(ClientVisit.NewClientInfo.Lastname));
            table.FindCellByMacros(isNewMen).SetCheckValue(ClientVisit.NewClientInfo.Sex == "1", false);
            table.FindCellByMacros(isNewFemale).SetCheckValue(ClientVisit.NewClientInfo.Sex == "2", false);
            table.FindCellByMacros(birthdate).SetValue(GetDateString(ClientVisit.NewClientInfo.Birthday.Value, "dd/MM/yyyy"));
            table.FindCellByMacros(birthplace).SetValue(GetNotNullString(ClientVisit.NewClientInfo.Birthplace));
            table.FindCellByMacros(newDocumentType).SetValue(GetNotNullString(ClientVisit.NewDocument.DocumentType != null ? ClientVisit.NewDocument.DocumentType.Name : null));
            table.FindCellByMacros(newDocumentSeries).SetValue(GetNotNullString(ClientVisit.NewDocument.Series));
            table.FindCellByMacros(newDocumentNumber).SetValue(GetNotNullString(ClientVisit.NewDocument.Number));
            table.FindCellByMacros(newDocumentIssueDate).SetValue(GetDateString(ClientVisit.NewDocument.IssueDate, "dd/MM/yyyy"));
            table.FindCellByMacros(newDocumentIssueDepartment).SetValue(GetNotNullString(ClientVisit.NewDocument.IssueDepartment));
            table.FindCellByMacros(enp).SetValue(GetNotNullString(ClientVisit.NewPolicy.UnifiedPolicyNumber));
            table.FindCellByMacros(citizenship).SetValue(GetNotNullString(ClientVisit.NewClientInfo.Citizenship != null ? ClientVisit.NewClientInfo.Citizenship.Name : null));
            table.FindCellByMacros(citizenship).SetValue(GetNotNullString(ClientVisit.NewClientInfo.Citizenship != null ? ClientVisit.NewClientInfo.Citizenship.Name : null));
            table.FindCellByMacros(isBOMZH).SetCheckValue(false, false);
            if (string.IsNullOrEmpty(ClientVisit.RegistrationAddress.Region))
            {
                table.FindCellByMacros(registrationPostIndex).SetValue(GetNotNullString(ClientVisit.LivingAddress.PostIndex));
                table.FindCellByMacros(registrationArea).SetValue(GetNotNullString(ClientVisit.LivingAddress.Area));
                table.FindCellByMacros(registrationRegion).SetValue(GetNotNullString(ClientVisit.LivingAddress.Region));
                table.FindCellByMacros(registrationCity).SetValue(GetNotNullString(ClientVisit.LivingAddress.City));
                table.FindCellByMacros(registrationLocality).SetValue(GetNotNullString(ClientVisit.LivingAddress.Locality));
                table.FindCellByMacros(registrationStreet).SetValue(GetNotNullString(ClientVisit.LivingAddress.Street));
                table.FindCellByMacros(registrationHouse).SetValue(GetNotNullString(ClientVisit.LivingAddress.House));
                table.FindCellByMacros(registrationBuilding).SetValue(
                    GetNotNullString(
                    string.IsNullOrEmpty(ClientVisit.LivingAddress.Building) ?
                    ClientVisit.LivingAddress.Housing :
                    ClientVisit.LivingAddress.Building));
                table.FindCellByMacros(registrationHousing).SetValue(GetNotNullString(ClientVisit.LivingAddress.Housing));
                table.FindCellByMacros(registrationAppartment).SetValue(GetNotNullString(ClientVisit.LivingAddress.Appartment));
            }
            else
            {
                table.FindCellByMacros(registrationPostIndex).SetValue(GetNotNullString(ClientVisit.RegistrationAddress.PostIndex));
                table.FindCellByMacros(registrationArea).SetValue(GetNotNullString(ClientVisit.RegistrationAddress.Area));
                table.FindCellByMacros(registrationRegion).SetValue(GetNotNullString(ClientVisit.RegistrationAddress.Region));
                table.FindCellByMacros(registrationCity).SetValue(GetNotNullString(ClientVisit.RegistrationAddress.City));
                table.FindCellByMacros(registrationLocality).SetValue(GetNotNullString(ClientVisit.RegistrationAddress.Locality));
                table.FindCellByMacros(registrationStreet).SetValue(GetNotNullString(ClientVisit.RegistrationAddress.Street));
                table.FindCellByMacros(registrationHouse).SetValue(GetNotNullString(ClientVisit.RegistrationAddress.House));
                table.FindCellByMacros(registrationBuilding).SetValue(
                    GetNotNullString(
                    string.IsNullOrEmpty(ClientVisit.RegistrationAddress.Building) ?
                    ClientVisit.RegistrationAddress.Housing :
                    ClientVisit.RegistrationAddress.Building));
                table.FindCellByMacros(registrationHousing).SetValue(GetNotNullString(ClientVisit.RegistrationAddress.Housing));
                table.FindCellByMacros(registrationAppartment).SetValue(GetNotNullString(ClientVisit.RegistrationAddress.Appartment));
            }
            table.FindCellByMacros(registrationDate).SetValue(GetDateString(ClientVisit.RegistrationAddressDate, "dd/MM/yyyy"));
            table.FindCellByMacros(livingPostIndex).SetValue(GetNotNullString(ClientVisit.LivingAddress.PostIndex));
            table.FindCellByMacros(livingArea).SetValue(GetNotNullString(ClientVisit.LivingAddress.Area));
            table.FindCellByMacros(livingRegion).SetValue(GetNotNullString(ClientVisit.LivingAddress.Region));
            table.FindCellByMacros(livingCity).SetValue(GetNotNullString(ClientVisit.LivingAddress.City));
            table.FindCellByMacros(livingLocality).SetValue(GetNotNullString(ClientVisit.LivingAddress.Locality));
            table.FindCellByMacros(livingStreet).SetValue(GetNotNullString(ClientVisit.LivingAddress.Street));
            table.FindCellByMacros(livingHouse).SetValue(GetNotNullString(ClientVisit.LivingAddress.House));
            table.FindCellByMacros(livingBuilding).SetValue(
                GetNotNullString(
                string.IsNullOrEmpty(ClientVisit.LivingAddress.Building) ?
                ClientVisit.LivingAddress.Housing :
                ClientVisit.LivingAddress.Building));
            table.FindCellByMacros(livingHousing).SetValue(GetNotNullString(ClientVisit.LivingAddress.Housing));
            table.FindCellByMacros(livingAppartment).SetValue(GetNotNullString(ClientVisit.LivingAddress.Appartment));
            table.FindCellByMacros(foreignDocumentStartDate).SetValue(GetDateString(ClientVisit.NewForeignDocument.IssueDate, "dd/MM/yyyy"));
            table.FindCellByMacros(foreignDocumentExpirationDate).SetValue(GetDateString(ClientVisit.NewForeignDocument.ExpirationDate, "dd/MM/yyyy"));
            table.FindCellByMacros(SNILS).SetValue(GetNotNullString(ClientVisit.NewClientInfo.SNILS));
            table.FindCellByMacros(phone).SetValue(GetNotNullString(ClientVisit.Phone));
            table.FindCellByMacros(email).SetValue(GetNotNullString(ClientVisit.Email));
            table.FindCellByMacros(representativeLastname).SetValue(GetNotNullString(ClientVisit.Representative.Lastname));
            table.FindCellByMacros(representativeFirstname).SetValue(GetNotNullString(ClientVisit.Representative.Firstname));
            table.FindCellByMacros(representativeSecondname).SetValue(GetNotNullString(ClientVisit.Representative.Secondname));
            table.FindCellByMacros(RepresentativeDocumentType).SetValue(GetNotNullString(ClientVisit.Representative.DocumentType != null ? ClientVisit.Representative.DocumentType.Name : null));
            table.FindCellByMacros(RepresentativeDocumentSeries).SetValue(GetNotNullString(ClientVisit.Representative.Series));
            table.FindCellByMacros(RepresentativeDocumentNumber).SetValue(GetNotNullString(ClientVisit.Representative.Number));
            table.FindCellByMacros(RepresentativeDocumentIssueDate).SetValue(GetDateString(ClientVisit.Representative.IssueDate, "dd/MM/yyyy"));
            string fio = string.Format("{0} {1} {2}",
                ClientVisit.NewClientInfo.Lastname ?? string.Empty,
                ClientVisit.NewClientInfo.Firstname ?? string.Empty,
                ClientVisit.NewClientInfo.Secondname ?? string.Empty);

            string fioRepresentative;

            if (ClientVisit.Representative.RepresentativeTypeId != 1)
            {
                fioRepresentative = string.Format("{0} {1} {2}",
                  ClientVisit.Representative.Lastname ?? string.Empty,
                  ClientVisit.Representative.Firstname ?? string.Empty,
                  ClientVisit.Representative.Secondname ?? string.Empty);
                table.FindCellByMacros(FIO).SetValue(fioRepresentative);
                table.FindCellByMacros(FIO).SetValue(fioRepresentative);
                table.FindCellByMacros(FIO).SetValue(fioRepresentative);
            }
            else
            {
                table.FindCellByMacros(FIO).SetValue(fio);
                table.FindCellByMacros(FIO).SetValue(fio);
                table.FindCellByMacros(FIO).SetValue(fio);
            }
            table.FindCellByMacros(SignatureDate).SetValue(GetDateString(DateTime.Now, "dd/MM/yyyy"));
            table.FindCellByMacros(SignatureDate2).SetValue(GetDateString(DateTime.Now, "dd/MM/yyyy"));
            string registrator = string.Format("{0} {1} {2}", ClientVisit.Registrator.Lastname ?? string.Empty,
                ClientVisit.Registrator.Firstname ?? string.Empty,
                ClientVisit.Registrator.Secondname ?? string.Empty);
            table.FindCellByMacros(Registrator).SetValue(GetNotNullString(registrator));
            table.FindCellByMacros(Registrator).SetValue(GetNotNullString(registrator));
            table.FindCellByMacros(TemporaryPolicyNumber).SetValue(GetNotNullString(ClientVisit.TemporaryPolicyNumber));
            table.FindCellByMacros(IssueDate).SetValue(GetDateString(ClientVisit.IssueDate, "dd/MM/yyyy"));
        }
    }
}
