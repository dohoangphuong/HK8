using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CertMClient.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            // 
            CertMServiceData.CertMServiceClient service = new CertMServiceData.CertMServiceClient();

            //create variable "cp" to take advantabge of CAS specific features like Proxy Tickets and Attributes
            DotNetCasClient.Security.CasPrincipal cp = User as DotNetCasClient.Security.CasPrincipal;

            //get type of account with acount name of the current user, then return view of user. if accontname is not esixt in database, system add accountname and return view of student
            if (cp != null)
            {
                if(service.CheckAccount(cp.Identity.Name))
                {
                    if (String.Compare(service.GetTypeAccount(cp.Identity.Name), "admin", true) == 0)
                        return View("admin");
                    else if (String.Compare(service.GetTypeAccount(cp.Identity.Name), "teacher", true) == 0)
                        return View("teacher");
                    else if (String.Compare(service.GetTypeAccount(cp.Identity.Name), "manager", true) == 0)
                        return View("manager");
                    else return View("student");
                }
                
                else
                {
                        service.AddAccountStudent(cp.Identity.Name);
                        return View("student");
                }
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}