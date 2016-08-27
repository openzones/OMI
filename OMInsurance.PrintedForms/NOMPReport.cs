using NPOI.SS.UserModel;
using OMInsurance.Entities;
using OMInsurance.PrintedForms.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OMInsurance.PrintedForms
{
    public class NOMPReport : PrintedForm
    {
        private List<NOMP> ListNOMP;
        private Nomernik.History NompHistory;

        #region Marks
        protected const string title = "$Title$";
        protected const string dateTimeNOMP = "$dateTimeNOMP$";
        protected const string countAll = "$CountAll$";
        protected const string countOur = "$CountOur$";
        protected const string _1 = "$1$";
        protected const string _2 = "$2$";
        protected const string _3 = "$3$";
        protected const string _4 = "$4$";
        protected const string _5 = "$5$";
        protected const string sum = "$sum$";

        #endregion

        public NOMPReport(List<NOMP> listNOMP, Nomernik.History nompHistory)
        {
            ListNOMP = listNOMP;
            NompHistory = nompHistory;
            TemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Templates", "NOMPReport.xlsx");
        }

        protected override void Process(ISheet table)
        {
            table.FindCellByMacros(title).SetValue("Отчет по обработанным записям. Дата обработки (загрузки): " + NompHistory.LoadDate.ToString());
            table.FindCellByMacros(dateTimeNOMP).SetValue(string.Format("Номерник: {0:Y}", NompHistory.FileDate));
            table.FindCellByMacros(countAll).SetValue(NompHistory.CountAll.ToString());
            table.FindCellByMacros(countOur).SetValue(NompHistory.CountOur.ToString());

            int[] countStatus = new int[6] { 0, 0, 0, 0, 0, 0 };

            int rowNumber = 12;

                for (int i = 0; i < ListNOMP.Count; i++)
                {
                    var nomp = ListNOMP[i];
                    IRow row = table.CreateRow(rowNumber);
                    row.CreateCell(0).SetValue(GetNotNullString((i + 1).ToString()));
                    row.CreateCell(1).SetValue(GetNotNullString(nomp.S_CARD));
                    row.CreateCell(2).SetValue(GetNotNullString(nomp.N_CARD.ToString()));
                    row.CreateCell(3).SetValue(GetNotNullString(nomp.ENP));
                    row.CreateCell(4).SetValue(GetNotNullString(nomp.VSN));
                    row.CreateCell(5).SetValue(GetNotNullString(nomp.LPU_ID.ToString()));
                    row.CreateCell(6).SetValue(GetDateString(nomp.DATE_IN, "dd.MM.yyyy"));
                    row.CreateCell(7).SetValue(GetNotNullString(nomp.SPOS.ToString()));
                    row.CreateCell(8).SetValue(GetNotNullString(nomp.Status.ToString()));

                    string StatusMessage;
                    if (nomp.Status == 1)
                    {
                        StatusMessage = "Ошибка, клиент отсутствует (не найден)";
                    }
                    else if (nomp.Status == 2)
                    {
                        StatusMessage = "Ошибка, найдено более 1 клиента";
                    }
                    else if (nomp.Status == 3)
                    {
                        StatusMessage = "Данные в системе актуальны";
                    }
                    else if (nomp.Status == 4)
                    {
                        StatusMessage = "Данные обновлены у клиента";
                    }
                    else if (nomp.Status == 5)
                    {
                        StatusMessage = "Данные загружены клиенту";
                    }
                    else StatusMessage = "Без статуса";
                    row.CreateCell(9).SetValue(StatusMessage);
                    row.CreateCell(10).SetValue(GetNotNullString(nomp.ClientID.ToString()));
                    row.CreateCell(11).SetValue(GetNotNullString(nomp.Comment));

                    countStatus[nomp.Status]++;
                    rowNumber++;
                }

            table.FindCellByMacros(_1).SetValue(countStatus[1].ToString());
            table.FindCellByMacros(_2).SetValue(countStatus[2].ToString());
            table.FindCellByMacros(_3).SetValue(countStatus[3].ToString());
            table.FindCellByMacros(_4).SetValue(countStatus[4].ToString());
            table.FindCellByMacros(_5).SetValue(countStatus[5].ToString());
            table.FindCellByMacros(sum).SetValue(NompHistory.CountOur.ToString());
        }
    }
}
