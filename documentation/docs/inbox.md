---
sidebar_position: 4
---

# Inbox

The inbox is responsible for getting messages from the transport and adding them to the storage, and then processes these messages.



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

### Add messages to inbox settings
<table>
  <tr>
    <td><b>Name</b></td>
    <td><b>Type</b></td>
    <td><b>Default Value</b></td>
  </tr>
  <tr>
    <td>DefaultTimeToLiveIdempotencyKey</td>
    <td>TimeSpan</td>
    <td>7 days</td>
  </tr>
</table>

### Clean up inbox settings
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

### Clean up idempotency keys settings
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
    <td>Period</td>
    <td>TimeSpan</td>
    <td>1 hour</td>
  </tr>
</table>

### Configure deserialization

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