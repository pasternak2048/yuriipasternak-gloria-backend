using Contracts.Dtos.Advert;
using Contracts.Dtos.Common;
using FluentValidation;

namespace Advert.API.Validators
{
	public class AdvertCreateRequestValidator : AbstractValidator<AdvertCreateRequest>
	{
		public AdvertCreateRequestValidator()
		{
			RuleFor(x => x.RealtyId)
				.NotEmpty().WithMessage("RealtyId is required.");

			RuleFor(x => x.AdvertType)
				.IsInEnum().WithMessage("Invalid advert type.");

			RuleFor(x => x.Price)
				.GreaterThan(0).WithMessage("Price must be greater than 0.");

			RuleFor(x => x.Currency)
				.IsInEnum().WithMessage("Invalid currency code.");

			RuleFor(x => x.Title)
				.NotEmpty().WithMessage("Title is required.")
				.MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

			RuleFor(x => x.Description)
				.MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

			RuleFor(x => x.Address)
				.NotNull().WithMessage("Address is required.")
				.SetValidator(new AddressValidator());
		}
	}

	public class AdvertUpdateRequestValidator : AbstractValidator<AdvertUpdateRequest>
	{
		public AdvertUpdateRequestValidator()
		{
			RuleFor(x => x.AdvertType)
				.IsInEnum().WithMessage("Invalid advert type.");

			RuleFor(x => x.Price)
				.GreaterThan(0).WithMessage("Price must be greater than 0.");

			RuleFor(x => x.Currency)
				.IsInEnum().WithMessage("Invalid currency code.");

			RuleFor(x => x.Title)
				.NotEmpty().WithMessage("Title is required.")
				.MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

			RuleFor(x => x.Description)
				.MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

			RuleFor(x => x.Status)
				.IsInEnum().WithMessage("Invalid advert status.");

			RuleFor(x => x.Address)
				.NotNull().WithMessage("Address is required.")
				.SetValidator(new AddressValidator());
		}
	}

	public class AdvertFiltersValidator : AbstractValidator<AdvertFilters>
	{
		public AdvertFiltersValidator()
		{
			RuleFor(x => x.MinPrice)
				.GreaterThanOrEqualTo(0)
				.When(x => x.MinPrice.HasValue).WithMessage("MinPrice must be at least 0.");

			RuleFor(x => x.MaxPrice)
				.GreaterThanOrEqualTo(0)
				.When(x => x.MaxPrice.HasValue).WithMessage("MaxPrice must be at least 0.");

			RuleFor(x => x)
				.Must(x => !x.MinPrice.HasValue || !x.MaxPrice.HasValue || x.MinPrice <= x.MaxPrice)
				.WithMessage("MinPrice must be less than or equal to MaxPrice.");

			RuleFor(x => x.AdvertType)
				.IsInEnum()
				.When(x => x.AdvertType.HasValue).WithMessage("Invalid advert type.");

			RuleFor(x => x.Status)
				.IsInEnum()
				.When(x => x.Status.HasValue).WithMessage("Invalid advert status.");

			RuleFor(x => x.City)
				.MaximumLength(100).When(x => !string.IsNullOrEmpty(x.City)).WithMessage("City must not exceed 100 characters.");

			RuleFor(x => x.Region)
				.MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Region)).WithMessage("Region must not exceed 100 characters.");

			RuleFor(x => x.Street)
				.MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Street)).WithMessage("Street must not exceed 100 characters.");

			RuleFor(x => x.ZipCode)
				.MaximumLength(20).When(x => !string.IsNullOrEmpty(x.ZipCode)).WithMessage("ZipCode must not exceed 20 characters.");
		}
	}

	public class AddressValidator : AbstractValidator<Address>
	{
		public AddressValidator()
		{
			RuleFor(x => x.Street)
				.NotEmpty().WithMessage("Street is required.")
				.MaximumLength(200).WithMessage("Street must not exceed 200 characters.");

			RuleFor(x => x.City)
				.NotEmpty().WithMessage("City is required.")
				.MaximumLength(100).WithMessage("City must not exceed 100 characters.");

			RuleFor(x => x.Region)
				.NotEmpty().WithMessage("Region is required.")
				.MaximumLength(100).WithMessage("Region must not exceed 100 characters.");

			RuleFor(x => x.ZipCode)
				.NotEmpty().WithMessage("ZipCode is required.")
				.MaximumLength(20).WithMessage("ZipCode must not exceed 20 characters.");
		}
	}
}
