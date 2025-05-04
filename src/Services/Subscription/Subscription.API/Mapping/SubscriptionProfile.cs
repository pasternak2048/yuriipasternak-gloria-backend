using AutoMapper;
using BuildingBlocks.Utils;
using Subscription.API.Models.DTOs.Requests;
using Subscription.API.Models.DTOs.Responses;
using Subscription.API.Models.Entities;

namespace Subscription.API.Mapping
{
	public class SubscriptionProfile : Profile
	{
		public SubscriptionProfile()
		{
			CreateMap<SubscriptionEntity, SubscriptionResponse>();

			CreateMap<SubscriptionCreateRequest, SubscriptionEntity>()
				.ForMember(dest => dest.FilterJson, opt => opt.MapFrom(src => JsonNormalizer.Normalize(src.Filter)));

			CreateMap<SubscriptionUpdateRequest, SubscriptionEntity>()
				.ForMember(dest => dest.FilterJson, opt => opt.MapFrom(src => JsonNormalizer.Normalize(src.Filter)));

			CreateMap<SubscriptionEntity, SubscriptionResponse>();
		}
	}
}
