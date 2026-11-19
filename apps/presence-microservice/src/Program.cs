using MessageClient;
using PresenceMicroservice; //Code to the actual service
using PresenceMicroservice.Handlers; //Code that handles the message posted event

var builder = WebApplication.CreateBuilder(args);

var rabbitMqConnection =
    builder.Configuration["RabbitMQ:ConnectionString"] ?? "host=localhost";

// Register the RabbitMqMessageClient - dependency injection
builder.Services.AddSingleton<IMessageClient>(
    _ => new RabbitMqMessageClient(rabbitMqConnection));

builder.Services.AddSingleton<PresenceMessageHandler>();
builder.Services.AddHostedService<MessagePostedConsumer>();

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

app.MapGet("/api/presence/{userId:guid}", (Guid userId) =>
{
    if (PresenceMessageHandler.LastActive.TryGetValue(userId, out var lastActive))
    {
        return Results.Ok(new
        {
            UserId = userId,
            LastActiveAtUtc = lastActive
        });
    }

    return Results.NotFound();
});

app.Run();
