---
sidebar_position: 1
---

# Apache Kafka

## Configuration
### Install package
```csharp
dotnet add package TransactionalBox.Outbox.Kafka
```

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