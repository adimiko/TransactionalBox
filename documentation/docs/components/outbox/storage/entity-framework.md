---
sidebar_position: 1
---

# Entity Framework

TransactionalBox is not responsible for configuring Entity Framework.
All it needs is to use the already existing DbContext and add the model to the ModelBuilder.

:::tip

Use `AddDbContexPoll` when configuring Entity Framework.
```csharp
builder.Services.AddDbContextPool<SampleDbContext>(x => x.Use...(connectionString));
```
:::

## Configuration
### Install package
```csharp
dotnet add package TransactionalBox.Outbox.EntityFramework
```

### Register entity framework as outbox storage
```csharp
builder.Services.AddTransactionalBox(x =>
{
    x.AddOutbox(storage => storage.UseEntityFramework<SampleDbContext>())
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