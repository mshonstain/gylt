using GYLTRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GYLT.Models
{
	public class WeightLossMilestoneModel
	{
		public DateTime? TargetDate { get; set; }
		public decimal? TargetWeight { get; set; }
		public string Goal { get; set; }
		public DateTime? DateAchieved { get; set; }

		public WeightLossMilestoneModel() { }

		public WeightLossMilestoneModel(WeightLossMilestone milestone)
		{
			TargetDate = milestone.TargetDate;
			TargetWeight = milestone.TargetWeight;
			Goal = milestone.Goal;
			DateAchieved = milestone.DateAchieved;
		}
	}
}