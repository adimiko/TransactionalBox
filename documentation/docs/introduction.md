---
sidebar_position: 1
---

# Introduction

Transactional box is an implementation of the outbox and inbox pattern in .NET.
Ensures eventual consistency when modules need to communicate with each other over the network.

Examples of problems that occur during network communication:

- lost messages
- the same messages were processed again
- unavailable services

## :european_castle: Architecture
The transactional box consists of four basic components. The following diagrams show the basic flow (omits many details). They are designed to provide a general understanding of how transactional box works.

### Outbox
Outbox is responsible for adding messages to the storage.

<div align="center">
![Outbox](assets/diagram-outbox.png)
</div>
### Outbox Worker
Outbox worker is responsible for getting the messages from storage and adding them to the transport.

<div align="center">
![Outbox Worker](assets/diagram-outbox-worker.png)
</div>

### Inbox Worker
Inbox worker is responsible for getting messages from transport and adding them to the storage.

<div align="center">
![Inbox Worker](assets/diagram-inbox-worker.png)
</div>

### Inbox 
Inbox is responsible for processing messages from the storage.

<div align="center">
![Inbox](assets/diagram-inbox.png)
</div>