namespace MessageClient;

public interface IMessageClient
{
    Task PublishAsync<T>(
        T message,
        CancellationToken cancellationToken = default);

    Task SubscribeAsync<T>(
        string subscription,
        Func<T, Task> handler,
        CancellationToken cancellationToken = default);
}
