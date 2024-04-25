using Microsoft.EntityFrameworkCore;
using TransactionalBox;
using TransactionalBox.Inbox;
using TransactionalBox.Inbox.Storage.EntityFramework;
using TransactionalBox.Base.Inbox.StorageModel.Internals;
using TransactionalBox.InboxWorker;
using TransactionalBox.InboxWorker.Storage.EntityFramework;
using TransactionalBox.InboxWorker.Transport.Kafka;
using TransactionalBox.Sample.InboxWithWorker;

const string connectionString = "Server=mssql;Database=msdb;User Id=sa;Password=Password123!@#;TrustServerCertificate=true;";
const string bootstrapServers = "plaintext://kafka:9092";


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextPool<ServiceWithInboxDbContext>(x => x.UseSqlServer(connectionString));

builder.Services.AddTransactionalBox(x =>
{
    x.AddInbox(storage => storage.UseEntityFramework<ServiceWithInboxDbContext>())
     .WithWorker(storage => storage.UseEntityFramework(), transport => transport.UseKafka(settings => settings.BootstrapServers = bootstrapServers));
},
settings => settings.ServiceId = "ServiceWithInbox");

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<ServiceWithInboxDbContext>().Database.EnsureCreated();
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