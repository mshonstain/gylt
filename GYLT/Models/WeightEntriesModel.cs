using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GYLT.Models
{
	public class WeightEntriesModel
	{
		public IEnumerable<WeightEntryModel> WeightEntryModels { get; set; }

		public string WeightEntryValues
		{
			get
			{
				return JsonConvert.SerializeObject(WeightEntryModels.Select(rwem => rwem.Weight ?? 0).ToArray());
			}
		}

		public string WeightEntryLabels
		{
			get
			{
				return JsonConvert.SerializeObject(WeightEntryModels.Select(rwem => rwem.Date.ToString("MMM dd")).ToArray());
			}
		}
	}
}