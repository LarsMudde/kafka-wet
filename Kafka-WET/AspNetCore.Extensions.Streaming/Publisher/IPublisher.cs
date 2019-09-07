using System.Threading;
using System.Threading.Tasks;

namespace AspNetCore.Extensions.Streaming.Publisher
{
    public interface IPublisher<in TEvent>
    {
        Task PublishAsync(TEvent message, CancellationToken cancellationToken = default);
    }
}