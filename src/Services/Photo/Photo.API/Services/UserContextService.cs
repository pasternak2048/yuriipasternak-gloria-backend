using Photo.API.Services.Interfaces;
using System.Security.Claims;

namespace Photo.API.Services
{
	public class UserContextService : IUserContextService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public UserContextService(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public Guid? GetUserId()
		{
			var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (Guid.TryParse(userId, out var guid))
			{
				return guid;
			}

			return null;
		}
	}
}
