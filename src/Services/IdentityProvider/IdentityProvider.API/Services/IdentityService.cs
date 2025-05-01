using BuildingBlocks.Exceptions;
using IdentityProvider.API.Models.DTOs;
using IdentityProvider.API.Models.Identity;
using IdentityProvider.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace IdentityProvider.API.Services
{
	public class IdentityService : IIdentityService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly ITokenService _tokenService;

		public IdentityService(
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			ITokenService tokenService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_tokenService = tokenService;
		}

		public async Task<TokenResponseDto> LoginAsync(LoginDto dto)
		{
			if (dto == null)
				throw new UnauthorizedException("Unauthorized access", "No credentials provided.");

			var user = await _userManager.FindByEmailAsync(dto.Email);
			if (user == null)
				throw new NotFoundException("User not found.");

			var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
			if (!result.Succeeded)
				throw new UnauthorizedException("Unauthorized access", "Wrong password.");

			var token = await _tokenService.GenerateTokenAsync(user, _userManager);
			var roles = await _userManager.GetRolesAsync(user);

			return new TokenResponseDto
			{
				Token = token,
				UserId = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email!,
				UserName = user.UserName!,
				Roles = roles.ToList()
			};
		}

		public async Task<TokenResponseDto> RegisterAsync(RegisterDto dto)
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

			var token = await _tokenService.GenerateTokenAsync(user, _userManager);
			var roles = await _userManager.GetRolesAsync(user);

			return new TokenResponseDto
			{
				Token = token,
				UserId = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email!,
				UserName = user.UserName!,
				Roles = roles.ToList()
			};
		}
	}
}
