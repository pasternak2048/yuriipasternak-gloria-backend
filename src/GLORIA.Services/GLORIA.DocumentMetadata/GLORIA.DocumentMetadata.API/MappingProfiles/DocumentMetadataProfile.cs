using AutoMapper;
using GLORIA.Contracts.Dtos.DocumentMetadata;
using GLORIA.DocumentMetadata.API.Models.Entities;

namespace GLORIA.DocumentMetadata.API.MappingProfiles
{
	public class DocumentMetadataProfile : Profile
	{
		public DocumentMetadataProfile()
		{
			CreateMap<DocumentMetadataEntity, DocumentMetadataResponse>();
			CreateMap<DocumentMetadataCreateRequest, DocumentMetadataEntity>();
			CreateMap<DocumentMetadataUpdateRequest, DocumentMetadataEntity>();
		}
	}
}
