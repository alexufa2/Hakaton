using TemplateApp.DAL.Dto;
using TemplateApp.DAL.Users;
using TemplateApp.Models;
using TemplateApp.Services;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TemplateApp.Utils.Crypto;

namespace TemplateApp.Controllers
{
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[ApiController]
	[Route("api/[controller]")]
	public class UsersController : ControllerBase
	{
		private IUserService _userService;
		private INotificationService _notificationService;
		private IConfiguration _configuration;
		private IAntiforgery _antiForgery;

		public UsersController(IUserService userService,
			INotificationService notificationService,
			IConfiguration configuration,
			IAntiforgery antiForgery)
		{
			_userService = userService;
			_notificationService = notificationService;
			_configuration = configuration;
			_antiForgery = antiForgery;
		}

		[AllowAnonymous]
		[HttpPost("authenticate")]
		[IgnoreAntiforgeryToken]
		public IActionResult Authenticate([FromBody]UserDto userParam)
		{
			var user = _userService.Authenticate(userParam.Email, userParam.Password);

			if (user == null)
				return Ok(new OperationResult() { Code = 200, Data = new { message = "Неправильный логин или пароль" }, Success = false });

			UserDto result = CreateAuthInfoWithToken(user);

			// return basic user info (without password) and token to store client side
			return Ok(new OperationResult() { Code = 200, Data = result, Success = true });

		}

		private UserDto CreateAuthInfoWithToken(User user)
		{
			//var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_configuration.GetSection("Default").GetValue<string>("secret"));
			//var tokenDescriptor = new SecurityTokenDescriptor
			//{
			//	Subject = new ClaimsIdentity(new Claim[]
			//	{
			//		new Claim(ClaimTypes.Name, user.Id.ToString())
			//	}),
			//	Expires = DateTime.UtcNow.AddDays(7),
			//	SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			//};
			//var token = tokenHandler.CreateToken(tokenDescriptor);
			//var tokenString = tokenHandler.WriteToken(token);

			GOSTSecurityToken token = new GOSTSecurityToken(user.Id.ToString(), "TemplaApp", new SymmetricSecurityKey(key),
				new SymmetricSecurityKey(key), DateTime.Now, DateTime.Now.AddMonths(1));

			string tokenString = GOSTSecurityToken.WriteToken(token);

			UserDto result = user.ToDto();
			result.Token = tokenString;
			return result;
		}

		[AllowAnonymous]
		[IgnoreAntiforgeryToken]
		[HttpPost("register")]
		public IActionResult Register([FromBody]UserDto userParam)
		{
			var task = _userService.Register(userParam);
			task.Wait();
			var user = task.Result;

			if (user == null)
				return Ok(new OperationResult() { Code = 200, Data = new { message = "Пользователь с таким E-Mail уже зарегистрирован" }, Success = false });

			_notificationService.AddUserRegistrationNotification(user);

			user.PasswordHash = null;
			user.PasswordSalt = null;
			user.ActivationKey = null;

			return Ok(new OperationResult() { Code = 200, Data = user, Success = true });
		}


		[AllowAnonymous]
		[IgnoreAntiforgeryToken]
		[HttpGet("confirmbykey")]
		public IActionResult ConfirmByKey(string key)
		{
			User user = _userService.TryConfirmByKey(key);

			// user account confirmed - go to auth
			if (user != null)
			{
				return Ok(CreateAuthInfoWithToken(user));
			}

			// Not found
			return Ok(new OperationResult() { Code = 200, Data = new { status = "not-found" }, Success = false });
		}

		[HttpPost("UpdateUserProfile")]
		public void UpdateUserProfile([FromBody]UserDto userDto)
		{
			_userService.UpdateCurrentUserProfile(userDto);
		}

		[HttpPost("ChangePassword")]
		public IActionResult ChangePassword([FromBody]ChangePasswordRequestDto changePasswordRequestDto)
		{

			_userService.ChangePassword(changePasswordRequestDto.OldPassword, changePasswordRequestDto.NewPassword);


			return Ok(new OperationResult() { Success = true, Data = "Пароль успешно изменен", Code = 200 });
		}

		[HttpPost("UpdateCurrentUserTimeZone")]
		public void UpdateCurrentUserTimeZone([FromBody]UpdateTimeZoneRequestDto offset)
		{
			_userService.UpdateCurrentUserTimeZone(offset.TimeZoneName, offset.TimeZoneOffset);
		}

		[HttpGet("testauth")]
		public IActionResult TestAuth()
		{
			return Ok(new OperationResult() { Code = 200, Data = new { status = "Auth-OK" }, Success = false });
		}

		[IgnoreAntiforgeryToken]
		[HttpGet("queryxrf")]
		public IActionResult QueryXrf()
		{
			var tokens = _antiForgery.GetAndStoreTokens(HttpContext);
			Response.Cookies.Append("XSRF-REQUEST-TOKEN", tokens.RequestToken, new Microsoft.AspNetCore.Http.CookieOptions
			{
				HttpOnly = false
			});
			return NoContent();
		}

		//[HttpGet]
		//public async Task<IActionResult> GetAll()
		//{
		//	var users = await _userService.GetAll();
		//	return Ok(users);
		//}
	}
}
