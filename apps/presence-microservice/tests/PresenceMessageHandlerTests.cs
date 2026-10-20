using MessageClient;
using PresenceMicroservice.Handlers;
using Shared.Contracts.Events;

namespace PresenceMicroservice.Tests;

public class PresenceMessageHandlerTests
{
    [Fact]
    public async Task Handle_PublishesEventWithSameMessageIdAndAuthorId()
    {
        var messageClient = new FakeMessageClient();
        var handler = new PresenceMessageHandler(messageClient);

        var messageId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        var postedAt = DateTime.UtcNow;

        var message = new MessagePostedEvent
        {
            MessageId = messageId,
            ChannelId = Guid.NewGuid(),
            AuthorId = authorId,
            Content = "Test message",
            PostedAt = postedAt
        };

        await handler.Handle(message);

        var result = Assert.IsType<PresenceUpdatedEvent>(
            messageClient.LastPublished);

        Assert.Equal(messageId, result.MessageId);
        Assert.Equal(authorId, result.AuthorId);
        Assert.Equal(postedAt, result.LastActiveAtUtc);
    }

    private sealed class FakeMessageClient : IMessageClient
    {
        public object? LastPublished { get; private set; }

        public Task PublishAsync<T>(
            T message,
            CancellationToken cancellationToken = default)
        {
            LastPublished = message;
            return Task.CompletedTask;
        }

        public Task SubscribeAsync<T>(
            string subscription,
            Func<T, Task> handler,
            CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
