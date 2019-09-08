using Kafka_WET.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kafka_WET.Services.Service
{
    public interface ISubscriptionService
    {
        Task ProcessSubscriptionAsync(Guid subscriptionId, CancellationToken cancellation = default);
        Task PublishSubscriptionAsync(Subscription subscription);
    }
}
