using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using System.ComponentModel;

namespace OMInsurance.WebApps.Models
{
    public class UserModel
    {
        public UserModel()
        { }

        public UserModel(User user)
        {
            this.Id = user.Id;
            this.Login = user.Login;
            this.Department = user.Department;
            this.Firstname = user.Firstname;
            this.Secondname = user.Secondname;
            this.Lastname = user.Lastname;
        }

        public long? Id { get; set; }

        [DisplayName("Логин")]
        public string Login { get; set; }

        [DisplayName("Пароль")]
        public string Password { get; set; }

        [DisplayName("Отделение")]
        public ReferenceItem Department { get; set; }

        [DisplayName("Имя")]
        public string Firstname { get; set; }

        [DisplayName("Отчество")]
        public string Secondname { get; set; }

        [DisplayName("Фамилия")]
        public string Lastname { get; set; }
    }
}