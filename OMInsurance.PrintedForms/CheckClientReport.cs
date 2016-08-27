using NPOI.SS.UserModel;
using OMInsurance.Entities.Check;
using OMInsurance.Entities.Searching;
using OMInsurance.PrintedForms.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OMInsurance.PrintedForms
{
    public class CheckClientReport : PrintedForm
    {
        private List<CheckClient> ListCheckClient;
        private long? CountDublicate;
        private string TextCheck = string.Empty;
        private ViewColumn ViewColumn;

        #region Marks
        protected const string titleConst = "$Title$";
        protected const string countConst = "$Count$";
        protected const string countDuplicateConst = "$CountDuplicate$";
        protected const string textCheckConst = "$TextCheck$";
        #endregion

        public CheckClientReport(List<CheckClient> listCheckClient, long? countDublicate, CheckClientSearchCriteria criteriaSearch, ViewColumn viewColumn)
        {
            ListCheckClient = listCheckClient;
            CountDublicate = countDublicate;
            ViewColumn = viewColumn;

            TextCheck = string.Empty;
            if (criteriaSearch.IsLastname)
            {
                TextCheck = TextCheck + "Фамилия, ";
            }
            if (criteriaSearch.IsFirstname)
            {
                TextCheck = TextCheck + "Имя, ";
            }
            if (criteriaSearch.IsSecondname)
            {
                TextCheck = TextCheck + "Отчество, ";
            }
            if (criteriaSearch.IsBirthday)
            {
                TextCheck = TextCheck + "Дата рождения, ";
            }
            if (criteriaSearch.IsSex)
            {
                TextCheck = TextCheck + "Пол, ";
            }
            if (criteriaSearch.IsPolicySeries)
            {
                TextCheck = TextCheck + "Серия полиса, ";
            }
            if (criteriaSearch.IsPolicyNumber)
            {
                TextCheck = TextCheck + "Номер полиса, ";
            }
            if (criteriaSearch.IsUnifiedPolicyNumber)
            {
                TextCheck = TextCheck + "ЕНП, ";
            }
            if (criteriaSearch.IsDocumentSeries)
            {
                TextCheck = TextCheck + "Серия паспорта, ";
            }
            if (criteriaSearch.IsDocumentNumber)
            {
                TextCheck = TextCheck + "Номер паспорта, ";
            }
            if (!string.IsNullOrEmpty(TextCheck))
            {
                TextCheck = TextCheck.Trim(',', ' ');
            }

            TemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Templates", "CheckClientReport.xlsx");
        }

        protected override void Process(ISheet table)
        {
            int rowNumber = 6;

            table.FindCellByMacros(titleConst).SetValue("Отчет по возможным дубликатам клиентов " + DateTime.Now.ToShortDateString());
            if (ListCheckClient == null)
            {
                table.FindCellByMacros(countConst).SetCellValue("Дубликатов нет!");
                return;
            }
            else
            {
                table.FindCellByMacros(countConst).SetCellValue(ListCheckClient.Count());
            }
            table.FindCellByMacros(countDuplicateConst).SetValue(GetNotNullString(CountDublicate.ToString()));
            table.FindCellByMacros(textCheckConst).SetValue(GetNotNullString(TextCheck));

            //создаем стиль для шапки таблицы
            ICellStyle style = table.Workbook.CreateCellStyle();
            style.BorderRight = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            IFont font = table.Workbook.CreateFont();
            font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
            style.SetFont(font);

            //заполняем шапку таблицы
            IRow rowHeadTable = table.CreateRow(rowNumber - 1);
            int colHeadTable = 0;
            if (ViewColumn.IsId)
            {
                rowHeadTable.CreateCell(colHeadTable).SetCellValue("ClientId");
                colHeadTable++;
            }
            if (ViewColumn.IsLastname)
            {
                rowHeadTable.CreateCell(colHeadTable).SetValue("Фамилия");
                colHeadTable++;
            }
            if (ViewColumn.IsFirstname)
            {
                rowHeadTable.CreateCell(colHeadTable).SetValue("Имя");
                colHeadTable++;
            }
            if (ViewColumn.IsSecondname)
            {
                rowHeadTable.CreateCell(colHeadTable).SetValue("Отчество");
                colHeadTable++;
            }
            if (ViewColumn.IsBirthday)
            {
                rowHeadTable.CreateCell(colHeadTable).SetValue("Дата рождения");
                colHeadTable++;
            }
            if (ViewColumn.IsSex)
            {
                rowHeadTable.CreateCell(colHeadTable).SetValue("Пол");
                colHeadTable++;
            }
            if (ViewColumn.IsPolicySeries)
            {
                rowHeadTable.CreateCell(colHeadTable).SetValue("Серия полиса");
                colHeadTable++;
            }
            if (ViewColumn.IsPolicyNumber)
            {
                rowHeadTable.CreateCell(colHeadTable).SetValue("Номер полиса");
                colHeadTable++;
            }
            if (ViewColumn.IsUnifiedPolicyNumber)
            {
                rowHeadTable.CreateCell(colHeadTable).SetValue("ЕНП");
                colHeadTable++;
            }
            if (ViewColumn.IsDocumentSeries)
            {
                rowHeadTable.CreateCell(colHeadTable).SetValue("Серия паспорта");
                colHeadTable++;
            }
            if (ViewColumn.IsDocumentNumber)
            {
                rowHeadTable.CreateCell(colHeadTable).SetValue("Номер паспорта");
                colHeadTable++;
            }
            if (ViewColumn.IsLivingFullAddressString)
            {
                rowHeadTable.CreateCell(colHeadTable).SetValue("Адрес проживания");
                colHeadTable++;
            }
            if (ViewColumn.IsOfficialFullAddressString)
            {
                rowHeadTable.CreateCell(colHeadTable).SetValue("Адрес регистрации");
                colHeadTable++;
            }
            if (ViewColumn.IsTemporaryPolicyNumber)
            {
                rowHeadTable.CreateCell(colHeadTable).SetValue("ВС(БСО)");
                colHeadTable++;
            }
            if (ViewColumn.IsTemporaryPolicyDate)
            {
                rowHeadTable.CreateCell(colHeadTable).SetValue("Дата обращения");
                colHeadTable++;
            }
            if (ViewColumn.IsSNILS)
            {
                rowHeadTable.CreateCell(colHeadTable).SetValue("СНИЛС");
                colHeadTable++;
            }
            if (ViewColumn.IsPhone)
            {
                rowHeadTable.CreateCell(colHeadTable).SetValue("Телефон");
                colHeadTable++;
            }



            for (int i = 0; i < ListCheckClient.Count; i++)
            {
                int col = 0;
                var check = ListCheckClient[i];
                IRow row = table.CreateRow(rowNumber);

                if (ViewColumn.IsId)
                {
                    row.CreateCell(col).SetCellValue(check.Id);
                    col++;
                }
                if (ViewColumn.IsLastname)
                {
                    row.CreateCell(col).SetValue(GetNotNullString(check.Lastname));
                    col++;
                }
                if (ViewColumn.IsFirstname)
                {
                    row.CreateCell(col).SetValue(GetNotNullString(check.Firstname));
                    col++;
                }
                if (ViewColumn.IsSecondname)
                {
                    row.CreateCell(col).SetValue(GetNotNullString(check.Secondname));
                    col++;
                }
                if (ViewColumn.IsBirthday)
                {
                    row.CreateCell(col).SetValue(GetDateString(check.Birthday, "dd.MM.yyyy"));
                    col++;
                }
                if (ViewColumn.IsSex)
                {
                    row.CreateCell(col).SetValue(GetNotNullString(check.Sex));
                    col++;
                }
                if (ViewColumn.IsPolicySeries)
                {
                    row.CreateCell(col).SetValue(GetNotNullString(check.PolicySeries));
                    col++;
                }
                if (ViewColumn.IsPolicyNumber)
                {
                    row.CreateCell(col).SetValue(GetNotNullString(check.PolicyNumber));
                    col++;
                }
                if (ViewColumn.IsUnifiedPolicyNumber)
                {
                    row.CreateCell(col).SetValue(GetNotNullString(check.UnifiedPolicyNumber));
                    col++;
                }
                if (ViewColumn.IsDocumentSeries)
                {
                    row.CreateCell(col).SetValue(GetNotNullString(check.DocumentSeries));
                    col++;
                }
                if (ViewColumn.IsDocumentNumber)
                {
                    row.CreateCell(col).SetValue(GetNotNullString(check.DocumentNumber));
                    col++;
                }
                if (ViewColumn.IsLivingFullAddressString)
                {
                    row.CreateCell(col).SetValue(GetNotNullString(check.LivingFullAddressString));
                    col++;
                }
                if (ViewColumn.IsOfficialFullAddressString)
                {
                    row.CreateCell(col).SetValue(GetNotNullString(check.OfficialFullAddressString));
                    col++;
                }
                if (ViewColumn.IsTemporaryPolicyNumber)
                {
                    row.CreateCell(col).SetValue(GetNotNullString(check.TemporaryPolicyNumber));
                    col++;
                }
                if (ViewColumn.IsTemporaryPolicyDate)
                {
                    row.CreateCell(col).SetValue(GetDateString(check.TemporaryPolicyDate, "dd.MM.yyyy"));
                    col++;
                }
                if (ViewColumn.IsSNILS)
                {
                    row.CreateCell(col).SetValue(GetNotNullString(check.SNILS));
                    col++;
                }
                if (ViewColumn.IsPhone)
                {
                    row.CreateCell(col).SetValue(GetNotNullString(check.Phone));
                    col++;
                }

                rowNumber++;
            }

            //применяем стиль для шапки таблицы
            for (int i = 0; i < colHeadTable; i++)
            {
                rowHeadTable.GetCell(i).CellStyle = style;
            }
            for (int i = 2; i < colHeadTable; i++)
            {
                table.AutoSizeColumn(i);
            }
        }
    }
}
