using GYLTRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GYLT.Models
{
	public class UserModel
	{
		public int UserID { get; set; }
		public string Username { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string Gender { get; set; }
		public string EmailAddress { get; set; }
		public string PhoneNumber { get; set; }
		public string IconName { get; set; }

		public string IconPath
		{
			get
			{
				return $"/GYLT/Content/Images/{(IconName ?? "MaleSilhouette_Gray")}.png";
			}
		}

		public string FullName
		{
			get
			{
				return $"{FirstName} {(String.IsNullOrWhiteSpace(MiddleName) ? String.Empty : MiddleName + " ")}{LastName}";
			}
		}

		public UserModel() { }

		public UserModel(User user)
		{
			DateOfBirth = user.DateOfBirth;
			EmailAddress = user.EmailAddress;
			FirstName = user.FirstName;
			Gender = user.Gender;
			IconName = user.IconName;
			LastName = user.LastName;
			MiddleName = user.MiddleName;
			PhoneNumber = user.PhoneNumber;
			UserID = user.UserID;
			Username = user.Username;
		}

		public User AsDbRecord()
		{
			return new User
			{
				DateOfBirth = this.DateOfBirth,
				EmailAddress = this.EmailAddress,
				FirstName = this.FirstName,
				Gender = this.Gender,
				IconName = this.IconName,
				IsActive = true,
				LastName = this.LastName,
				MiddleName = this.MiddleName,
				PhoneNumber = this.PhoneNumber,
				UserID = this.UserID,
				Username = this.Username
			};
		}
	}
}