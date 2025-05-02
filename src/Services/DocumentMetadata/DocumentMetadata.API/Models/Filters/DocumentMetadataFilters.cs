using BuildingBlocks.Common.Enums;
using BuildingBlocks.Filtering;
using DocumentMetadata.API.Models.Enums;

namespace DocumentMetadata.API.Models.Filters
{
	public class DocumentMetadataFilters : BaseFilters
	{
		public Guid? OwnerUserId { get; set; }

		public DocumentType? DocumentType { get; set; }

		public Guid? OwnerObjectId { get; set; }

		public OwnerObjectType? OwnerObjectType { get; set; }

		public override string CacheKey() =>
			$"user={OwnerUserId?.ToString() ?? "any"}:type={DocumentType?.ToString() ?? "any"}:objId={OwnerObjectId?.ToString() ?? "any"}:objType={OwnerObjectType?.ToString() ?? "any"}";
		
	}
}
