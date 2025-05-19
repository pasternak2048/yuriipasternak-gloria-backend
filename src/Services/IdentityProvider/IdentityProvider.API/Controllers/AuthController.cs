using BuildingBlocks.Exceptions;
using Contracts.Dtos.IdentityProvider;
using IdentityProvider.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityProvider.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly IIdentityService _identityService;

		public AuthController(IIdentityService identityService)
		{
			_identityService = identityService;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto dto)
		{
			var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
			var device = Request.Headers["User-Agent"].ToString();
			var result = await _identityService.LoginAsync(dto, ip, device);
			return Ok(result);
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterDto dto)
		{
			var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
			var device = Request.Headers["User-Agent"].ToString();
			var result = await _identityService.RegisterAsync(dto, ip, device);
			return Ok(result);
		}

		[HttpPost("refresh")]
		public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto dto)
		{
			var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
			var device = Request.Headers["User-Agent"].ToString();
			var result = await _identityService.RefreshAsync(dto, ip, device);
			return Ok(result);
		}

		[Authorize(AuthenticationSchemes = "Bearer")]
		[HttpGet("sessions")]
		public async Task<IActionResult> GetSessions()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var refreshToken = Request.Headers["X-Refresh-Token"].FirstOrDefault();
			var result = await _identityService.GetActiveSessionsAsync(userId, refreshToken);
			return Ok(result);
		}

		[Authorize(AuthenticationSchemes = "Bearer")]
		[HttpDelete("sessions/{token}")]
		public async Task<IActionResult> RevokeSession(string token)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
			await _identityService.RevokeSessionAsync(userId, token, ip);
			return NoContent();
		}

		[Authorize(AuthenticationSchemes = "Bearer")]
		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var refreshToken = Request.Headers["X-Refresh-Token"].FirstOrDefault();

			if (string.IsNullOrWhiteSpace(refreshToken))
				throw new BadRequestException("Missing refresh token.");

			var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

			await _identityService.LogoutAsync(userId, refreshToken, ip);
			return NoContent();
		}

		[Authorize(AuthenticationSchemes = "Bearer")]
		[HttpPost("logout/all")]
		public async Task<IActionResult> LogoutAll()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
			await _identityService.LogoutFromAllAsync(userId, ip);
			return NoContent();
		}
	}
}
