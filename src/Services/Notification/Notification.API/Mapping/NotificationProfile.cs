using AutoMapper;
using Contracts.Dtos.Notification;
using Notification.API.Models.Entities;

namespace Notification.API.Mapping
{
	public class NotificationProfile : Profile
	{
		public NotificationProfile()
		{
			CreateMap<NotificationCreateRequest, NotificationEntity>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
				.ForMember(dest => dest.IsRead, opt => opt.MapFrom(src => false));

			CreateMap<NotificationEntity, NotificationCreateRequest>()
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
				.ForMember(dest => dest.EventType, opt => opt.MapFrom(src => src.EventType))
				.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
				.ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message));
		}
	}
}
