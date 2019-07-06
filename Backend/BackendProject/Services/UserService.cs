using TemplateApp.DAL;
using TemplateApp.DAL.Dto;
using TemplateApp.DAL.Users;
using TemplateApp.Utils.Notifications;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GostCrypto;

namespace TemplateApp.Services
{
	public interface IUserService
	{
		User Authenticate(string username, string password);
		Task<IEnumerable<User>> GetAll();
		Task<User> Register(UserDto user);
		DAL.Users.User GetCurrentUser();
		User GetUserEntityByEmail(string email);
		void UpdateCurrentUserTimeZone(string timeZOneName, int offset);
		void UpdateCurrentUserProfile(UserDto userDto);
		User ChangePassword(string oldPassword, string newPassword);
		User TryConfirmByKey(string key);
		User GetById(int userId);
		void SetPasswordHashAndSaltUnModified(User user, bool setToModified = false);
	}

	public class UserService : IUserService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		public ApplicationContext _context;

		public DAL.Users.User GetCurrentUser()
		{
			int userId = 0;

			int.TryParse(_httpContextAccessor.HttpContext.User.Identity.Name, out userId);

			if (userId == 0)
				throw new Exception("Unauthorized!");

			var creator = _context.Users
			.FirstOrDefault(x => x.Id == userId);

			_context.Entry(creator).Property(x => x.PasswordHash).IsModified = false;
			_context.Entry(creator).Property(x => x.PasswordSalt).IsModified = false;

			return creator;
		}

		public UserService(ApplicationContext appContext, IHttpContextAccessor httpContextAccessor)
		{
			_context = appContext;
			_httpContextAccessor = httpContextAccessor;
		}

		public void UpdateCurrentUserTimeZone(string timeZoneName, int offset)
		{
			var currentUser = GetCurrentUser();

			currentUser.TimeZoneOffsetMinutes = offset;
			currentUser.TimeZoneName = timeZoneName;

			_context.SaveChanges();
		}

		public User Authenticate(string username, string password)
		{
			if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
				return null;

			var user = _context.Users.SingleOrDefault(x => x.Email == username);

			// check if username exists
			if (user == null)
				return null;

			// check if password is correct
			if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
				return null;

			user.PasswordSalt = null;
			user.PasswordHash = null;

			// authentication successful
			return user;
		}

		public User ChangePassword(string oldPassword, string newPassword)
		{

			var currentUser = GetCurrentUser();

			if (!VerifyPasswordHash(oldPassword, currentUser.PasswordHash, currentUser.PasswordSalt))
			{
				throw new Exception("Старый пароль неправильный!");
			}

			if (string.IsNullOrEmpty(newPassword))
			{
				throw new Exception("Новый пароль не может быть пустым");
			}

			byte[] passwordHash, passwordSalt;
			CreatePasswordHash(newPassword, out passwordHash, out passwordSalt);

			currentUser.PasswordHash = passwordHash;
			currentUser.PasswordSalt = passwordSalt;

			SetPasswordHashAndSaltUnModified(currentUser, true);

			_context.SaveChanges();

			currentUser.PasswordHash = null;
			currentUser.PasswordSalt = null;

			return currentUser;
		}

		public async Task<User> Register(UserDto _user)
		{
			var user = await Task.Run(() => _context.Users.
			SingleOrDefault(x => x.Email == _user.Email));

			if (user != null)
				return null;


			byte[] passwordHash, passwordSalt;
			CreatePasswordHash(_user.Password, out passwordHash, out passwordSalt);

			var newUser = _context.Users.Add(new User()
			{
				Email = _user.Email,
				PasswordHash = passwordHash,
				PasswordSalt = passwordSalt,
				Name = _user.Name,
				Surname = _user.Surname,
				EmailConfirmed = false,
				ActivationKey = RandomStringGen.RandomString(1024)
			});

			SetDefaultTimezone(newUser);

			_context.SaveChanges();

			return newUser.Entity;

		}

		public User TryConfirmByKey(string key)
		{
			var user = _context.Users.
			SingleOrDefault(x => x.ActivationKey == key);

			if (user != null)
			{
				user.ActivationKey = null;
				user.EmailConfirmed = true;
				SetPasswordHashAndSaltUnModified(user);

				_context.SaveChanges();

				user.PasswordHash = null;
				user.PasswordHash = null;

				return user;
			}

			return null;
		}

		public void SetPasswordHashAndSaltUnModified(User user, bool setToModified = false)
		{
			_context.Entry(user).Property(x => x.PasswordHash).IsModified = setToModified;
			_context.Entry(user).Property(x => x.PasswordSalt).IsModified = setToModified;
		}

		private void SetDefaultTimezone(EntityEntry<User> newUser)
		{
			newUser.Entity.TimeZoneName = "Europe/Moscow";
			newUser.Entity.TimeZoneOffsetMinutes = 180;
		}


		// private helper methods

		private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			if (password == null) throw new ArgumentNullException("password");
			if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

			//using (var hmac = new System.Security.Cryptography.HMACSHA512())
			//{
			//	passwordSalt = hmac.Key;
			//	passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
			//}

			BigInteger b = new BigInteger();
			b.genRandomBits(512, new Random());

			passwordSalt = b.getBytes();

			GostCrypto.Gost34102012Signer signer = new GostCrypto.Gost34102012Signer(new BigInteger(passwordSalt));
			passwordHash = Encoding.UTF8.GetBytes(signer.Sign(password));


		}

		private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
		{
			if (password == null) throw new ArgumentNullException("password");
			if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
			////if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
			//if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

			//using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
			//{
			//	var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
			//	for (int i = 0; i < computedHash.Length; i++)
			//	{
			//		if (computedHash[i] != storedHash[i]) return false;
			//	}
			//}

			GostCrypto.Gost34102012Signer signer = new GostCrypto.Gost34102012Signer(new BigInteger(storedSalt));
			return signer.SignIsValid(password, Encoding.UTF8.GetString(storedHash));
		}

		public User GetById(int Id)
		{
			var result = _context.Users.FirstOrDefault(x => x.Id == Id);
			if (result != null)
			{
				SetPasswordHashAndSaltUnModified(result);
			}

			return result;
		}

		public User GetUserEntityByEmail(string email)
		{
			var result = _context.Users.FirstOrDefault(x => x.Email.ToLower().Contains(email));
			if (result != null)
			{
				SetPasswordHashAndSaltUnModified(result);
			}

			return result;
		}

		public async Task<IEnumerable<User>> GetAll()
		{
			// return users without passwords
			return await Task.Run(() => _context.Users.ToList().Select(x =>
			{
				x.PasswordHash = null;
				x.PasswordSalt = null;
				return x;
			}));
		}

		public void UpdateCurrentUserProfile(UserDto userDto)
		{
			var currentUser = GetCurrentUser();
			_context.Entry(currentUser).Reload();

			currentUser.Name = userDto.Name;
			currentUser.Surname = userDto.Surname;

			_context.SaveChanges();
		}
	}
}
