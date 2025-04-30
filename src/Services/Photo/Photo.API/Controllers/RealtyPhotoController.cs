using BuildingBlocks.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Photo.API.Models;
using Photo.API.Models.DTOs.Requests;
using Photo.API.Models.DTOs.Responses;

namespace Photo.API.Controllers
{
	public class RealtyPhotoController
	: GenericBaseController<RealtyPhotoMetadataResponse, CreateRealtyPhotoMetadataRequest, UpdateRealtyPhotoMetadataRequest, RealtyPhotoFilters>
	{
		private readonly IGenericService<RealtyPhotoMetadataResponse, CreateRealtyPhotoMetadataRequest, UpdateRealtyPhotoMetadataRequest, RealtyPhotoFilters> _service;
		public RealtyPhotoController(IGenericService<RealtyPhotoMetadataResponse, CreateRealtyPhotoMetadataRequest, UpdateRealtyPhotoMetadataRequest, RealtyPhotoFilters> service) : base(service) { 
			_service = service;
		}

		[HttpPost()]
		[Authorize]
		public override async Task<IActionResult> Create([FromForm] CreateRealtyPhotoMetadataRequest request, CancellationToken cancellationToken)
		{
			await _service.CreateAsync(request, cancellationToken);
			return NoContent();
		}


	}
}
