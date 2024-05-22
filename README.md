<div align="center">
    <img src="assets/rounded-social-logo.png">
</div>

<div align="center">

[![🚧 - Under Development](https://img.shields.io/badge/🚧-Under_Development-orange)](https://)
![Licence - MIT](https://img.shields.io/badge/Licence-MIT-2ea44f)
[![Nugets](https://img.shields.io/badge/Nugets-2ea44f?logo=nuget)](https://www.nuget.org/packages?q=TransactionalBox)
[![Linkedin](https://img.shields.io/badge/Linkedin-2ea44f?logo=linkedin)](https://www.linkedin.com/in/adimiko/)

</div>

###### :star: - The star motivates me a lot!   

**Transactional box is an implementation of the outbox and inbox pattern in .NET.**   
**Ensures reliable network communication (eventual consistency) between services.**

All complexity is taken over by the transactional box and simplifies communication between services to the maximum extent possible.   
It is designed for a low entry threshold and quick learning.

Examples of problems that occur during network communication:
-  **Lost message**

*Amount was taken from bank account and transfer was never executed.*

- **The same message were processed again**

*Transfer was ordered and amount was debited from bank account twice.*

- **Unavailable service**

*Transfer order attempt fails.*

For more information, see the [documentation:book:](https://transactionalbox.com/).  
Packages are hosted by [nuget.org](https://www.nuget.org/packages?q=TransactionalBox).  

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
- [ ] Get messages from inbox and process them

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

## :european_castle: Architecture
The transactional box consists of four basic components.
The following diagrams show the basic flow (omits details).

### Outbox
Outbox is responsible for adding messages to the storage.
<div align="center">
    <img src="assets/diagrams/diagram-outbox.png">
</div>

### Outbox Worker
Outbox worker is responsible for getting the messages from storage and adding them to the transport.
<div align="center">
    <img src="assets/diagrams/diagram-outbox-worker.png">
</div>

### Inbox Worker
Inbox worker is responsible for getting messages from transport and adding them to the storage.
<div align="center">
    <img src="assets/diagrams/diagram-inbox-worker.png">
</div>

### Inbox 
Inbox is responsible for processing messages from the storage.
<div align="center">
    <img src="assets/diagrams/diagram-inbox.png">
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

## :medal_sports: Competition '100commitow'
The project is part of the competition [100 commitow](https://100commitow.pl).

### Topics
- Distributed lock (prevent race condition)
- Hook processing
- Concurrency control
- Scaling and parallel processing (distributed processing)
- Synchronization primitives
- Idempotency
- Retry Pattern
