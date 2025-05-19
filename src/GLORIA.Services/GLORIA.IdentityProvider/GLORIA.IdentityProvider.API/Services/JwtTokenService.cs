using GLORIA.BuildingBlocks.Configuration;
using GLORIA.IdentityProvider.API.Models.Entities.Identity;
using GLORIA.IdentityProvider.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GLORIA.IdentityProvider.API.Services
{
	public class JwtTokenService : IJwtTokenService
	{
		private readonly JwtSettings _jwtSettings;

		public JwtTokenService(IOptions<JwtSettings> jwtOptions)
		{
			_jwtSettings = jwtOptions.Value;
		}

		public async Task<string> GenerateAsync(ApplicationUser user, UserManager<ApplicationUser> userManager)
		{
			var roles = await userManager.GetRolesAsync(user);
			var claims = new List<Claim>
		{
			new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
			new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
			new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
			new Claim(ClaimTypes.Email, user.Email ?? ""),
			new Claim(ClaimTypes.Name, user.UserName ?? ""),
			new Claim(ClaimTypes.GivenName, user.FirstName ?? ""),
			new Claim(ClaimTypes.Surname, user.LastName ?? "")
		};

			claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _jwtSettings.Issuer,
				audience: _jwtSettings.Audience,
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenLifetimeMinutes),
				signingCredentials: creds);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
