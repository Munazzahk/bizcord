# Bizcord – Presence Microservice

## Overview

The Presence Microservice tracks when a user was last active. It consumes `MessagePostedEvent` messages from RabbitMQ, updates the user's last activity timestamp, and publishes a `PresenceUpdatedEvent`.

## Architecture

- **PresenceMicroservice:** REST API and background event consumer.
- **Shared.Contracts:** Shared event definitions.
- **MessageClient:** Abstraction and RabbitMQ implementation for messaging.
- **RabbitMQ:** Message broker for asynchronous communication.
- **Docker Compose:** Runs the service and RabbitMQ locally.

## Requirements

- .NET 10 SDK
- Docker Desktop
- Git

## Run the application

From the repository root:

```powershell
docker compose up -d --build

Check the running containers:
 ```powershell
docker compose ps

Check the running containers:>>
 ```powershell
 docker compose down

API
Health check
GET http://localhost:5001/health

Returns the service health status.
Get user presence
GET http://localhost:5001/api/presence/{userId}

Returns the user's last activity timestamp if the user has been processed. Returns HTTP 404 if no activity is known for that user.
Events
MessagePostedEvent
Contains the message ID, channel ID, author ID, content, and posting timestamp.

When the service receives this event, it updates the author's last activity timestamp and publishes PresenceUpdatedEvent.
PresenceUpdatedEvent
Contains the message ID, author ID, and last activity timestamp in UTC.

The original message ID is preserved so downstream services can correlate events.
Project structure
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


Build
dotnet build Bizcord.slnx


Design notes
The service uses asynchronous messaging to process posted-message events. The message client separates RabbitMQ communication from business logic. Shared contracts provide common event definitions.

The current prototype stores presence timestamps in memory. They are lost when the service restarts; persistent storage can be added in a future iteration.
