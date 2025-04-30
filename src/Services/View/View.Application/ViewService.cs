using BuildingBlocks.Exceptions;
using BuildingBlocks.Identity;
using BuildingBlocks.Pagination;
using View.Infrastructure.Persistence;
using ViewEntity = View.Domain.Entities.View;

namespace View.Application
{
	public class ViewService : IViewService
	{
		private readonly IViewRepository _repository;
		private readonly IUserIdentityProvider _userIdentityProvider;

		public ViewService(IViewRepository repository, IUserIdentityProvider userIdentityProvider)
		{
			_repository = repository;
			_userIdentityProvider = userIdentityProvider;
		}

		public async Task<ViewEntity> CreateAsync(ViewEntity view, CancellationToken cancellationToken = default)
		{
			return await _repository.CreateAsync(view, cancellationToken);
		}
			

		public async Task<ViewEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
		{
			var view = await _repository.GetByIdAsync(id, cancellationToken);

			if (view == null)
				return null;

			if (view.ClientId != _userIdentityProvider.UserId && view.AgentId != _userIdentityProvider.UserId)
				throw new ForbiddenAccessException("You are not authorized to view this item.");

			return view;
		}

		public async Task<PaginatedResult<ViewEntity>> GetAllForUserAsync(PaginatedRequest request, CancellationToken cancellationToken = default)
		{
			var allViews = await _repository.GetPaginatedAsync(request, cancellationToken);

			var filtered = allViews.Data
				.Where(v => v.ClientId == _userIdentityProvider.UserId || v.AgentId == _userIdentityProvider.UserId)
				.ToList();

			var result = new PaginatedResult<ViewEntity>(
				request.PageIndex,
				request.PageSize,
				filtered.Count,
				filtered
			);

			return result;
		}

		public async Task<PaginatedResult<ViewEntity>> GetByRealtyIdForUserAsync(Guid realtyId, PaginatedRequest request, CancellationToken cancellationToken = default)
		{
			var viewsForRealty = await _repository.GetByRealtyIdAsync(realtyId, request, cancellationToken);

			var filtered = viewsForRealty.Data
				.Where(v => v.ClientId == _userIdentityProvider.UserId || v.AgentId == _userIdentityProvider.UserId)
				.ToList();

			var result = new PaginatedResult<ViewEntity>(
				request.PageIndex,
				request.PageSize,
				filtered.Count,
				filtered
			);

			return result;
		}

		public async Task UpdateAsync(ViewEntity view, CancellationToken cancellationToken = default)
		{
			if (view == null)
				throw new NotFoundException($"View with id {view.Id} not found");

			if (view.ClientId != _userIdentityProvider.UserId && view.AgentId != _userIdentityProvider.UserId)
				throw new ForbiddenAccessException("You are not allowed to delete this item.");

			await _repository.UpdateAsync(view, cancellationToken);
		}
			

		public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
		{
			var view = await _repository.GetByIdAsync(id, cancellationToken);

			if (view == null)
				throw new NotFoundException($"View with id {id} not found");

			if (view.ClientId != _userIdentityProvider.UserId && view.AgentId != _userIdentityProvider.UserId)
				throw new ForbiddenAccessException("You are not allowed to delete this item.");

			await _repository.DeleteAsync(id, cancellationToken);
		}
	}
}
