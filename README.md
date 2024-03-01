# TransactionalBox

<div align="center">
    <img src="assets/logo.png" width="500">
</div>

TransactionalBox is an implementation of the outbox and inbox pattern in .NET.   
Ensures eventual consistency when modules need to communicate with each other over the network.

Examples of problems that occur during network communication:
- lost messages
- the same message was processed again
- unavailable component