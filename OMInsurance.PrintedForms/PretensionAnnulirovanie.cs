using NPOI.SS.UserModel;
using OMInsurance.Entities;
using OMInsurance.PrintedForms.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OMInsurance.PrintedForms
{
    public class PretensionAnnulirovanie : PrintedForm
    {
        private ClientPretension Pretension;

        #region Marks
        protected const string Title = "$Title$";
        protected const string M_dakt = "$M_dakt$";
        protected const string M_expert = "$M_expert$";
        protected const string M_smo = "$M_smo$";
        protected const string M_mcod = "$M_mcod$";
        protected const string M_mo = "$M_mo$";
        protected const string M_period = "$M_period$";
        protected const string M_snpol = "$M_snpol$";
        protected const string M_fd = "$M_fd$";
        protected const string M_nd = "$M_nd$";
        protected const string M_osn230 = "$M_osn230$";
        protected const string M_er_c = "$M_er_c$";
        protected const string M_straf = "$M_straf$";
        protected const string Sum = "$sum$"; //= m.straf * $Coefficient$

        #endregion

        public PretensionAnnulirovanie(ClientPretension pretension)
        {
            Pretension = pretension;
            TemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Templates", "PretensionAnnulirovanie.xlsx");
        }

        protected override void Process(ISheet table)
        {
            string title = Pretension.M_nakt + " от " + GetDateString(Pretension.M_dakt, "dd.MM.yyyy") + " г.";
            table.FindCellByMacros(Title).SetValue(title);

            for (int i = 0; i < 4; i++) //4 раз выводится
            {
                table.FindCellByMacros(M_dakt).SetValue(GetDateString(Pretension.M_dakt, "dd.MM.yyyy"));
            }
            for (int i = 0; i < 2; i++) //2 раз выводится
            {
                table.FindCellByMacros(M_expert).SetValue(Pretension.M_expert);
            }

            table.FindCellByMacros(M_smo).SetValue("АО МСК \"УралСиб\"");
            table.FindCellByMacros(M_mcod).SetValue(Pretension.M_mcod);
            table.FindCellByMacros(M_mo).SetValue(Pretension.M_mo);
            table.FindCellByMacros(M_period).SetValue(Pretension.M_period);
            table.FindCellByMacros(M_snpol).SetValue(Pretension.M_snpol);

            for (int i = 0; i < 2; i++) //2 раз выводится
            {
                table.FindCellByMacros(M_fd).SetValue(Pretension.M_fd.Replace("  ", " "));
            }

            string All_M_nd;
            string Confirm1;
            string Confirm2;
            if (Pretension.IsConfirm.HasValue)
            {
                if (Pretension.IsConfirm == true)
                {
                    Confirm1 = " представлено. ";
                    Confirm2 = " обоснованным. ";
                }
                else
                {
                    Confirm1 = " не представлено. ";
                    Confirm2 = " необоснованным. ";
                }
            }
            else
            {
                Confirm1 = " не выбрано! ";
                Confirm2 = " не выбрано! ";
            }
            All_M_nd = Pretension.M_nd1 + Confirm1 + Pretension.M_nd2 + Confirm2;
            table.FindCellByMacros(M_nd).SetValue(All_M_nd.Replace("  ", " "));


            string osn230 = Pretension.M_osn230Ref.Where(a => a.Id == Pretension.M_osn230_Id).Select(b => b.Code).FirstOrDefault();
            string err = Pretension.M_osn230Ref.Where(a => a.Id == Pretension.M_osn230_Id).Select(b => b.ErrCode).FirstOrDefault();
            double? straf = (double?)Pretension.M_straf;
            double? sum = straf * (double?)Pretension.Coefficient;
            if (Pretension.IsConfirm == true)
            {
                osn230 = string.Empty;
                err = string.Empty;
                straf = null;
                sum = null;
            }
            table.FindCellByMacros(M_osn230).SetValue(osn230);
            table.FindCellByMacros(M_er_c).SetValue(err);
            for (int i = 0; i < 1; i++) //1 раз выводится
            {
                if (straf.HasValue)
                {
                    table.FindCellByMacros(M_straf).SetCellValue(Math.Round((double)straf, 2));
                }
            }

            for (int i = 0; i < 3; i++)// 3 раза выводится
            {
                if (sum.HasValue)
                {
                    table.FindCellByMacros(Sum).SetCellValue(Math.Round((double)sum, 2));
                }
            }

        }
    }
}
