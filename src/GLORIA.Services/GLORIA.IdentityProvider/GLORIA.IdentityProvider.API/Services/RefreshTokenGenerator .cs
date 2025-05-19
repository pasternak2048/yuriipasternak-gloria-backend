using GLORIA.IdentityProvider.API.Services.Interfaces;
using System.Security.Cryptography;

namespace GLORIA.IdentityProvider.API.Services
{
	public class RefreshTokenGenerator : IRefreshTokenGenerator
	{
		public string Generate()
		{
			var bytes = RandomNumberGenerator.GetBytes(64);

			return Convert.ToBase64String(bytes);
		}
	}
}
