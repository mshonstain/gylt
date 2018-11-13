using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYLTRepository
{
	public class WeightLossRepository
	{
		private GYLTContext _db;

		public WeightLossRepository()
		{
			_db = new GYLTContext();
		}

		public void AddOrUpdateWeightEntry(WeightEntry entry)
		{
			var existingEntry = _db.WeightEntries.FirstOrDefault(we => we.DateDeleted == null && we.WeightLossAttemptID == entry.WeightLossAttemptID && we.Date == entry.Date);
		}

		public IEnumerable<WeightLossAttempt> GetActiveWeightLossAttempts(int userId)
		{
			return from wla in _db.WeightLossAttempts
				   where wla.UserID == userId && wla.IsActive && wla.DateDeleted == null
				   select wla;
		}

		public WeightLossAttempt GetActiveWeightLossAttempt(int userId)
		{
			var activeAttempts = GetActiveWeightLossAttempts(userId);
			if (activeAttempts.Count() > 1)
			{
				throw new Exception("Too many active attempts");
			}

			var activeAttempt = activeAttempts.FirstOrDefault();
			if (activeAttempt == null)
			{
				throw new Exception("No active attempts");
			}

			return activeAttempt;
		}

		public void AddWeightLossAttempt(WeightLossAttempt attempt)
		{
			_db.WeightLossAttempts.Add(attempt);
			_db.SaveChanges();
		}

		public void EnterOrUpdateWeightEntry(DateTime date, decimal weight, string notes, int userId, bool saveChanges = true)
		{
			var existingWeightEntry = (from we in _db.WeightEntries
									   where we.WeightLossAttempt.UserID == userId
									   && we.WeightLossAttempt.IsActive
									   && we.Date == date
									   select we).FirstOrDefault();
			if (existingWeightEntry != null)
			{
				existingWeightEntry.Weight = weight;
				existingWeightEntry.Notes = notes;
			}
			else
			{
				_db.WeightEntries.Add(new WeightEntry
				{
					Date = date,
					DateCreated = DateTime.Now,
					Notes = notes,
					Weight = weight,
					WeightLossAttempt = GetActiveWeightLossAttempt(userId)
				});
			}

			if (saveChanges)
			{
				_db.SaveChanges();
			}
		}

		public void EnterOrUpdateWeightEntries(IEnumerable<WeightEntry> weightEntries, int userId)
		{
			foreach (var entry in weightEntries)
			{
				EnterOrUpdateWeightEntry(entry.Date, entry.Weight, entry.Notes, userId, false);
			}

			_db.SaveChanges();
		}
		

		public void SelectActiveAttempt(int attemptId)
		{
			foreach (var attempt in _db.WeightLossAttempts.Where(wla => wla.DateDeleted == null))
			{
				attempt.IsActive = (attempt.WeightLossAttemptID == attemptId);
			}

			_db.SaveChanges();
		}

		public WeightLossProfile GetCurrentWeightLossProfile(int userId)
		{
			var activeAttempt = GetActiveWeightLossAttempt(userId);
			var daysToDisplay = ConfigurationManager.AppSettings["NumberOfWeightEntriesToDisplayOnProfilePage"];
			var days = Int32.Parse(daysToDisplay);
			var profile = new WeightLossProfile
			{
				Description = activeAttempt.Description,
				Goal = activeAttempt.Goal,
				Name = activeAttempt.Name,
				RecentWeightEntries = activeAttempt.WeightEntries.Where(we => we.Date >= DateTime.Today.AddDays(-1 * days)),
				StartDate = activeAttempt.StartDate,
				StartingWeight = activeAttempt.StartingWeight,
				TargetDate = activeAttempt.TargetDate,
				TargetWeight = activeAttempt.TargetWeight,
				WeightLossMilestones = activeAttempt.WeightLossMilestones
			};

			return profile;
		}

		public IEnumerable<WeightEntry> GetWeightLossEntries(int userId)
		{
			return from we in _db.WeightEntries
				   where we.WeightLossAttempt.IsActive
					&& we.WeightLossAttempt.UserID == userId
				   orderby we.Date
				   select we;
		}
	}
}