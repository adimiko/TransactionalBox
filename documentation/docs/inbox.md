---
sidebar_position: 4
---

# Inbox

The inbox is responsible for getting messages from the transport and adding them to the storage, and then processes these messages.

:::tip
You can run inbox as a standalone service. You add reference to shared assemblies, but deploy as separate services.
This way, processing in inbox does not affect the performance of your service.
:::


```csharp
builder.Services.AddTransactionalBox(x =>
{
    x.AddInbox(
        storage => ...,
        transport => ...,
        settings => ...,
        assembly => ...
    );
},
settings => settings.ServiceId = "ServiceName");
```

### Inbox Message
:::info
Inbox message class name must be unique per service.
:::
```csharp
public class CreateCustomerCommandMessage : InboxMessage
{
    public Guid Id { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }

    public int Age { get; init; }
}
```

### Inbox Definition
:::info
You do not need to define a inbox definition for each message.   
Listens by default for incoming messages to the current service (does not listen on events).   
If you want to listen for events, you need to define the `PublishedBy` property.
:::

```csharp
public class CreatedCustomerEventMessageDefinition : InboxDefinition<CreatedCustomerEventMessage>
{
    public CreatedCustomerEventMessageDefinition() 
    {
        PublishedBy = "Customers";
    }
}
```

### Handling message in inbox handler
:::info
Each `InboxMessage` must have a `IInboxHandler` declared.
:::
```csharp
public class CreatedCustomerEventMessageHandler : IInboxHandler<CreatedCustomerEventMessage>
{
    ...
    
    public async Task Handle(CreatedCustomerEventMessage message, IExecutionContext executionContext)
    {
        // Your logic
    }
}
```

## Storage

<table>
  <tr>
    <td><b>Options</b></td>
  </tr>
  <tr>
    <td>In Memory (Default)</td>
  </tr>
    <tr>
    <td>Entity Framework Core (Relational)</td>
  </tr>
</table>

## Transport

<table>
  <tr>
    <td><b>Options</b></td>
  </tr>
  <tr>
    <td>In Memory (Default)</td>
  </tr>
    <tr>
    <td>Apache Kafka </td>
  </tr>
</table>


## Settings

### AddMessagesToInboxSettings
<table>
  <tr>
    <td><b>Name</b></td>
    <td><b>Type</b></td>
    <td><b>Default Value</b></td>
    <td><b>Description</b></td>
  </tr>
  <tr>
    <td>DefaultTimeToLiveIdempotencyKey</td>
    <td>TimeSpan</td>
    <td>7 days</td>
    <td>Default time to live idempotency key. After the time expires, the key will be removed.</td>
  </tr>
</table>

### CleanUpInboxSettings
<table>
  <tr>
    <td><b>Name</b></td>
    <td><b>Type</b></td>
    <td><b>Default Value</b></td>
    <td><b>Description</b></td>
  </tr>
  <tr>
    <td>MaxBatchSize</td>
    <td>int</td>
    <td>10000</td>
    <td>Maximum number of messages to clean up per job.</td>
  </tr>
  <tr>
    <td>IsEnabled</td>
    <td>bool</td>
    <td>true</td>
    <td>Value responsible for whether the cleaning process is enabled.</td>
  </tr>
</table>

### CleanUpIdempotencyKeysSettings
<table>
  <tr>
    <td><b>Name</b></td>
    <td><b>Type</b></td>
    <td><b>Default Value</b></td>
    <td><b>Description</b></td>
  </tr>
  <tr>
    <td>MaxBatchSize</td>
    <td>int</td>
    <td>10000</td>
    <td>Maximum number of idepotency keys to clean up per job.</td>
  </tr>
  <tr>
    <td>Period</td>
    <td>TimeSpan</td>
    <td>1 hour</td>
    <td>Time interval between cleaning idepotency keys.</td>
  </tr>
</table>

### ConfigureDeserialization

<table>
  <tr>
    <td><b>Name</b></td>
    <td><b>Extension</b></td>
  </tr>
  <tr>
    <td>System.Text.Json (Default)</td>
    <td>UseSystemTextJson()</td>
  </tr>
</table>