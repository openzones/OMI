using System.ComponentModel;

namespace OMInsurance.WebApps.Models
{
    public class LoginModel
    {
        [DisplayName("Логин")]
        public string Login { get; set; }

        [DisplayName("Пароль")]
        public string Password { get; set; }
    }
}