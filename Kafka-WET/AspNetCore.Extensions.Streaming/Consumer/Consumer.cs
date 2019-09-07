using AspNetCore.Extensions.Streaming.Configuration;
using AspNetCore.Extensions.Streaming.Events;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AspNetCore.Extensions.Streaming.Consumer
{
    public sealed class Consumer<TEvent> : IConsumer<TEvent>, IDisposable where TEvent : IEvent
    {
        private readonly ILogger<Consumer<TEvent>> _logger;
        private readonly KafkaConfig _config;

        private const long PollingIntervalInMilliseconds = 1000 * 60;
        private static readonly string Topic = $"topic-{typeof(TEvent).Name.ToLower()}";

        private Consumer<Null, string> _consumer;

        public Consumer(ILogger<Consumer<TEvent>> logger, IOptions<KafkaConfig> kafkaOptions)
        {
            _logger = logger;
            _config = kafkaOptions.Value ?? new KafkaConfig();
        }

        public void Listen(Action<TEvent> message, CancellationToken stoppingToken)
        {
            _logger.LogDebug($"{nameof(Consumer<TEvent>)}/{nameof(Listen)} called.");

            stoppingToken.ThrowIfCancellationRequested();
            stoppingToken.Register(StopPolling);

            _consumer = new Consumer<Null, string>(_config.GetConsumerConfig(), null, new StringDeserializer(Encoding.UTF8));

            _consumer.OnMessage += (_, msg) =>
            {
                _logger.LogInformation($"Read '{msg.Value}' from: {msg.TopicPartitionOffset}");
                message.Invoke(JsonConvert.DeserializeObject<TEvent>(msg.Value));
            };

            _consumer.OnError += (_, error) =>
            {
                _logger.LogError($"Error: {error}");
            };

            _consumer.OnConsumeError += (_, msg) =>
            {
                _logger.LogWarning($"Consume error ({msg.TopicPartitionOffset}): {msg.Error}");
            };

            _consumer.Subscribe(Topic);

            while (!stoppingToken.IsCancellationRequested)
            {
                _consumer.Poll(TimeSpan.FromMilliseconds(PollingIntervalInMilliseconds));
            }
        }

        public List<(string, int, long)> GetCurrentOffset(DateTime? since = null)
        {
            return _consumer
                .OffsetsForTimes(
                    _consumer.Assignment
                        .Select(topicPartition =>
                            new TopicPartitionTimestamp(
                                topicPartition,
                                new Timestamp(
                                    since.GetValueOrDefault(DateTime.MinValue),
                                    TimestampType.NotAvailable)))
                    , TimeSpan.FromSeconds(60))
                .Select(tpoe => (tpoe.Topic, tpoe.Partition, tpoe.Offset.Value))
                .ToList();
        }

        public void SetOffset(int partition, long offset)
        {
            _consumer.Assign(new List<TopicPartitionOffset>
            {
                new TopicPartitionOffset(
                    Topic,
                    partition,
                    new Offset(offset))
            });
        }

        public void StopPolling()
        {
            _consumer.Dispose();
            _consumer = null;
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _consumer?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Consumer()
        {
            Dispose(false);
        }
    }
}
