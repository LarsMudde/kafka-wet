using AspNetCore.Extensions.Streaming.Publisher;
using Kafka_WET.Domain;
using Kafka_WET.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kafka_WET.Services.Service
{
    public class SubscriptionService : ISubscriptionService
    {

        private readonly IPublisher<SubscriptionEvent> _publisher;

        public SubscriptionService(IPublisher<SubscriptionEvent> publisher)
        {
            _publisher = publisher;
        }

        public async Task PublishSubscriptionAsync(Subscription subscription)
        {

            await _publisher.PublishAsync(new SubscriptionEvent(Guid.NewGuid(), Guid.NewGuid())
            {
                 subscription = subscription
            });

        }

        public async Task ProcessSubscriptionAsync(Guid subscriptionId, CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }
    }
}
