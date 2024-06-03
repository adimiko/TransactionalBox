---
sidebar_position: 2
---

# Getting Started

Let's discover **TransactionalBox in less than 3 minutes**.
Example shows in-memory implementation.

## Configuration
### Install packages
```csharp
dotnet add package TransactionalBox
```
### Register
```csharp
using TransactionalBox;
```

```csharp
builder.Services.AddTransactionalBox(x =>
{
    x.AddOutbox();

    x.AddInbox();
},
settings => settings.ServiceId = "ServiceName");
```
## Usage
### Outbox
#### Declare outbox message
```csharp
public sealed class ExampleMessage : OutboxMessage
{
    public string Name { get; init; }

    public int Age { get; init; }
}
```
#### Add message to outbox

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

        await _outbox.Add(message);
        await _outbox.TransactionCommited();
    }
}
```

### Inbox
#### Declare inbox message
```csharp
public sealed class ExampleMessage : InboxMessage
{
    public string Name { get; init; }

    public int Age { get; init; }
}
```
#### Handle message

```csharp
internal sealed class ExampleMessageHandler : IInboxHandler<ExampleMessage>
{
    public async Task Handle(ExampleMessage message, IExecutionContext executionContext)
    {
        // Your Logic
    }
}
```
## Summary
Follow the workflow of your code with breakpoints. Enjoy learning :stuck_out_tongue_winking_eye:.