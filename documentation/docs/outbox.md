---
sidebar_position: 3
---

# Outbox

The outbox is responsible for adding messages to the storage  and then adding at least once to the transport.

```csharp
builder.Services.AddTransactionalBox(x =>
{
    x.AddOutbox(
        storage => ...,
        transport => ...,
        settings => ...,
        assembly => ...
    );
},
settings => settings.ServiceId = "ServiceName");
```
### Outbox Message

```csharp
public class CreateCustomerCommandMessage : OutboxMessage
{
    public Guid Id { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }

    public int Age { get; init; }
}
```

### Outbox Definition
:::info
You do not need to define a outbox definition for each message.   
By default, the message will be published because receiver is not indicated.
:::
```csharp
internal class CreateCustomerCommandMessageDefinition : OutboxDefinition<CreateCustomerCommandMessage>
{
    public CreateCustomerCommandMessageDefinition() 
    {
        Receiver = "Customers";
    }
}

```

### Adding a message to the outbox

```csharp

// Add in the same transaction the result of the business operation and message to outbox

await outbox.Add(message);

// Commit transaction

// After transaction is successfully commited, execute the following method
await outbox.TransactionCommited();

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

### Add messages to transport settings

<table>
  <tr>
    <td><b>Name</b></td>
    <td><b>Type</b></td>
    <td><b>Default Value</b></td>
  </tr>
  <tr>
    <td>MaxBatchSize</td>
    <td>int</td>
    <td>5000</td>
  </tr>
    <tr>
    <td>LockTimeout</td>
    <td>TimeSpan</td>
    <td>10 seconds</td>
  </tr>
</table>

### Clean up outbox settings
<table>
  <tr>
    <td><b>Name</b></td>
    <td><b>Type</b></td>
    <td><b>Default Value</b></td>
  </tr>
  <tr>
    <td>MaxBatchSize</td>
    <td>int</td>
    <td>10000</td>
  </tr>
    <tr>
    <td>IsEnabled</td>
    <td>bool</td>
    <td>true</td>
  </tr>
</table>

### Configure serialization

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

###  Configure compression

<table>
  <tr>
    <td><b>Name</b></td>
    <td><b>Extension</b></td>
  </tr>
  <tr>
    <td>No compression (Default)</td>
    <td>UseNoCompression()</td>
  </tr>
  <tr>
    <td>Brotli</td>
    <td>UseBrotli()</td>
  </tr>
  <tr>
    <td>GZip</td>
    <td>UseGZip()</td>
  </tr>
</table>
