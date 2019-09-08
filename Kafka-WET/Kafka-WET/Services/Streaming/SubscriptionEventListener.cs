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
    public class SubscriptionEventListener : IHostedService, IDisposable
    {

        private readonly ILogger<SubscriptionEventListener> _logger;
        private Task _executingTask;
        private readonly CancellationTokenSource _stoppingCts;
        private readonly IConsumer<SubscriptionEvent> _consumer;
        private readonly IServiceProvider _serviceProvider;

        public SubscriptionEventListener(ILogger<SubscriptionEventListener> logger, IConsumer<SubscriptionEvent> consumer, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _consumer = consumer;
            _serviceProvider = serviceProvider;
            _stoppingCts = new CancellationTokenSource();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Background Service SubscriptionEventListener is starting.");

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

        private async Task HandleMessageAsync(SubscriptionEvent message, CancellationToken cancellation)
        {
            using (var scope = _serviceProvider.CreateScope())
            {

                // possible room for expansion?
                // var subscriptionService = scope.ServiceProvider.GetRequiredService<ISubscriptionService>();
                // var test = await testService.GetTestAsync(message.subscription.Id, cancellation);

                // Change color to make msg stand out
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Black;

                // Print the received message 
                Console.WriteLine($"Message with traceid: {message.Header.TraceId} bevat subscription met naam: {message.subscription.Name}");

                // Change back color to default
                Console.ResetColor();

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
