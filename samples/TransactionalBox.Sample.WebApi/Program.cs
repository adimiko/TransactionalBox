using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;
using Testcontainers.Kafka;
using Testcontainers.PostgreSql;
using TransactionalBox;
using TransactionalBox.Inbox;
using TransactionalBox.Inbox.EntityFramework;
using TransactionalBox.InboxBase.StorageModel.Internals;
using TransactionalBox.InboxWorker;
using TransactionalBox.InboxWorker.EntityFramework;
using TransactionalBox.InboxWorker.GZipDecompression;
using TransactionalBox.InboxWorker.Kafka;
using TransactionalBox.Outbox;
using TransactionalBox.Outbox.EntityFramework;
using TransactionalBox.OutboxBase.StorageModel.Internals;
using TransactionalBox.OutboxWorker;
using TransactionalBox.OutboxWorker.EntityFramework;
using TransactionalBox.OutboxWorker.GZipCompression;
using TransactionalBox.OutboxWorker.Kafka;
using TransactionalBox.Sample.WebApi;
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
     .WithWorker(
        storage => storage.UseEntityFramework(), 
        transport => transport.UseKafka(settings => settings.BootstrapServers = bootstrapServers), 
        settings =>
     {
         settings.NumberOfOutboxProcessor = 10;
         settings.ConfigureCompressionAlgorithm = x => x.UseGZipCompression(x => x.CompressionLevel = CompressionLevel.Optimal);
     });

    x.AddInbox(storage => storage.UseEntityFramework<SampleDbContext>())
     .WithWorker(
        storage => storage.UseEntityFramework(),
        transport => transport.UseKafka(settings => settings.BootstrapServers = bootstrapServers),
        settings =>
     {
         settings.ConfigureDecompressionAlgorithm = x => x.UseGZipDecompression();
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
        m.OccurredUtc = DateTime.UtcNow;
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

app.MapGet("/locks", async (DbContext dbContext) =>
{
    var locks = await dbContext.Set<OutboxDistributedLock>().AsNoTracking().ToListAsync();

    return locks;
});

app.Run();
