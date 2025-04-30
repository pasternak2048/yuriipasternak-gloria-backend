using System.ComponentModel.DataAnnotations;

namespace DocumentStorage.API.Models
{
	public class DocumentUploadRequest
	{
		[Required]
		public IFormFile File { get; set; }


		[Required]
		public DocumentType DocumentType { get; set; }
	}
}
