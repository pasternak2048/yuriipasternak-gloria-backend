using IdentityProvider.API.Configurations;
using IdentityProvider.API.Models.Identity;
using IdentityProvider.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityProvider.API.Services
{
	public class TokenService : ITokenService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly JwtSettings _jwtSettings;

		public TokenService(UserManager<ApplicationUser> userManager, IOptions<JwtSettings> jwtOptions)
		{
			_userManager = userManager;
			_jwtSettings = jwtOptions.Value;
		}

		public async Task<string> GenerateTokenAsync(ApplicationUser user)
		{
			var userRoles = await _userManager.GetRolesAsync(user);

			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
				new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
				new Claim(ClaimTypes.Name, user.UserName ?? "")
			};

			// Add Roles
			claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _jwtSettings.Issuer,
				audience: _jwtSettings.Audience,
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenLifetimeMinutes),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
