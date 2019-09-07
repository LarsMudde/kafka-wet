using System;

namespace AspNetCore.Extensions.Streaming.Events
{
    public struct Header
    {
        public Guid TraceId { get; set; }
        public Guid OnBehalfOf { get; set; }
    }
}