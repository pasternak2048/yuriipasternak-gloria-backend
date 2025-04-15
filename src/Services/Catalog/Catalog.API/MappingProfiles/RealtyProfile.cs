using AutoMapper;
using Catalog.API.Models.DTOs.Responses;
using Catalog.API.Models;

namespace Catalog.API.MappingProfiles
{
	public class RealtyProfile : Profile
	{
		public RealtyProfile()
		{
			CreateMap<Realty, RealtyResponse>()
				.ForMember(dest => dest.FullAddress, opt => opt.MapFrom(src => FormatAddress(src.Address)));
		}

		private static string FormatAddress(Address address)
		{
			if (address == null) return string.Empty;

			return $"{address.Street}, {address.City}, {address.Region}, {address.ZipCode}".Trim().Replace("  ", " ");
		}
	}
}
