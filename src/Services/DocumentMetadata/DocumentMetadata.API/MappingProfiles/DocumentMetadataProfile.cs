using AutoMapper;
using DocumentMetadata.API.Models.DTOs.Requests;
using DocumentMetadata.API.Models.DTOs.Responses;
using DocumentMetadata.API.Models.Entities;

namespace DocumentMetadata.API.MappingProfiles
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
