using AspNetCore.Extensions.Streaming.Configuration;
using AspNetCore.Extensions.Streaming.Events;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCore.Extensions.Streaming.Publisher
{
    public sealed class Publisher<TEvent> : IPublisher<TEvent> where TEvent : IEvent
    {
        private readonly ILogger<Publisher<TEvent>> _logger;
        private readonly KafkaConfig _config;

        public Publisher(ILogger<Publisher<TEvent>> logger, IOptions<KafkaConfig> kafkaOptions)
        {
            _logger = logger;
            _config = kafkaOptions.Value ?? new KafkaConfig();
        }

        public async Task PublishAsync(TEvent message, CancellationToken cancellationToken = default)
        {
            using (var producer = new Producer<Null, string>(_config.GetProducerConfig(), null, new StringSerializer(Encoding.UTF8)))
            {
                producer.OnError += (_, error)
                    => _logger.LogError($"Error: {error}");

                var dr = await producer.ProduceAsync($"topic-{typeof(TEvent).Name.ToLower()}", null, JsonConvert.SerializeObject(message));
                _logger.LogInformation($"Delivered '{dr.Value}' to: {dr.TopicPartitionOffset}");
            }
        }
    }
}
