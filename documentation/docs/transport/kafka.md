---
sidebar_position: 1
---

# Apache Kafka

## Install package
```csharp
dotnet add package TransactionalBox.Kafka
```
## Outbox
### Register
```csharp
builder.Services.AddTransactionalBox(x =>
{
    x.AddOutbox(
        storage => ...,
        transport => transport.UseKafka(settings => settings.BootstrapServers = bootstrapServers)
    );
});

```

## Inbox
### Register
```csharp
builder.Services.AddTransactionalBox(x =>
{
    x.AddInbox(
        storage => ...,
        transport => transport.UseKafka(settings => settings.BootstrapServers = bootstrapServers)
    );
});

```