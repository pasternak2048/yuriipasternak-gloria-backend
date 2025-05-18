using BuildingBlocks.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace DocumentStorage.API.Models.DTOs.Requests
{
	public class DocumentStorageUploadRequest
	{
		[Required]
		public IFormFile File { get; set; }


		[Required]
		public DocumentType DocumentType { get; set; }
	}
}
