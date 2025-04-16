using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Catalog.API.Controllers
{
	public class TestController : BaseApiController
	{
		[HttpPost("auth")]
		[Authorize]
		public async Task<IActionResult> Auth()
		{
			string user = User.FindFirst(ClaimTypes.Name)?.Value;
			return Ok();
		}
	}
}