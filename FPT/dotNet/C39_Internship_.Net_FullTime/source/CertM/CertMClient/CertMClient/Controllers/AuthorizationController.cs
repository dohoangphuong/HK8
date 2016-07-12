using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CertMClient.Controllers
{
    public class AuthorizationController : Controller
    {
        // GET: Authorization
        private CertMServiceData.CertMServiceClient Service = new CertMServiceData.CertMServiceClient();
        public ActionResult Index()
        {

            DotNetCasClient.Security.CasPrincipal cp = User as DotNetCasClient.Security.CasPrincipal;
            if (cp != null && String.Compare(Service.GetTypeAccount(cp.Identity.Name), "admin", true) == 0)
            {

                return View(Service.getAccounts());
            }
            throw new NotImplementedException();
        }

        public ActionResult Edit(string id)
        {
            CertMServiceData.ACCOUNT account = null;
            foreach (var ac in Service.getAccounts())
            {
                if(ac.Email==id)
                {
                    account = ac;
                }
            }
            return View(account);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Email,Type")] CertMServiceData.ACCOUNT account)
        {
            if (ModelState.IsValid)
            {
                if (String.Compare(account.Type, "student", true) == 0 ||
                   String.Compare(account.Type, "manager", true) == 0 ||
                   String.Compare(account.Type, "teacher", true) == 0 ||
                   String.Compare(account.Type, "admin", true) == 0)
                {
                    Service.modifyAccount(account);
                    return RedirectToAction("Index");
                }
                else
                    ModelState.AddModelError("", "Type must be: Student or Manager or Teacher or Admin!!!!");

            }
            return View(account);
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: ACCOUNTs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Email,Type")] CertMServiceData.ACCOUNT account)
        {
            if (ModelState.IsValid)
            {
                if (String.Compare(account.Type, "student", true) == 0 ||
                   String.Compare(account.Type, "manager", true) == 0 ||
                   String.Compare(account.Type, "teacher", true) == 0 ||
                   String.Compare(account.Type, "admin", true) == 0)
                {
                    if (Service.AddAccount(account))
                        return RedirectToAction("Create");
                    else
                    {
                        ModelState.AddModelError("", "Email already exists!!!!");
                    }
                }
                else
                    ModelState.AddModelError("", "Type must be: Student or Manager or Teacher or Admin!!!!");

            }

            return View("Create");
        }
    }
}
