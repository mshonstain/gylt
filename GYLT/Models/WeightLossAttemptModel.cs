using GYLTRepository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GYLT.Models
{
	public class WeightLossAttemptModel
	{
		[DisplayName("Plan Name")]
		public string Name { get; set; }
		public string Description { get; set; }
		[DataType(DataType.Date)]
		[DisplayName("Start Date")]
		public DateTime StartDate { get; set; }
		[DisplayName("Starting Weight")]
		public decimal StartingWeight { get; set; }
		[DataType(DataType.Date)]
		[DisplayName("Target Date")]
		public DateTime? TargetDate { get; set; }
		[DisplayName("Target Weight")]
		public decimal? TargetWeight { get; set; }
		[DisplayName("My Goal")]
		public string Goal { get; set; }

		public WeightLossAttempt AsDbRecord()
		{
			return new WeightLossAttempt
			{
				DateCreated = DateTime.Now,
				Description = this.Description,
				Goal = this.Goal,
				Name = this.Name,
				StartDate = this.StartDate,
				StartingWeight = this.StartingWeight,
				TargetDate = this.TargetDate,
				TargetWeight = this.TargetWeight,
				IsActive = true
			};
		}
	}
}