using BuildingBlocks.Controllers;
using BuildingBlocks.Infrastructure;
using Contracts.Dtos.DocumentMetadata;

namespace DocumentMetadata.API.Controllers
{
	public class DocumentMetadataController
		: GenericBaseController<DocumentMetadataResponse, DocumentMetadataCreateRequest, DocumentMetadataUpdateRequest, DocumentMetadataFilters>
	{
		public DocumentMetadataController(IGenericService<DocumentMetadataResponse, DocumentMetadataCreateRequest, DocumentMetadataUpdateRequest, DocumentMetadataFilters> service) : base(service) { }
	}
}
