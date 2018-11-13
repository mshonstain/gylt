using GYLTRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GYLT.Models;

namespace GYLT.Controllers
{
    public class WeightLossController : Controller
    {
		private WeightLossRepository _repository = new WeightLossRepository();
		public ActionResult Index()
		{
			if (Session[Constants.UserIdKey] == null || Session[Constants.UserIdKey].ToString() == "0")
			{
				return RedirectToAction("UserSelection", "User");
			}

			try
			{
				var userId = Int32.Parse(Session[Constants.UserIdKey].ToString());
				var profile = _repository.GetCurrentWeightLossProfile(userId);
				var profileModel = new WeightLossProfileModel(profile);
				profileModel.FillInMissingEntryDates();
				return View(profileModel);
			}
			catch(Exception ex)
			{
				if (ex.Message == "Too many active attempts")
				{
					return RedirectToAction("SelectActiveAttempt");
				}
				else if (ex.Message == "No active attempts")
				{
					return RedirectToAction("CreateWeightLossAttempt");
				}

				throw;
			}
        }

		public ActionResult SelectActiveAttempt()
		{
			if (Session[Constants.UserIdKey] == null | Session[Constants.UserIdKey].ToString() == "0")
			{
				return RedirectToAction("UserSelection", "User");
			}

			var userId = Int32.Parse(Session[Constants.UserIdKey].ToString());
			var allAttempts = _repository.GetActiveWeightLossAttempts(userId);
			return View(allAttempts);
		}
		
		public ActionResult UpdateActiveAttempt(int id)
		{
			_repository.SelectActiveAttempt(id);
			return RedirectToAction("Index");
		}
		
		public ActionResult CreateWeightLossAttempt()
		{
			return View();
		}

		[HttpPost]
		public ActionResult CreateWeightLossAttempt(WeightLossAttemptModel model)
		{
			var attempt = model.AsDbRecord();
			attempt.UserID = (int)Session[Constants.UserIdKey];
			_repository.AddWeightLossAttempt(attempt);
			return RedirectToAction("Index");
		}
		
		[HttpPost]
		public ActionResult UpdateTodayWeightEntry(WeightEntryModel model)
		{
			if (model == null || model.Weight == null)
			{
				throw new Exception("Weight cannot be null");
			}

			if (Session[Constants.UserIdKey] == null || Session[Constants.UserIdKey].ToString() == "0")
			{
				return RedirectToAction("UserSelection", "User");
			}

			var userId = Int32.Parse(Session[Constants.UserIdKey].ToString());

			_repository.EnterOrUpdateWeightEntry(model.Date, model.Weight.Value, model.Notes, userId);

			return PartialView("_TodayWeightEntry", model);
		}

		[HttpPost]
		public ActionResult UpdateWeightEntries(IEnumerable<WeightEntryModel> models)
		{
			if (Session[Constants.UserIdKey] == null || Session[Constants.UserIdKey].ToString() == "0")
			{
				return RedirectToAction("UserSelection", "User");
			}

			var userId = Int32.Parse(Session[Constants.UserIdKey].ToString());

			var validModels = models.Where(m => m.Weight != null).Select(m => m.AsDbRecord());
			if (validModels.Any())
			{
				_repository.EnterOrUpdateWeightEntries(validModels, userId);
			}

			return PartialView("_RecentWeightEntries", models);
		}

		[HttpGet]
		public ActionResult WeightEntries()
		{
			if (Session[Constants.UserIdKey] == null || Session[Constants.UserIdKey].ToString() == "0")
			{
				return RedirectToAction("UserSelection", "User");
			}

			var userId = Int32.Parse(Session[Constants.UserIdKey].ToString());

			var weightEntries = _repository.GetWeightLossEntries(userId);

			var weightEntriesModel = new WeightEntriesModel { WeightEntryModels = weightEntries.Select(we => new WeightEntryModel(we)) };

			return View(weightEntriesModel);
		}
	}
}