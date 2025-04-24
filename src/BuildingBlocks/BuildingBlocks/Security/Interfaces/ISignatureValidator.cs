using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Security.Interfaces
{
	public interface ISignatureValidator
	{
		bool IsValid(HttpRequest request);
	}
}
