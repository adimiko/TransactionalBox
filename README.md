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