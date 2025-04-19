namespace Photo.API.Models.DTOs.Responses
{
	public class RealtyPhotoMetadataResponse
	{
		public Guid Id { get; set; }

		public string RealtyId { get; set; } = string.Empty;

		public string FileName { get; set; } = string.Empty;

		public string ContentType { get; set; } = string.Empty;

		public string Url { get; set; } = string.Empty;

		public DateTime CreatedAt { get; set; }
	}
}
