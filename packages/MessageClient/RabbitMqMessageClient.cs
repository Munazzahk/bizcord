using EasyNetQ;

namespace MessageClient;

public sealed class RabbitMqMessageClient : IMessageClient, IDisposable
{
    private readonly IBus _bus;

    public RabbitMqMessageClient(string connectionString)
    {
        _bus = RabbitHutch.CreateBus(connectionString);
    }

    public Task PublishAsync<T>(
        T message,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return _bus.PubSub.PublishAsync(message, cancellationToken);
    }

    public Task SubscribeAsync<T>(
        string subscription,
        Func<T, Task> handler,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return _bus.PubSub.SubscribeAsync<T>(
            subscription,
            handler,
            cancellationToken: cancellationToken);
    }

    public void Dispose()
    {
        _bus.Dispose();
    }
}
