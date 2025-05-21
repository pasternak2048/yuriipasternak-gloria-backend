using AutoMapper;
using GLORIA.Contracts.Dtos.Subscription;
using GLORIA.Contracts.Events;
using GLORIA.Subscription.API.Repositories.Interfaces;

namespace GLORIA.Subscription.API.Services
{
    public class AdvertSubscriptionMatchingService
    {
        private readonly IAdvertSubscriptionLookupRepository _repository;
        private readonly IMapper _mapper;

        public AdvertSubscriptionMatchingService(IAdvertSubscriptionLookupRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyCollection<AdvertSubscriptionMatchingResponse>> GetMatchingSubscriptionsAsync(
            AdvertCreatedEvent @event,
            CancellationToken cancellationToken)
        {
            var entities = await _repository.GetMatchingAsync(@event, cancellationToken);
            return _mapper.Map<IReadOnlyCollection<AdvertSubscriptionMatchingResponse>>(entities);
        }
    }
}
