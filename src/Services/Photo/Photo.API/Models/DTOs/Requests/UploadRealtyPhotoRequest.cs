using System.ComponentModel.DataAnnotations;

namespace Photo.API.Models.DTOs.Requests
{
	public class UploadRealtyPhotoRequest
	{
		[Required]
		public IFormFile File { get; set; }


		[Required]
		public Guid RealtyId { get; set; }
	}
}
