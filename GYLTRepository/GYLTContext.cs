using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYLTRepository
{
	public class GYLTContext : DbContext
	{
		public GYLTContext()
            : base("name=GYLT_DB")
        {
		}

		public virtual DbSet<WeightEntry> WeightEntries { get; set; }
		public virtual DbSet<WeightLossAttempt> WeightLossAttempts { get; set; }
		public virtual DbSet<WeightLossMilestone> WeightLossMilestones { get; set; }
		public virtual DbSet<User> Users { get; set; }
	}
}
