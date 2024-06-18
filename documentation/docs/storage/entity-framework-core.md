---
sidebar_position: 2
---

# Entity Framework Core (Relational)

TransactionalBox is not responsible for configuring Entity Framework Core.
All it needs is to use the already existing `DbContext` and add the model to the `ModelBuilder`.

Transactional box add messages to your `DbContext`.
You need to invoke `SaveChangesAsync()` on `DbContext`.

:::warning
Remember, all changes need to be in one transaction.
Using `ExecuteUpdate` or `ExecuteDelete` you need to create own transaction.
:::

:::tip

Use `AddDbContexPoll` when configuring Entity Framework Core.
```csharp
builder.Services.AddDbContextPool<SampleDbContext>(x => x.Use...(connectionString));
```
:::

## Install package
```csharp
dotnet add package TransactionalBox.EntityFrameworkCore
```

## Outbox
### Register Entity Framework Core as outbox storage
```csharp
builder.Services.AddTransactionalBox(x =>
{
    x.AddOutbox(storage => storage.UseEntityFrameworkCore<SampleDbContext>());
});

```

### Add Outbox to ModelBuilder
```csharp
public class SampleDbContext : DbContext
{
    ...
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AddOutbox();
    }
    ...
}
```

## Inbox
### Register Entity Framework Core as inbox storage
```csharp
builder.Services.AddTransactionalBox(x =>
{
    x.AddInbox(storage => storage.UseEntityFrameworkCore<SampleDbContext>());
});

```

### Add Inbox to ModelBuilder
```csharp
public class SampleDbContext : DbContext
{
    ...
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AddInbox();
    }
    ...
}
```