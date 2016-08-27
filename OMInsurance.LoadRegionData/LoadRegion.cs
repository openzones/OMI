using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.IO;
using System.Collections.Generic;
using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System.Text.RegularExpressions;

namespace OMInsurance.LoadRegionData
{
    public abstract class LoadRegion
    {
        public string Path { get; set; }

        public List<PolicyFromRegion> GetPolicy(DataPreLoad dataPreLoad)
        {
            HSSFWorkbook templateWorkbook = null;
            XSSFWorkbook xssf = null;
            ISheet table;
            string extention = System.IO.Path.GetExtension(Path);

            using (FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read))
            {
                if (extention != ".xls")
                {
                    xssf = new XSSFWorkbook(fs);
                    table = xssf[0];
                }
                else
                {
                    templateWorkbook = new HSSFWorkbook(fs, true);
                    table = templateWorkbook[0];
                }
            }

            return Process(table, dataPreLoad);
        }

        protected abstract List<PolicyFromRegion> Process(ISheet table, DataPreLoad dataPreLoad);


        public class DataPreLoad
        {
            public long CountField { get; set; }
            public long CountRow { get; set; }
            public long? AutoRegionId { get; set; }
        }

        /// <summary>
        /// Функция предварительной обработки файла
        /// </summary>
        /// <returns></returns>
        public static LoadRegion.DataPreLoad PreLoadExcel(string Path)
        {
            LoadRegion.DataPreLoad preLoadRegion = new DataPreLoad();

            HSSFWorkbook templateWorkbook = null;
            XSSFWorkbook xssf = null;
            ISheet table;
            long countField = 0;
            long countRow = 0;
            long countEmpty = 0;
            string extention = System.IO.Path.GetExtension(Path);

            using (FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read))
            {
                if (extention != ".xls")
                {
                    xssf = new XSSFWorkbook(fs);
                    table = xssf[0];
                }
                else
                {
                    templateWorkbook = new HSSFWorkbook(fs, true);
                    table = templateWorkbook[0];
                }
            }

            for (int i = 0; ; i++)
            {
                if (table.GetRow(0).GetCell(i) == null)
                {
                    countEmpty++;
                    if (countEmpty > 10) break;
                }
                else
                {
                    countField++;
                    countEmpty = 0;
                }
            }

            countEmpty = 0;
            for (int i = 0; ; i++)
            {
                if (table.GetRow(i) == null)
                {
                    countEmpty++;
                    if (countEmpty > 10) break;
                }
                else
                {
                    countRow++;
                    countEmpty = 0;
                }
            }
            preLoadRegion.CountRow = countRow - 1;
            preLoadRegion.CountField = countField;

            //делаем автоопределение региона
            if (preLoadRegion.CountField == Constants.DefaultCountFieldInRegionExcel)
            {
                preLoadRegion.AutoRegionId = (long)ListRegionId.Default;
            }
            if (preLoadRegion.CountField == Constants.CountFieldInUfa)
            {
                preLoadRegion.AutoRegionId = (long)ListRegionId.Ufa;
            }
            if (preLoadRegion.CountField == Constants.CountFieldInMoscowObl)
            {
                preLoadRegion.AutoRegionId = (long)ListRegionId.MoscowObl;
            }

            return preLoadRegion;
        }

        protected virtual long GetLong(ICell cell)
        {
            CellType type = cell.CellType;
            if (type == CellType.Numeric)
            {
                return (long)cell.NumericCellValue;
            }
            else
            {
                if (type == CellType.String)
                {
                    try
                    {
                        return long.Parse(cell.StringCellValue);
                    }
                    catch
                    {
                        return (long)0;
                    }

                }
                else
                {
                    return (long)0;
                }
            }
        }

        protected virtual DateTime? GetDateTime(ICell cell)
        {
            CellType type = cell.CellType;
            if (type == CellType.Numeric)
            {
                try
                {
                    return cell.DateCellValue;
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                if (type == CellType.String)
                {
                    try
                    {
                        string value = cell.StringCellValue;
                        return DateTime.Parse(value);
                    }
                    catch
                    {
                        return null;
                    }

                }
                else
                {
                    return null;
                }
            }
        }

        //проверяем телефон, и если формат не правильный, то пытаем привести к нашему виду типа: (123)456-78-90
        protected virtual string GetPhone(string phone)
        {
            if (string.IsNullOrEmpty(phone)) return null;
            string pattern = Constants.PhoneRegex;

            try
            {
                if (Regex.IsMatch(phone, pattern, RegexOptions.IgnoreCase))
                {
                    return phone;
                }
                else
                {
                    string str = phone.Trim();
                    string[] trimChars = { "-", "(", ")", "+", "_", " ", "*", ":", "#", ",", "/", ".", "'" };

                    foreach (var ch in trimChars)
                    {
                        str = str.Replace(ch, "");
                    }

                    if (str.Length > 10)
                    {
                        str = str.Substring(str.Length - 10);
                    }

                    str = "(" + str;
                    str = str.Insert(4, ")");
                    str = str.Insert(8, "-");
                    str = str.Insert(11, "-");
                    return str;
                }
            }
            catch
            {//если что-то пойдет не так - возвращаем null
                return null;
            }
        }

        //обработка пола
        protected virtual string GetSex(string sex)
        {
            if (string.IsNullOrEmpty(sex)) return null;
            if (sex.Length > 1) sex = sex.Remove(1);
            if (string.Equals(sex.ToUpper(), "М") || string.Equals(sex.ToUpper(), "1") || string.Equals(sex.ToUpper(), "M"))
            {
                return "M";
            }
            else
            {
                return "Ж";
            }
        }
    }
}
