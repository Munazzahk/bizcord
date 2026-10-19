# Analysis — Bizcord Presence Service

## 1. Project overview

Bizcord is a backend platform for real-time communication. It consists of multiple microservices, each responsible for a specific business domain.

This project focuses on the Presence Service.

## 2. Domain responsibilities

The Presence Service tracks when a user was last active.

Its responsibilities are:
- Store a user's latest activity timestamp.
- Update activity when a user posts a message.
- Provide an API for retrieving presence information.

The service does not manage messages, channels, or user profile information.

## 3. Service boundaries

- **Presence Service:** Owns presence information and last-active timestamps.
- **Messaging Service:** Owns messages and publishes message-related events.
- **Channel Management Service:** Owns channels and channel membership.
- **User Profiles Service:** Owns user profile information.

These are proposed service boundaries for this project.

## 4. Data ownership

The Presence Service owns its presence data and is responsible for updating it.

Other services must not access the Presence Service's database directly. They communicate through its API or messages.

## 5. Service interactions

When a message is posted, the Messaging Service publishes a `MessagePostedEvent`.

The Presence Service consumes this event and updates the last-active timestamp for the message author.

RabbitMQ is used for asynchronous messaging.

## 6. Assumptions

- Each user has a unique identifier.
- A message event identifies its author and when the message was posted.
- Events are shared through a common contract.
- The service should tolerate receiving the same event more than once.

These assumptions must be checked against the course's shared contracts and workshop material.