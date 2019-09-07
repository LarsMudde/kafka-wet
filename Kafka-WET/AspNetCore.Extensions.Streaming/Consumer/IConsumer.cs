using AspNetCore.Extensions.Streaming.Events;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AspNetCore.Extensions.Streaming.Consumer
{
    public interface IConsumer<out TEvent> where TEvent : IEvent
    {
        void Listen(Action<TEvent> message, CancellationToken stoppingToken);
        List<(string, int, long)> GetCurrentOffset(DateTime? since = null);
        void SetOffset(int partition, long offset);
    }
}