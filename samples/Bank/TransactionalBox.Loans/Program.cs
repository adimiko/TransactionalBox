using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using TransactionalBox.Loans.Database;
using TransactionalBox;

const string connectionString = "Host=db-loans;Port=5433;Database=postgres;Username=postgres;Password=postgres";
const string bootstrapServers = "plaintext://kafka:9092";

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddDbContextPool<LoansDbContext>(x => x.UseNpgsql(connectionString));

services.AddTransactionalBox(x =>
{
    x.AddInbox(
        storage => storage.UseEntityFramework<LoansDbContext>(),
        transport => transport.UseKafka(settings => settings.BootstrapServers = bootstrapServers),
        assemblyConfiguraton: a => a.RegisterFromAssemblies(typeof(LoansDbContext).Assembly));
},
x => x.ServiceId = "Loans");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    try
    {
        await Task.Delay(RandomNumberGenerator.GetInt32(0, 1000));
        await scope.ServiceProvider.GetRequiredService<LoansDbContext>().Database.EnsureCreatedAsync();
    }
    finally { }
}

app.MapGet("/loans", async (LoansDbContext dbContext) => await dbContext.Loans.ToListAsync());

app.Run();