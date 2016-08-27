using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OMInsurance.WebApps.Models;
using OMInsurance.Interfaces;
using OMInsurance.BusinessLogic;
using OMInsurance.WebApps.Security;

namespace OMInsurance.WebApps.Controllers
{
    [AuthorizeUser]
    public class PostController : OMInsuranceController
    {
        IPostBusinessLogic postBusinessLogic;

        public PostController()
        {
            postBusinessLogic = new PostBusinessLogic();
        }

        public ActionResult Index()
        {

            return View();
        }

        [HttpPost, ActionName("Index")]
        public ActionResult Create_post(PostModel model)
        {

            ViewBag.HtmlContent = model.Content;

            return View(model);
        }


        [HttpGet]
        public ActionResult Create(long? id)
        {
            PostModel model = new PostModel();
            if (id.HasValue)
            {
                var post = postBusinessLogic.Post_GetByID(id.Value);
                model = new PostModel(post);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(PostModel model)
        {
            if (model.PostId.HasValue)
            {
                model.UpdateDate = DateTime.Now;
                model.UpdateUser = new UserModel(CurrentUser);
                model.UpdateUserName = CurrentUser.Fullname;
            }
            else
            {
                model.CreateDate = DateTime.Now;
                model.CreateUser = new UserModel(CurrentUser);
                model.CreateUserName = CurrentUser.Fullname;
            }
            long postId = postBusinessLogic.Post_Save(model.GetPost(CurrentUser));
            var post = postBusinessLogic.Post_GetByID(postId);
            PostModel m = new PostModel(post);
            return RedirectToAction("Create", new { id = postId });
        }
    }
}