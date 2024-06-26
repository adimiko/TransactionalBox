using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;
using Testcontainers.Kafka;
using Testcontainers.PostgreSql;
using TransactionalBox;
using TransactionalBox.Sample.WebApi;
using TransactionalBox.Sample.WebApi.OutboxMessages;
using TransactionalBox.Internals.Outbox.Storage;
using TransactionalBox.Internals.Inbox.Storage;


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
    x.AddOutbox(storage => storage.UseEntityFrameworkCore<SampleDbContext>(),
        transport => transport.UseKafka(settings =>
        {
            settings.BootstrapServers = bootstrapServers;
        }),
        settings =>
     {
         settings.AddMessagesToTransportSettings.LockTimeout = TimeSpan.FromSeconds(1);
         settings.ConfigureCompression = x => x.UseBrotli(x => x.CompressionLevel = CompressionLevel.Fastest);
     });

    x.AddInbox(
        storage => storage.UseEntityFrameworkCore<SampleDbContext>(),
        transport => transport.UseKafka(settings => settings.BootstrapServers = bootstrapServers),
        settings => {},
        assembly => assembly.RegisterFromAssemblies(typeof(ExampleMessage).Assembly));
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



app.MapPost("/send-messages-outbox", async ([FromBody] ExampleMessage message, IOutbox outbox, SampleDbContext sampleDbContext) =>
{
    var messages = new List<ExampleMessage>();

    for (var i = 0; i < 100; i++)
    {
        await outbox.Add(message);
    }

    await sampleDbContext.SaveChangesAsync();
    await outbox.TransactionCommited();
});

app.MapGet("/get-messages-from-outbox", async (DbContext dbContext) =>
{
    var messages = await dbContext.Set<OutboxMessageStorage>().AsNoTracking().ToListAsync();

    return messages;
});

app.MapGet("/get-messages-from-inbox", async (DbContext dbContext) =>
{
    var messages = await dbContext.Set<InboxMessageStorage>().AsNoTracking().ToListAsync();

    return messages;
});

app.MapGet("/get-idempotent-keys-from-inbox", async (DbContext dbContext) =>
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
