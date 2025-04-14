using BuildingBlocks.Pagination;
using Catalog.API.Models.DTOs.Requests;
using Catalog.API.Models.DTOs.Responses;
using Catalog.API.Models;
using Catalog.API.Repositories.Interfaces;
using Catalog.API.Services.Interfaces;
using AutoMapper;

namespace Catalog.API.Services
{
	public class RealtyService : IRealtyService
	{
		private readonly IRealtyRepository _repository;
		private readonly IMapper _mapper;

		public RealtyService(IRealtyRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<List<RealtyResponse>> GetAllAsync(CancellationToken cancellationToken)
		{
			var items = await _repository.GetAllAsync(cancellationToken);
			return items.Select(r => _mapper.Map<RealtyResponse>(r)).ToList();
		}

		public async Task<RealtyResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
		{
			var entity = await _repository.GetByIdAsync(id, cancellationToken);
			return entity is null ? null : _mapper.Map<RealtyResponse>(entity);
		}

		public async Task CreateAsync(CreateRealtyRequest request, CancellationToken cancellationToken)
		{
			var entity = _mapper.Map<Realty>(request);
			await _repository.CreateAsync(entity, cancellationToken);
		}

		public Task DeleteAsync(Guid id, CancellationToken cancellationToken) => _repository.DeleteAsync(id, cancellationToken);

		public async Task<PaginatedResult<RealtyResponse>> GetFilteredAsync(GetRealtiesRequest request, CancellationToken cancellationToken)
		{
			var (items, count) = await _repository.GetFilteredAsync(request.Type, request.Status, request.PageIndex * request.PageSize, request.PageSize, cancellationToken);
			var mapped = items.Select(r => _mapper.Map<RealtyResponse>(r));
			return new PaginatedResult<RealtyResponse>(request.PageIndex, request.PageSize, count, mapped);
		}
	}
}
