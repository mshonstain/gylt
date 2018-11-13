using GYLTRepository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GYLT.Models
{
	public class WeightEntryModel
	{
		[DisplayFormat(DataFormatString = "{0:MMM dd}")]
		public DateTime Date { get; set; }
		[DisplayFormat(DataFormatString = "{0:N1}", ApplyFormatInEditMode = true)]
		public decimal? Weight { get; set; }
		public string Notes { get; set; }

		public WeightEntryModel() { }

		public WeightEntryModel(WeightEntry entry)
		{
			Date = entry.Date;
			Weight = entry.Weight;
			Notes = entry.Notes;
		}

		public WeightEntry AsDbRecord()
		{
			if (this.Weight == null)
			{
				throw new Exception("Cannot save entry with null weight");
			}

			return new WeightEntry
			{
				Date = this.Date,
				DateCreated = DateTime.Now,
				Notes = this.Notes,
				Weight = this.Weight.Value
			};
		}
	}
}