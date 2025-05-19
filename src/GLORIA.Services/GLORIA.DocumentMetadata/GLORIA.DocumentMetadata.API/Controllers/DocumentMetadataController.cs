using GLORIA.BuildingBlocks.Abstractions;
using GLORIA.BuildingBlocks.Controllers;
using GLORIA.Contracts.Dtos.DocumentMetadata;

namespace GLORIA.DocumentMetadata.API.Controllers
{
	public class DocumentMetadataController
		: GenericBaseController<DocumentMetadataResponse, DocumentMetadataCreateRequest, DocumentMetadataUpdateRequest, DocumentMetadataFilters>
	{
		public DocumentMetadataController(IGenericService<DocumentMetadataResponse, DocumentMetadataCreateRequest, DocumentMetadataUpdateRequest, DocumentMetadataFilters> service) : base(service) { }
	}
}
