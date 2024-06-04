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

## Outbox
### Register
```csharp
builder.Services.AddTransactionalBox(x =>
{
    x.AddOutbox();
});

```

## Inbox
### Register
```csharp
builder.Services.AddTransactionalBox(x =>
{
    x.AddInbox();
});

```