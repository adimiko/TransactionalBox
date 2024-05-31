---
sidebar_position: 1
---

# General

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
