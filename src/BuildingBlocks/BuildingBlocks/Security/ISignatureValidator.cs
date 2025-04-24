using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Security
{
	public interface ISignatureValidator
	{
		bool IsValid(HttpRequest request);
	}
}
