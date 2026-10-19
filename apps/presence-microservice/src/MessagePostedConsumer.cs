using MessageClient;
using PresenceMicroservice.Handlers;
using Shared.Contracts.Events;

namespace PresenceMicroservice;

public sealed class MessagePostedConsumer : BackgroundService
{
    private readonly IMessageClient _messageClient;
    private readonly PresenceMessageHandler _handler;

    public MessagePostedConsumer(
        IMessageClient messageClient,
        PresenceMessageHandler handler)
    {
        _messageClient = messageClient;
        _handler = handler;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _messageClient.SubscribeAsync<MessagePostedEvent>(
            "presence-service",
            message => _handler.Handle(message, stoppingToken),
            stoppingToken);

        try
        {
            await Task.Delay(Timeout.InfiniteTimeSpan, stoppingToken);
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
        }
    }
}
