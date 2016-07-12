using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CertMClient.Controllers
{
    public class TemplateController : Controller
    {
        // GET: Template
        public ActionResult UploadTemplate()
        {
            return View("UploadTemplate");
        }

        [HttpPost]
        public ActionResult Upload(string name, HttpPostedFileBase file1, HttpPostedFileBase file2)
        {
            CertMServiceData.CertMServiceClient client = new CertMServiceData.CertMServiceClient();

            if (name == "") return Content("Tên template không được bỏ trống!");

            if (!client.CheckTemplateName(name))
            {
                return Content("Tên template đã có. Xin hãy nhập lại!");
            }

            if (file1 == null) return Content("File mặt trước không được bỏ trống!");

            try
            {
                client.UploadFile(file1.FileName, file1.ContentLength, file1.InputStream);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

            try
            {
                client.UploadFile(file2.FileName, file2.ContentLength, file2.InputStream);
            }
            catch (Exception ex)
            {
                //return Content(ex.Message);
            }

            client.SaveToDatabase(name, file1.FileName, file2.FileName != null ? file2.FileName : "");

            return Content("Upload template mới thành công!");
        }
    }
}