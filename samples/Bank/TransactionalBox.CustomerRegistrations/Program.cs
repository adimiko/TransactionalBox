using Microsoft.EntityFrameworkCore;
using System.Net;
using TransactionalBox;
using TransactionalBox.CustomerRegistrations.Database;
using TransactionalBox.CustomerRegistrations.Models;
using TransactionalBox.CustomerRegistrations.Requests;

var connectionString = "";
var bootstrapServers = "";

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

// Controllers
app.MapPost("/create-customer-registration", async (
    CreateCustomerRegistrationRequest request,
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

    return Task.FromResult(id);
});

app.MapPut("/approve-customer-registration", async (
    ApproveCustomerRegistrationRequest request,
    CustomerRegistrationDbContext dbContext) =>
{
    var customerRegistration = await dbContext.CustomerRegistrations.FindAsync(request.Id);

    if (customerRegistration is null)
    {
        return HttpStatusCode.NotFound;
    }

    customerRegistration.IsApproved = true;

    await dbContext.SaveChangesAsync();

    return HttpStatusCode.OK;
});

app.MapGet("/customer-registrations", async (CustomerRegistrationDbContext dbContext) => await dbContext.CustomerRegistrations.ToListAsync());

app.Run();