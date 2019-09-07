using AspNetCore.Extensions.Streaming.Consumer;
using Kafka_WET.Domain.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kafka_WET.Services.Streaming
{
    public class InschrijvingEventListener : IHostedService, IDisposable
    {

        private readonly ILogger<InschrijvingEventListener> _logger;
        private Task _executingTask;
        private readonly CancellationTokenSource _stoppingCts;
        private readonly IConsumer<InschrijvingEvent> _consumer;
        private readonly IServiceProvider _serviceProvider;



        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
