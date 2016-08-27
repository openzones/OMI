using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace OMInsurance.WebApps.Models.PrintedForms
{
    public class BSOMoveReportYearModel
    {
        public BSOMoveReportYearModel()
        {
            Years = FillYears();
            Year = DateTime.Now.Year;
        }

        [DisplayName("Год")]
        public int Year { get; set; }
        public List<SelectListItem> Years { get; set; }

        public static List<SelectListItem> FillYears()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            DateTime now = DateTime.Now;
            for (int year = 2015; year <= now.Year; year++)
            {
                listItems.Add(new SelectListItem()
                {
                    Text = year.ToString(),
                    Value = year.ToString()
                });
            }
            return listItems;
        }
    }
}