# Analysis — Bizcord Presence Service

## 1. Project overview

Bizcord is a backend platform for real-time communication. This project implements the Presence Service, which tracks the last recorded activity of each user.

## 2. Domain responsibilities

The Presence Service is responsible for:

- Tracking a user's latest activity timestamp.
- Updating activity when a user posts a message.
- Exposing an API to retrieve presence information.

It does not own messages, channels, or user profile information.

## 3. Service boundaries

The wider Bizcord domain is divided into the following proposed services:

- **Presence Service:** Owns presence information and last-active timestamps.
- **Messaging Service:** Owns messages and publishes message-related events.
- **Channel Management Service:** Owns channels and channel membership.
- **User Profiles Service:** Owns user profile information.

Only the Presence Service is implemented in this assignment.

## 4. Data ownership

The Presence Service owns its presence state. In the current prototype, this state is stored in memory rather than a persistent database.

Other services should not modify the Presence Service's state directly. Services communicate through REST endpoints or messages.

## 5. Service interactions

When a message is posted, the Messaging Service publishes a `MessagePostedEvent` through RabbitMQ.

The Presence Service consumes the event, identifies the author, and updates the author's last-active timestamp. It then publishes a `PresenceUpdatedEvent` containing the original message ID, author ID, and timestamp.

The original message ID supports correlation with the message that triggered the update.

## 6. Assumptions and limitations

- Each user has a unique identifier.
- A message event contains the author's ID and posting timestamp.
- Event definitions are shared through the Shared.Contracts package.
- RabbitMQ provides asynchronous communication.
- The handler avoids moving a user's recorded activity backwards when an older event is received.
- Presence data is currently in memory and is lost on restart.
- The proposed boundaries for the other Bizcord services are analytical boundaries, not implemented services.
