using TransactionalBox;
using TransactionalBox.Outbox;
using TransactionalBox.Outbox.EntityFramework;
using TransactionalBox.OutboxWorker;
using TransactionalBox.OutboxWorker.EntityFramework;
using TransactionalBox.OutboxWorker.Transport.Kafka;
using TransactionalBox.Sample.OutboxWithWorker;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TransactionalBox.OutboxBase.StorageModel.Internals;

const string connectionString = "Host=postgres;Port=5432;Database=postgres;Username=postgres;Password=postgres";
const string bootstrapServers = "plaintext://kafka:9092";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<ServiceWithOutboxDbContext>(x => x.UseNpgsql(connectionString));

builder.Services.AddTransactionalBox(x =>
{
    x.AddOutbox(storage => storage.UseEntityFramework<ServiceWithOutboxDbContext>())
     .WithWorker(storage => storage.UseEntityFramework(), transport => transport.UseKafka(settings => settings.BootstrapServers = bootstrapServers), settings => settings.AddMessagesToTransportSettings.NumberOfInstances = 3);
},
settings => settings.ServiceId = "ServiceWithOutbox");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{

    scope.ServiceProvider.GetRequiredService<ServiceWithOutboxDbContext>().Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/add-message-to-outbox", async ([FromBody] ExampleMessage message, IOutbox outbox, DbContext dbContext) =>
{
    for (var i = 0; i < 100; i++)
    {
        //TODO AddRange
        await outbox.Add(message, m =>
        {
            m.Receiver = "ServiceWithInbox";
        });
    }

    await dbContext.SaveChangesAsync();
});

app.MapGet("/get-messages-from-outbox", async (DbContext dbContext) =>
{
    var messages = await dbContext.Set<OutboxMessage>().AsNoTracking().ToListAsync();

    return messages;
});

app.MapGet("/outbox-distributed-locks", async (DbContext dbContext) =>
{
    var locks = await dbContext.Set<OutboxDistributedLock>().AsNoTracking().ToListAsync();

    return locks;
});

app.Run();