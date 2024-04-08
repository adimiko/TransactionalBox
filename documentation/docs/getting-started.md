---
sidebar_position: 2
---

# Getting Started

Let's discover **TransactionalBox in less than 5 minutes**.   
Don't worry about the number of packages and if you don't understand something. I will try to introduce you step by step to the transactional box.

## Configuration
### Install packages
```csharp
dotnet add package TransactionalBox.Outbox.Storage.InMemory
dotnet add package TransactionalBox.OutboxWorker.Storage.InMemory
dotnet add package TransactionalBox.OutboxWorker.Transport.InMemory
dotnet add package TransactionalBox.InboxWorker.Transport.InMemory
dotnet add package TransactionalBox.InboxWorker.Storage.InMemory
dotnet add package TransactionalBox.Inbox.Storage.InMemory
```
### Register
```csharp
using TransactionalBox;
using TransactionalBox.Outbox;
using TransactionalBox.Outbox.Storage.InMemory;
using TransactionalBox.OutboxWorker;
using TransactionalBox.OutboxWorker.Storage.InMemory;
using TransactionalBox.OutboxWorker.Transport.InMemory;
using TransactionalBox.InboxWorker;
using TransactionalBox.InboxWorker.Transport.InMemory;
using TransactionalBox.InboxWorker.Storage.InMemory;
using TransactionalBox.Inbox;
using TransactionalBox.Inbox.Storage.InMemory;
```

```csharp
builder.Services.AddTransactionalBox(x =>
{
    x.AddOutbox(storage => storage.UseInMemory())
     .WithWorker(
        storage => storage.UseInMemory(),
        transport => transport.UseInMemory());

    x.AddInbox(storage => storage.UseInMemory())
     .WithWorker(
        storage => storage.UseInMemory(),
        transport => transport.UseInMemory());
},
settings => settings.ServiceId = "ServiceName");
```
## Usage
### Outbox
#### Declare outbox message
```csharp
public sealed class ExampleMessage : IOutboxMessage
{
    public string Name { get; init; }

    public int Age { get; init; }
}
```
#### Add message to outbox (to send)

```csharp
public sealed class AddMessageToOutbox
{
    private readonly IOutbox _outbox;

    public AddMessageToOutbox(IOutbox outbox) 
    {
        _outbox = outbox;
    }

    public async Task Example()
    {
        var message = new ExampleMessage()
        {
            Name = "Name",
            Age = 25,
        };

        await _outbox.Add(message, envelope => envelope.Receiver = "ServiceName");
    }
}
```

### Inbox
#### Declare inbox message
```csharp
public sealed class ExampleMessage : IInboxMessage
{
    public string Name { get; init; }

    public int Age { get; init; }
}
```
#### Handle message

```csharp
internal sealed class ExampleMessageHandler : IInboxMessageHandler<ExampleMessage>
{
    public async Task Handle(ExampleMessage message, IExecutionContext executionContext)
    {
        // Your Logic
    }
}
```
## Summary
Follow the workflow of your code with breakpoints. Enjoy learning :stuck_out_tongue_winking_eye:.