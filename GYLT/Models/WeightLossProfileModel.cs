using GYLTRepository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace GYLT.Models
{
	public class WeightLossProfileModel
	{
		public string Name { get; set; }
		public DateTime StartDate { get; set; }
		public decimal StartingWeight { get; set; }
		public DateTime? TargetDate { get; set; }
		public decimal? TargetWeight { get; set; }
		public string Goal { get; set; }
		public string Description { get; set; }
		public WeightEntryModel TodayWeightEntryModel { get; set; }
		public IEnumerable<WeightEntryModel> RecentWeightEntryModels { get; set; }
		public IEnumerable<WeightLossMilestoneModel> WeightLossMilestoneModels { get; set; }

		public decimal? CurrentWeight
		{
			get
			{
				return TodayWeightEntryModel.Weight ?? RecentWeightEntryModels.OrderByDescending(rwem => rwem.Date).FirstOrDefault()?.Weight;
			}
		}

		public decimal? SevenDayAverage
		{
			get
			{
				var entryModels = RecentWeightEntryModels.Where(rwem => rwem.Date > DateTime.Today.AddDays(-7)).ToList();
				entryModels.Add(TodayWeightEntryModel);
				return entryModels.Where(em => em.Weight != null).Average(em => em.Weight);
			}
		}
		public decimal TotalLost
		{
			get
			{
				var current = TodayWeightEntryModel.Weight ?? RecentWeightEntryModels.OrderByDescending(rwem => rwem.Date).FirstOrDefault()?.Weight;
				if (CurrentWeight == null || CurrentWeight == 0)
				{
					return 0;
				}
				return StartingWeight - CurrentWeight.Value;
			}
		}

		public string RecentWeightEntryValues
		{
			get
			{
				return JsonConvert.SerializeObject(RecentWeightEntryModels.Select(rwem => rwem.Weight ?? 0).ToArray());
			}
		}

		public string RecentWeightEntryLabels
		{
			get
			{
				return JsonConvert.SerializeObject(RecentWeightEntryModels.Select(rwem => rwem.Date.ToString("MMM dd")).ToArray());
			}
		}

		public WeightLossProfileModel() { }

		public WeightLossProfileModel(WeightLossProfile profile)
		{
			Name = profile.Name;
			StartDate = profile.StartDate;
			StartingWeight = profile.StartingWeight;
			TargetDate = profile.TargetDate;
			TargetWeight = profile.TargetWeight;
			Goal = profile.Goal;
			Description = profile.Description;
			//RecentWeightEntryModels = profile.RecentWeightEntries.Where(rwe => rwe.Date < DateTime.Today).Select(we => new WeightEntryModel(we)).OrderBy(we => we.Date);
			RecentWeightEntryModels = from rwe in profile.RecentWeightEntries
									  where rwe.Date < DateTime.Today
									  orderby rwe.Date
									  select new WeightEntryModel(rwe);
			//WeightLossMilestoneModels = profile.WeightLossMilestones.Select(wlm => new WeightLossMilestoneModel(wlm));
			WeightLossMilestoneModels = from wlm in profile.WeightLossMilestones
										select new WeightLossMilestoneModel(wlm);
			TodayWeightEntryModel = (from rwe in profile.RecentWeightEntries
									 where rwe.Date == DateTime.Today
									 select new WeightEntryModel(rwe)).FirstOrDefault() ?? new WeightEntryModel { Date = DateTime.Today };
		}

		public void FillInMissingEntryDates()
		{
			var entryList = RecentWeightEntryModels.ToList();
			var daysToDisplay = ConfigurationManager.AppSettings["NumberOfWeightEntriesToDisplayOnProfilePage"];
			var days = Int32.Parse(daysToDisplay);
			for (int i = (-1 * days); i < 0; i++)
			{
				var date = DateTime.Today.AddDays(i);
				var existingEntry = entryList.FirstOrDefault(rwem => rwem.Date == date);
				if (existingEntry == null)
				{
					entryList.Add(new WeightEntryModel { Date = date });
				}
			}

			RecentWeightEntryModels = entryList.OrderBy(el => el.Date).ToList();
		}
	}
}