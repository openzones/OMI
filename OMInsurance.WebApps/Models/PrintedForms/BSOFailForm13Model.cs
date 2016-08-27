using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OMInsurance.WebApps.Models.PrintedForms
{
    public class BSOFailForm13Model
    {
        public BSOFailForm13Model()
        {
            DateFrom =  new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTo = DateTime.Now;
        }

        [DisplayName("Дата статуса с")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public DateTime DateFrom { get; set; }

        [DisplayName("Дата статуса по (включительно)")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public DateTime DateTo { get; set; }

    }
}