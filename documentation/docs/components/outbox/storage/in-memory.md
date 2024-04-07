---
sidebar_position: 2
---

# InMemory
:::warning
Intended only for testing and learning.
:::
## Configuration
### Install
```csharp
dotnet add package TransactionalBox.Outbox.Transport.InMemory
```

### Register
```csharp
builder.Services.AddTransactionalBox(x =>
{
    x.AddOutbox(storage => storage.UseInMemory());
});

```