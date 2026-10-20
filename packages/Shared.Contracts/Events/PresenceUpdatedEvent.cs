namespace Shared.Contracts.Events;

public class PresenceUpdatedEvent
{
    public Guid MessageId { get; set; }
    public Guid AuthorId { get; set; }
    public DateTime LastActiveAtUtc { get; set; }
}
