using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYLTRepository
{
	public class GYLTRepository
	{
		private GYLTContext _db;

		public GYLTRepository()
		{
			_db = new GYLTContext();
		}

		public IEnumerable<User> GetActiveUsers()
		{
			return from u in _db.Users
				   where u.IsActive && u.DateDeleted == null
				   select u;
		}

		public User GetUser(int userId)
		{
			return (from u in _db.Users
					where u.UserID == userId && u.IsActive && u.DateDeleted == null
					select u).FirstOrDefault();
		}

		public void AddOrUpdateUser(User user)
		{
			var dbUser = GetUser(user.UserID);
			if (dbUser == null)
			{
				user.DateCreated = DateTimeOffset.Now;
				_db.Users.Add(user);
			}
			else
			{
				dbUser.DateOfBirth = user.DateOfBirth;
				dbUser.EmailAddress = user.EmailAddress;
				dbUser.FirstName = user.FirstName;
				dbUser.Gender = user.Gender;
				dbUser.IconName = user.IconName;
				dbUser.LastName = user.LastName;
				dbUser.MiddleName = user.MiddleName;
				dbUser.PhoneNumber = user.PhoneNumber;
				dbUser.Username = user.Username;
			}

			_db.SaveChanges();
		}
	}
}
