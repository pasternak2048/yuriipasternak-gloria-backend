using Contracts.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Contracts.Dtos.DocumentStorage
{
	public class DocumentStorageUploadRequest
	{
		[Required]
		public IFormFile File { get; set; }

		[Required]
		public DocumentType DocumentType { get; set; }
	}
}
