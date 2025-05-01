using IdentityProvider.API.Models.DTOs;
using IdentityProvider.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IdentityProvider.API.Controllers
{
	public class AuthController : BaseApiController
	{
		private readonly IIdentityService _identityService;

		public AuthController(IIdentityService identityService)
		{
			_identityService = identityService;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto dto)
		{
			var result = await _identityService.LoginAsync(dto);
			return Ok(result);
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterDto dto)
		{
			var result = await _identityService.RegisterAsync(dto);
			return Ok(result);
		}
	}
}
