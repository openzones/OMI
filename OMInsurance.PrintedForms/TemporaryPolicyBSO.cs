using NPOI.SS.UserModel;
using OMInsurance.Entities;
using OMInsurance.PrintedForms.Helpers;
using System;
using System.IO;

namespace OMInsurance.PrintedForms
{
    public class TemporaryPolicyBSO : PrintedForm
    {
        public ClientVisit ClientVisit { get; set; }

        #region Marks

        protected const string SMOName = "$SMOName$";
        protected const string addressAndPhone = "$AddressAndPhone$";
        protected const string issueDateDay = "$IssueD$";
        protected const string issueDateMonth = "$IssueM$";
        protected const string issueDateYear = "$IssueY$";
        protected const string FIO = "$FIO$";
        protected const string isMen = "$IsMan$";
        protected const string isWoman = "$IsWoman$";
        protected const string BithdayDocTypeDocSeriesNumberIssueDate = "$BithdayDocTypeDocSeriesNumberIssueDate$";
        protected const string DocumentIssueDepartment = "$DocumentIssueDepartment$";
        protected const string birthplace = "$Birthplace$";
        protected const string ExpDay = "$ExpDay$";
        protected const string ExpM = "$ExpM$";
        protected const string ExpY = "$ExpY$";
        protected const string Registrator = "$Registrator$";

        #endregion

        public TemporaryPolicyBSO(ClientVisit visit)
        {
            TemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Templates", "TemporaryPolicyBSO.xls");
            this.ClientVisit = visit;
        }

        protected override void Process(ISheet table)
        {
            table.FindCellByMacros(SMOName).SetValue(GetNotNullString(ClientVisit.DeliveryCenter.DisplayName.Trim() + string.Format(" \"P2{0}\"", ClientVisit.DeliveryCenter.Code ?? string.Empty)));
            table.FindCellByMacros(isMen).SetCheckValue(ClientVisit.NewClientInfo.Sex == "1", false);
            table.FindCellByMacros(isWoman).SetCheckValue(ClientVisit.NewClientInfo.Sex == "2", false);
            string addressAndPhoneString = string.Format("{0}, тел. {1}",
                string.IsNullOrEmpty(ClientVisit.DeliveryCenter.Address) ? string.Empty : ClientVisit.DeliveryCenter.Address.Trim(),
                ClientVisit.DeliveryCenter.Phone ?? string.Empty);
            table.FindCellByMacros(addressAndPhone).SetValue(GetNotNullString(addressAndPhoneString.Trim()));
            table.FindCellByMacros(birthplace).SetValue(GetNotNullString(ClientVisit.NewClientInfo.Birthplace));
            if (ClientVisit.TemporaryPolicyDate.HasValue)
            {
                table.FindCellByMacros(issueDateDay).SetValue(GetNotNullString(GetTwoCharactersDate(ClientVisit.TemporaryPolicyDate.Value.Day)));
                table.FindCellByMacros(issueDateMonth).SetValue(GetNotNullString(GetTwoCharactersDate(ClientVisit.TemporaryPolicyDate.Value.Month)));
                table.FindCellByMacros(issueDateYear).SetValue(GetNotNullString(ClientVisit.TemporaryPolicyDate.Value.Year.ToString()));
            }
            else
            {
                table.FindCellByMacros(issueDateDay).SetValue(string.Empty);
                table.FindCellByMacros(issueDateMonth).SetValue(string.Empty);
                table.FindCellByMacros(issueDateYear).SetValue(string.Empty);
            }
            if (ClientVisit.TemporaryPolicyExpirationDate.HasValue)
            {
                table.FindCellByMacros(ExpDay).SetValue(GetNotNullString(GetTwoCharactersDate(ClientVisit.TemporaryPolicyExpirationDate.Value.Day)));
                table.FindCellByMacros(ExpM).SetValue(GetNotNullString(GetTwoCharactersDate(ClientVisit.TemporaryPolicyExpirationDate.Value.Month)));
                table.FindCellByMacros(ExpY).SetValue(GetNotNullString(ClientVisit.TemporaryPolicyExpirationDate.Value.Year.ToString()));
            }
            else
            {
                table.FindCellByMacros(ExpDay).SetValue(string.Empty);
                table.FindCellByMacros(ExpM).SetValue(string.Empty);
                table.FindCellByMacros(ExpY).SetValue(string.Empty);
            }

            string fio = string.Format("{0} {1} {2}",
                ClientVisit.NewClientInfo.Lastname ?? string.Empty,
                ClientVisit.NewClientInfo.Firstname ?? string.Empty,
                ClientVisit.NewClientInfo.Secondname ?? string.Empty);
            table.FindCellByMacros(FIO).SetValue(fio);
            string docInfo = string.Format("{0}, {1} {2} {3}, выдан {4}",
                GetDateString(ClientVisit.NewClientInfo.Birthday, "dd-MM-yyyy"),
                ClientVisit.NewDocument.DocumentType.Name,
                ClientVisit.NewDocument.Series,
                ClientVisit.NewDocument.Number,
                GetDateString(ClientVisit.NewDocument.IssueDate, "dd-MM-yyyy"));

            table.FindCellByMacros(BithdayDocTypeDocSeriesNumberIssueDate).SetValue(docInfo);
            table.FindCellByMacros(DocumentIssueDepartment).SetValue(GetNotNullString(ClientVisit.NewDocument.IssueDepartment));

            string registrator = string.Format("{0} {1} {2}", ClientVisit.Registrator.Lastname ?? string.Empty,
                ClientVisit.Registrator.Firstname ?? string.Empty,
                ClientVisit.Registrator.Secondname ?? string.Empty);
            table.FindCellByMacros(Registrator).SetValue(GetNotNullString(registrator));
        }

        private string GetTwoCharactersDate(int p)
        {
            if (p < 10)
            {
                return string.Format("0{0}", p);
            }
            else
            {
                return p.ToString();
            }
        }
    }
}
