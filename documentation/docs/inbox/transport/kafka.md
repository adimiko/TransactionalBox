---
sidebar_position: 1
---

# Apache Kafka

## Configuration
### Install package
```csharp
dotnet add package TransactionalBox.Kafka
```

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