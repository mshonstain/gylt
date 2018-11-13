using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYLTRepository
{
	public class WeightLossProfile
	{
		public string Name { get; set; }
		public DateTime StartDate { get; set; }
		public decimal StartingWeight { get; set; }
		public DateTime? TargetDate { get; set; }
		public decimal? TargetWeight { get; set; }
		public string Goal { get; set; }
		public string Description { get; set; }
		public IEnumerable<WeightEntry> RecentWeightEntries { get; set; }
		public IEnumerable<WeightLossMilestone> WeightLossMilestones { get; set; }
	}
}
