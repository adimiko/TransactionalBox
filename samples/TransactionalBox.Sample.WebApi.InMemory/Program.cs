using TransactionalBox;
using TransactionalBox.Outbox;
using TransactionalBox.Outbox.Storage.InMemory;
using TransactionalBox.OutboxWorker;
using TransactionalBox.OutboxWorker.Storage.InMemory;
using TransactionalBox.OutboxWorker.Transport.InMemory;
using TransactionalBox.InboxWorker;
using TransactionalBox.InboxWorker.Transport.InMemory;
using TransactionalBox.InboxWorker.Storage.InMemory;
using TransactionalBox.Inbox;
using TransactionalBox.Inbox.Storage.InMemory;
using TransactionalBox.Base.Inbox.Storage.InMemory;
using TransactionalBox.Base.Outbox.Storage.InMemory;
using Microsoft.AspNetCore.Mvc;
using TransactionalBox.Sample.WebApi.InMemory.ServiceWithOutbox;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ExampleServiceWithOutbox>();

builder.Services.AddTransactionalBox(x =>
{
    x.AddOutbox(storage => storage.UseInMemory())
     .WithWorker(
        storage => storage.UseInMemory(),
        transport => transport.UseInMemory());

    x.AddInbox(storage => storage.UseInMemory())
     .WithWorker(
        storage => storage.UseInMemory(),
        transport => transport.UseInMemory());
},
settings => settings.ServiceId = "ServiceName");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/add-message-to-outbox", async (ExampleServiceWithOutbox exampleServiceWithOutbox) => await exampleServiceWithOutbox.Execute());

app.MapGet("/get-messages-from-outbox", async (IOutboxStorageReadOnly outboxStorageReadOnly) => outboxStorageReadOnly.OutboxMessages);

app.MapGet("/get-messages-from-inbox", async (IInboxStorageReadOnly inboxStorage) => inboxStorage.InboxMessages);

app.MapGet("/get-idempotent-messages-from-inbox", async (IInboxStorageReadOnly inboxStorage) => inboxStorage.IdempotentInboxKeys);

app.Run();
