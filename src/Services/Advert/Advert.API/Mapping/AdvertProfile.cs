using Advert.API.Models.DTOs.Requests;
using Advert.API.Models.DTOs.Responses;
using Advert.API.Models.Enums;
using AutoMapper;
using BuildingBlocks.Common.DTOs;
using Contracts.Events;
using AdvertEntity = Advert.API.Models.Entities.Advert;

namespace Advert.API.Mapping
{
	public class AdvertProfile : Profile
	{
		public AdvertProfile()
		{
			CreateMap<AdvertEntity, AdvertResponse>()
				.ForMember(dest => dest.FullAddress, opt => opt.MapFrom(src => FormatAddress(src.Address)));

			CreateMap<AdvertCreateRequest, AdvertEntity>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.Status, opt => opt.MapFrom(_ => AdvertStatus.Active))
				.ForMember(dest => dest.Address, opt => opt.Ignore())

				.ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
				.ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
				.ForMember(dest => dest.ModifiedAt, opt => opt.Ignore())
				.ForMember(dest => dest.ModifiedBy, opt => opt.Ignore());

			CreateMap<AdvertUpdateRequest, AdvertEntity>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.RealtyId, opt => opt.Ignore())
				.ForMember(dest => dest.Address, opt => opt.Ignore())
				.ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
				.ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
				.ForMember(dest => dest.ModifiedAt, opt => opt.Ignore())
				.ForMember(dest => dest.ModifiedBy, opt => opt.Ignore());

			CreateMap<AdvertEntity, AdvertCreatedEvent>()
				.ForMember(dest => dest.AdvertId, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
				.ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.Address.Region))
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));
		}

		private static string FormatAddress(Address address)
		{
			if (address == null) return string.Empty;

			return $"{address.Street}, {address.City}, {address.Region}, {address.ZipCode}".Trim().Replace("  ", " ");
		}
	}
}
