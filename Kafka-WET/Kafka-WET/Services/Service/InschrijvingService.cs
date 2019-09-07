using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kafka_WET.Services.Service
{
    public class InschrijvingService : IInschrijvingService
    {
        public Task ProcessInschrijvingAsync(Guid inschrijvingId, CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }
    }
}
