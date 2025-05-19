using AutoMapper;
using GLORIA.Catalog.API.Models.Entities;
using GLORIA.Contracts.Dtos.Catalog;
using GLORIA.Contracts.Dtos.Common;

namespace GLORIA.Catalog.API.Mapping
{
	public class RealtyProfile : Profile
	{
		public RealtyProfile()
		{
			CreateMap<RealtyEntity, RealtyResponse>()
				.ForMember(dest => dest.FullAddress, opt => opt.MapFrom(src => FormatAddress(src.Address)));

			CreateMap<RealtyCreateRequest, RealtyEntity>()
			.ForMember(dest => dest.Id, opt => opt.Ignore())
			.ForMember(dest => dest.Status, opt => opt.Ignore())
			.ForMember(dest => dest.BuildDate, opt => opt.MapFrom(src => src.BuildDate.UtcDateTime));

			CreateMap<RealtyUpdateRequest, RealtyEntity>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.BuildDate, opt => opt.MapFrom(src => src.BuildDate.UtcDateTime));
		}

		private static string FormatAddress(Address address)
		{
			if (address == null) return string.Empty;

			return $"{address.Street}, {address.City}, {address.Region}, {address.ZipCode}".Trim().Replace("  ", " ");
		}
	}
}
