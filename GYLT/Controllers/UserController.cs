using GYLT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GYLTRepository;

namespace GYLT.Controllers
{
    public class UserController : Controller
	{
		private GYLTRepository.GYLTRepository _repository = new GYLTRepository.GYLTRepository();

		// GET: User
		public ActionResult Index()
        {
			if (Session[Constants.UserIdKey] == null || Session[Constants.UserIdKey].ToString() == "0")
			{
				return RedirectToAction("UserSelection");
			}

			return RedirectToAction("UserProfile");
        }

		[HttpGet]
		public ActionResult UserSelection()
		{
			Session[Constants.UserIdKey] = null;
			var users = _repository.GetActiveUsers();
			return View(users.Select(u => new UserModel(u)).ToList());
		}

		[HttpPost]
		public ActionResult SelectUser(UserModel model)
		{
			Session[Constants.UserIdKey] = model.UserID;

			if (model.UserID == 0)
			{
				return RedirectToAction("UserProfile");
			}

			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public ActionResult UserProfile()
		{
			var userId = (int)Session[Constants.UserIdKey];

			if (userId == 0)
			{
				return View(new UserModel());
			}

			var user = _repository.GetUser(userId);
			return View(user);
		}

		[HttpPost]
		public ActionResult UserProfile(UserModel model)
		{
			_repository.AddOrUpdateUser(model.AsDbRecord());
			return View(model);
		}

		[HttpGet]
		public ActionResult EditUsers()
		{
			var users = _repository.GetActiveUsers();
			return View(users.Select(u => new UserModel(u)));
		}
    }
}