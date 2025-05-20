using GLORIA.Contracts.Dtos.Common;
using GLORIA.Contracts.Enums;
using MongoDB.Driver;

namespace GLORIA.Contracts.Dtos.DocumentMetadata
{
	public class DocumentMetadataFilters : BaseFilters
	{
		public Guid? OwnerUserId { get; set; }

		public DocumentType? DocumentType { get; set; }

		public Guid? OwnerObjectId { get; set; }

		public OwnerObjectType? OwnerObjectType { get; set; }

		public override string CacheKey() =>
			$"user={OwnerUserId?.ToString() ?? "any"}:type={DocumentType?.ToString() ?? "any"}:objId={OwnerObjectId?.ToString() ?? "any"}:objType={OwnerObjectType?.ToString() ?? "any"}";

		public override FilterDefinition<DocumentMetadataEntity> ToFilter<DocumentMetadataEntity>()
		{
			var builder = Builders<DocumentMetadataEntity>.Filter;
			var filter = builder.Empty;

			if (OwnerUserId.HasValue)
				filter &= builder.Eq("OwnerUserId", OwnerUserId.Value);

			if (DocumentType.HasValue)
				filter &= builder.Eq("DocumentType", DocumentType.Value);

			if (OwnerObjectId.HasValue)
				filter &= builder.Eq("OwnerObjectId", OwnerObjectId.Value);

			if (OwnerObjectType.HasValue)
				filter &= builder.Eq("OwnerObjectType", OwnerObjectType.Value);

			return filter;
		}
	}
}
