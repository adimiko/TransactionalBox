<div align="center">
    <img src="assets/rounded-social-logo.png">
</div>

<h3 align="center">Outbox and Inbox pattern in .NET</h3>

  <p align="center">
    <i>Ensures reliable network communication (eventual consistency) between services.</i>
    <br />
    <br />
    <a href="https://transactionalbox.com/"><strong>Documentation</strong></a>
    |
    <a href="https://www.nuget.org/packages?q=TransactionalBox"><strong>Packages</strong></a>
  </p>


<div align="center">

[![🚧 - Under Development](https://img.shields.io/badge/🚧-Under_Development-orange)](https://)
![Licence - MIT](https://img.shields.io/badge/Licence-MIT-2ea44f)
[![Linkedin](https://img.shields.io/badge/Linkedin-2ea44f?logo=linkedin)](https://www.linkedin.com/in/adimiko/)

</div>

###### :star: - The star motivates me a lot!   
### Examples of network communication problems
-  **Lost message**

*Amount was taken from bank account and transfer was never executed.*

- **The same message was processed again**

*Transfer was ordered and amount was debited from bank account twice.*

- **Unavailable service**

*Transfer order attempt fails.*

### Benefits
- Easy to use
    - *Quick learning and low entry threshold*
    - *Configured by default to prevent overwhelm for beginners*
    - *Add message to outbox and then appears in inbox handler*
- Eventual consistency
    - *Outbox sends a message at least once*
    - *Inbox deduplicates the message and processes exactly once*
- Scalability & Fault Tolerance
    - *Retry pattern with delay*
    - *Multiple instances of the same service (distributed processing)*
- Highly configurable and extendable
    - *Components are configurable via settings*
    - *Extendable with new transport and storage providers*
- Reduce latency and increase bandwidth
    - *Hook processing (instead of interval processing)*
    - *Compression algorithms*
    - *Grouping messages by topic and type to transport message (better compression)*
    - *Adjusting optimal transport message size*


## :clapper: Run Sample
> [!NOTE]
> Docker is required.

Clone this repo and open `TransactionalBox.sln` via Visual Studio 2022. Set the `TransactionalBox.Sample.WebApi` as startup and then run. You should see the following view.

<div align="center">
    <img src="assets/samples/web-api-sample.png">
</div>

*Project is at an early stage of life cycle, if you find some bug, let me know* :telephone:.   

Have fun :smiley:!

## ✨ Features
#### Actions
- [x] Add a message to send to the outbox
- [x] Add a message to publish to the outbox
- [x] Get messages from outbox and add them to transport
- [x] Get a message from transport and add them to the inbox
- [x] Get a message from inbox and process it

#### Storage
- [x] InMemory (Default)
- [x] Entity Framework (Relational)
    - [x] Migrations
    - [X] Distributed Lock (Based on atomic write operation, Standalone Package)
- [ ] MongoDB

#### Transport
- [x] InMemory (Default)
- [x] Apache Kafka
- [ ] RabbitMQ
- [ ] Iggy

#### Scalability & Fault Tolerance
- [x] Support for multiple outbox worker instances
    - [x] Multiple instances of the same service 
    - [x] Multiple processes in the same service
- [x] Support for multiple inbox instances
    - [x] Multiple instances of the same service 
    - [x] Multiple processes in the same service
- [x] Standalone inbox
- [x] Error handling in background services
- [ ] Dead messages
- [x] Hook processing
    - [x] Outbox
    - [x] Inbox

#### Observability
- [ ] Support for OpenTelemetry
- [ ] Outbox size
- [ ] Inbox size
- [ ] Message failure rate
- [ ] Message delivery latency
- [ ] Number of duplicated messages
- [ ] Message duplication rate

#### Maintenecne
- [x] Remove processed messages from the outbox
- [x] Remove processed messages from the inbox
- [x] Remove expired idempotency keys
- [ ] Archive processed messages from the outbox
- [ ] Archive processed messages from the inbox
- [x] Correlation ID

#### Other
- [x] Modular package architecture
- [x] Support for TimeProvider
- [x] Unordered messages
- [X] Internal high-performance logging (Source Generators)
- [x] Execution context in Inbox
- [x] High-performance invoking of inbox message handlers from assemblies (Compiled Lambda Expressions)
- [x] Grouping of messages
    - [x] Group by topic outbox messages to a single transport message from batch (better compression)
    - [x] Adjusting optimal transport message size
- [X] Messages serialization and deserialization
    - [X] System.Text.Json (default)
- [x] Messages compression and decompression
    - [X] No compression (default)
    - [X] Brotli
    - [x] GZip
- [ ] Message streaming
- [ ] Package configuration using appsetings.json
- [ ] Own transport message serialier and deseralizer (with StringBuilder)
- [x] Idempotent messages
- [x] Keyed in memory lock (based on SemaphoreSlim and ConcurrentDictionary)
- [ ] Transport discriminator (one outbox many transport, tagged message)
- [x] Hook startup (checking if there are messages to be processed after startup)
- [ ] Inbox based on the header, selects the appropriate algorithm to:
    - [x] Decompression
    - [ ] Deserialization
    - [ ] Separator

## :european_castle: Architecture
The transactional box consists of two basic components.
The following diagrams show the basic flow (omits details).
They are designed to provide a general understanding of how transactional box works.

### Outbox
The outbox is responsible for adding messages to the storage and then adding at least once to the transport.
<div align="center">
    <img src="assets/diagrams/outbox.png">
</div>

### Inbox 
The inbox is responsible for getting messages from the transport and adding them to the storage, and then processes these messages.
<div align="center">
    <img src="assets/diagrams/inbox.png">
</div>

## :world_map: Roadmap
Name of **TransactionalBox** fits perfectly with the future of the project. It will be possible to use different `transactional boxes`. 

### Outbox and Inbox

`No guarantee of message order between services.`

Implementation under the competition.

*e.g. Payment service asynchronously sends notification of payment to user.*

Improvements:
- Code refactor
- More tests
- Support for more storage providers (e.g. Marten, RavenDB)
- Support for more transport providers (e.g. HTTP, gRPC, Azure Service Bus)
- Performance optimization
- Encrypted transport messages

### StreamOubox and StreamInbox

`Guarantee of message order in stream between services.`

**Idea**  
Ensuring the order of messages within a stream. Messages are sent by StreamOutbox and the order is respected in StreamInbox. Transport provider does not have to support message order.

*e.g. (CQRS Pattern) When transfer is made in the write service, the event asynchronously refreshes the account balance in the read service.*

### InternalBox

`Guarantee of message order in stream inside the service.`

**Idea**   
Ensuring the order of messages within a stream insite the service. Messages are added to storage provider and then processed. Transport provider is unnecessary.

*e.g. Asynchronous internal communication between Aggregate Roots using domain events in the same service.*

## :medal_sports: Competition 100commitow
The project is part of the competition [100 commitow](https://100commitow.pl).

### Topics
- Distributed lock (prevent race condition)
- Hook processing
- Concurrency control
- Scaling and parallel processing (distributed processing)
- Synchronization primitives
- Idepotent messages (message deduplication)
- Retry Pattern
