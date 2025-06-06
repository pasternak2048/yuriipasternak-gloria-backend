﻿using AutoMapper;
using GLORIA.Contracts.Dtos.Subscription;
using GLORIA.Subscription.API.Models.Entities;

namespace GLORIA.Subscription.API.Mapping
{
	public class AdvertSubscriptionProfile : Profile
	{
		public AdvertSubscriptionProfile()
		{
			CreateMap<AdvertSubscriptionEntity, AdvertSubscriptionResponse>();

			CreateMap<AdvertSubscriptionCreateRequest, AdvertSubscriptionEntity>()
				.ForMember(dest => dest.Id, opt => opt.Ignore()) // буде генеруватись у Service
				.ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
				.ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
				.ForMember(dest => dest.ModifiedAt, opt => opt.Ignore())
				.ForMember(dest => dest.ModifiedBy, opt => opt.Ignore());

			CreateMap<AdvertSubscriptionUpdateRequest, AdvertSubscriptionEntity>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
				.ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
				.ForMember(dest => dest.ModifiedAt, opt => opt.Ignore())
				.ForMember(dest => dest.ModifiedBy, opt => opt.Ignore());

            CreateMap<AdvertSubscriptionEntity, AdvertSubscriptionMatchingResponse>();
        }
	}
}
