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
    public class BSOMoveReportYear : PrintedForm
    {
        private List<BSOHistoryItem> ListBsoHistory;
        private List<ReferenceItem> ListDeliveryCenter;
        private int Year;

        #region Marks
        protected const string B1 = "$B1$";
        protected const string Months1 = "$Months1$";
        protected const string Months2 = "$Months2$";
        protected const string Months3 = "$Months3$";
        protected const string Months4 = "$Months4$";
        protected const string Months5 = "$Months5$";
        protected const string Months6 = "$Months6$";
        protected const string Months7 = "$Months7$";
        protected const string Months8 = "$Months8$";
        protected const string Months9 = "$Months9$";
        protected const string Months10 = "$Months10$";
        protected const string Months11 = "$Months11$";
        protected const string Months12 = "$Months12$";
        protected const string FilialMonths1 = "$FilialMonths1$";
        protected const string FilialMonths2 = "$FilialMonths2$";
        protected const string FilialMonths3 = "$FilialMonths3$";
        protected const string FilialMonths4 = "$FilialMonths4$";
        protected const string FilialMonths5 = "$FilialMonths5$";
        protected const string FilialMonths6 = "$FilialMonths6$";
        protected const string FilialMonths7 = "$FilialMonths7$";
        protected const string FilialMonths8 = "$FilialMonths8$";
        protected const string FilialMonths9 = "$FilialMonths9$";
        protected const string FilialMonths10 = "$FilialMonths10$";
        protected const string FilialMonths11 = "$FilialMonths11$";
        protected const string FilialMonths12 = "$FilialMonths12$";
        protected const string totalYear1 = "$totalYear1$";
        protected const string totalYear2 = "$totalYear2$";
        protected const string issued = "$issued$";
        protected const string ruined = "$ruined$";
        #endregion

        public BSOMoveReportYear(List<BSOHistoryItem> listBsoHistory, List<ReferenceItem> listDeliveryCenter, int year)
        {
            ListBsoHistory = listBsoHistory;
            ListDeliveryCenter = listDeliveryCenter;
            Year = year;
            TemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Templates", "BSOMoveReportYear.xls");
        }

        protected override void Process(ISheet table)
        {
            FillTemplate(table, Year);
            int rowNumber = 3;

            for (int i = 0; i < ListDeliveryCenter.Count; i++)
            {
                IRow row = table.CreateRow(rowNumber + i);

                //B
                var res = ListBsoHistory.Where(a => a.DeliveryCenterId == ListDeliveryCenter[i].Id)
                                        .Where(b => b.Status.Id == (long)ListBSOStatusID.OnDelivery)
                                        .Where(c => c.StatusDate < new DateTime(Year, 1, 1))
                                        .GroupBy(d=>d.Id);
                row.CreateCell(0).SetValue(ListDeliveryCenter[i].Name.ToString());
                if(res.Count() != 0) row.CreateCell(1).SetCellValue(res.Count());
                
                for (int month = 1; month <= 12; month++)
                {
                    //C-M
                    if (month < 12)
                    {
                        var res1 = ListBsoHistory.Where(a => a.DeliveryCenterId == ListDeliveryCenter[i].Id)
                             .Where(b => b.Status.Id == (long)ListBSOStatusID.OnDelivery)
                             .Where(c => c.StatusDate < new DateTime(Year, month + 1, 1) && c.StatusDate >= new DateTime(Year, month, 1))
                             .GroupBy(d=>d.Id);
                        if (res1.Count() != 0) row.CreateCell(1 + month).SetCellValue(res1.Count());
                    }
                    else
                    {
                        //N
                        var res1 = ListBsoHistory.Where(a => a.DeliveryCenterId == ListDeliveryCenter[i].Id)
                                 .Where(b => b.Status.Id == (long)ListBSOStatusID.OnDelivery)
                                 .Where(c => c.StatusDate < new DateTime(Year + 1, month + 1 - month, 1) && c.StatusDate >= new DateTime(Year, month, 1))
                                 .GroupBy(d => d.Id);
                        if (res1.Count() != 0) row.CreateCell(1 + month).SetCellValue(res1.Count());
                    }

                }

                //O
                row.CreateCell(14).SetCellFormula(string.Format( "SUM(C{0}:N{1})", rowNumber+1+i, rowNumber+1+i));

                //Q-R * 12 month
                for (int month = 1, j = 0; month <= 12; month++, j=j+3)
                {
                    if(month < 12)
                    {
                        var resQ = ListBsoHistory.Where(a => a.DeliveryCenterId == ListDeliveryCenter[i].Id)
                            .Where(b => b.Status.Id == (long)ListBSOStatusID.OnClient)
                            .Where(c => c.StatusDate < new DateTime(Year, month + 1, 1) && c.StatusDate >= new DateTime(Year, month, 1))
                            .GroupBy(d => d.Id);
                        if (resQ.Count() != 0) row.CreateCell(16 + j).SetCellValue(resQ.Count());
                        var resR = ListBsoHistory.Where(a => a.DeliveryCenterId == ListDeliveryCenter[i].Id)
                                .Where(b => b.Status.Id == (long)ListBSOStatusID.FailOnResponsible)
                                .Where(c => c.StatusDate < new DateTime(Year, month + 1, 1) && c.StatusDate >= new DateTime(Year, month, 1))
                                .GroupBy(d => d.Id);
                        if (resR.Count() != 0) row.CreateCell(17 + j).SetCellValue(resR.Count());
                    }
                    else
                    {
                        var resQ = ListBsoHistory.Where(a => a.DeliveryCenterId == ListDeliveryCenter[i].Id)
                            .Where(b => b.Status.Id == (long)ListBSOStatusID.OnClient)
                            .Where(c => c.StatusDate < new DateTime(Year + 1, 1, 1) && c.StatusDate >= new DateTime(Year, 12, 1))
                            .GroupBy(d => d.Id);
                        if (resQ.Count() != 0) row.CreateCell(16 + j).SetCellValue(resQ.Count());
                        var resR = ListBsoHistory.Where(a => a.DeliveryCenterId == ListDeliveryCenter[i].Id)
                                .Where(b => b.Status.Id == (long)ListBSOStatusID.FailOnResponsible)
                                .Where(c => c.StatusDate < new DateTime(Year + 1, 1, 1) && c.StatusDate >= new DateTime(Year, 12, 1))
                                .GroupBy(d => d.Id);
                        if (resR.Count() != 0) row.CreateCell(17 + j).SetCellValue(resR.Count());
                    }

                }

                row.CreateCell(18).SetCellFormula(string.Format("B{0}+C{0}-Q{0}-R{0}", rowNumber + 1 + i));
                row.CreateCell(21).SetCellFormula(string.Format("S{0}+D{0}-T{0}-U{0}", rowNumber + 1 + i));
                row.CreateCell(24).SetCellFormula(string.Format("V{0}+E{0}-W{0}-X{0}", rowNumber + 1 + i));
                row.CreateCell(27).SetCellFormula(string.Format("Y{0}+F{0}-Z{0}-AA{0}", rowNumber + 1 + i));
                row.CreateCell(30).SetCellFormula(string.Format("AB{0}+G{0}-AC{0}-AD{0}", rowNumber + 1 + i));
                row.CreateCell(33).SetCellFormula(string.Format("AE{0}+H{0}-AF{0}-AG{0}", rowNumber + 1 + i));
                row.CreateCell(36).SetCellFormula(string.Format("AH{0}+I{0}-AI{0}-AJ{0}", rowNumber + 1 + i));
                row.CreateCell(39).SetCellFormula(string.Format("AK{0}+J{0}-AL{0}-AM{0}", rowNumber + 1 + i));
                row.CreateCell(42).SetCellFormula(string.Format("AN{0}+K{0}-AO{0}-AP{0}", rowNumber + 1 + i));
                row.CreateCell(45).SetCellFormula(string.Format("AQ{0}+L{0}-AR{0}-AS{0}", rowNumber + 1 + i));
                row.CreateCell(48).SetCellFormula(string.Format("AT{0}+M{0}-AU{0}-AV{0}", rowNumber + 1 + i));
                row.CreateCell(51).SetCellFormula(string.Format("AW{0}+N{0}-AX{0}-AY{0}", rowNumber + 1 + i));

                //BA-BD
                row.CreateCell(52).SetCellFormula(string.Format("O{0}", rowNumber + 1 + i));
                row.CreateCell(53).SetCellFormula(string.Format("Q{0}+T{0}+W{0}+Z{0}+AC{0}+AF{0}+AI{0}+AL{0}+AO{0}+AR{0}+AU{0}+AX{0}", rowNumber + 1 + i));
                row.CreateCell(54).SetCellFormula(string.Format("R{0}+U{0}+X{0}+AA{0}+AD{0}+AG{0}+AJ{0}+AM{0}+AP{0}+AS{0}+AV{0}+AY{0}", rowNumber + 1 + i));
                row.CreateCell(55).SetCellFormula(string.Format("B{0}+O{0}-BB{0}-BC{0}", rowNumber + 1 + i));
            }

            //стиль - жирный, курсивный
            ICellStyle style = table.Workbook.CreateCellStyle();
            IFont font = table.Workbook.CreateFont();
            font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
            font.IsItalic = true;
            style.SetFont(font);

            //нижняя строка (суммирующая)
            int numberLastRow = rowNumber + ListDeliveryCenter.Count;
            IRow rowSum = table.CreateRow(numberLastRow);
            List<string> listColumn = new List<string>() {"B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z",
                "AA","AB","AC","AD","AE","AF","AG","AH","AI","AJ","AK","AL","AM","AN","AO","AP","AQ","AR","AS","AT","AU","AV","AW","AX","AY","AZ",
                "BA","BB","BC","BD"};
            for(int i = 0; i < listColumn.Count; i++)
            {
                rowSum.CreateCell(1+i).SetCellFormula(string.Format("SUM({0}{1}:{0}{2})", listColumn[i], rowNumber + 1, numberLastRow));
                rowSum.GetCell(1 + i).CellStyle = style;
            }

            IRow row1 = table.CreateRow(numberLastRow+1);
            IRow row2 = table.CreateRow(numberLastRow+2);
            row1.CreateCell(0).SetValue("Остаток бланков в Дирекции:");
            row2.CreateCell(0).SetValue("Остаток бланков ВСЕГО:");
            row1.GetCell(0).CellStyle = style;
            row2.GetCell(0).CellStyle = style;

            var result1 = ListBsoHistory.Where(b => b.Status.Id == (long)ListBSOStatusID.OnStorage)
                                    .Where(c => c.StatusDate < new DateTime(Year, 1, 1))
                                    .GroupBy(a => a.Id);
            row1.CreateCell(1).SetCellValue(result1.Count());
            row2.CreateCell(1).SetCellFormula(string.Format("B{0}+B{1}", numberLastRow+1, numberLastRow+2));

            row1.CreateCell(15).SetValue("Остаток бланков в Дирекции:");
            row2.CreateCell(15).SetValue("Остаток бланков ВСЕГО:");
            row1.GetCell(15).CellStyle = style;
            row2.GetCell(15).CellStyle = style;

            row1.CreateCell(18).SetCellFormula(string.Format("P{0}+N{0}+M{0}+L{0}+K{0}+J{0}+I{0}+H{0}+G{0}+F{0}+E{0}+D{0}", numberLastRow + 1));
            row2.CreateCell(18).SetCellFormula(string.Format("S{0}+S{1}", numberLastRow + 1, numberLastRow + 2));
            row1.CreateCell(21).SetCellFormula(string.Format("P{0}+N{0}+M{0}+L{0}+K{0}+J{0}+I{0}+H{0}+G{0}+F{0}+E{0}", numberLastRow + 1));
            row2.CreateCell(21).SetCellFormula(string.Format("V{0}+V{1}", numberLastRow + 1, numberLastRow + 2));
            row1.CreateCell(24).SetCellFormula(string.Format("P{0}+N{0}+M{0}+L{0}+K{0}+J{0}+I{0}+H{0}+G{0}+F{0}", numberLastRow + 1));
            row2.CreateCell(24).SetCellFormula(string.Format("Y{0}+Y{1}", numberLastRow + 1, numberLastRow + 2));
            row1.CreateCell(27).SetCellFormula(string.Format("P{0}+N{0}+M{0}+L{0}+K{0}+J{0}+I{0}+H{0}+G{0}", numberLastRow + 1));
            row2.CreateCell(27).SetCellFormula(string.Format("AB{0}+AB{1}", numberLastRow + 1, numberLastRow + 2));
            row1.CreateCell(30).SetCellFormula(string.Format("P{0}+N{0}+M{0}+L{0}+K{0}+J{0}+I{0}+H{0}", numberLastRow + 1));
            row2.CreateCell(30).SetCellFormula(string.Format("AE{0}+AE{1}", numberLastRow + 1, numberLastRow + 2));
            row1.CreateCell(33).SetCellFormula(string.Format("P{0}+N{0}+M{0}+L{0}+K{0}+J{0}+I{0}", numberLastRow + 1));
            row2.CreateCell(33).SetCellFormula(string.Format("AH{0}+AH{1}", numberLastRow + 1, numberLastRow + 2));
            row1.CreateCell(36).SetCellFormula(string.Format("P{0}+N{0}+M{0}+L{0}+K{0}+J{0}", numberLastRow + 1));
            row2.CreateCell(36).SetCellFormula(string.Format("AK{0}+AK{1}", numberLastRow + 1, numberLastRow + 2));
            row1.CreateCell(39).SetCellFormula(string.Format("P{0}+N{0}+M{0}+L{0}+K{0}", numberLastRow + 1));
            row2.CreateCell(39).SetCellFormula(string.Format("AN{0}+AN{1}", numberLastRow + 1, numberLastRow + 2));
            row1.CreateCell(42).SetCellFormula(string.Format("P{0}+N{0}+M{0}+L{0}", numberLastRow + 1));
            row2.CreateCell(42).SetCellFormula(string.Format("AQ{0}+AQ{1}", numberLastRow + 1, numberLastRow + 2));
            row1.CreateCell(45).SetCellFormula(string.Format("P{0}+N{0}+M{0}", numberLastRow + 1));
            row2.CreateCell(45).SetCellFormula(string.Format("AT{0}+AT{1}", numberLastRow + 1, numberLastRow + 2));
            row1.CreateCell(48).SetCellFormula(string.Format("P{0}+N{0}", numberLastRow + 1));
            row2.CreateCell(48).SetCellFormula(string.Format("AW{0}+AW{1}", numberLastRow + 1, numberLastRow + 2));
            row1.CreateCell(51).SetCellFormula(string.Format("P{0}", numberLastRow + 1));
            row2.CreateCell(51).SetCellFormula(string.Format("AZ{0}+AZ{1}", numberLastRow + 1, numberLastRow + 2));
            row1.CreateCell(55).SetCellFormula(string.Format("P{0}", numberLastRow + 1));
            row2.CreateCell(55).SetCellFormula(string.Format("BD{0}+BD{1}", numberLastRow + 1, numberLastRow + 2));

            ICellStyle styleBorder = table.Workbook.CreateCellStyle();
            styleBorder.BorderTop = BorderStyle.Thin;
            styleBorder.BorderRight = BorderStyle.Thin;
            styleBorder.BorderLeft = BorderStyle.Thin;
            styleBorder.BorderBottom = BorderStyle.Thin;

            for (int y = 3; y < ListDeliveryCenter.Count + 3; y++)
            {
                for (int x = 0; x < 56; x++)
                {
                    if (table.GetRow(y).GetCell(x) != null)
                    {
                        table.GetRow(y).GetCell(x).CellStyle = styleBorder;
                    }
                    else
                    {
                        table.GetRow(y).CreateCell(x).CellStyle = styleBorder;
                    }
                }
            }
           

        }

        //заполняем шапку
        private static void FillTemplate(ISheet table, int year)
        {
            table.FindCellByMacros(B1).SetValue(string.Format("Остаток бланков временных свидетельств на {0} г.", new DateTime(year, 1, 1).ToShortDateString()));
            for(int i = 0; i < 2; i++)
            {
                table.FindCellByMacros(Months1).SetCellValue(new DateTime(year, 1, 1));
                table.FindCellByMacros(Months2).SetCellValue(new DateTime(year, 2, 1));
                table.FindCellByMacros(Months3).SetCellValue(new DateTime(year, 3, 1));
                table.FindCellByMacros(Months4).SetCellValue(new DateTime(year, 4, 1));
                table.FindCellByMacros(Months5).SetCellValue(new DateTime(year, 5, 1));
                table.FindCellByMacros(Months6).SetCellValue(new DateTime(year, 6, 1));
                table.FindCellByMacros(Months7).SetCellValue(new DateTime(year, 7, 1));
                table.FindCellByMacros(Months8).SetCellValue(new DateTime(year, 8, 1));
                table.FindCellByMacros(Months9).SetCellValue(new DateTime(year, 9, 1));
                table.FindCellByMacros(Months10).SetCellValue(new DateTime(year, 10, 1));
                table.FindCellByMacros(Months11).SetCellValue(new DateTime(year, 11, 1));
                table.FindCellByMacros(Months12).SetCellValue(new DateTime(year, 12, 1));
                table.FindCellByMacros(FilialMonths12).SetValue(string.Format("остаток в филиалах на {0}", new DateTime(year + 1, 1, 1).ToShortDateString()));
            }

            table.FindCellByMacros(FilialMonths1).SetValue(string.Format("остаток в филиалах на {0}", new DateTime(year, 1 + 1, 1).ToShortDateString()));
            table.FindCellByMacros(FilialMonths2).SetValue(string.Format("остаток в филиалах на {0}", new DateTime(year, 2 + 1, 1).ToShortDateString()));
            table.FindCellByMacros(FilialMonths3).SetValue(string.Format("остаток в филиалах на {0}", new DateTime(year, 3 + 1, 1).ToShortDateString()));
            table.FindCellByMacros(FilialMonths4).SetValue(string.Format("остаток в филиалах на {0}", new DateTime(year, 4 + 1, 1).ToShortDateString()));
            table.FindCellByMacros(FilialMonths5).SetValue(string.Format("остаток в филиалах на {0}", new DateTime(year, 5 + 1, 1).ToShortDateString()));
            table.FindCellByMacros(FilialMonths6).SetValue(string.Format("остаток в филиалах на {0}", new DateTime(year, 6 + 1, 1).ToShortDateString()));
            table.FindCellByMacros(FilialMonths7).SetValue(string.Format("остаток в филиалах на {0}", new DateTime(year, 7 + 1, 1).ToShortDateString()));
            table.FindCellByMacros(FilialMonths8).SetValue(string.Format("остаток в филиалах на {0}", new DateTime(year, 8 + 1, 1).ToShortDateString()));
            table.FindCellByMacros(FilialMonths9).SetValue(string.Format("остаток в филиалах на {0}", new DateTime(year, 9 + 1, 1).ToShortDateString()));
            table.FindCellByMacros(FilialMonths10).SetValue(string.Format("остаток в филиалах на {0}", new DateTime(year, 10 + 1, 1).ToShortDateString()));
            table.FindCellByMacros(FilialMonths11).SetValue(string.Format("остаток в филиалах на {0}", new DateTime(year, 11 + 1, 1).ToShortDateString()));

            table.FindCellByMacros(totalYear1).SetValue(string.Format("Итого за {0} год", year));
            table.FindCellByMacros(totalYear2).SetValue(string.Format("получено из Дирекции за {0} г.", year));
            table.FindCellByMacros(issued).SetValue(string.Format("выдано за {0} г. ", year));
            table.FindCellByMacros(ruined).SetValue(string.Format("испорчено за {0} г.", year));
        }
    }
}
