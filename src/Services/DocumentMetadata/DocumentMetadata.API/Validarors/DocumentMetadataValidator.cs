using DocumentMetadata.API.Models.DTOs.Requests;
using DocumentMetadata.API.Models.Filters;
using FluentValidation;

namespace DocumentMetadata.API.Validarors
{
	public class DocumentMetadataCreateRequestValidator : AbstractValidator<DocumentMetadataCreateRequest>
	{
		public DocumentMetadataCreateRequestValidator()
		{
			RuleFor(x => x.Url)
				.NotEmpty().WithMessage("Url is required.")
				.Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
				.WithMessage("Url must be a valid absolute URI.");

			RuleFor(x => x.ThumbnailUrl)
				.Must(uri => string.IsNullOrEmpty(uri) || Uri.IsWellFormedUriString(uri, UriKind.Absolute))
				.WithMessage("ThumbnailUrl must be a valid absolute URI.");

			RuleFor(x => x.FileName)
				.NotEmpty().WithMessage("FileName is required.")
				.MaximumLength(200).WithMessage("FileName must not exceed 200 characters.");

			RuleFor(x => x.MimeType)
				.NotEmpty().WithMessage("MimeType is required.")
				.MaximumLength(100).WithMessage("MimeType must not exceed 100 characters.");

			RuleFor(x => x.OwnerUserId)
				.Must(id => !id.HasValue || id.Value != Guid.Empty)
				.WithMessage("OwnerUserId must be a valid GUID.");

			RuleFor(x => x.DocumentType)
				.IsInEnum().WithMessage("Invalid document type.");

			RuleFor(x => x.OwnerObjectId)
				.NotEmpty().WithMessage("OwnerObjectId is required.");

			RuleFor(x => x.OwnerObjectType)
				.IsInEnum().WithMessage("Invalid owner object type.");
		}
	}

	public class DocumentMetadataUpdateRequestValidator : AbstractValidator<DocumentMetadataUpdateRequest>
	{
		public DocumentMetadataUpdateRequestValidator()
		{
			RuleFor(x => x.Url)
				.NotEmpty().WithMessage("Url is required.")
				.Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
				.WithMessage("Url must be a valid absolute URI.");

			RuleFor(x => x.ThumbnailUrl)
				.Must(uri => string.IsNullOrEmpty(uri) || Uri.IsWellFormedUriString(uri, UriKind.Absolute))
				.WithMessage("ThumbnailUrl must be a valid absolute URI.");

			RuleFor(x => x.FileName)
				.NotEmpty().WithMessage("FileName is required.")
				.MaximumLength(200).WithMessage("FileName must not exceed 200 characters.");

			RuleFor(x => x.MimeType)
				.NotEmpty().WithMessage("MimeType is required.")
				.MaximumLength(100).WithMessage("MimeType must not exceed 100 characters.");

			RuleFor(x => x.OwnerUserId)
				.Must(id => !id.HasValue || id.Value != Guid.Empty)
				.WithMessage("OwnerUserId must be a valid GUID.");

			RuleFor(x => x.DocumentType)
				.IsInEnum().WithMessage("Invalid document type.");

			RuleFor(x => x.OwnerObjectId)
				.NotEmpty().WithMessage("OwnerObjectId is required.");

			RuleFor(x => x.OwnerObjectType)
				.IsInEnum().WithMessage("Invalid owner object type.");
		}
	}

	public class DocumentMetadataFiltersValidator : AbstractValidator<DocumentMetadataFilters>
	{
		public DocumentMetadataFiltersValidator()
		{
			RuleFor(x => x.OwnerUserId)
				.Must(id => !id.HasValue || id.Value != Guid.Empty)
				.WithMessage("OwnerUserId must be a valid GUID.");

			RuleFor(x => x.DocumentType)
				.IsInEnum().When(x => x.DocumentType.HasValue)
				.WithMessage("Invalid document type filter.");

			RuleFor(x => x.OwnerObjectId)
				.Must(id => !id.HasValue || id.Value != Guid.Empty)
				.WithMessage("OwnerObjectId must be a valid GUID.");

			RuleFor(x => x.OwnerObjectType)
				.IsInEnum().When(x => x.OwnerObjectType.HasValue)
				.WithMessage("Invalid owner object type filter.");
		}
	}
}
