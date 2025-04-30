using BuildingBlocks.Infrastructure;
using DocumentMetadata.API.Models;
using DocumentMetadata.API.Models.DTOs.Requests;
using DocumentMetadata.API.Models.DTOs.Responses;

namespace DocumentMetadata.API.Controllers
{
	public class DocumentMetadataController
		: GenericBaseController<DocumentMetadataResponse, DocumentMetadataCreateRequest, DocumentMetadataUpdateRequest, DocumentMetadataFilters>
	{
		public DocumentMetadataController(IGenericService<DocumentMetadataResponse, DocumentMetadataCreateRequest, DocumentMetadataUpdateRequest, DocumentMetadataFilters> service) : base(service) { }
	}
}
