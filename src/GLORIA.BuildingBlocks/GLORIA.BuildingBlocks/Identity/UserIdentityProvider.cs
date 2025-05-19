using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace GLORIA.BuildingBlocks.Identity
{
	public class UserIdentityProvider : IUserIdentityProvider
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public UserIdentityProvider(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public Guid? UserId
		{
			get
			{
				var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

				if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
				{
					return null;
				}

				return userId;
			}
		}

		public bool IsAdmin
		{
			get 
			{
				return _httpContextAccessor.HttpContext?.User?.IsInRole("Admin") ?? false;
			} 
		} 
	}
}
