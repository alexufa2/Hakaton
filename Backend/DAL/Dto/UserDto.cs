using System;
using TemplateApp.DAL.Users;

namespace TemplateApp.DAL.Dto
{
	public class UserDto
	{
		public int Id { get; set; }
		public string Email { get; set; }

		public string Name { get; set; }
		public string Surname { get; set; }

		public string Password { get; set; }

		public string TimeZoneName { get; set; }

		public string GetFullName()
		{
			string desc = Name;

			if (!string.IsNullOrEmpty(Surname))
			{
				desc += " " + Surname;
			}

			return desc;
		}

		public string Token { get; set; }
	}
}
