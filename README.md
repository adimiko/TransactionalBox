# TransactionalBox

<div align="center">
    <img src="assets/logo.png" width="500">
</div>

TransactionalBox is an implementation of the outbox and inbox pattern in .NET.   
Ensures eventual consistency when modules need to communicate with each other over the network.

Examples of problems that occur during network communication:
- lost messages
- the same messages were processed again
- unavailable services

## Features
#### Actions
- [x] Add a message to send to the outbox
- [ ] Add a message to publish to the outbox
- [x] Get messages from outbox and add them to transport
- [x] Get a message from transport and add them to the inbox
- [x] Get a message from inbox and process it

#### Storage
- [x] Support for Entity Framework
    - [x] Support for EF Migrations
    - [x] Support for EF Database Providers
- [ ] Support for InMemory
- [ ] Support for PostgreSQL
- [ ] Support for Microsoft SQL Server
- [ ] Support for MySQL
- [ ] Support for MongoDB

#### Transport
- [x] Support for Apache Kafka
- [ ] Support for InMemory
- [ ] Supprot for HTTP
- [ ] Support for RabbitMQ
- [ ] Support for Iggy
- [ ] Supprot for gRPC

#### Scalability & Fault Tolerance
- [ ] Support for multiple outbox worker instances
- [ ] Support for multiple inbox worker instances
- [ ] Support for multiple inbox instances
- [ ] Standalone outbox worker
- [ ] Standalone inbox worker

#### Maintenecne
- [ ] Remove processed messages from the outbox
- [ ] Remove processed messages from the inbox
- [ ] Archive processed messages from the outbox
- [ ] Archive processed messages from the inbox
- [ ] Correlation ID

#### Other
- [x] Modular package architecture
- [x] Support for TimeProvider
- [x] Unordered messages
- [ ] Message streaming
- [ ] Package configuration using appsetings.json
- [ ] Idempotent messages


## Architecture
The transactional box consists of four basic components.
The following diagrams show the basic flow (omits details).

### Outbox
Outbox is responsible for adding messages to the storage.
<div align="center">
    <img src="assets/diagrams/outbox.svg">
</div>

### Outbox Worker
Outbox worker is responsible for getting the messages from storage and adding them to the transport.
<div align="center">
    <img src="assets/diagrams/outbox-worker.svg">
</div>

### Inbox Worker
Inbox worker is responsible for getting messages from transport and adding them to the storage.
<div align="center">
    <img src="assets/diagrams/inbox-worker.svg">
</div>

### Inbox 
Inbox is responsible for processing messages from the storage.
<div align="center">
    <img src="assets/diagrams/inbox.svg">
</div>