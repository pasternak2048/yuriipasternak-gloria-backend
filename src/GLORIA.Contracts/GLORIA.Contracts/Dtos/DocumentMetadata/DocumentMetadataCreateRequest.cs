﻿using GLORIA.Contracts.Enums;

namespace GLORIA.Contracts.Dtos.DocumentMetadata
{
	public class DocumentMetadataCreateRequest
	{
		public string Url { get; set; } = string.Empty;

		public string? ThumbnailUrl { get; set; }

		public string FileName { get; set; } = string.Empty;

		public string MimeType { get; set; } = string.Empty;

		public Guid? OwnerUserId { get; set; }

		public DocumentType DocumentType { get; set; }

		public Guid OwnerObjectId { get; set; }

		public OwnerObjectType OwnerObjectType { get; set; }
	}
}
