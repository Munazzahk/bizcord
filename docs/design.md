# Design — Bizcord Presence Service

## 1. Architecture

The Presence Service is an independent .NET Web API application.

It communicates with other services through REST and RabbitMQ. Its data is owned by the Presence Service.

## 2. Domain model

### UserPresence

- `UserId`: Unique identifier for the user.
- `LastActiveAtUtc`: Timestamp of the user's latest recorded activity.

The model represents presence information only. It does not contain the user's profile or message content.

## 3. Messaging

The service consumes the shared `MessagePostedEvent` contract.

When an event is received, the handler updates the corresponding user's `LastActiveAtUtc` value using the event timestamp.

The handler should be designed to handle duplicate event delivery safely.

## 4. REST API

The proposed API includes:

- `GET /api/presence/{userId}` — retrieve a user's presence information.
- `PUT /api/presence/{userId}` — update presence information if required by the final implementation.

The API should return appropriate HTTP status codes and validate incoming data.

## 5. Infrastructure

- .NET Web API for the service.
- RabbitMQ for asynchronous messaging.
- EasyNetQ through a shared MessageClient package.
- Docker and Docker Compose for running the service and RabbitMQ.

## 6. Testing

The implementation should include tests for:
- Updating a user's last-active timestamp.
- Handling the shared message contract.
- Message consumption using RabbitMQ.
- End-to-end behaviour across the relevant services, as required by the W40 task.

## 7. Implementation notes

The final API, event properties, persistence approach, and test setup must match the course examples and requirements. Unnecessary infrastructure should be avoided.dotnet sln Bizcord.slnx add packages/Shared.Contracts/Shared.Contracts.csproj