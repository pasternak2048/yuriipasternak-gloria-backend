using GLORIA.BuildingBlocks.Exceptions;
using GLORIA.Contracts.Dtos.IdentityProvider;
using GLORIA.IdentityProvider.API.Models.Entities;
using GLORIA.IdentityProvider.API.Models.Entities.Identity;
using GLORIA.IdentityProvider.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace GLORIA.IdentityProvider.API.Services
{
	public class IdentityService : IIdentityService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IJwtTokenService _jwtTokenService;
		private readonly IRefreshTokenService _refreshTokenService;
		private readonly IRefreshTokenGenerator _refreshTokenGenerator;

		public IdentityService(
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			IJwtTokenService jwtTokenService,
			IRefreshTokenService refreshTokenService,
			IRefreshTokenGenerator refreshTokenGenerator)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_jwtTokenService = jwtTokenService;
			_refreshTokenService = refreshTokenService;
			_refreshTokenGenerator = refreshTokenGenerator;
		}

		public async Task<TokenResponseDto> LoginAsync(LoginDto dto, string? ip, string? device)
		{
			if (dto == null)
				throw new UnauthorizedException("Unauthorized access", "No credentials provided.");

			var user = await _userManager.FindByEmailAsync(dto.Email);
			if (user == null)
				throw new NotFoundException("User not found.");

			var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
			if (!result.Succeeded)
				throw new UnauthorizedException("Unauthorized access", "Wrong password.");

			var accessToken = await _jwtTokenService.GenerateAsync(user, _userManager);
			var refreshToken = _refreshTokenGenerator.Generate();

			await _refreshTokenService.SaveAsync(new RefreshToken
			{
				Token = refreshToken,
				UserId = user.Id,
				ExpiresAt = DateTime.UtcNow.AddDays(7),
				CreatedByIp = ip,
				Device = device
			});

			var roles = await _userManager.GetRolesAsync(user);

			return new TokenResponseDto
			{
				AccessToken = accessToken,
				RefreshToken = refreshToken,
				UserId = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email!,
				UserName = user.UserName!,
				Roles = roles.ToList()
			};
		}

		public async Task<TokenResponseDto> RegisterAsync(RegisterDto dto, string? ip, string? device)
		{
			var existingUser = await _userManager.FindByEmailAsync(dto.Email);
			if (existingUser != null)
				throw new BadRequestException("User with this email already exists.");

			var user = new ApplicationUser
			{
				FirstName = dto.FirstName,
				LastName = dto.LastName,
				UserName = dto.UserName,
				Email = dto.Email
			};

			var result = await _userManager.CreateAsync(user, dto.Password);
			if (!result.Succeeded)
				throw new BadRequestException("Failed to create user.");

			await _userManager.AddToRoleAsync(user, "User");

			var accessToken = await _jwtTokenService.GenerateAsync(user, _userManager);
			var refreshToken = _refreshTokenGenerator.Generate();

			await _refreshTokenService.SaveAsync(new RefreshToken
			{
				Token = refreshToken,
				UserId = user.Id,
				ExpiresAt = DateTime.UtcNow.AddDays(7),
				CreatedByIp = ip,
				Device = device
			});

			var roles = await _userManager.GetRolesAsync(user);

			return new TokenResponseDto
			{
				AccessToken = accessToken,
				RefreshToken = refreshToken,
				UserId = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email!,
				UserName = user.UserName!,
				Roles = roles.ToList()
			};
		}

		public async Task<TokenResponseDto> RefreshAsync(RefreshRequestDto dto, string? ip, string? device)
		{
			var oldToken = await _refreshTokenService.GetByTokenAsync(dto.RefreshToken);
			if (oldToken == null || oldToken.IsRevoked || oldToken.ExpiresAt < DateTime.UtcNow)
				throw new UnauthorizedException("Invalid or expired refresh token.");

			var user = await _userManager.FindByIdAsync(oldToken.UserId.ToString());
			if (user == null)
				throw new UnauthorizedException("User not found.");

			await _refreshTokenService.RevokeAsync(oldToken, ip);

			var newAccessToken = await _jwtTokenService.GenerateAsync(user, _userManager);
			var newRefreshToken = _refreshTokenGenerator.Generate();

			await _refreshTokenService.SaveAsync(new RefreshToken
			{
				Token = newRefreshToken,
				UserId = user.Id,
				ExpiresAt = DateTime.UtcNow.AddDays(7),
				CreatedByIp = ip,
				Device = device,
				ReplacedByToken = oldToken.Token
			});

			var roles = await _userManager.GetRolesAsync(user);

			return new TokenResponseDto
			{
				AccessToken = newAccessToken,
				RefreshToken = newRefreshToken,
				UserId = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email!,
				UserName = user.UserName!,
				Roles = roles.ToList()
			};
		}

		public async Task<IEnumerable<ActiveSessionDto>> GetActiveSessionsAsync(string userId, string? currentRefreshToken)
		{
			if (!Guid.TryParse(userId, out var parsedUserId))
				throw new UnauthorizedException("Invalid user id.");

			var tokens = await _refreshTokenService.GetActiveTokensByUser(parsedUserId);

			return tokens.Select(token => new ActiveSessionDto
			{
				Device = token.Device ?? "Unknown",
				CreatedAt = token.CreatedAt,
				IpAddress = token.CreatedByIp,
				ExpiresAt = token.ExpiresAt,
				IsCurrent = token.Token == currentRefreshToken
			});
		}

		public async Task RevokeSessionAsync(string userId, string refreshToken, string? ip)
		{
			if (!Guid.TryParse(userId, out var parsedUserId))
				throw new UnauthorizedException("Invalid user ID.");

			var found = await _refreshTokenService.GetByTokenAsync(refreshToken);
			if (found is null || found.IsRevoked)
				throw new NotFoundException("Token not found or already revoked.");

			if (found.UserId != parsedUserId)
				throw new ForbiddenAccessException("Token does not belong to current user.");

			await _refreshTokenService.RevokeAsync(found, ip);
		}

		public async Task LogoutAsync(string userId, string refreshToken, string? ip)
		{
			if (!Guid.TryParse(userId, out var parsedUserId))
				throw new UnauthorizedException("Invalid user ID");

			var token = await _refreshTokenService.GetByTokenAsync(refreshToken);

			if (token is null || token.IsRevoked || token.UserId != parsedUserId)
				throw new NotFoundException("Refresh token not found or already revoked.");

			await _refreshTokenService.RevokeAsync(token, ip);
		}

		public async Task LogoutFromAllAsync(string userId, string? ip)
		{
			if (!Guid.TryParse(userId, out var parsedUserId))
				throw new UnauthorizedException("Invalid user ID.");

			var tokens = await _refreshTokenService.GetActiveTokensByUser(parsedUserId);

			foreach (var token in tokens)
			{
				await _refreshTokenService.RevokeAsync(token, ip);
			}
		}
	}
}
