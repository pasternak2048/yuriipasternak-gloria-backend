namespace BuildingBlocks.Identity
{
	public interface IUserIdentityProvider
	{
		Guid? UserId { get; }
	}
}
