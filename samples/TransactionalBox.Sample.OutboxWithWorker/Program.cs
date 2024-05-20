using TransactionalBox;
using TransactionalBox.Outbox;
using TransactionalBox.Sample.OutboxWithWorker;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;
using TransactionalBox.Outbox.Internals.Storage;
using System.Security.Cryptography;

const string connectionString = "Host=postgres;Port=5432;Database=postgres;Username=postgres;Password=postgres";
const string bootstrapServers = "plaintext://kafka:9092";

var builder = WebApplication.CreateBuilder(args);

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true)
    .Build();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .Enrich.WithEnvironmentName()
    .Enrich.WithMachineName()
    .WriteTo.Console()
    .WriteTo.Debug()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://elasticsearch:9200"))
    {
        AutoRegisterTemplate = true,
        //AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
        IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name!.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
        NumberOfReplicas = 1,
        NumberOfShards = 2
    })
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddDbContextPool<ServiceWithOutboxDbContext>(x => x.UseNpgsql(connectionString));

builder.Services.AddTransactionalBox(x =>
{
    x.AddOutbox(
        storage => storage.UseEntityFramework<ServiceWithOutboxDbContext>(),
        transport => transport.UseKafka(settings => settings.BootstrapServers = bootstrapServers));
},
settings => settings.ServiceId = "ServiceWithOutbox");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        await Task.Delay(RandomNumberGenerator.GetInt32(0, 100));
        await scope.ServiceProvider.GetRequiredService<ServiceWithOutboxDbContext>().Database.EnsureCreatedAsync();
    }
    finally { }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/add-message-to-outbox", async ([FromBody] ExampleMessage message, IOutbox outbox, Microsoft.Extensions.Logging.ILogger<ExampleMessage> logger, IEntityFrameworkOutboxUnitOfWork uow) =>
{
    var tx = await uow.BeginTransactionAsync();

    try
    {
        for (var i = 0; i < 10; i++)
        {
            await outbox.Add(message);
        }

        await tx.CommitAsync();
    }
    catch (Exception ex)
    {
        await tx.RollbackAsync();
    }
});

app.MapPost("/publish-message", async ([FromBody] PublishableMessage message, IOutbox outbox, Microsoft.Extensions.Logging.ILogger<ExampleMessage> logger, IEntityFrameworkOutboxUnitOfWork uow) =>
{
    var tx = await uow.BeginTransactionAsync();

    try
    {
        for (var i = 0; i < 10; i++)
        {
            await outbox.Add(message);
        }

        await tx.CommitAsync();
    }
    catch (Exception ex)
    {
        await tx.RollbackAsync();
    }
});

app.MapGet("/get-messages-from-outbox", async (DbContext dbContext) =>
{
    var messages = await dbContext.Set<OutboxMessageStorage>().AsNoTracking().ToListAsync();

    return messages;
});

app.MapGet("/outbox-distributed-locks", async (DbContext dbContext) =>
{
    var locks = await dbContext.Set<OutboxDistributedLock>().AsNoTracking().ToListAsync();

    return locks;
});

app.Run();