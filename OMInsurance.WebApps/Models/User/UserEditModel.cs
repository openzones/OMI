using OMInsurance.Entities;
using OMInsurance.Entities.Core;
using OMInsurance.Entities.SMS;
using OMInsurance.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;

namespace OMInsurance.WebApps.Models
{
    public class UserEditModel
    {
        #region Constructors
        public UserEditModel()
        {
            Departments = ReferencesProvider.GetReferences(Constants.DeliveryCenterForOperatorRef, null, true);
            DeliveryPoints = ReferencesProvider.GetReferences(Constants.DeliveryPointRef, null, true);
            RolesList = ReferencesProvider.GetRoles();
            Roles = new List<long>();
        }

        public UserEditModel(User user)
            : this()
        {
            Id = user.Id;
            Login = user.Login;
            IsBlocked = user.IsBlocked ?? false;
            Lastname = user.Lastname;
            Firstname = user.Firstname;
            Secondname = user.Secondname;
            DepartmentId = user.Department.Id;
            DeliveryPointId = user.DeliveryPoint.Id;
            Roles = user.Roles.Select(r => r.Id).ToList();
            foreach (var role in RolesList)
            {
                role.Selected = Roles.Exists(item => item.ToString().Equals(role.Value));
            }
            Position = user.Position;
            Email = user.Email;
            Phone = user.Phone;
        }
        #endregion

        #region Properties
        public long? Id { get; set; }

        [DisplayName("Логин")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string Login { get; set; }

        [DisplayName("Пароль")]
        [Required(ErrorMessage = "Поле обязательно для заполнения"), StringLength(12, MinimumLength = 6)]
        public string Password { get; set; }

        [DisplayName("Заблокирован")]
        public bool IsBlocked { get; set; }

        [DisplayName("Отделение")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public long DepartmentId { get; set; }
        public List<SelectListItem> Departments { get; set; }

        [DisplayName("Точка продаж")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public long DeliveryPointId { get; set; }
        public List<SelectListItem> DeliveryPoints { get; set; }

        [DisplayName("Роли")]
        public List<long> Roles { get; set; }
        public List<SelectListItem> RolesList { get; set; }

        [DisplayName("Имя")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string Firstname { get; set; }

        [DisplayName("Отчество")]
        public string Secondname { get; set; }

        [DisplayName("Фамилия")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string Lastname { get; set; }

        [DisplayName("Должность")]
        public string Position { get; set; }

        [DisplayName("E-mail")]
        [RegularExpression(Constants.EmailRegex, ErrorMessage = "Неверное значение")]
        public string Email { get; set; }

        [DisplayName("Мобильный телефон")]
        [RegularExpression(Constants.PhoneRegex, ErrorMessage = "Неверное значение")]
        public string Phone { get; set; }
        #endregion

        #region Methods

        public User.SaveData GetUserSaveData()
        {
            User.SaveData data = new User.SaveData();
            data.Id = this.Id;
            data.Login = this.Login;
            data.PasswordHash = string.IsNullOrEmpty(this.Password) ? null : PasswordHash.CreateHash(this.Password);
            data.DepartmentId = this.DepartmentId;
            data.DeliveryPointId = this.DeliveryPointId;
            data.Firstname = this.Firstname;
            data.Secondname = this.Secondname;
            data.Lastname = this.Lastname;
            data.Roles = new List<long>(Roles);
            data.Position = this.Position;
            data.Email = this.Email;
            data.Phone = this.Phone;
            return data;
        }

        public static string GeneratePassword()
        {
            string password = string.Empty;
            //Делаем простой пароль вида qwe123
            //три первые латинские прописные буквы + три числа
            string latinLetter = "abcdefghijklmnopqrstuvwxyz";
            Random r = new Random();
            for (int i = 0; i < 3; i++)
            {
                password = password + latinLetter[r.Next(latinLetter.Count() - 1)];
            }
            password = password + r.Next(100, 999);
            return password;
        }

        public string SendSms()
        {
            if (!string.IsNullOrEmpty(this.Phone) && !string.IsNullOrEmpty(this.Password))
            {
                SMSMessage sms = new SMSMessage()
                {
                    Id = new Random().Next(99999),
                    SenderId = "MSK-UralSib",
                    Phone = SmsSender.SmsSender.ClearPhone(this.Phone),
                    Message = "Ваш новый пароль для портала ОМС: " + this.Password
                };
                return SmsSender.SmsSender.SendOneSms(sms);
            }
            else
            {
                return "Смс не было отправлено";
            }
        }

        public void SendEmail()
        {
            if (!string.IsNullOrEmpty(this.Email) && !string.IsNullOrEmpty(this.Password))
            {
                SmtpClient Smtp = new SmtpClient("smtp.uralsibins.ru", 25);
                //Smtp.Credentials = new NetworkCredential("gavrilenkovoa@uralsibins.ru", "Password");
                MailMessage Message = new MailMessage();
                Message.From = new MailAddress("oms@uralsibins.ru", "Портал ОМС");
                Message.To.Add(new MailAddress(this.Email.Trim(' ', ',')));
                Message.Subject = "Изменение пароля";
                Message.Body = "Ваш новый пароль для портала ОМС: " + this.Password;
                Smtp.Send(Message);
            }
        }

        public string ReplaceEmail(string email)
        {
            string replace = string.Empty;
            var r = email.IndexOf("@");

            for (int i = 0; i < email.Count(); i++)
            {
                if (i > 1 && i < r - 2)
                {
                    replace = replace + "*";
                }
                else
                {
                    replace = replace + email[i];
                }
            }
            return replace;
        }

        public string ReplacePhone(string sms)
        {
            string replace = string.Empty;

            for (int i = 0; i < sms.Count(); i++)
            {
                if ((i > 4 && i < 8) || (i > 8 && i < 11))
                {
                    replace = replace + "*";
                }
                else
                {
                    replace = replace + sms[i];
                }
            }

            return replace;
        }
        #endregion
    }
}