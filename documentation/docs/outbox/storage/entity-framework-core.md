---
sidebar_position: 2
---

# Entity Framework Core (Relational)

TransactionalBox is not responsible for configuring Entity Framework.
All it needs is to use the already existing `DbContext` and add the model to the `ModelBuilder`.

Transactional box add messages to your `DbContext`.
You need to invoke `SaveChangesAsync()` on `DbContext`.

:::warning
Remember, all changes need to be in one transaction.
Using `ExecuteUpdate` or `ExecuteDelete` you need to create own transaction.
:::

:::tip

Use `AddDbContexPoll` when configuring Entity Framework.
```csharp
builder.Services.AddDbContextPool<SampleDbContext>(x => x.Use...(connectionString));
```
:::



## Configuration
### Install package
```csharp
dotnet add package TransactionalBox.EntityFrameworkCore
```

### Register entity framework as outbox storage
```csharp
builder.Services.AddTransactionalBox(x =>
{
    x.AddOutbox(storage => storage.UseEntityFramework<SampleDbContext>());
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