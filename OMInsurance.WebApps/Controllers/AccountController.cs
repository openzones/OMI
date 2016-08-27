using OMInsurance.Entities.Core;
using OMInsurance.WebApps.Models;
using OMInsurance.WebApps.Security;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OMInsurance.SmsSender;
using System;

namespace OMInsurance.WebApps.Controllers
{
    [AuthorizeUser]
    public class AccountController : OMInsuranceController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Help()
        {
            return View();
        }

        /// <summary>
        /// Registrates a new user
        /// </summary>
        /// <returns>View model for registration</returns>
        [AuthorizeUser(Roles = "Administrator")]
        public ActionResult Edit(long? id)
        {
            UserEditModel m = new UserEditModel();
            if (id.HasValue && id != 0)
            {
                var user = userBusinessLogic.User_Get(id.Value);
                m = new UserEditModel(user);
            }
            return View(m);
        }

        /// <summary>
        /// Registrates a new user
        /// </summary>
        /// <param name="userModel">Data to create a new user</param>
        /// <returns>Redirect to login action</returns>
        [HttpPost]
        [AuthorizeUser(Roles = "Administrator")]
        public ActionResult Edit(long? id, UserEditModel userModel)
        {
            long userId = userBusinessLogic.User_Save(userModel.GetUserSaveData());
            var user = userBusinessLogic.User_Get(userId);
            userModel = new UserEditModel(user);
            return PartialView("Edit", userModel);
        }

