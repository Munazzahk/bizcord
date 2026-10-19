using System.Collections.Concurrent;
using MessageClient;
using Shared.Contracts.Events;

namespace PresenceMicroservice.Handlers;

public sealed class PresenceMessageHandler : IMessageHandler<MessagePostedEvent>
{
    private readonly IMessageClient _messageClient;

    public static ConcurrentDictionary<Guid, DateTime> LastActive { get; } = new();

    public PresenceMessageHandler(IMessageClient messageClient)
    {
        _messageClient = messageClient;
    }

    public async Task Handle(
        MessagePostedEvent message,
        CancellationToken cancellationToken = default)
    {
        var lastActiveAtUtc = message.PostedAt.Kind == DateTimeKind.Utc
            ? message.PostedAt
            : message.PostedAt.ToUniversalTime();

        LastActive.AddOrUpdate(
            message.AuthorId,
            lastActiveAtUtc,
            (_, current) => lastActiveAtUtc > current ? lastActiveAtUtc : current);

        var result = new PresenceUpdatedEvent
        {
            MessageId = message.MessageId,
            AuthorId = message.AuthorId,
            LastActiveAtUtc = lastActiveAtUtc
        };

        await _messageClient.PublishAsync(result, cancellationToken);
    }
}
