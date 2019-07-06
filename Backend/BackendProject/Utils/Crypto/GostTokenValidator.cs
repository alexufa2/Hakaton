using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;
using TemplateApp.DAL.Users;
using TemplateApp.Services;
using System.IdentityModel.Tokens.Jwt;

namespace TemplateApp.Utils.Crypto
{
	public class GostTokenValidator : ISecurityTokenValidator
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly byte[] _signingSecurityKey;

		public GostTokenValidator(IServiceProvider serviceProvider, byte[] signingSecurityKey)
		{
			this._serviceProvider = serviceProvider;
			this._signingSecurityKey = signingSecurityKey;
		}

		public bool CanReadToken(string securityToken)
		{
			return true;
		}

		public ClaimsPrincipal ValidateToken(string securityToken,
			TokenValidationParameters validationParameters,
			out SecurityToken validatedToken)
		{
			ClaimsPrincipal result = null;
			SecurityToken token = null;

			Task.WaitAll(Task.Run(async () =>
			{

				GOSTSecurityToken gostToken = GOSTSecurityToken.Decode(securityToken, _signingSecurityKey);

				if (gostToken != null) // if not null - verify OK
				{

					var identity = new ClaimsIdentity("Bearer");
					identity.AddClaim(new Claim(ClaimTypes.Name, gostToken.Id.ToString()));

					result = new ClaimsPrincipal(identity);

					token = gostToken;
				}


			}));

			validatedToken = token;
			return result;
		}

		public bool CanValidateToken => true;
		public int MaximumTokenSizeInBytes { get; set; }
	}
}
