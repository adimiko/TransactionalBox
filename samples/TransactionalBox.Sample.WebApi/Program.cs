using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;
using Testcontainers.Kafka;
using Testcontainers.PostgreSql;
using TransactionalBox;
using TransactionalBox.Inbox;
using TransactionalBox.Inbox.Storage.EntityFramework;
using TransactionalBox.Inbox.Storage.InMemory;
using TransactionalBox.Base.Inbox.StorageModel.Internals;
using TransactionalBox.InboxWorker;
using TransactionalBox.InboxWorker.Storage.EntityFramework;
using TransactionalBox.InboxWorker.Decompression.GZip;
using TransactionalBox.InboxWorker.Transport.Kafka;
using TransactionalBox.Outbox;
using TransactionalBox.Outbox.Storage.EntityFramework;
using TransactionalBox.Outbox.Storage.InMemory;
using TransactionalBox.Base.Outbox.StorageModel.Internals;
using TransactionalBox.OutboxWorker;
using TransactionalBox.OutboxWorker.Storage.EntityFramework;
using TransactionalBox.OutboxWorker.Storage.InMemory;
using TransactionalBox.OutboxWorker.Compression.Brotli;
using TransactionalBox.OutboxWorker.Compression.GZip;
using TransactionalBox.InboxWorker.Decompression.Brotli;
using TransactionalBox.OutboxWorker.Transport.Kafka;
using TransactionalBox.Sample.WebApi;
using TransactionalBox.OutboxWorker.Transport.InMemory;
using TransactionalBox.InboxWorker.Transport.InMemory;
using TransactionalBox.Base.Outbox.Storage.InMemory;
using TransactionalBox.InboxWorker.Storage.InMemory;
using TransactionalBox.Base.Inbox.Storage.InMemory;


var postgreSqlContainer = new PostgreSqlBuilder()
  .WithImage("postgres:15.1")
  .Build();

var kafkaContainer = new KafkaBuilder()
  .WithImage("confluentinc/cp-kafka:6.2.10")
  .Build();

var postgresStartTask = postgreSqlContainer.StartAsync();
var kafkaStartTask = kafkaContainer.StartAsync();

await Task.WhenAll(postgresStartTask, kafkaStartTask);

var connectionString = postgreSqlContainer.GetConnectionString();
var bootstrapServers = kafkaContainer.GetBootstrapAddress();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextPool<SampleDbContext>(x => x.UseNpgsql(connectionString));

builder.Services.AddTransactionalBox(
x =>
{
    x.AddOutbox(storage => storage.UseEntityFramework<SampleDbContext>())
    //x.AddOutbox(storage => storage.UseInMemory())
     .WithWorker(
        storage => storage.UseEntityFramework(), 
        //storage => storage.UseInMemory(), 
        transport => transport.UseKafka(settings =>
        {
            settings.BootstrapServers = bootstrapServers;
            settings.TransportMessageSizeSettings.OptimalTransportMessageSize = 1000;
         }),
        //transport => transport.UseInMemory(),
        settings =>
     {
         settings.AddMessagesToTransportSettings.LockTimeout = TimeSpan.FromSeconds(1);
         settings.AddMessagesToTransportSettings.NumberOfInstances = 1;
         settings.CleanUpProcessedOutboxMessagesSettings.NumberOfInstances = 0;
         settings.ConfigureCompressionAlgorithm = x => x.UseBrotliCompression(x => x.CompressionLevel = CompressionLevel.Fastest);
     });

    x.AddInbox(storage => storage.UseEntityFramework<SampleDbContext>(), settings =>
    //x.AddInbox(storage => storage.UseInMemory(), settings =>
    {
        settings.NumberOfInstances = 4;
    })
     .WithWorker(
        storage => storage.UseEntityFramework(),
        //storage => storage.UseInMemory(),
        transport => transport.UseKafka(settings => settings.BootstrapServers = bootstrapServers),
        //transport => transport.UseInMemory(),
        settings =>
     {
         settings.CleanUpProcessedInboxMessagesSettings.NumberOfInstances = 0;
         settings.AddMessagesToInboxStorageSettings.NumberOfInstances = 1;
         settings.ConfigureDecompressionAlgorithm = x => x.UseBrotliDecompression();
     });
},
settings => settings.ServiceId = "Registrations");

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{

    scope.ServiceProvider.GetRequiredService<SampleDbContext>().Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapPost("/add-message-to-outbox", async ([FromBody] ExampleMessage message, IOutbox outbox, DbContext dbContext) =>
{
    var messages = new List<ExampleMessage>();

    for (var i = 0; i < 100; i++)
    {
        messages.Add(message);
    }

    await outbox.AddRange(messages, m =>
    {
        m.Receiver = "Registrations";
    });

    await dbContext.SaveChangesAsync();
});

app.MapGet("/get-messages-from-outbox", async (DbContext dbContext) =>
{
    var messages = await dbContext.Set<OutboxMessage>().AsNoTracking().ToListAsync();

    return messages;
});

app.MapGet("/get-messages-from-inbox", async (DbContext dbContext) =>
{
    var messages = await dbContext.Set<InboxMessage>().AsNoTracking().ToListAsync();

    return messages;
});

app.MapGet("/get-idempotent-messages-from-inbox", async (DbContext dbContext) =>
{
    var messages = await dbContext.Set<IdempotentInboxKey>().AsNoTracking().ToListAsync();

    return messages;
});

app.MapGet("/outbox-distributed-locks", async (DbContext dbContext) =>
{
    var locks = await dbContext.Set<OutboxDistributedLock>().AsNoTracking().ToListAsync();

    return locks;
});

app.MapGet("/inbox-distributed-locks", async (DbContext dbContext) =>
{
    var locks = await dbContext.Set<InboxDistributedLock>().AsNoTracking().ToListAsync();

    return locks;
});

app.Run();
