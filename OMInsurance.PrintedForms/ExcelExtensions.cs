using NPOI.SS.UserModel;
using System.Collections;
using System.Linq;

namespace OMInsurance.PrintedForms.Helpers
{
	public static class ExcelExtensions
	{
		public static void SetCellValue(this ISheet sheet, int row, int col, string value)
		{
			if (sheet != null)
			{
				IRow dataRow = sheet.GetRow(row);
				ICell cell = dataRow.GetCell(col);
				cell.SetCellValue(value);
			}
		}

        public static ICell FindCellByMacros(this ISheet sheet, string name)
		{
			if (sheet != null)
			{
				IEnumerator enumerator = sheet.GetRowEnumerator();
				while (enumerator.MoveNext())
				{
                    IRow item = (IRow)enumerator.Current;
					ICell cell = item.Cells.FirstOrDefault(c => c.CellType == CellType.String && c.StringCellValue != null && c.StringCellValue.Contains(name));
					if (cell != null)
					{
						return cell;
					}
				}
			}

			return null;
		}

		public static void RemoveMacros(this ICell cell, string name)
		{
			if (cell != null && cell.StringCellValue != null)
			{
				string data = cell.StringCellValue.Replace(name, string.Empty);
				cell.SetCellValue(data);
			}
		}

		public static void CheckCellValue(this ISheet sheet, int row, int col, bool isChecked, bool saveCellData)
		{
			if (sheet != null)
			{
				IRow dataRow = sheet.GetRow(row);
				ICell cell = dataRow.GetCell(col);
				string val = isChecked ? "[X]" : "[  ]";

				if (saveCellData)
				{
                    string data = cell.StringCellValue;
					val = val + "    {0}";
					val = string.Format(val, data);
				}
				cell.SetCellValue(val);
			}
		}

		public static string GetCellValue(this ISheet sheet, int row, int col, string value)
		{
			var result = string.Empty;
			if (sheet != null)
			{
				IRow dataRow = sheet.GetRow(row);
				ICell cell = dataRow.GetCell(col);
				result = cell.StringCellValue;
			}
			return result;
		}

		public static void SetValue(this ICell cell, string value)
		{
			if (cell != null)
			{
				cell.SetCellValue(value);
			}
		}

		public static void SetCheckValue(this ICell cell, bool isChecked, bool withSaveData)
		{
			if (cell != null)
			{
				string val = isChecked ? "V" : string.Empty;

				if (withSaveData)
				{
					string data = cell.StringCellValue;
					val = val + "    {0}";
                    val = string.Format(val, data);
				}
				cell.SetCellValue(val);
			}
		}
	}
}
