using IdentityProvider.API.Models.DTOs;
using IdentityProvider.API.Models.Identity;
using IdentityProvider.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityProvider.API.Controllers
{
	public class AuthController : BaseApiController
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly ITokenService _tokenService;

		public AuthController(
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			ITokenService tokenService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_tokenService = tokenService;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto dto)
		{
			var user = await _userManager.FindByEmailAsync(dto.Email);

			if (user == null)
			{
				return Unauthorized("User not found");
			}

			var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
			if (!result.Succeeded)
			{
				return Unauthorized("Wrong password");
			}

			var token = await _tokenService.GenerateTokenAsync(user);
			var roles = await _userManager.GetRolesAsync(user);

			return Ok(new TokenResponseDto
			{
				Token = token,
				UserId = user.Id,
				Email = user.Email!,
				UserName = user.UserName!,
				Roles = roles.ToList()
			});
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterDto dto)
		{
			var existingUser = await _userManager.FindByEmailAsync(dto.Email);
			if (existingUser != null)
			{
				return BadRequest("User with this email already exists");
			}
				
			var user = new ApplicationUser
			{
				UserName = dto.UserName,
				Email = dto.Email
			};

			var result = await _userManager.CreateAsync(user, dto.Password);

			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					//_logger.LogError("Error creating user: {Error}", error.Description);
				}
				return BadRequest("Failed to create user");
			}

			await _userManager.AddToRoleAsync(user, "User");

			var token = await _tokenService.GenerateTokenAsync(user);
			var roles = await _userManager.GetRolesAsync(user);

			return Ok(new TokenResponseDto
			{
				Token = token,
				UserId = user.Id,
				Email = user.Email!,
				UserName = user.UserName!,
				Roles = roles.ToList()
			});
		}
	}
}