        [AuthorizeUser(Roles = "Administrator")]
        public JsonResult Find(string name)
        {
            IEnumerable<SelectListItem> items = userBusinessLogic.Find(name)
                .Select(item => new SelectListItem()
                {
                    Value = item.Id.ToString(),
                    Text = string.Format("{0} {1} {2}", item.Lastname ?? string.Empty, item.Firstname ?? string.Empty, item.Secondname ?? string.Empty)
                })
                .OrderBy(a => a.Text);
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ResetAutoGeneration(long? id)
        {
            UserEditModel userModel = new UserEditModel();
            if (id.HasValue && id != 0)
            {
                var user = userBusinessLogic.User_Get(id.Value);
                userModel = new UserEditModel(user);
                userModel.Password = UserEditModel.GeneratePassword();
                long userId = userBusinessLogic.User_Save(userModel.GetUserSaveData());
                userModel.SendSms();
                userModel.SendEmail();
            }
            return PartialView("Edit", userModel);
        }

        public ActionResult ResetManualGeneration(long? id, string password)
        {
            UserEditModel userModel = new UserEditModel();
            if (id.HasValue && id != 0)
            {
                var user = userBusinessLogic.User_Get(id.Value);
                userModel = new UserEditModel(user);
                userModel.Password = password;
                long userId = userBusinessLogic.User_Save(userModel.GetUserSaveData());
                userModel.SendSms();
                userModel.SendEmail();
            }
            return PartialView("Edit", userModel);
        }

        public ActionResult ResetAutoGenerationForUser(long? id)
        {
            UserEditModel userModel = new UserEditModel();
            if (id.HasValue && id != 0)
            {
                var user = userBusinessLogic.User_Get(id.Value);
                userModel = new UserEditModel(user);
                userModel.Password = UserEditModel.GeneratePassword();
                long userId = userBusinessLogic.User_Save(userModel.GetUserSaveData());
                //userModel.SendSms();
                userModel.SendEmail();
            }
            return View("UserAccount", userModel);
        }

        public ActionResult ResetManualGenerationForUser(long? id, string password)
        {
            UserEditModel userModel = new UserEditModel();
            if (id.HasValue && id != 0)
            {
                var user = userBusinessLogic.User_Get(id.Value);
                userModel = new UserEditModel(user);
                userModel.Password = password;
                long userId = userBusinessLogic.User_Save(userModel.GetUserSaveData());
                //userModel.SendSms();
                userModel.SendEmail();
            }
            return View("UserAccount", userModel);
        }

        /// <summary>
        /// Gets a view for user log in
        /// </summary>
        /// <returns>View model for login</returns>
        [AllowAnonymous]
        [AuthorizeUser(Roles = "Anonymous")]
        public ActionResult Login()
        {
            LoginModel model = new LoginModel();
            return View();
        }

        /// <summary>
        /// Authenticates user with specified creditnails
        /// </summary>
        /// <returns>Redirects user to index page</returns>
        [HttpPost]
        [AllowAnonymous]
        [AuthorizeUser(Roles = "Anonymous")]
        public ActionResult Login(LoginModel model)
        {
            if (AuthenticationManager.Login(userBusinessLogic, this.HttpContext, model.Login, model.Password))
            {
                if (CurrentUser.Roles.Contains(Role.Registrator))
                {
                    return RedirectToAction("Help", "Account");
                }
                if (CurrentUser.Roles.Contains(Role.AdministratorBSO) && CurrentUser.Roles.Count == 1)
                {
                    return RedirectToAction("Index", "BSO");
                }

                return RedirectToAction("Actuals", "ClientVisit");
            }
            else
            {
                ViewBag.Message = "Неверное имя пользователя или пароль.";
                return View(model);
            }
        }

        /// <summary>
        /// Allows to log a current user out
        /// </summary>
        public ActionResult LogOut()
        {
            AuthenticationManager.LogOut(this.HttpContext);
            return RedirectToAction("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        [AuthorizeUser(Roles = "Anonymous")]
        public ActionResult RestorePassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [AuthorizeUser(Roles = "Anonymous")]
        public ActionResult RestorePassword(RestoreModel model)
        {
            var user = userBusinessLogic.User_GetByLogin(model.Login);
            if (user != null)
            {
                if (model.Choice == "email")
                {
                    if (!string.IsNullOrEmpty(user.Email))
                    {
                        UserEditModel userModel = new UserEditModel();
                        userModel = new UserEditModel(user);
                        userModel.Password = UserEditModel.GeneratePassword();
                        try
                        {
                            userModel.SendEmail();
                            long userId = userBusinessLogic.User_Save(userModel.GetUserSaveData());
                            ViewBag.Message = "Письмо успешно отправлено на почту " + userModel.ReplaceEmail(user.Email);
                        }
                        catch (Exception e)
                        {
                            ViewBag.Message = e.Message;
                        }
                    }
                    else
                    {
                        ViewBag.Message = string.Format("У пользователя [{0}] не указан e-mail. Обратитесь к администратору.", model.Login);
                    }
                }

                if (model.Choice == "sms")
                {
                    if (!string.IsNullOrEmpty(user.Phone))
                    {
                        UserEditModel userModel = new UserEditModel();
                        userModel = new UserEditModel(user);
                        userModel.Password = UserEditModel.GeneratePassword();
                        try
                        {
                            string result = userModel.SendSms();
                            if (string.IsNullOrEmpty(result))
                            {
                                long userId = userBusinessLogic.User_Save(userModel.GetUserSaveData());
                                ViewBag.Message = "СМС отправлено на телефон " + userModel.ReplacePhone(userModel.Phone);
                            }
                            else
                            {
                                ViewBag.Message = result;
                            }
                        }
                        catch (Exception e)
                        {
                            ViewBag.Message = e.Message;
                        }
                    }
                    else
                    {
                        ViewBag.Message = string.Format("У пользователя [{0}] не указан телефон. Обратитесь к администратору.", model.Login);
                    }
                }
            }
            else
            {
                ViewBag.Message = string.Format("Пользователь [{0}] не найден", model.Login);
            }

            return View();
        }

        public ActionResult UserAccount()
        {
            var user = userBusinessLogic.User_GetByLogin(CurrentUser.Login);
            UserEditModel userModel = new UserEditModel();
            userModel = new UserEditModel(user);
            return View(userModel);
        }
    }
}