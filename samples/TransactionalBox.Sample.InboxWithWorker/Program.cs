using Microsoft.EntityFrameworkCore;
using TransactionalBox;
using TransactionalBox.Inbox;
using TransactionalBox.Inbox.Storage.EntityFramework;
using TransactionalBox.Inbox.Transport.Kafka;
using TransactionalBox.Sample.InboxWithWorker;
using TransactionalBox.Inbox.Internals.Storage;
using System.Reflection;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Serilog.Events;

const string connectionString = "Server=mssql;Database=msdb;User Id=sa;Password=Password123!@#;TrustServerCertificate=true;";
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

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextPool<ServiceWithInboxDbContext>(x => x.UseSqlServer(connectionString));

builder.Services.AddTransactionalBox(x =>
{
    x.AddInbox(storage => storage.UseEntityFramework<ServiceWithInboxDbContext>())
     .WithWorker(transport => transport.UseKafka(settings => settings.BootstrapServers = bootstrapServers));
},
settings => settings.ServiceId = "ServiceWithInbox");

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        scope.ServiceProvider.GetRequiredService<ServiceWithInboxDbContext>().Database.EnsureCreated();
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

app.MapGet("/get-messages-from-inbox", async (DbContext dbContext) => await dbContext.Set<InboxMessageStorage>().AsNoTracking().ToListAsync());

app.MapGet("/get-idempotent-messages-from-inbox", async (DbContext dbContext) => await dbContext.Set<IdempotentInboxKey>().AsNoTracking().ToListAsync());

app.MapGet("/inbox-distributed-locks", async (DbContext dbContext) => await dbContext.Set<InboxDistributedLock>().AsNoTracking().ToListAsync());


app.Run();