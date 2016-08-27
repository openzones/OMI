using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.IO;

namespace OMInsurance.PrintedForms
{
    public abstract class PrintedForm
    {
        public string TemplatePath { get; set; }
        public byte[] GetExcel()
        {
            HSSFWorkbook templateWorkbook = null;
            XSSFWorkbook xssf = null;
            ISheet table;
            string extention = System.IO.Path.GetExtension(TemplatePath);

            using (FileStream fs = new FileStream(TemplatePath, FileMode.Open, FileAccess.Read))
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

            Process(table);

            using (MemoryStream ms = new MemoryStream())
            {
                if (templateWorkbook != null) templateWorkbook.Write(ms);
                if (xssf != null) xssf.Write(ms);
                return ms.ToArray();
            }
        }

        protected virtual string GetDateString(DateTime? dateTime, string format)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToString(format);
            }
            return string.Empty;
        }

        protected virtual string GetNotNullString(string str)
        {
            return string.IsNullOrEmpty(str) ? string.Empty : str;
        }

        protected abstract void Process(ISheet table);
    }
}
