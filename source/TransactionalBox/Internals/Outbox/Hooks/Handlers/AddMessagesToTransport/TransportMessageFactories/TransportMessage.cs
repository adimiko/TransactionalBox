namespace TransactionalBox.Internals.Outbox.Hooks.Handlers.AddMessagesToTransport.TransportMessageFactories
{
    internal sealed record TransportMessage(Guid Id, string Topic, DateTime OccurredUtc, string Payload);
}
