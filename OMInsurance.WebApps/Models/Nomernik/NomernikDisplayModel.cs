using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OMInsurance.WebApps.Models
{
    public class NomernikDisplayModel
    {
        public NomernikDisplayModel()
        {
            Messages = new List<string>();
            Months = FillMonths();
            Years = FillYears();
            Year = DateTime.Now.Year;
        }

        [DisplayName("Статус загрузки")]
        public bool StatusLoad { get; set; }

        [DisplayName("Размер файла")]
        public string FileSizeToString { get; set; }

        [DisplayName("Всего записей")]
        public long? CountAllRow { get; set; }

        [DisplayName("Всего найдено по нашей СГ")]
        public long? CountOurRow { get; set; }

        [DisplayName("Всего обновлено/изменено")]
        public long? CountChangeRow { get; set; }

        public string FilePath { get; set; }

        public List<string> Messages { get; set; }

        [DisplayName("Месяц")]
        public int MonthID { get; set; }

        [DisplayName("Год")]
        public int Year { get; set; }

        [DisplayName("Дата загрузки")]
        public DateTime LoadDate { get; set; }

        [DisplayName("Месяц/Год загрузки")]
        public DateTime FileDate { get; set; }

        [DisplayName("Загрузил")]
        public string FIO { get; set; }

        public long HistoryID { get; set; }

        public List<SelectListItem> Months { get; set; }
        public List<SelectListItem> Years { get; set; }
        public static List<SelectListItem> FillMonths()
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
        public static List<SelectListItem> FillYears()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            DateTime now = DateTime.Now;
            for( int year = 2015; year <= now.Year; year++)
            {
                listItems.Add(new SelectListItem() {
                    Text = year.ToString(),
                    Value = year.ToString()
                });
            }
            return listItems;
        }
    }


}