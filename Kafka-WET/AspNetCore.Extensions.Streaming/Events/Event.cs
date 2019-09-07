using Newtonsoft.Json;
using System;

namespace AspNetCore.Extensions.Streaming.Events
{
    public abstract class Event : IEvent
    {
        protected Event(Guid traceId, Guid onBehalfOf)
        {
            Header = new Header
            {
                TraceId = traceId,
                OnBehalfOf = onBehalfOf
            };
        }

        public Header Header { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
