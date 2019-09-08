using AspNetCore.Extensions.Streaming.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kafka_WET.Domain.Events
{
    public class SubscriptionEvent : Event
    {
        public SubscriptionEvent(Guid traceId, Guid onBehalfOf) : base(traceId, onBehalfOf)
        {
        }

        public Subscription subscription { get; set; }
     }
}

