using Microsoft.EntityFrameworkCore;
using TransactionalBox.Customers.Database;
using TransactionalBox;
using System.Security.Cryptography;

const string connectionString = "Server=mssql;Database=msdb;User Id=sa;Password=Password123!@#;TrustServerCertificate=true;";
const string bootstrapServers = "plaintext://kafka:9092";

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddDbContextPool<CustomersDbContext>(x => x.UseSqlServer(connectionString));

services.AddTransactionalBox(x =>
{
    x.AddOutbox(
        storage => storage.UseEntityFrameworkCore<CustomersDbContext>(),
        transport => transport.UseKafka(settings => settings.BootstrapServers = bootstrapServers)
    );

    x.AddInbox(
        storage => storage.UseEntityFrameworkCore<CustomersDbContext>(),
        transport => transport.UseKafka(settings => settings.BootstrapServers = bootstrapServers)
    );
},
x => x.ServiceId = "Customers");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    try
    {
        await Task.Delay(RandomNumberGenerator.GetInt32(0, 1000));
        await scope.ServiceProvider.GetRequiredService<CustomersDbContext>().Database.EnsureCreatedAsync();
    }
    finally { }
}

app.MapGet("/customers", async (CustomersDbContext dbContext) => await dbContext.Customers.ToListAsync());

app.Run();