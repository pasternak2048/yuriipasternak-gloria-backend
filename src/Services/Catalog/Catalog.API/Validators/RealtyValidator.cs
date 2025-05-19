using Contracts.Dtos.Catalog;
using Contracts.Dtos.Common;
using FluentValidation;

namespace Catalog.API.Validators
{
	public class RealtyCreateRequestValidator : AbstractValidator<RealtyCreateRequest>
	{
		public RealtyCreateRequestValidator()
		{
			RuleFor(x => x.Type)
				.IsInEnum().WithMessage("Invalid realty type.");

			RuleFor(x => x.WallType)
				.IsInEnum().WithMessage("Invalid wall type.");

			RuleFor(x => x.HeatingType)
				.IsInEnum().WithMessage("Invalid heating type.");

			RuleFor(x => x.Area)
				.GreaterThan(0).WithMessage("Area must be greater than 0.");

			RuleFor(x => x.Floor)
				.GreaterThanOrEqualTo(0).WithMessage("Floor must be at least 0.");

			RuleFor(x => x.Rooms)
				.GreaterThan(0).WithMessage("Rooms must be at least 1.");

			RuleFor(x => x.Baths)
				.GreaterThanOrEqualTo(0).WithMessage("Baths must be at least 0.");

			RuleFor(x => x.BuildDate)
				.Must(d => d <= DateTimeOffset.UtcNow)
				.WithMessage("Build date cannot be in the future.");

			RuleFor(x => x.Address)
				.NotNull().WithMessage("Address is required.")
				.SetValidator(new AddressValidator());
		}
	}

	public class RealtyUpdateRequestValidator : AbstractValidator<RealtyUpdateRequest>
	{
		public RealtyUpdateRequestValidator()
		{
			RuleFor(x => x.Type)
				.IsInEnum().WithMessage("Invalid realty type.");

			RuleFor(x => x.WallType)
				.IsInEnum().WithMessage("Invalid wall type.");

			RuleFor(x => x.HeatingType)
				.IsInEnum().WithMessage("Invalid heating type.");

			RuleFor(x => x.Area)
				.GreaterThan(0).WithMessage("Area must be greater than 0.");

			RuleFor(x => x.Floor)
				.GreaterThanOrEqualTo(0).WithMessage("Floor must be at least 0.");

			RuleFor(x => x.Rooms)
				.GreaterThan(0).WithMessage("Rooms must be at least 1.");

			RuleFor(x => x.Baths)
				.GreaterThanOrEqualTo(0).WithMessage("Baths must be at least 0.");

			RuleFor(x => x.BuildDate)
				.Must(d => d <= DateTimeOffset.UtcNow)
				.WithMessage("Build date cannot be in the future.");

			RuleFor(x => x.Address)
				.NotNull().WithMessage("Address is required.")
				.SetValidator(new AddressValidator());

			RuleFor(x => x.RealtyStatus)
				.IsInEnum().WithMessage("Invalid realty status.");
		}
	}

	public class RealtyFiltersValidator : AbstractValidator<RealtyFilters>
	{
		public RealtyFiltersValidator()
		{
			RuleFor(x => x.Type)
				.IsInEnum().When(x => x.Type.HasValue)
				.WithMessage("Invalid realty type filter.");

			RuleFor(x => x.Status)
				.IsInEnum().When(x => x.Status.HasValue)
				.WithMessage("Invalid realty status filter.");
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
				.NotEmpty().WithMessage("Zip code is required.")
				.MaximumLength(20).WithMessage("Zip code must not exceed 20 characters.");
		}
	}
}
