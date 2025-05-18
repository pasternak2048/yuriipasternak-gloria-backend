using FluentValidation;
using Notification.API.Models.DTOs.Requests;
using Notification.API.Models.Filters;

namespace Notification.API.Validators
{
	public class CreateNotificationRequestValidator : AbstractValidator<NotificationCreateRequest>
	{
		public CreateNotificationRequestValidator()
		{
			RuleFor(x => x.UserId)
				.NotEmpty().WithMessage("UserId is required.");

			RuleFor(x => x.EventType)
				.IsInEnum().WithMessage("Invalid event type.");

			RuleFor(x => x.Title)
				.NotEmpty().WithMessage("Title is required.")
				.MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

			RuleFor(x => x.Message)
				.NotEmpty().WithMessage("Message is required.")
				.MaximumLength(1000).WithMessage("Message must not exceed 1000 characters.");
		}
	}

	public class NotificationFiltersValidator : AbstractValidator<NotificationFilters>
	{
		public NotificationFiltersValidator()
		{
			RuleFor(x => x.UserId)
				.NotEmpty().When(x => x.UserId.HasValue)
				.WithMessage("UserId must be a valid GUID.");
		}
	}
}
