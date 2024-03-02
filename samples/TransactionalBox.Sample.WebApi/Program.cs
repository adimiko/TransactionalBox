using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using TransactionalBox;
using TransactionalBox.Outbox;
using TransactionalBox.Outbox.EntityFramework;
using TransactionalBox.Outbox.Internals;
using TransactionalBox.Sample.WebApi;

var postgreSqlContainer = new PostgreSqlBuilder()
  .WithImage("postgres:15.1")
  .Build();

await postgreSqlContainer.StartAsync();

var connectionString = postgreSqlContainer.GetConnectionString();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextPool<SampleDbContext>(x => x.UseNpgsql(connectionString));

builder.Services.AddTransactionalBox(x =>
{
    x.AddOutbox(x => x.AddEntityFramework<SampleDbContext>());
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

    await outboxSender.Send(message, "asd", DateTime.UtcNow);
    await dbContext.SaveChangesAsync();
});

app.MapGet("/outbox", (DbContext dbContext) =>
{
    var messages = dbContext.Set<OutboxMessage>().ToList();

    return messages;
});

app.Run();
