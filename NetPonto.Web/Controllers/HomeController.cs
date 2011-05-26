using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetPonto.Infrastructure.Log;
using NetPonto.DAL;

namespace NetPonto.Web.Controllers
{
    public class HomeController : Controller
    {
        //public HomeController(ILogger log)
        //{
        //    this.log = log;
        //}


        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string passwd)
        {

            return Json("");
        }

        public int CRIA_USER()
        {
            // cria utilizador admin só pq posso para não ter trabalho :P
            //MembershipCreateStatus status;
            //MembershipUser usr = Membership.CreateUser("Zeus", "Password!1", "Holyonept@gmail.com", null, null, true, out status);
            //Roles.AddUserToRole("Zeus", "Overseer");
            //log.Info("teste");

            return 1;
        }
    }
}
