using TemplateApp.DAL.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TemplateApp.DAL.Users
{
	public class User
	{
		public int Id { get; set; }
		public string Email { get; set; }

		[Required]
		public byte[] PasswordHash { get; set; }

		[Required]
		public byte[] PasswordSalt { get; set; }

		public string Name { get; set; }
		public string Surname { get; set; }

		[Required]
		public int TimeZoneOffsetMinutes { get; set; }

		[Required]
		public string TimeZoneName { get; set; }

		[Required]
		public bool EmailConfirmed { get; set; }

		public string ActivationKey { get; set; }

		public UserDto ToDto()
		{
			return new UserDto()
			{
				Id = Id,
				Email = Email,
				Name = Name,
				Surname = Surname,
				TimeZoneName = TimeZoneName
			};
		}
	}
}
