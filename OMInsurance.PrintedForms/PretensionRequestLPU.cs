using NPOI.SS.UserModel;
using OMInsurance.Entities;
using OMInsurance.PrintedForms.Helpers;
using System;
using System.IO;
using System.Linq;

namespace OMInsurance.PrintedForms
{
    public class PretensionRequestLPU : PrintedForm
    {
        private ClientPretension Pretension;
        private Client Client;

        #region Marks
        protected const string FULLNAME_LPU = "$FULLNAME_LPU$";
        protected const string GENERATIONString1 = "$GenerationString1$";
        protected const string GENERATIONString2 = "$GenerationString2$";
        protected const string DATE = "$Date$";
        protected const string USER_FIO = "$UserFIO$";

        #endregion

        public PretensionRequestLPU(ClientPretension pretension, Client client)
        {
            Pretension = pretension;
            Client = client;
            TemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Templates", "PretensionRequestLPU.xlsx");
        }

        protected override void Process(ISheet table)
        {
            table.FindCellByMacros(FULLNAME_LPU).SetValue(Pretension.M_mo);
            string string1 = "В связи с поступившими обращениями застрахованных о нарушении их прав на выбор медицинской организации, АО «МСК «УралСиб» просит предоставить для проведения целевой медико-экономической экспертизы заявление о выборе " +
                Pretension.M_mo + " следующих граждан:";
            table.FindCellByMacros(GENERATIONString1).SetValue(string1);


            string string2 = Client.ActualVersion.Lastname + " " +
                Client.ActualVersion.Firstname + " " +
                Client.ActualVersion.Secondname + ", " +
                (Client.ActualVersion.Birthday.HasValue ? GetDateString(Client.ActualVersion.Birthday, "dd.MM.yyyy") : string.Empty) + " г.р., " +
                "дата прикрепления " +
                GetDateString(Pretension.DATE_IN, "dd.MM.yyyy") + " г., ";
            string ENP = Client.Visits.OrderBy(a => a.StatusDate).LastOrDefault().UnifiedPolicyNumber;
            if (string.IsNullOrEmpty(ENP))
            {
                string2 = string2 + "серия " + Client.Visits.OrderBy(a => a.StatusDate).LastOrDefault().PolicySeries +
                    " номер " + Client.Visits.OrderBy(a => a.StatusDate).LastOrDefault().PolicyNumber;
            }
            else
            {
                string2 = string2 + "ЕНП " + ENP;
            }
            table.FindCellByMacros(GENERATIONString2).SetValue(string2);

            table.FindCellByMacros(DATE).SetValue(GetDateString(Pretension.M_dakt, "dd.MM.yyyy"));
            table.FindCellByMacros(USER_FIO).SetValue(Pretension.UserFIO);
        }
    }
}
