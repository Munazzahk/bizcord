# Design — Bizcord Presence Service

## 1. Architecture

The Presence Service is a .NET Web API application. It exposes a REST endpoint and runs a background consumer for RabbitMQ events.

The solution is organised as a monorepo with separate projects for the service, shared contracts, and messaging client.

## 2. Domain model

The service tracks the following presence information:

- `UserId`: Unique identifier of the user.
- `LastActiveAtUtc`: Timestamp of the user's latest recorded activity in UTC.

The current implementation stores these values in an in-memory dictionary.

## 3. Shared event contracts

### MessagePostedEvent

The incoming event contains:

- `MessageId`
- `ChannelId`
- `AuthorId`
- `Content`
- `PostedAt`

### PresenceUpdatedEvent

The outgoing event contains:

- `MessageId`
- `AuthorId`
- `LastActiveAtUtc`

The original `MessageId` is preserved for correlation.

## 4. Messaging design

The `MessageClient` package defines `IMessageClient` and its RabbitMQ implementation using EasyNetQ.

The `MessagePostedConsumer` subscribes to `MessagePostedEvent` messages. `PresenceMessageHandler` updates the author's timestamp and publishes `PresenceUpdatedEvent`.

This separates messaging infrastructure from the handler's business logic.

## 5. REST API

### Health check

`GET /health`

Returns HTTP 200 when the service is responding.

### Retrieve presence

`GET /api/presence/{userId}`

Returns the user's last-active timestamp when known, or HTTP 404 if the user has no recorded activity.

## 6. Infrastructure and deployment

- .NET 10 for the application.
- RabbitMQ for asynchronous messaging.
- EasyNetQ for message publishing and subscriptions.
- Docker for containerising the Presence Service.
- Docker Compose for running the service and RabbitMQ together.

## 7. Testing

The test project includes a unit test for the handler. It verifies that the published `PresenceUpdatedEvent` preserves the incoming message ID and author ID and uses the expected timestamp.

The solution can be built and tested with:

```powershell
dotnet build Bizcord.slnx
dotnet test Bizcord.slnx
```

## 8. Limitations and future improvements

Presence data is stored in memory, so it is not persistent across restarts. Further improvements could include persistent storage, additional error-handling tests, and an integration test using a running RabbitMQ broker.
