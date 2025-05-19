using Contracts.Dtos.Subscription;
using FluentValidation;

namespace Subscription.API.Validators
{
	public class AdvertSubscriptionCreateRequestValidator : AbstractValidator<AdvertSubscriptionCreateRequest>
	{
		public AdvertSubscriptionCreateRequestValidator()
		{
			RuleFor(x => x.EventType)
				.IsInEnum().WithMessage("Invalid event type.");

			RuleFor(x => x.AdvertType)
				.IsInEnum().WithMessage("Invalid advert type.");

			RuleFor(x => x.Title)
				.NotEmpty().WithMessage("Title is required.")
				.MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

			RuleFor(x => x.Street)
				.NotEmpty().WithMessage("Street is required.")
				.MaximumLength(200).WithMessage("Street must not exceed 200 characters.");

			RuleFor(x => x.City)
				.NotEmpty().WithMessage("City is required.")
				.MaximumLength(100).WithMessage("City must not exceed 100 characters.");

			RuleFor(x => x.Region)
				.NotEmpty().WithMessage("Region is required.")
				.MaximumLength(100).WithMessage("Region must not exceed 100 characters.");

			RuleFor(x => x.MinPrice)
				.GreaterThanOrEqualTo(0).WithMessage("MinPrice must be at least 0.");

			RuleFor(x => x.MaxPrice)
				.GreaterThanOrEqualTo(0).WithMessage("MaxPrice must be at least 0.");

			RuleFor(x => x)
				.Must(x => x.MinPrice <= x.MaxPrice)
				.WithMessage("MinPrice must be less than or equal to MaxPrice.");

			RuleFor(x => x.Currency)
				.IsInEnum().WithMessage("Invalid currency code.");
		}
	}

	public class AdvertSubscriptionUpdateRequestValidator : AbstractValidator<AdvertSubscriptionUpdateRequest>
	{
		public AdvertSubscriptionUpdateRequestValidator()
		{
			RuleFor(x => x.EventType)
				.IsInEnum().WithMessage("Invalid event type.");

			RuleFor(x => x.AdvertType)
				.IsInEnum().WithMessage("Invalid advert type.");

			RuleFor(x => x.Title)
				.NotEmpty().WithMessage("Title is required.")
				.MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

			RuleFor(x => x.Street)
				.NotEmpty().WithMessage("Street is required.")
				.MaximumLength(200).WithMessage("Street must not exceed 200 characters.");

			RuleFor(x => x.City)
				.NotEmpty().WithMessage("City is required.")
				.MaximumLength(100).WithMessage("City must not exceed 100 characters.");

			RuleFor(x => x.Region)
				.NotEmpty().WithMessage("Region is required.")
				.MaximumLength(100).WithMessage("Region must not exceed 100 characters.");

			RuleFor(x => x.MinPrice)
				.GreaterThanOrEqualTo(0).WithMessage("MinPrice must be at least 0.");

			RuleFor(x => x.MaxPrice)
				.GreaterThanOrEqualTo(0).WithMessage("MaxPrice must be at least 0.");

			RuleFor(x => x)
				.Must(x => x.MinPrice <= x.MaxPrice)
				.WithMessage("MinPrice must be less than or equal to MaxPrice.");

			RuleFor(x => x.Currency)
				.IsInEnum().WithMessage("Invalid currency code.");
		}
	}

	public class AdvertSubscriptionFiltersValidator : AbstractValidator<AdvertSubscriptionFilters>
	{
		public AdvertSubscriptionFiltersValidator()
		{
			RuleFor(x => x.UserId)
				.Must(id => id.HasValue && id.Value != Guid.Empty)
				.When(x => x.UserId.HasValue)
				.WithMessage("UserId must be a valid GUID.");

			RuleFor(x => x.EventType)
				.IsInEnum().When(x => x.EventType.HasValue)
				.WithMessage("Invalid event type filter.");

			RuleFor(x => x.AdvertType)
				.IsInEnum().When(x => x.AdvertType.HasValue)
				.WithMessage("Invalid advert type filter.");

			RuleFor(x => x.Title)
				.MaximumLength(200).When(x => !string.IsNullOrEmpty(x.Title))
				.WithMessage("Title must not exceed 200 characters.");

			RuleFor(x => x.Street)
				.MaximumLength(200).When(x => !string.IsNullOrEmpty(x.Street))
				.WithMessage("Street must not exceed 200 characters.");

			RuleFor(x => x.City)
				.MaximumLength(100).When(x => !string.IsNullOrEmpty(x.City))
				.WithMessage("City must not exceed 100 characters.");

			RuleFor(x => x.Region)
				.MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Region))
				.WithMessage("Region must not exceed 100 characters.");

			RuleFor(x => x.MinPrice)
				.GreaterThanOrEqualTo(0).When(x => x.MinPrice.HasValue)
				.WithMessage("MinPrice must be at least 0.");

			RuleFor(x => x.MaxPrice)
				.GreaterThanOrEqualTo(0).When(x => x.MaxPrice.HasValue)
				.WithMessage("MaxPrice must be at least 0.");

			RuleFor(x => x)
				.Must(x => !x.MinPrice.HasValue || !x.MaxPrice.HasValue || x.MinPrice <= x.MaxPrice)
				.WithMessage("MinPrice must be less than or equal to MaxPrice.");

			RuleFor(x => x.Currency)
				.IsInEnum().When(x => x.Currency.HasValue)
				.WithMessage("Invalid currency code filter.");
		}
	}
}
