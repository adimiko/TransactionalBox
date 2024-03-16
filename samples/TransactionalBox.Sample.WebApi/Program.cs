using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Testcontainers.Kafka;
using Testcontainers.PostgreSql;
using TransactionalBox;
using TransactionalBox.Inbox;
using TransactionalBox.Inbox.EntityFramework;
using TransactionalBox.InboxBase.StorageModel;
using TransactionalBox.InboxWorker;
using TransactionalBox.InboxWorker.EntityFramework;
using TransactionalBox.InboxWorker.Kafka;
using TransactionalBox.Outbox;
using TransactionalBox.Outbox.EntityFramework;
using TransactionalBox.OutboxBase.StorageModel;
using TransactionalBox.OutboxWorker;
using TransactionalBox.OutboxWorker.EntityFramework;
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
     .WithWorker(storage => storage.UseEntityFramework(), transport => transport.UseKafka(settings => settings.BootstrapServers = bootstrapServers), settings => settings.NumberOfOutboxProcessor = 1);

    x.AddInbox(storage => storage.UseEntityFramework<SampleDbContext>())
     .WithWorker(storage => storage.UseEntityFramework(), transport => transport.UseKafka(settings => settings.BootstrapServers = bootstrapServers));
},
settings => settings.ServiceName = "Registrations");

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
    for(var i = 0; i < 10; i++)
    {
        await outbox.Add(message, m =>
        {
            m.Receiver = "Registrations";
            m.OccurredUtc = DateTime.UtcNow;
        });
    }

    await dbContext.SaveChangesAsync();
});

app.MapGet("/get-messages-from-outbox", (DbContext dbContext) =>
{
    var messages = dbContext.Set<OutboxMessage>().ToList();

    return messages;
});

app.MapGet("/get-messages-from-inbox", (DbContext dbContext) =>
{
    var messages = dbContext.Set<InboxMessage>().ToList();

    return messages;
});

app.MapGet("/locks", (DbContext dbContext) =>
{
    var messages = dbContext.Set<OutboxLock>().ToList();

    return messages;
});

app.Run();
