namespace DocumentStorage.API.Models.DTOs.Responses
{
	public class DocumentStorageResponse
	{
		public string Url { get; set; } = string.Empty;

		public string? ThumbnailUrl { get; set; } = string.Empty;

		public string FileName { get; set; } = string.Empty;

		public string MimeType { get; set; } = string.Empty;
	}
}
