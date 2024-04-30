using TransactionalBox;
using TransactionalBox.Outbox;
using TransactionalBox.Outbox.Transport.InMemory;
using TransactionalBox.Inbox;
using TransactionalBox.Inbox.Transport.InMemory;
using TransactionalBox.Inbox.Storage.InMemory;
using Microsoft.AspNetCore.Mvc;
using TransactionalBox.Sample.WebApi.InMemory.ServiceWithOutbox;
using TransactionalBox.Outbox.Internals.Storage.InMemory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ExampleServiceWithOutbox>();

builder.Services.AddTransactionalBox(x =>
{
    x.AddOutbox()
     .WithWorker(transport => transport.UseInMemory());

    x.AddInbox(storage => storage.UseInMemory(), assemblyConfiguraton: x => x.RegisterFromAssemblies(typeof(ExampleMessage).Assembly))
    .WithWorker(transport => transport.UseInMemory());
}, 
configuration: builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/add-message-to-outbox", async (ExampleServiceWithOutbox exampleServiceWithOutbox) => await exampleServiceWithOutbox.Execute());

app.MapGet("/get-messages-from-outbox",  (IOutboxStorageReadOnly outboxStorageReadOnly) => outboxStorageReadOnly.OutboxMessages);

app.MapGet("/get-messages-from-inbox", (IInboxStorageReadOnly inboxStorage) => inboxStorage.InboxMessages);

app.MapGet("/get-idempotent-messages-from-inbox", (IInboxStorageReadOnly inboxStorage) => inboxStorage.IdempotentInboxKeys);

app.Run();
