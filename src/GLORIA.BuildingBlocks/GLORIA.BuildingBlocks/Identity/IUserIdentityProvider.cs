namespace GLORIA.BuildingBlocks.Identity
{
	public interface IUserIdentityProvider
	{
		Guid? UserId { get; }

		bool IsAdmin { get; }
	}
}
