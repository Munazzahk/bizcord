namespace MessageClient;

public interface IMessageHandler<T>
{
    Task Handle(T message, CancellationToken cancellationToken = default);
}
