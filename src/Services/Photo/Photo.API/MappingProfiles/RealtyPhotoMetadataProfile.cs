using AutoMapper;
using Photo.API.Models;
using Photo.API.Models.DTOs.Responses;

namespace Photo.API.MappingProfiles
{
	public class RealtyPhotoMetadataProfile : Profile
	{
		public RealtyPhotoMetadataProfile()
		{
			CreateMap<RealtyPhotoMetadata, RealtyPhotoMetadataResponse>();
		}
	}
}
