using AspNetCore.Extensions.Streaming.Consumer;
using Kafka_WET.Domain.Events;
using Kafka_WET.Services.Service;
using Microsoft.Extensions.DependencyInjection;
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

        public InschrijvingEventListener(ILogger<InschrijvingEventListener> logger, IConsumer<InschrijvingEvent> consumer, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _consumer = consumer;
            _serviceProvider = serviceProvider;
            _stoppingCts = new CancellationTokenSource();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Background Service InschrijvingEventListener is starting.");

            // Store the task we're executing
            _executingTask = Task.Run(() => _consumer.Listen(async message => await HandleMessageAsync(message, _stoppingCts.Token), _stoppingCts.Token), cancellationToken);

            // If the task is completed then return it, 
            // this will bubble cancellation and failure to the caller
            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }

            // Otherwise it's running
            return Task.CompletedTask;
        }

        private async Task HandleMessageAsync(InschrijvingEvent message, CancellationToken cancellation)
        {
            using (var scope = _serviceProvider.CreateScope())
            {

                // possible room for expansion?
                // var inschrijvingService = scope.ServiceProvider.GetRequiredService<IInschrijvingService>();
                // var test = await testService.GetTestAsync(message.inschrijving.Id, cancellation);

                // Change color to make msg stand out
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Black;

                Console.WriteLine($"Message with traceid: {message.Header.TraceId} bevat inschrijving met naam: {message.inschrijving.Naam}");

                // Change back color to default
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;

            }
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Background Service TestConsumer is stopping.");

            // Stop called without start
            if (_executingTask == null)
            {
                return;
            }

            try
            {
                // Signal cancellation to the executing method
                _stoppingCts.Cancel();
            }
            finally
            {
                // Wait until the task completes or the stop token triggers
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }

        public void Dispose()
        {
            _stoppingCts.Cancel();
        }
    }
}
