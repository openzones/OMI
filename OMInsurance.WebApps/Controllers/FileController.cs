using OMInsurance.Configuration;
using OMInsurance.WebApps.Security;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace OMInsurance.WebApps.Controllers
{
    [Authorize]
    [AllowAnonymous]
    public class FileController : Controller
    {
        [AuthorizeUser(Roles = "Anonymous")]
        public ActionResult Upload(string signatureName, string photoName)
        {
            return View();
        }

        [HttpPost]
        [AuthorizeUser(Roles = "Anonymous")]
        public JsonResult UploadImage(string filename, string source)
        {
            string file = filename ?? Guid.NewGuid().ToString();
            string path = Path.Combine(ConfiguraionProvider.FileStorageFolder, file);
            try
            {
                if (!string.IsNullOrEmpty(source))
                {
                    string base64 = source.Substring(source.IndexOf(',') + 1).Trim('\0');
                    byte[] chartData = Convert.FromBase64String(base64);
                    System.IO.File.WriteAllBytes(path, chartData);
                    return Json(new { answer = "OK" });
                }
                else
                {
                    HttpPostedFileBase upload = Request.Files[0];
                    upload.SaveAs(path);
                    return Json(new { filename = file, answer = "OK" });
                }
            }
            catch(Exception ex)
            {
                return Json(new { message = ex.Message, answer = "Ошибка" });
            }
        }

        public ActionResult Image(string filename)
        {
            if (!string.IsNullOrEmpty(filename))
            {
                string path = Path.Combine(ConfiguraionProvider.FileStorageFolder, filename);
                if (System.IO.File.Exists(path))
                {
                    return File(path, "image/jpeg");
                }
            }
            return File("~/Content/no-image.png", "image/jpeg");
        }

        [HttpPost]
        public ActionResult DeleteFile(string filename)
        {
            string path = Path.Combine(ConfiguraionProvider.FileStorageFolder, filename);
            if (!string.IsNullOrEmpty(path) && System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            return Json(new { filename = filename, answer = "OK" });
        }

        public ActionResult GetFileFromAppData(string filename)
        {
            string path = Server.MapPath(string.Format("~/App_Data/{0}", filename));
            return File(path, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }

        public ActionResult FileGet(string filename, string fileurl)
        {
            string path = Path.Combine(ConfiguraionProvider.FileStorageFolder, fileurl);
            return File(path, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }
    }
}