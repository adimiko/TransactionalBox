using Microsoft.EntityFrameworkCore;
using TransactionalBox.BankAccounts.Database;
using TransactionalBox;
using System.Security.Cryptography;
using BankLogger;

const string connectionString = "server=mssql;uid=root;pwd=password;database=db";
const string bootstrapServers = "plaintext://kafka:9092";

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

builder.AddBankLogger(configuration, typeof(BankAccountsDbContext).Assembly);

var services = builder.Services;

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddDbContextPool<BankAccountsDbContext>(x => x.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

services.AddTransactionalBox(x =>
{
    x.AddInbox(
        storage => storage.UseEntityFrameworkCore<BankAccountsDbContext>(),
        transport => transport.UseKafka(settings => settings.BootstrapServers = bootstrapServers));
},
x => x.ServiceId = "BankAccounts");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    try
    {
        await Task.Delay(RandomNumberGenerator.GetInt32(0, 1000));
        await scope.ServiceProvider.GetRequiredService<BankAccountsDbContext>().Database.EnsureCreatedAsync();
    }
    finally { }
}

app.MapGet("/bank-accounts", async (BankAccountsDbContext dbContext) => await dbContext.BankAccounts.ToListAsync());

app.Run();