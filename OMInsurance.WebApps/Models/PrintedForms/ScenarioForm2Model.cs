using OMInsurance.Entities.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace OMInsurance.WebApps.Models.PrintedForms
{
    public class ScenarioForm2Model
    {
        public ScenarioForm2Model()
        {
            Months = FillMonths();
            Years = FillYears();
            this.Year = DateTime.Now.Year;
            this.MonthID = DateTime.Now.Month;
        }

        [DisplayName("Месяц")]
        public int MonthID { get; set; }

        [DisplayName("Год")]
        public int Year { get; set; }
        public List<SelectListItem> Months { get; set; }
        public List<SelectListItem> Years { get; set; }

        private static List<SelectListItem> FillMonths()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            //'Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь', 'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'
            listItems.Add(new SelectListItem() { Text = "Январь", Value = "1", Selected = true });
            listItems.Add(new SelectListItem() { Text = "Февраль", Value = "2", Selected = false });
            listItems.Add(new SelectListItem() { Text = "Март", Value = "3", Selected = false });
            listItems.Add(new SelectListItem() { Text = "Апрель", Value = "4", Selected = false });
            listItems.Add(new SelectListItem() { Text = "Май", Value = "5", Selected = false });
            listItems.Add(new SelectListItem() { Text = "Июнь", Value = "6", Selected = false });
            listItems.Add(new SelectListItem() { Text = "Июль", Value = "7", Selected = false });
            listItems.Add(new SelectListItem() { Text = "Август", Value = "8", Selected = false });
            listItems.Add(new SelectListItem() { Text = "Сентябрь", Value = "9", Selected = false });
            listItems.Add(new SelectListItem() { Text = "Октябрь", Value = "10", Selected = false });
            listItems.Add(new SelectListItem() { Text = "Ноябрь", Value = "11", Selected = false });
            listItems.Add(new SelectListItem() { Text = "Декабрь", Value = "12", Selected = false });
            return listItems;
        }
        private static List<SelectListItem> FillYears()
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