using System.ComponentModel.DataAnnotations;

namespace Photo.API.Models.DTOs.Requests
{
	public class CreateRealtyPhotoMetadataRequest
	{
		[Required]
		public IFormFile File { get; set; }


		[Required]
		public Guid RealtyId { get; set; }
	}
}
