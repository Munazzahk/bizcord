# Bizcord – Presence Microservice

## Overview

The Presence Microservice tracks when a user was last active. It consumes `MessagePostedEvent` messages from RabbitMQ, updates the author's last-active timestamp, and publishes a `PresenceUpdatedEvent`.

## Architecture

- **PresenceMicroservice:** REST API and background event consumer.
- **Shared.Contracts:** Shared event definitions.
- **MessageClient:** Messaging abstraction and RabbitMQ implementation.
- **RabbitMQ:** Message broker for asynchronous communication.
- **Docker Compose:** Starts the service and RabbitMQ.

## Requirements

- .NET 10 SDK
- Docker Desktop
- Git

## Run the application

Run these commands from the repository root:

```powershell
docker compose up -d --build
docker compose ps
```

To stop the application:

```powershell
docker compose down
```

## API

### Health check

`GET http://localhost:5001/health`

Returns the service health status.

### Get user presence

`GET http://localhost:5001/api/presence/{userId}`

Returns the user's last activity timestamp. Returns HTTP 404 if no activity is known for that user.

## Events

### MessagePostedEvent

Contains the message ID, channel ID, author ID, content, and posting timestamp. The service uses this event to update the author's last-active timestamp.

### PresenceUpdatedEvent

Contains the message ID, author ID, and last-active timestamp in UTC. The original message ID is preserved so downstream services can correlate events.

## Project structure

```text
apps/
  presence-microservice/
    src/
    tests/
packages/
  MessageClient/
  Shared.Contracts/
docs/
  analysis.md
  design.md
docker-compose.yml
Bizcord.slnx
README.md
```

## Build and test

Build the solution:

```powershell
dotnet build Bizcord.slnx
```

Run the tests:

```powershell
dotnet test Bizcord.slnx
```

## Design notes

The service uses asynchronous messaging to process posted-message events. The shared message client separates RabbitMQ communication from business logic, while Shared.Contracts defines the event models.

**Current limitation:** Presence timestamps are stored in memory and are lost when the service restarts. Persistent storage is not implemented in this prototype.
