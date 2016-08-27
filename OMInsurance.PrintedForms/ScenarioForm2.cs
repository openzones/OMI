using NPOI.SS.UserModel;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using OMInsurance.PrintedForms.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace OMInsurance.PrintedForms
{
    public class ScenarioForm2 : PrintedForm
    {
        private List<int> ListInt;

        #region Marks
        protected const string title = "$Title$";
        #endregion

        public ScenarioForm2(List<int> listInt)
        {
            ListInt = listInt;
            TemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Templates", "ScenarioForm2.xls");
        }

        protected override void Process(ISheet table)
        {
            if (ListInt.Count == 0)
            {
                table.FindCellByMacros(title).SetValue(GetNotNullString("Отчет по сценариям (Форма 2). Нет данных!"));
                return;
            }
            table.FindCellByMacros(title).SetValue(GetNotNullString(string.Format("Отчет по сценариям (Форма 2)")));

            ICellStyle style = table.Workbook.CreateCellStyle();
            style.BorderRight = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;

            table.GetRow(3).CreateCell(1).SetCellValue(ListInt.ElementAt(0)); table.GetRow(3).GetCell(1).CellStyle = style;
            table.GetRow(4).CreateCell(1).SetCellValue(ListInt.ElementAt(1)); table.GetRow(4).GetCell(1).CellStyle = style;
            table.GetRow(5).CreateCell(1).SetCellValue(ListInt.ElementAt(2)); table.GetRow(5).GetCell(1).CellStyle = style;
            table.GetRow(3).CreateCell(2).SetCellValue(ListInt.ElementAt(3)); table.GetRow(3).GetCell(2).CellStyle = style;
            table.GetRow(4).CreateCell(2).SetCellValue(ListInt.ElementAt(4)); table.GetRow(4).GetCell(2).CellStyle = style;
            table.GetRow(5).CreateCell(2).SetCellValue(ListInt.ElementAt(5)); table.GetRow(5).GetCell(2).CellStyle = style;

            table.GetRow(6).CreateCell(1).SetCellValue(ListInt.ElementAt(6));
            table.GetRow(6).CreateCell(2).SetCellValue(ListInt.ElementAt(7));
        }
    }
}
