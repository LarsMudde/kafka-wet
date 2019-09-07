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
    public class InschrijvingService : IInschrijvingService
    {

        private readonly IPublisher<InschrijvingEvent> _publisher;

        public InschrijvingService(IPublisher<InschrijvingEvent> publisher)
        {
            _publisher = publisher;
        }

        public async Task PublishInschrijvingAsync(Inschrijving inschrijving)
        {

            await _publisher.PublishAsync(new InschrijvingEvent(Guid.NewGuid(), Guid.NewGuid())
            {
                 inschrijving = inschrijving
            });

        }

        public async Task ProcessInschrijvingAsync(Guid inschrijvingId, CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }
    }
}
