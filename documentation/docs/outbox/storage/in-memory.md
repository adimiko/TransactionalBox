---
sidebar_position: 1
---

# InMemory
:::info
InMemory is registered as default storage.
:::
:::warning
Intended only for testing and learning.
:::

### Register
```csharp
builder.Services.AddTransactionalBox(x =>
{
    x.AddOutbox();
});

```