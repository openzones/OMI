using System.ComponentModel;

namespace OMInsurance.WebApps.Models
{
    public class RestoreModel
    {
        [DisplayName("Логин")]
        public string Login { get; set; }

        [DisplayName("Выберите")]
        public string Choice { get; set; }
    }
}