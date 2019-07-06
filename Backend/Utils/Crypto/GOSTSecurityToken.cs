using System;
using System.Text;
using GostCrypto;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TemplateApp.Utils.Crypto
{
	public class GOSTSecurityToken : SecurityToken
	{
		public GOSTSecurityToken(string id, string issuer, SymmetricSecurityKey securityKey, SymmetricSecurityKey signingKey, DateTime validFrom, DateTime validTo)
		{
			this.Id = id;
			this.Issuer = issuer;
			this.SecurityKey = securityKey;
			this.SigningKey = signingKey;
			this.ValidFrom = validFrom;
			this.ValidTo = validTo;

		}

		public override string Id { get; }
		public override string Issuer { get; }
		public override SecurityKey SecurityKey { get; }
		public override SecurityKey SigningKey { get; set; }
		public override DateTime ValidFrom { get; }
		public override DateTime ValidTo { get; }


		public static GOSTSecurityToken Decode(string token, byte[] key, bool verify = true)
		{
			string[] parts = token.Split('.');
			string header = parts[0];
			string payload = parts[1];
			byte[] signature = System.Convert.FromBase64String(parts[2]);

			string headerJson = Encoding.UTF8.GetString(System.Convert.FromBase64String(header));
			JObject headerData = JObject.Parse(headerJson);

			string payloadJson = Encoding.UTF8.GetString(System.Convert.FromBase64String(payload));
			JObject payloadData = JObject.Parse(payloadJson);

			if (verify)
			{
				GostCrypto.Gost34102012Signer signer = new Gost34102012Signer(new BigInteger(key));

				if (!signer.SignIsValid(header + "." + payload, Encoding.UTF8.GetString(signature)))
				{
					return null;
				}
			}

			return new GOSTSecurityToken(payloadData.GetValue("userId").Value<string>(),
				payloadData.GetValue("iss").Value<string>(), new SymmetricSecurityKey(key),
				new SymmetricSecurityKey(key),
				DateTime.Parse(payloadData.GetValue("nbf").Value<string>()),
				DateTime.Parse(payloadData.GetValue("exp").Value<string>()));
		}

		internal static string WriteToken(GOSTSecurityToken token)
		{
			string header = JsonConvert.SerializeObject(new { alg = "gost34.11.2012", typ = "JWT" });

			// TODO claims // audience
			string payLoad = JsonConvert.SerializeObject(new
			{
				userId = token.Id,
				iss = token.Issuer,
				aud = "TemplateApp",
				nbf = token.ValidFrom.ToString(),
				exp = token.ValidTo.ToString()
			});    //1

			byte[] securityKey = ((SymmetricSecurityKey)token.SecurityKey).Key;
			string unsignedToken = ToBase64(header) + '.' + ToBase64(payLoad);
			GostCrypto.Gost34102012Signer signer = new GostCrypto.Gost34102012Signer(new BigInteger(securityKey));

			string signature = signer.Sign(unsignedToken);

			return ToBase64(header) + "." + ToBase64(payLoad) + "." + ToBase64(signature);

		}

		private static string ToBase64(string input)
		{
			byte[] utf8Bytes = Encoding.UTF8.GetBytes(input);

			return System.Convert.ToBase64String(utf8Bytes);
		}
	}
}