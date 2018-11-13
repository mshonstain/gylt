using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GYLT.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
			if (Session[Constants.UserIdKey] == null || Session[Constants.UserIdKey] == "0")
			{
				return RedirectToAction("UserSelection", "User");
			}
            return View();
        }
    }
}