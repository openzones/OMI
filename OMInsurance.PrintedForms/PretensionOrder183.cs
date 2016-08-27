using NPOI.SS.UserModel;
using OMInsurance.Entities;
using OMInsurance.PrintedForms.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OMInsurance.PrintedForms
{
    public class PretensionOrder183 : PrintedForm
    {
        private ClientPretension Pretension;

        #region Marks
        protected const string Generator_M_dakt = "$Generator_M_dakt$";
        protected const string M_mo = "$M_mo$";
        protected const string M_mcod_M_mo = "$M_mcod_M_mo$";
        protected const string PeriodFrom = "$PeriodFrom$";
        protected const string PeriodTo = "$PeriodTo$";
        protected const string M_nakt = "$M_nakt$";
        protected const string M_dakt = "$M_dakt$";
        protected const string M_osn230 = "$M_osn230$";
        protected const string Coefficient = "$Coefficient$";
        protected const string Sum = "$sum$"; //= m.straf * $Coefficient$
        protected const string UserPosition_UserFIO = "$UserPosition_UserFIO$";
        protected const string MCOD = "$MCOD$";

        #endregion

        public PretensionOrder183(ClientPretension pretension)
        {
            Pretension = pretension;
            TemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Templates", "PretensionOrder183.xlsx");
        }

        protected override void Process(ISheet table)
        {
            table.FindCellByMacros(Generator_M_dakt).SetValue("Предписание  № " + string.Format("{0,4:D4}", Pretension.Generator) + " от " + GetDateString(Pretension.M_dakt, "dd.MM.yyyy") + " г.");
            table.FindCellByMacros(M_mo).SetValue(Pretension.M_mo);
            table.FindCellByMacros(M_mcod_M_mo).SetValue(Pretension.M_mcod + " " + Pretension.M_mo);
            table.FindCellByMacros(PeriodFrom).SetValue(GetDateString(Pretension.PeriodFrom, "dd.MM.yyyy"));
            table.FindCellByMacros(PeriodTo).SetValue(GetDateString(Pretension.PeriodTo, "dd.MM.yyyy"));
            table.FindCellByMacros(M_nakt).SetValue(Pretension.M_nakt);
            table.FindCellByMacros(MCOD).SetValue("В соответствии с Договором на оказание и оплату медицинской помощи по обязательному медицинскому страхованию № "+ Pretension.MCOD + ".23 от 29 декабря 2015 г.");

            for (int i = 0; i < 3; i++)// 3 раза выводится
            {
                table.FindCellByMacros(M_dakt).SetValue(GetDateString(Pretension.M_dakt, "dd.MM.yyyy"));
            }

            string osn230 = Pretension.M_osn230Ref.Where(a => a.Id == Pretension.M_osn230_Id).Select(b => b.Code).FirstOrDefault();
            double? straf = (double?)Pretension.M_straf;
            double? sum = straf * (double?)Pretension.Coefficient;
            if (Pretension.IsConfirm == true)
            {
                osn230 = string.Empty;
                straf = null;
                Pretension.Coefficient = null;
                sum = null;
            }
            table.FindCellByMacros(M_osn230).SetValue(osn230);

            if (Pretension.Coefficient.HasValue)
            {
                table.FindCellByMacros(Coefficient).SetCellValue(Math.Round((double)Pretension.Coefficient, 2));
            }


            for (int i = 0; i < 3; i++)// 3 раза выводится
            {
                if (sum.HasValue)
                {
                    table.FindCellByMacros(Sum).SetCellValue(Math.Round((double)sum, 2));
                }
            }

            table.FindCellByMacros(UserPosition_UserFIO).SetValue(Pretension.UserPosition + ", " + Pretension.UserFIO);

        }
    }
}
