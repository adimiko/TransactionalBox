using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using TransactionalBox;
using TransactionalBox.CustomerRegistrations.Database;
using TransactionalBox.CustomerRegistrations.Messages;
using TransactionalBox.CustomerRegistrations.Models;
using TransactionalBox.CustomerRegistrations.Requests;

const string connectionString = "Host=postgres;Port=5432;Database=postgres;Username=postgres;Password=postgres";
const string bootstrapServers = "plaintext://kafka:9092";

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddDbContextPool<CustomerRegistrationDbContext>(x => x.UseNpgsql(connectionString));

services.AddTransactionalBox(x =>
{
    x.AddOutbox(
        storage => storage.UseEntityFramework<CustomerRegistrationDbContext>(),
        transport => transport.UseKafka(settings => settings.BootstrapServers = bootstrapServers));
}, 
x => x.ServiceId = "CustomerRegistrations");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    try
    {
        await Task.Delay(RandomNumberGenerator.GetInt32(0, 1000));
        await scope.ServiceProvider.GetRequiredService<CustomerRegistrationDbContext>().Database.EnsureCreatedAsync();
    }
    finally { }
}

// Controllers
app.MapPost("/create-customer-registration", async (
    [FromBody] CreateCustomerRegistrationRequest request,
    CustomerRegistrationDbContext dbContext) =>
{
    var id = Guid.NewGuid();
    var nowUtc = DateTime.UtcNow;

    var customerRegistration = new CustomerRegistration()
    {
        Id = id,
        FirstName = request.FirstName,
        LastName = request.LastName,
        Age = request.Age,
        CreatedAtUtc = nowUtc,
        UpdatedAtUtc = nowUtc,
    };

    await dbContext.AddAsync(customerRegistration);
    await dbContext.SaveChangesAsync();

    return id;
});

app.MapPut("/approve-customer-registration", async (
    [FromBody] ApproveCustomerRegistrationRequest request,
    CustomerRegistrationDbContext dbContext,
    IOutbox outbox) =>
{
    var nowUtc = DateTime.UtcNow;

    var customerRegistration = await dbContext.CustomerRegistrations.FindAsync(request.Id);

    if (customerRegistration is null)
    {
        return HttpStatusCode.NotFound;
    }

    customerRegistration.IsApproved = true;
    customerRegistration.UpdatedAtUtc = nowUtc;

    var commandMessage = new CreateCustomerCommandMessage()
    {
        Id = customerRegistration.Id,
        FirstName = customerRegistration.FirstName,
        LastName = customerRegistration.LastName,
        Age = customerRegistration.Age,
    };

    await outbox.Add(commandMessage);

    await dbContext.SaveChangesAsync();

    await outbox.TransactionCommited();

    return HttpStatusCode.OK;
});

app.MapGet("/customer-registrations", async (CustomerRegistrationDbContext dbContext) => await dbContext.CustomerRegistrations.ToListAsync());

app.Run();