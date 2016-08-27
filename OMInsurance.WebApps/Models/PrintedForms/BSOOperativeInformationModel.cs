using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OMInsurance.WebApps.Models.PrintedForms
{
    public class BSOOperativeInformationModel
    {
        public BSOOperativeInformationModel()
        {
            Date = DateTime.Now;
        }

        [DisplayName("На указанную дату (включительно)")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public DateTime Date { get; set; }
    }
}