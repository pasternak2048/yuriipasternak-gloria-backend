using Microsoft.AspNetCore.Http;

namespace GLORIA.BuildingBlocks.Security
{
	public interface ISignatureValidator
	{
		bool IsValid(HttpRequest request);
	}
}
