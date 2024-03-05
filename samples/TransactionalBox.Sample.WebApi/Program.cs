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

builder.Services.AddTransactionalBox(x =>
{
    x.AddOutbox(storage => storage.UseEntityFramework<SampleDbContext>())
     .WithWorker(storage => storage.UseEntityFramework(), transport => transport.UseKafka(bootstrapServers));

    x.AddInbox(storage => storage.UseEntityFramework<SampleDbContext>())
     .WithWorker(storage => storage.UseEntityFramework(), transport => transport.UseKafka(bootstrapServers));
});

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

app.MapPost("/outbox", async (IOutboxSender outboxSender, DbContext dbContext) =>
{
    var message = new ExampleMessage();

    await outboxSender.Send(message, "ModuleName", DateTime.UtcNow);
    await dbContext.SaveChangesAsync();
});

app.MapGet("/outbox", (DbContext dbContext) =>
{
    var messages = dbContext.Set<OutboxMessage>().ToList();

    return messages;
});

app.MapGet("/inbox", (DbContext dbContext) =>
{
    var messages = dbContext.Set<InboxMessageStorageModel>().ToList();

    return messages;
});

app.Run();
